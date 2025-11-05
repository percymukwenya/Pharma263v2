/**
 * Pharma263 Calculation Service
 * Moves business calculations from frontend to API for consistency across integrations
 * Ensures DRY principle and pharmaceutical accuracy
 */

class Pharma263Calculations {
    constructor() {
        // Use relative URL to call the MVC CalculationsController
        // This matches the pattern used throughout the app (e.g., /Customer/GetCustomer)
        this.apiBaseUrl = '/api/calculations';
        this.cache = new Map();
        this.cacheTimeout = 30000; // 30 seconds
    }

    /**
     * Calculate sales/quotation totals on the server
     */
    async calculateSalesTotal(items) {
        const cacheKey = this.generateCacheKey('sales_total', items);
        
        if (this.cache.has(cacheKey)) {
            return this.cache.get(cacheKey);
        }

        try {
            const response = await $.ajax({
                url: `${this.apiBaseUrl}/sales-total`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ items }),
                formSubmission: true // Prevent global loading
            });

            // Extract data from ApiResponse wrapper
            const result = response.data || response;
            
            // Cache the result
            this.setCacheValue(cacheKey, result);
            
            return result;
        } catch (error) {
            console.error('Sales calculation error:', error);
            // Fallback to client-side calculation if API fails
            return this.fallbackSalesCalculation(items);
        }
    }

    /**
     * Calculate purchase totals on the server
     */
    async calculatePurchaseTotal(items) {
        const cacheKey = this.generateCacheKey('purchase_total', items);
        
        if (this.cache.has(cacheKey)) {
            return this.cache.get(cacheKey);
        }

        try {
            const response = await $.ajax({
                url: `${this.apiBaseUrl}/purchase-total`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ items }),
                formSubmission: true
            });

            const result = response.data || response;
            this.setCacheValue(cacheKey, result);
            return result;
        } catch (error) {
            console.error('Purchase calculation error:', error);
            return this.fallbackPurchaseCalculation(items);
        }
    }

    /**
     * Validate and calculate discount amounts
     */
    async calculateDiscount(subtotal, discountPercent, discountAmount, discountType = 'percentage') {
        try {
            const response = await $.ajax({
                url: `${this.apiBaseUrl}/discount`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    subtotal,
                    discountPercent,
                    discountAmount,
                    discountType
                }),
                formSubmission: true
            });

            return response.data || response;
        } catch (error) {
            console.error('Discount calculation error:', error);
            return this.fallbackDiscountCalculation(subtotal, discountPercent, discountAmount, discountType);
        }
    }

    /**
     * Calculate stock availability and validate quantities
     */
    async validateStockQuantity(stockId, requestedQuantity, existingOrders = []) {
        try {
            const response = await $.ajax({
                url: `${this.apiBaseUrl}/stock-validation`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    stockId,
                    requestedQuantity,
                    existingOrders
                }),
                formSubmission: true
            });

            return response.data || response;
        } catch (error) {
            console.error('Stock validation error:', error);
            // Return error state - let the API handle all validation logic
            return {
                isValid: false,
                availableQuantity: 0,
                requestedQuantity: requestedQuantity,
                validationMessage: 'Unable to validate stock. Please try again.',
                error: 'API connection failed'
            };
        }
    }

    /**
     * Calculate pricing with business rules (volume discounts, customer pricing, etc.)
     */
    async calculatePricing(medicineId, customerId, quantity, basePrice) {
        const cacheKey = this.generateCacheKey('pricing', { medicineId, customerId, quantity });
        
        if (this.cache.has(cacheKey)) {
            return this.cache.get(cacheKey);
        }

        try {
            const response = await $.ajax({
                url: `${this.apiBaseUrl}/pricing`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    medicineId,
                    customerId,
                    quantity,
                    basePrice
                }),
                formSubmission: true
            });

            const result = response.data || response;
            this.setCacheValue(cacheKey, result);
            return result;
        } catch (error) {
            console.error('Pricing calculation error:', error);
            return { 
                finalPrice: basePrice, 
                appliedDiscounts: [], 
                volumeDiscount: 0,
                customerDiscount: 0 
            };
        }
    }

    /**
     * Calculate taxes and regulatory fees
     */
    async calculateTaxes(items, customerId, transactionType = 'sale') {
        try {
            const response = await $.ajax({
                url: `${this.apiBaseUrl}/taxes`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    items,
                    customerId,
                    transactionType
                }),
                formSubmission: true
            });

            return response.data || response;
        } catch (error) {
            console.error('Tax calculation error:', error);
            return { 
                totalTax: 0, 
                taxBreakdown: [], 
                taxableAmount: 0,
                exemptAmount: 0 
            };
        }
    }

    /**
     * Validate pharmaceutical business rules
     */
    async validatePharmaceuticalRules(items, customerId, transactionType) {
        try {
            const response = await $.ajax({
                url: `${this.apiBaseUrl}/pharmaceutical-validation`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    items,
                    customerId,
                    transactionType
                }),
                formSubmission: true
            });

            return response.data || response;
        } catch (error) {
            console.error('Pharmaceutical validation error:', error);
            return { isValid: true, warnings: [], errors: [] };
        }
    }

    /**
     * Fallback client-side calculations (when API is unavailable)
     */
    fallbackSalesCalculation(items) {
        let subtotal = 0;
        let totalDiscount = 0;
        let grandTotal = 0;

        items.forEach(item => {
            const itemSubtotal = item.price * item.quantity;
            const discountAmount = item.discountType === 'percentage' 
                ? (itemSubtotal * item.discountPercent / 100)
                : item.discountAmount;
            
            const itemTotal = itemSubtotal - discountAmount;
            
            subtotal += itemSubtotal;
            totalDiscount += discountAmount;
            grandTotal += itemTotal;
        });

        return {
            subtotal: Number(subtotal.toFixed(2)),
            totalDiscount: Number(totalDiscount.toFixed(2)),
            grandTotal: Number(grandTotal.toFixed(2)),
            items: items.map(item => ({
                ...item,
                subtotal: Number((item.price * item.quantity).toFixed(2)),
                discountAmount: item.discountType === 'percentage' 
                    ? Number(((item.price * item.quantity * item.discountPercent / 100)).toFixed(2))
                    : Number(item.discountAmount.toFixed(2)),
                total: Number(((item.price * item.quantity) - (item.discountType === 'percentage' 
                    ? (item.price * item.quantity * item.discountPercent / 100)
                    : item.discountAmount)).toFixed(2))
            }))
        };
    }

    fallbackPurchaseCalculation(items) {
        return this.fallbackSalesCalculation(items); // Same logic for now
    }

    fallbackDiscountCalculation(subtotal, discountPercent, discountAmount, discountType) {
        let finalDiscount = 0;
        
        if (discountType === 'percentage') {
            finalDiscount = subtotal * (discountPercent / 100);
        } else {
            finalDiscount = discountAmount;
        }

        // Ensure discount doesn't exceed subtotal
        finalDiscount = Math.min(finalDiscount, subtotal);

        return {
            discountAmount: Number(finalDiscount.toFixed(2)),
            finalTotal: Number((subtotal - finalDiscount).toFixed(2)),
            discountPercent: Number((finalDiscount / subtotal * 100).toFixed(2))
        };
    }

    /**
     * Real-time calculation for dynamic forms
     */
    async updateFormCalculations(formSelector, options = {}) {
        const $form = $(formSelector);
        const settings = {
            itemSelector: '.calculation-item',
            priceSelector: '[data-price]',
            quantitySelector: '[data-quantity]',
            discountSelector: '[data-discount]',
            totalSelector: '[data-total]',
            subtotalElement: '[data-subtotal]',
            grandTotalElement: '[data-grand-total]',
            calculateTaxes: false,
            customerId: null,
            ...options
        };

        // Collect all items
        const items = [];
        $form.find(settings.itemSelector).each(function() {
            const $item = $(this);
            const price = parseFloat($item.find(settings.priceSelector).val() || 0);
            const quantity = parseFloat($item.find(settings.quantitySelector).val() || 0);
            const discount = parseFloat($item.find(settings.discountSelector).val() || 0);
            const medicineId = $item.data('medicine-id');

            if (price > 0 && quantity > 0) {
                items.push({
                    medicineId,
                    price,
                    quantity,
                    discountPercent: discount,
                    discountType: 'percentage'
                });
            }
        });

        if (items.length === 0) {
            this.clearFormTotals($form, settings);
            return;
        }

        try {
            // Calculate totals
            const totals = await this.calculateSalesTotal(items);
            
            // Calculate taxes if required
            let taxes = { totalTax: 0 };
            if (settings.calculateTaxes && settings.customerId) {
                taxes = await this.calculateTaxes(items, settings.customerId);
            }

            // Update form display
            this.updateFormTotals($form, totals, taxes, settings);

            // Update individual item totals
            totals.items.forEach((item, index) => {
                const $itemRow = $form.find(settings.itemSelector).eq(index);
                $itemRow.find(settings.totalSelector).text(this.formatCurrency(item.total));
            });

        } catch (error) {
            console.error('Form calculation update failed:', error);
        }
    }

    updateFormTotals($form, totals, taxes, settings) {
        const finalTotal = totals.grandTotal + taxes.totalTax;

        $form.find(settings.subtotalElement).text(this.formatCurrency(totals.subtotal));
        $form.find(settings.grandTotalElement).text(this.formatCurrency(finalTotal));
        
        // Update discount display if element exists
        const $discountElement = $form.find('[data-total-discount]');
        if ($discountElement.length) {
            $discountElement.text(this.formatCurrency(totals.totalDiscount));
        }

        // Update tax display if element exists
        const $taxElement = $form.find('[data-total-tax]');
        if ($taxElement.length) {
            $taxElement.text(this.formatCurrency(taxes.totalTax));
        }
    }

    clearFormTotals($form, settings) {
        $form.find(settings.subtotalElement).text('0.00');
        $form.find(settings.grandTotalElement).text('0.00');
        $form.find('[data-total-discount]').text('0.00');
        $form.find('[data-total-tax]').text('0.00');
    }

    /**
     * Debounced calculation for real-time updates
     */
    debouncedCalculation(formSelector, options = {}, delay = 500) {
        if (this.calculationTimeout) {
            clearTimeout(this.calculationTimeout);
        }

        this.calculationTimeout = setTimeout(() => {
            this.updateFormCalculations(formSelector, options);
        }, delay);
    }

    /**
     * Utility methods
     */
    generateCacheKey(type, data) {
        return `${type}_${JSON.stringify(data)}`;
    }

    setCacheValue(key, value) {
        this.cache.set(key, value);
        setTimeout(() => {
            this.cache.delete(key);
        }, this.cacheTimeout);
    }

    formatCurrency(amount, currency = '') {
        return currency + Number(amount).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    }

    /**
     * Initialize event handlers for automatic calculations
     */
    initAutoCalculations(formSelector, options = {}) {
        const $form = $(formSelector);
        const settings = {
            ...options,
            debounceDelay: options.debounceDelay || 500
        };

        // Bind events for automatic calculations
        $form.on('input change', '[data-price], [data-quantity], [data-discount]', () => {
            this.debouncedCalculation(formSelector, settings, settings.debounceDelay);
        });

        // Initial calculation
        this.updateFormCalculations(formSelector, settings);
    }
}

// Global instance
window.Pharma263Calculations = new Pharma263Calculations();

// Auto-initialize for forms with calculation attributes
$(document).ready(function() {
    $('form[data-auto-calculate]').each(function() {
        const options = $(this).data('calculation-options') || {};
        window.Pharma263Calculations.initAutoCalculations(this, options);
    });
});