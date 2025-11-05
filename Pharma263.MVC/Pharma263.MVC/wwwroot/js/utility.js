/**
 * Pharma263 Utility Functions
 * Modernized utility functions and form helpers
 */

class Pharma263Utils {
    /**
     * Auto-complete functionality for medicine names
     */
    static initMedicineAutocomplete($input, options = {}) {
        const settings = {
            minLength: 2,
            source: '/api/medicines/search',
            delay: 300,
            ...options
        };

        if ($.fn.autocomplete) {
            $input.autocomplete({
                minLength: settings.minLength,
                delay: settings.delay,
                source: function(request, response) {
                    $.ajax({
                        url: settings.source,
                        data: { term: request.term },
                        success: function(data) {
                            response(data.map(item => ({
                                label: `${item.name} (${item.brand})`,
                                value: item.name,
                                data: item
                            })));
                        },
                        error: function() {
                            response([]);
                        }
                    });
                },
                select: function(event, ui) {
                    if (settings.onSelect) {
                        settings.onSelect(ui.item.data, $input);
                    }
                }
            });
        }
    }

    /**
     * Auto-complete functionality for customers
     */
    static initCustomerAutocomplete($input, options = {}) {
        const settings = {
            minLength: 2,
            source: '/api/customers/search',
            delay: 300,
            ...options
        };

        if ($.fn.autocomplete) {
            $input.autocomplete({
                minLength: settings.minLength,
                delay: settings.delay,
                source: function(request, response) {
                    $.ajax({
                        url: settings.source,
                        data: { term: request.term },
                        success: function(data) {
                            response(data.map(item => ({
                                label: `${item.name} - ${item.phone || 'No phone'}`,
                                value: item.name,
                                data: item
                            })));
                        },
                        error: function() {
                            response([]);
                        }
                    });
                },
                select: function(event, ui) {
                    if (settings.onSelect) {
                        settings.onSelect(ui.item.data, $input);
                    }
                }
            });
        }
    }

    /**
     * Auto-complete functionality for suppliers
     */
    static initSupplierAutocomplete($input, options = {}) {
        const settings = {
            minLength: 2,
            source: '/api/suppliers/search',
            delay: 300,
            ...options
        };

        if ($.fn.autocomplete) {
            $input.autocomplete({
                minLength: settings.minLength,
                delay: settings.delay,
                source: function(request, response) {
                    $.ajax({
                        url: settings.source,
                        data: { term: request.term },
                        success: function(data) {
                            response(data.map(item => ({
                                label: `${item.name} - ${item.contactPerson || ''}`,
                                value: item.name,
                                data: item
                            })));
                        },
                        error: function() {
                            response([]);
                        }
                    });
                },
                select: function(event, ui) {
                    if (settings.onSelect) {
                        settings.onSelect(ui.item.data, $input);
                    }
                }
            });
        }
    }

    /**
     * Initialize dynamic form fields (add/remove functionality)
     */
    static initDynamicFields(containerSelector) {
        const $container = $(containerSelector);
        
        // Add new field
        $container.on('click', '[data-add-field]', function() {
            const template = $(this).data('template');
            const $template = $(template);
            
            if ($template.length) {
                const $newField = $template.clone().removeAttr('id');
                $newField.find('input, select, textarea').val('');
                $container.find('[data-fields-container]').append($newField);
                
                // Initialize any components in the new field
                Pharma263Utils.initFieldComponents($newField);
            }
        });

        // Remove field
        $container.on('click', '[data-remove-field]', function() {
            $(this).closest('[data-field-row]').remove();
        });
    }

    /**
     * Initialize components in a field (date pickers, autocomplete, etc.)
     */
    static initFieldComponents($container) {
        // Initialize date pickers
        $container.find('.date-picker, .mydatepicker').each(function() {
            if (!$(this).hasClass('hasDatepicker') && $.fn.datepicker) {
                $(this).datepicker({
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "-50:+10"
                });
            }
        });

        // Initialize select2 if available
        if ($.fn.select2) {
            $container.find('select[data-select2]').select2({
                theme: 'bootstrap4',
                width: '100%'
            });
        }

        // Initialize autocomplete fields
        $container.find('[data-autocomplete="medicine"]').each(function() {
            Pharma263Utils.initMedicineAutocomplete($(this));
        });
        
        $container.find('[data-autocomplete="customer"]').each(function() {
            Pharma263Utils.initCustomerAutocomplete($(this));
        });
        
        $container.find('[data-autocomplete="supplier"]').each(function() {
            Pharma263Utils.initSupplierAutocomplete($(this));
        });
    }

