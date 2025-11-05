/**
 * Pharma263 Reports JavaScript Library
 * Provides common functionality for all reports including loading states,
 * error handling, data fetching, and export functionality
 */

class ReportManager {
    constructor() {
        this.activeCharts = new Map();
        this.reportCache = new Map();
        this.defaultTimeout = 30000; // 30 seconds
    }

    /**
     * Show loading state for a report
     */
    showLoading(reportId) {
        const loadingEl = document.getElementById(`loading-${reportId}`);
        const contentEl = document.getElementById(`content-${reportId}`);
        const errorEl = document.getElementById(`error-${reportId}`);
        
        if (loadingEl) loadingEl.classList.remove('d-none');
        if (contentEl) contentEl.classList.add('d-none');
        if (errorEl) errorEl.classList.add('d-none');
    }

    /**
     * Hide loading state for a report
     */
    hideLoading(reportId) {
        const loadingEl = document.getElementById(`loading-${reportId}`);
        const contentEl = document.getElementById(`content-${reportId}`);
        
        if (loadingEl) loadingEl.classList.add('d-none');
        if (contentEl) contentEl.classList.remove('d-none');
    }

    /**
     * Show error state for a report
     */
    showError(reportId, message = 'Unable to load report data. Please try again.') {
        const loadingEl = document.getElementById(`loading-${reportId}`);
        const contentEl = document.getElementById(`content-${reportId}`);
        const errorEl = document.getElementById(`error-${reportId}`);
        const errorMessageEl = errorEl?.querySelector('.error-message');
        
        if (loadingEl) loadingEl.classList.add('d-none');
        if (contentEl) contentEl.classList.add('d-none');
        if (errorEl) errorEl.classList.remove('d-none');
        if (errorMessageEl) errorMessageEl.textContent = message;
    }

    /**
     * Update last updated timestamp
     */
    updateTimestamp(reportId) {
        const timestampEl = document.getElementById(`last-updated-${reportId}`);
        if (timestampEl) {
            timestampEl.textContent = 'Just now';
        }
    }

