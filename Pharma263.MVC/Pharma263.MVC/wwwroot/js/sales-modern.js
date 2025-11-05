/**
 * Modern Sales Form with API-based Calculations
 * Replaces client-side calculations with server-side business logic
 */

class ModernSalesForm {
    constructor(formSelector) {
        this.$form = $(formSelector);
        this.tempOrders = [];
        this.currentCustomerId = null;
        this.init();
    }

    init() {
        this.initializeSelects();
        this.bindEvents();
        this.initializeCalculations();
    }

    async initializeSelects() {
        try {
            // Load customers
            const customers = await this.loadData('/Customer/GetCustomer');
            this.populateSelect('#Customer', customers, 'Select Customer');
            
            // Load medicines/stock
            const medicines = await this.loadData('/Admin/GetStocks');
            this.populateSelect('#SMedicine', medicines, 'Select Medicine');
            
            // Load payment methods
            const paymentMethods = await this.loadData('/Admin/GetPaymentMethod');
            this.populateSelect('#PaymentMethod', paymentMethods);
            
            // Load sale statuses
            const saleStatuses = await this.loadData('/Admin/GetSaleStatus');
            this.populateSelect('#SaleStatus', saleStatuses);
            
        } catch (error) {
            console.error('Failed to initialize form data:', error);
            if (window.Pharma263) {
                window.Pharma263.showToast('Failed to load form data', 'error');
            }
        }
    }

    bindEvents() {
        // Customer selection
        this.$form.on('change', '#Customer', (e) => {
            this.currentCustomerId = parseInt($(e.target).val()) || null;
            this.recalculateTotal();
        });

        // Medicine selection
        this.$form.on('change', '#SMedicine', (e) => {
            const medicineId = $(e.target).val();
            if (medicineId) {
                this.loadMedicineDetails(medicineId);
            }
        });

        // Add item to list
        this.$form.on('click', '#SaleaddToList', (e) => {
            e.preventDefault();
            this.addItemToList();
        });

        // Remove item from list
        this.$form.on('click', '.sdeleteItem', (e) => {
            e.preventDefault();
            this.removeItemFromList($(e.target).closest('tr'));
        });

        // Quantity or discount changes
        this.$form.on('input change', '#SQuantity, #SDiscountPercent', () => {
            this.validateCurrentItem();
        });

        // Dynamic discount changes in table
        this.$form.on('input change', '.discount-percent-input', (e) => {
            this.updateItemCalculation($(e.target).closest('tr'));
        });

        // Save sale
        this.$form.on('click', '#SBtnSave', (e) => {
            e.preventDefault();
            this.saveSale();
        });
    }

    initializeCalculations() {
        // Auto-calculate when items change
        this.$form.attr('data-auto-calculate', 'true');
        this.$form.data('calculation-options', {
            itemSelector: '#detailsTable tbody tr',
            calculateTaxes: true,
            customerId: () => this.currentCustomerId
        });

        if (window.Pharma263Calculations) {
            window.Pharma263Calculations.initAutoCalculations(this.$form[0], {
                itemSelector: '#detailsTable tbody tr',
                priceSelector: 'td:eq(2)',
                quantitySelector: 'td:eq(4)',
                discountSelector: '.discount-percent-input',
                totalSelector: '.amount',
                subtotalElement: '#SSubTotal',
                grandTotalElement: '#SGrandTotal',
                calculateTaxes: true,
                customerId: this.currentCustomerId
            });
        }
    }