    /**
     * Format number with thousand separators
     */
    static formatNumber(num, decimals = 2) {
        return parseFloat(num).toFixed(decimals).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    }

    /**
     * Parse number from formatted string
     */
    static parseNumber(str) {
        return parseFloat(str.toString().replace(/,/g, '')) || 0;
    }

    /**
     * Calculate totals for dynamic tables
     */
    static calculateTableTotals(tableSelector, config = {}) {
        const $table = $(tableSelector);
        const settings = {
            qtyColumn: '[data-qty]',
            priceColumn: '[data-price]',
            discountColumn: '[data-discount]',
            totalColumn: '[data-total]',
            grandTotalElement: '[data-grand-total]',
            ...config
        };

        let grandTotal = 0;

        $table.find('tbody tr').each(function() {
            const $row = $(this);
            const qty = Pharma263Utils.parseNumber($row.find(settings.qtyColumn).val() || 0);
            const price = Pharma263Utils.parseNumber($row.find(settings.priceColumn).val() || 0);
            const discount = Pharma263Utils.parseNumber($row.find(settings.discountColumn).val() || 0);
            
            const subtotal = qty * price;
            const total = subtotal - discount;
            
            $row.find(settings.totalColumn).text(Pharma263Utils.formatNumber(total));
            grandTotal += total;
        });

        $(settings.grandTotalElement).text(Pharma263Utils.formatNumber(grandTotal));
        return grandTotal;
    }

    /**
     * Initialize number formatting for input fields
     */
    static initNumberFormatting($container = $(document)) {
        $container.find('input[data-format="currency"]').on('blur', function() {
            const value = Pharma263Utils.parseNumber($(this).val());
            $(this).val(Pharma263Utils.formatNumber(value));
        });

        $container.find('input[data-format="number"]').on('blur', function() {
            const value = Pharma263Utils.parseNumber($(this).val());
            $(this).val(value);
        });
    }

    /**
     * Confirmation dialog wrapper
     */
    static confirm(message, title = 'Confirm Action') {
        return new Promise((resolve) => {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: title,
                    text: message,
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, continue',
                    cancelButtonText: 'Cancel'
                }).then((result) => {
                    resolve(result.isConfirmed);
                });
            } else {
                resolve(confirm(message));
            }
        });
    }

    /**
     * Copy text to clipboard
     */
    static async copyToClipboard(text) {
        try {
            await navigator.clipboard.writeText(text);
            if (window.Pharma263) {
                window.Pharma263.showToast('Copied to clipboard', 'success');
            }
            return true;
        } catch (err) {
            console.error('Failed to copy text: ', err);
            return false;
        }
    }

    /**
     * Download file from URL
     */
    static downloadFile(url, filename = null) {
        const link = document.createElement('a');
        link.href = url;
        if (filename) {
            link.download = filename;
        }
        link.target = '_blank';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    /**
     * Validate pharmaceutical license numbers
     */
    static validateLicense(license, type = 'MCAZ') {
        const patterns = {
            'MCAZ': /^[A-Z0-9\-\/]+$/,
            'HPA': /^HPA[0-9A-Z\-\/]+$/i,
            'VAT': /^[A-Z0-9]+$/
        };

        return patterns[type] ? patterns[type].test(license) : true;
    }

    /**
     * Check expiry date warnings
     */
    static checkExpiryWarning(expiryDate, warningDays = 90) {
        const expiry = new Date(expiryDate);
        const today = new Date();
        const diffTime = expiry - today;
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

        if (diffDays < 0) {
            return { status: 'expired', days: Math.abs(diffDays), class: 'text-danger' };
        } else if (diffDays <= warningDays) {
            return { status: 'warning', days: diffDays, class: 'text-warning' };
        } else {
            return { status: 'good', days: diffDays, class: 'text-success' };
        }
    }
}

// Auto-initialize utility functions when document is ready
$(document).ready(function() {
    // Initialize field components
    Pharma263Utils.initFieldComponents($(document));
    
    // Initialize number formatting
    Pharma263Utils.initNumberFormatting();
    
    // Initialize dynamic fields
    $('[data-dynamic-fields]').each(function() {
        Pharma263Utils.initDynamicFields(this);
    });

    // Global utility access
    window.Pharma263Utils = Pharma263Utils;
});