    /**
     * Load report data with error handling and caching
     */
    async loadReportData(reportId, url, params = {}, useCache = true) {
        const cacheKey = `${url}_${JSON.stringify(params)}`;
        
        // Check cache first
        if (useCache && this.reportCache.has(cacheKey)) {
            const cached = this.reportCache.get(cacheKey);
            if (Date.now() - cached.timestamp < 300000) { // 5 minutes
                return cached.data;
            }
        }

        this.showLoading(reportId);

        try {
            const controller = new AbortController();
            const timeoutId = setTimeout(() => controller.abort(), this.defaultTimeout);

            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                },
                body: JSON.stringify(params),
                signal: controller.signal
            });

            clearTimeout(timeoutId);

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }

            const data = await response.json();
            
            // Cache the result
            if (useCache) {
                this.reportCache.set(cacheKey, {
                    data: data,
                    timestamp: Date.now()
                });
            }

            this.hideLoading(reportId);
            this.updateTimestamp(reportId);
            
            return data;

        } catch (error) {
            console.error(`Error loading report ${reportId}:`, error);
            
            let errorMessage = 'Unable to load report data. Please try again.';
            if (error.name === 'AbortError') {
                errorMessage = 'Request timed out. Please try again.';
            } else if (error.message.includes('401') || error.message.includes('403')) {
                errorMessage = 'Session expired. Please refresh the page and try again.';
            } else if (error.message.includes('500')) {
                errorMessage = 'Server error. Please contact support if this persists.';
            }
            
            this.showError(reportId, errorMessage);
            throw error;
        }
    }

    /**
     * Create or update a chart
     */
    createChart(chartId, chartData, chartOptions = {}) {
        // Destroy existing chart if it exists
        if (this.activeCharts.has(chartId)) {
            this.activeCharts.get(chartId).destroy();
        }

        const ctx = document.getElementById(chartId);
        if (!ctx) {
            console.error(`Canvas element ${chartId} not found`);
            return null;
        }

        // Apply default styling
        const defaultOptions = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: 'white',
                    bodyColor: 'white',
                    borderColor: 'rgba(255, 255, 255, 0.1)',
                    borderWidth: 1,
                    cornerRadius: 6,
                    displayColors: true,
                    callbacks: {
                        label: function(context) {
                            if (context.parsed.y !== null) {
                                return context.dataset.label + ': ' + 
                                       new Intl.NumberFormat('en-US', {
                                           style: 'currency',
                                           currency: 'USD'
                                       }).format(context.parsed.y);
                            }
                            return '';
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    },
                    ticks: {
                        color: '#6c757d',
                        font: { size: 12 }
                    }
                },
                y: {
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    },
                    ticks: {
                        color: '#6c757d',
                        font: { size: 12 },
                        callback: function(value) {
                            return new Intl.NumberFormat('en-US', {
                                style: 'currency',
                                currency: 'USD',
                                minimumFractionDigits: 0
                            }).format(value);
                        }
                    }
                }
            }
        };

        const config = {
            type: chartOptions.type || 'line',
            data: chartData,
            options: this.mergeDeep(defaultOptions, chartOptions.options || {})
        };

        const chart = new Chart(ctx, config);
        this.activeCharts.set(chartId, chart);
        
        return chart;
    }

    /**
     * Deep merge objects
     */
    mergeDeep(target, source) {
        const output = Object.assign({}, target);
        if (this.isObject(target) && this.isObject(source)) {
            Object.keys(source).forEach(key => {
                if (this.isObject(source[key])) {
                    if (!(key in target))
                        Object.assign(output, { [key]: source[key] });
                    else
                        output[key] = this.mergeDeep(target[key], source[key]);
                } else {
                    Object.assign(output, { [key]: source[key] });
                }
            });
        }
        return output;
    }

    /**
     * Check if value is an object
     */
    isObject(item) {
        return item && typeof item === 'object' && !Array.isArray(item);
    }

    /**
     * Export report data
     */
    async exportReport(reportId, format, url, params = {}) {
        try {
            this.showLoading(reportId);

            const exportUrl = `${url}?format=${format}`;
            const response = await fetch(exportUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                },
                body: JSON.stringify(params)
            });

            if (!response.ok) {
                throw new Error(`Export failed: ${response.statusText}`);
            }

            // Create download link
            const blob = await response.blob();
            const downloadUrl = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = downloadUrl;
            a.download = `${reportId}-report.${format}`;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(downloadUrl);

            this.hideLoading(reportId);
            
            // Show success toast
            this.showToast('Report exported successfully!', 'success');

        } catch (error) {
            console.error('Export error:', error);
            this.showError(reportId, 'Failed to export report. Please try again.');
            this.showToast('Export failed. Please try again.', 'error');
        }
    }

    /**
     * Show toast notification
     */
    showToast(message, type = 'info') {
        if (typeof toastr !== 'undefined') {
            toastr[type](message);
        } else {
            console.log(`${type.toUpperCase()}: ${message}`);
        }
    }

    /**
     * Format currency
     */
    formatCurrency(amount, currency = 'USD') {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: currency
        }).format(amount);
    }

    /**
     * Format number
     */
    formatNumber(number, decimals = 0) {
        return new Intl.NumberFormat('en-US', {
            minimumFractionDigits: decimals,
            maximumFractionDigits: decimals
        }).format(number);
    }

    /**
     * Format percentage
     */
    formatPercentage(value, decimals = 1) {
        return new Intl.NumberFormat('en-US', {
            style: 'percent',
            minimumFractionDigits: decimals,
            maximumFractionDigits: decimals
        }).format(value / 100);
    }

    /**
     * Calculate trend direction and percentage
     */
    calculateTrend(current, previous) {
        if (!previous || previous === 0) {
            return { direction: 'neutral', percentage: 0, icon: 'fas fa-minus' };
        }

        const change = ((current - previous) / previous) * 100;
        
        if (change > 0) {
            return { 
                direction: 'up', 
                percentage: change, 
                icon: 'fas fa-arrow-up',
                class: 'text-success'
            };
        } else if (change < 0) {
            return { 
                direction: 'down', 
                percentage: Math.abs(change), 
                icon: 'fas fa-arrow-down',
                class: 'text-danger'
            };
        } else {
            return { 
                direction: 'neutral', 
                percentage: 0, 
                icon: 'fas fa-minus',
                class: 'text-muted'
            };
        }
    }
}

// Global instance
const reportManager = new ReportManager();

// Global functions for backward compatibility
function refreshReport(reportId) {
    location.reload();
}

function retryReport(reportId) {
    location.reload();
}

function exportReport(reportId, format) {
    const reportUrl = window.location.pathname;
    reportManager.exportReport(reportId, format, reportUrl);
}

// Global generateReport function (fallback if not defined in individual pages)
function generateReport(format) {
    console.log('Fallback generateReport called with format:', format);
    
    // Try to get the current page's report ID from the URL or page context
    const reportType = window.location.pathname.split('/').pop().toLowerCase();
    
    // Get date filters if they exist
    const startDate = document.getElementById('startDate')?.value;
    const endDate = document.getElementById('endDate')?.value;
    
    const params = {};
    if (startDate) params.startDate = startDate;
    if (endDate) params.endDate = endDate;
    
    if (format === 'preview') {
        // Reload the page with current filters
        const urlParams = new URLSearchParams();
        Object.keys(params).forEach(key => {
            if (params[key]) urlParams.append(key, params[key]);
        });
        
        const newUrl = urlParams.toString() ? 
            `${window.location.pathname}?${urlParams.toString()}` : 
            window.location.pathname;
        
        window.location.href = newUrl;
    } else {
        // Export functionality
        const exportUrl = window.location.pathname;
        reportManager.exportReport(reportType, format, exportUrl, params);
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    console.log('Reports Manager initialized');
});