    async loadData(url) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "GET",
                url: url,
                dataType: "json",
                success: resolve,
                error: reject
            });
        });
    }

    populateSelect(selector, data, emptyText = 'Select...') {
        const $select = $(selector);
        $select.empty().append(`<option value="">${emptyText}</option>`);
        
        data.forEach(item => {
            $select.append(`<option value="${item.id}">${item.name}</option>`);
        });
        
        if ($.fn.select2) {
            $select.select2();
        }
    }

    async loadMedicineDetails(medicineId) {
        try {
            const data = await this.loadData(`/Medicine/GetStockMedicineById?id=${medicineId}`);
            if (data) {
                const tempStockCount = this.getTempStockCount(data.id);
                const availableStock = data.totalQuantity - tempStockCount;
                
                $('#medicineDetails').html(`
                    <p style="font-size:11px">
                        Selling Price: <strong>${data.sellingPrice}</strong>, 
                        Available Stock: <strong>${availableStock}</strong>
                    </p>
                `);
                
                // Store for validation
                this.currentItemPrice = data.sellingPrice;
                this.currentAvailableStock = availableStock;
            }
        } catch (error) {
            console.error('Failed to load medicine details:', error);
        }
    }

    getTempStockCount(medicineId) {
        return this.tempOrders
            .filter(order => order.id === medicineId)
            .reduce((total, order) => total + order.quantity, 0);
    }

    async validateCurrentItem() {
        const medicineId = parseInt($('#SMedicine').val()) || 0;
        const quantity = parseInt($('#SQuantity').val()) || 0;

        if (!medicineId || !quantity) return false;

        try {
            // Validate stock quantity via API (with client-side fallback)
            const validation = await window.Pharma263Calculations.validateStockQuantity(
                medicineId,
                quantity,
                this.tempOrders
            );

            // Debug logging
            console.log('Stock Validation Request:', {
                stockId: medicineId,
                requestedQuantity: quantity,
                existingOrders: this.tempOrders
            });
            console.log('Stock Validation Response:', validation);

            if (!validation.isValid) {
                const errorMessage = validation.validationMessage || validation.error || 'Insufficient stock';
                $('#error_SQuantitycheck').text(errorMessage).show();
                return false;
            } else {
                $('#error_SQuantitycheck').hide();
                return true;
            }
        } catch (error) {
            console.error('Stock validation failed:', error);
            $('#error_SQuantitycheck').text('Unable to validate stock').show();
            return false;
        }
    }

    async addItemToList() {
        if (!this.validateItemForm()) return;
        if (!await this.validateCurrentItem()) return;

        const medicineId = parseInt($('#SMedicine').val());
        const medicineName = $('#SMedicine option:selected').text();
        const price = this.currentItemPrice;
        const quantity = parseInt($('#SQuantity').val());
        const discountPercent = parseFloat($('#SDiscountPercent').val()) || 0;

        try {
            // Calculate item total via API
            const itemCalculation = await window.Pharma263Calculations.calculateDiscount(
                price * quantity,
                discountPercent,
                0,
                'percentage'
            );

            // Add to temp orders
            this.tempOrders.push({
                id: medicineId,           // Keep for client-side filtering
                stockId: medicineId,      // API expects 'stockId' property
                quantity: quantity,
                ref: Date.now()
            });

            // Add row to table
            const rowHtml = `
                <tr data-medicine-id="${medicineId}">
                    <td>${medicineId}</td>
                    <td>${medicineName}</td>
                    <td data-price>${price.toFixed(2)}</td>
                    <td>
                        <input type="number" value="${discountPercent.toFixed(2)}" 
                               class="discount-percent-input form-control" 
                               style="width: 80px;" min="0" max="100" step="0.01" />
                    </td>
                    <td data-quantity>${quantity}</td>
                    <td class="amount" data-total>${itemCalculation.finalTotal.toFixed(2)}</td>
                    <td>
                        <button type="button" class="btn btn-sm btn-danger sdeleteItem" data-itemid="${medicineId}">
                            <i class="fa fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `;

            $('#detailsTable tbody').append(rowHtml);

            // Clear form
            this.clearItemForm();

            // Recalculate totals
            await this.recalculateTotal();

        } catch (error) {
            console.error('Failed to add item:', error);
            if (window.Pharma263) {
                window.Pharma263.showToast('Failed to add item', 'error');
            }
        }
    }

    async updateItemCalculation($row) {
        const price = parseFloat($row.find('[data-price]').text());
        const quantity = parseInt($row.find('[data-quantity]').text());
        const discountPercent = parseFloat($row.find('.discount-percent-input').val()) || 0;

        try {
            const itemCalculation = await window.Pharma263Calculations.calculateDiscount(
                price * quantity,
                discountPercent,
                0,
                'percentage'
            );

            $row.find('[data-total]').text(itemCalculation.finalTotal.toFixed(2));
            await this.recalculateTotal();

        } catch (error) {
            console.error('Item calculation failed:', error);
        }
    }

    async recalculateTotal() {
        const items = this.getTableItems();
        
        if (items.length === 0) {
            $('#SSubTotal').text('0.00');
            $('#SGrandTotal').text('0.00');
            return;
        }

        try {
            // Calculate totals via API
            const totals = await window.Pharma263Calculations.calculateSalesTotal(items);
            
            // Calculate taxes if customer is selected
            let taxes = { totalTax: 0 };
            if (this.currentCustomerId) {
                taxes = await window.Pharma263Calculations.calculateTaxes(
                    items, 
                    this.currentCustomerId, 
                    'sale'
                );
            }

            // Update display
            $('#SSubTotal').text(totals.subtotal.toFixed(2));
            $('#SGrandTotal').text((totals.grandTotal + taxes.totalTax).toFixed(2));

            // Update tax display if element exists
            if ($('#STax').length) {
                $('#STax').text(taxes.totalTax.toFixed(2));
            }

        } catch (error) {
            console.error('Total calculation failed:', error);
        }
    }

    getTableItems() {
        const items = [];
        $('#detailsTable tbody tr').each(function() {
            const $row = $(this);
            const medicineId = parseInt($row.data('medicine-id'));
            const price = parseFloat($row.find('[data-price]').text());
            const quantity = parseInt($row.find('[data-quantity]').text());
            const discountPercent = parseFloat($row.find('.discount-percent-input').val()) || 0;

            items.push({
                medicineId,
                price,
                quantity,
                discountPercent,
                discountType: 'percentage'
            });
        });
        return items;
    }

    removeItemFromList($row) {
        const medicineId = parseInt($row.data('medicine-id'));
        
        // Remove from temp orders
        this.tempOrders = this.tempOrders.filter(order => order.id !== medicineId);
        
        // Remove row with animation
        $row.fadeOut(300, () => {
            $row.remove();
            this.recalculateTotal();
            this.loadMedicineDetails($('#SMedicine').val());
        });
    }

    validateItemForm() {
        const medicine = $('#SMedicine').val();
        const quantity = $('#SQuantity').val();
        
        let isValid = true;

        if (!medicine) {
            $('#error_SMedicine').show();
            isValid = false;
        } else {
            $('#error_SMedicine').hide();
        }

        if (!quantity || quantity <= 0) {
            $('#error_SQuantity').show();
            isValid = false;
        } else {
            $('#error_SQuantity').hide();
        }

        return isValid;
    }

    clearItemForm() {
        $('#SQuantity').val('');
        $('#SDiscountPercent').val('');
        $('#SMedicine').val('').trigger('change');
        $('#medicineDetails').html('');
    }

    async saveSale() {
        if (!this.validateSaleForm()) return;

        const items = this.getTableItems();
        if (items.length === 0) {
            if (window.Pharma263) {
                window.Pharma263.showToast('Please add at least one item', 'warning');
            }
            return;
        }

        try {
            // Get final calculations from API
            const totals = await window.Pharma263Calculations.calculateSalesTotal(items);
            
            const saleData = {
                customerId: parseInt($('#Customer').val()),
                salesDate: this.formatDate($('#SDate').val()),
                paymentMethodId: parseInt($('#PaymentMethod').val()),
                saleStatusId: parseInt($('#SaleStatus').val()),
                total: totals.subtotal,
                discount: totals.totalDiscount,
                grandTotal: totals.grandTotal,
                notes: $('#SNotes').val() || '',
                items: totals.items.map(item => ({
                    stockId: item.medicineId,
                    medicineName: $('#detailsTable tbody tr').filter(`[data-medicine-id="${item.medicineId}"]`).find('td:eq(1)').text(),
                    price: item.price,
                    quantity: item.quantity,
                    discount: item.discountAmount,
                    amount: item.total
                }))
            };

            // Submit via our modern form handler
            if (window.Pharma263) {
                const response = await window.Pharma263.submitForm(this.$form[0], {
                    url: '/Sale/AddSale',
                    method: 'POST',
                    data: saleData,
                    showSuccess: true,
                    redirect: '/Sale/Index'
                });
                
                console.log('Sale saved successfully:', response);
            }

        } catch (error) {
            console.error('Failed to save sale:', error);
            if (window.Pharma263) {
                window.Pharma263.showToast('Failed to save sale', 'error');
            }
        }
    }

    validateSaleForm() {
        const customer = $('#Customer').val();
        const saleDate = $('#SDate').val();
        const paymentMethod = $('#PaymentMethod').val();
        const saleStatus = $('#SaleStatus').val();
        
        let isValid = true;

        if (!customer) {
            $('#error_Customer').show();
            isValid = false;
        } else {
            $('#error_Customer').hide();
        }

        if (!saleDate) {
            $('#error_SDate').show();
            isValid = false;
        } else {
            $('#error_SDate').hide();
        }

        if (!paymentMethod) {
            $('#error_PaymentMethod').show();
            isValid = false;
        } else {
            $('#error_PaymentMethod').hide();
        }

        if (!saleStatus) {
            $('#error_SaleStatus').show();
            isValid = false;
        } else {
            $('#error_SaleStatus').hide();
        }

        return isValid;
    }

    formatDate(dateString) {
        const parts = dateString.split('/');
        const date = new Date(parts[2], parts[1] - 1, parts[0]);
        return date.toISOString();
    }
}

// Initialize when document is ready
$(document).ready(function() {
    // Only initialize if we're on a sales form page
    if ($('#detailsTable').length && $('#SaleaddToList').length) {
        window.modernSalesForm = new ModernSalesForm('.wraper');
    }
});

// ============================================================================
// Legacy validation functions for backward compatibility with inline handlers
// ============================================================================

/**
 * Validates form field and shows/hides error message
 * @param {string} id - The element ID to validate
 */
function blankme(id) {
    var val = document.getElementById(id).value;
    var error_id = "error_" + id;
    if (val == "" || val === 0.0) {
        document.getElementById(error_id).style.display = "block";
    } else {
        document.getElementById(error_id).style.display = "none";
    }
}

/**
 * Calculates grand total after applying discount
 * Legacy function for backward compatibility
 */
function SDiscountAmount() {
    blankme("SDiscount");
    blankme("SGrandTotal");
    var discount = parseFloat($("#SDiscount").val()) || 0;
    var subtotal = parseFloat($("#SSubTotal").text()) || 0;
    var grandTotal = subtotal - discount;

    if (grandTotal < 0) grandTotal = 0;

    $("#SGrandTotal").text(grandTotal.toFixed(2));
}