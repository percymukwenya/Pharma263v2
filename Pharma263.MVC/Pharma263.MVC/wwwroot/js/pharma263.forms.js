/**
 * Pharma263 Form Components Library
 * Provides reusable form components and widgets
 */

class Pharma263Forms {
    /**
     * Initialize enhanced form validation
     */
    static initFormValidation($form) {
        // Add real-time validation
        $form.find('input[required], select[required], textarea[required]').each(function() {
            const $field = $(this);
            
            // Add real-time validation on blur and input
            $field.on('blur input change', function() {
                Pharma263Forms.validateField($field);
            });
        });

        // Enhanced validation for specific field types
        $form.find('input[type="email"]').on('blur', function() {
            Pharma263Forms.validateEmail($(this));
        });

        $form.find('input[type="number"]').on('input', function() {
            Pharma263Forms.validateNumber($(this));
        });

        // Form submission validation
        $form.on('submit', function(e) {
            if (!Pharma263Forms.validateForm($form)) {
                e.preventDefault();
                return false;
            }
        });

        // Apply pharmaceutical-specific validation
        Pharma263Forms.applyPharmaceuticalValidation($form);
    }

    /**
     * Validate individual field
     */
    static validateField($field) {
        const value = $field.val().trim();
        const isRequired = $field.prop('required');
        const fieldName = $field.attr('name') || $field.attr('id') || 'Field';
        
        // Clear previous validation state
        $field.removeClass('is-valid is-invalid');
        $field.siblings('.invalid-feedback').remove();

        if (isRequired && !value) {
            Pharma263Forms.showFieldError($field, `${fieldName} is required`);
            return false;
        }

        if (value && !Pharma263Forms.validateFieldType($field, value)) {
            return false;
        }

        // Show valid state
        $field.addClass('is-valid');
        return true;
    }

    /**
     * Validate field based on type
     */
    static validateFieldType($field, value) {
        const fieldType = $field.attr('type') || 'text';
        const min = $field.attr('min');
        const max = $field.attr('max');
        const pattern = $field.attr('pattern');

        switch (fieldType) {
            case 'email':
                return Pharma263Forms.validateEmail($field, value);
            case 'number':
                return Pharma263Forms.validateNumber($field, value, min, max);
            case 'tel':
                return Pharma263Forms.validatePhone($field, value);
            default:
                if (pattern) {
                    return Pharma263Forms.validatePattern($field, value, pattern);
                }
                return true;
        }
    }

    /**
     * Validate email field
     */
    static validateEmail($field, value = null) {
        if (value === null) value = $field.val().trim();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (value && !emailRegex.test(value)) {
            Pharma263Forms.showFieldError($field, 'Please enter a valid email address');
            return false;
        }
        return true;
    }

    /**
     * Validate number field
     */
    static validateNumber($field, value = null, min = null, max = null) {
        if (value === null) value = $field.val();
        if (min === null) min = $field.attr('min');
        if (max === null) max = $field.attr('max');
        
        const num = parseFloat(value);
        
        if (value && isNaN(num)) {
            Pharma263Forms.showFieldError($field, 'Please enter a valid number');
            return false;
        }
        
        if (min !== undefined && num < parseFloat(min)) {
            Pharma263Forms.showFieldError($field, `Value must be at least ${min}`);
            return false;
        }
        
        if (max !== undefined && num > parseFloat(max)) {
            Pharma263Forms.showFieldError($field, `Value must not exceed ${max}`);
            return false;
        }
        
        return true;
    }

    /**
     * Validate phone field
     */
    static validatePhone($field, value = null) {
        if (value === null) value = $field.val().trim();
        // Allow leading zeros, plus sign, and minimum 7 digits
        const phoneRegex = /^[\+]?[0-9][\d]{6,15}$/;
        
        if (value && !phoneRegex.test(value)) {
            Pharma263Forms.showFieldError($field, 'Please enter a valid phone number (7-16 digits)');
            return false;
        }
        return true;
    }

    /**
     * Validate pattern
     */
    static validatePattern($field, value, pattern) {
        const regex = new RegExp(pattern);
        if (!regex.test(value)) {
            const title = $field.attr('title') || 'Please enter a valid value';
            Pharma263Forms.showFieldError($field, title);
            return false;
        }
        return true;
    }

    /**
     * Show field error
     */
    static showFieldError($field, message) {
        $field.removeClass('is-valid').addClass('is-invalid');
        
        // Remove existing error message
        $field.siblings('.invalid-feedback').remove();
        
        // Add error message
        const $error = $(`<div class="invalid-feedback">${message}</div>`);
        $field.after($error);
    }

    /**
     * Validate entire form
     */
    static validateForm($form) {
        let isValid = true;
        
        $form.find('input[required], select[required], textarea[required]').each(function() {
            if (!Pharma263Forms.validateField($(this))) {
                isValid = false;
            }
        });

        // Validate non-required fields that have values
        $form.find('input:not([required]), select:not([required]), textarea:not([required])').each(function() {
            const $field = $(this);
            const value = $field.val();
            if (value && value.trim()) {
                if (!Pharma263Forms.validateFieldType($field, value.trim())) {
                    isValid = false;
                }
            }
        });

        if (!isValid) {
            // Focus on first invalid field
            const $firstInvalid = $form.find('.is-invalid').first();
            if ($firstInvalid.length) {
                $firstInvalid.focus();
                
                // Scroll to field if not visible
                if ($firstInvalid.offset().top < $(window).scrollTop() || 
                    $firstInvalid.offset().top > $(window).scrollTop() + $(window).height()) {
                    $('html, body').animate({
                        scrollTop: $firstInvalid.offset().top - 100
                    }, 300);
                }
            }
            
            if (window.Pharma263) {
                window.Pharma263.showToast('Please correct the errors and try again.', 'error');
            }
        }

        return isValid;
    }

    /**
     * Pharmaceutical-specific validation rules
     */
    static validateVAT($field, value = null) {
        if (value === null) value = $field.val().trim();
        const vatRegex = /^[A-Z0-9]+$/;
        
        if (value && !vatRegex.test(value)) {
            Pharma263Forms.showFieldError($field, 'Please enter a valid VAT number (alphanumeric)');
            return false;
        }
        return true;
    }

    static validateMCAZ($field, value = null) {
        if (value === null) value = $field.val().trim();
        const mcazRegex = /^[A-Z0-9\-\/]+$/;
        
        if (value && !mcazRegex.test(value)) {
            Pharma263Forms.showFieldError($field, 'Please enter a valid MCAZ license number');
            return false;
        }
        return true;
    }

    static validateDosage($field, value = null) {
        if (value === null) value = $field.val().trim();
        const dosageRegex = /^[a-zA-Z0-9\s\.\-]+$/;
        
        if (value && !dosageRegex.test(value)) {
            Pharma263Forms.showFieldError($field, 'Please enter a valid dosage form');
            return false;
        }
        return true;
    }

    /**
     * Auto-apply pharmaceutical validation based on field name or class
     */
    static applyPharmaceuticalValidation($form) {
        $form.find('input[name*="VAT"], input[class*="vat"]').on('blur', function() {
            Pharma263Forms.validateVAT($(this));
        });

        $form.find('input[name*="MCAZ"], input[class*="mcaz"]').on('blur', function() {
            Pharma263Forms.validateMCAZ($(this));
        });

        $form.find('input[name*="Dosage"], input[class*="dosage"]').on('blur', function() {
            Pharma263Forms.validateDosage($(this));
        });
    }

    /**
     * Create a loading overlay for forms
     */
    static createLoadingOverlay() {
        return `
            <div class="form-loading-overlay" style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; 
                 background: rgba(255,255,255,0.8); z-index: 1000; display: none;">
                <div style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); text-align: center;">
                    <i class="fas fa-spinner fa-spin fa-2x text-primary"></i>
                    <div style="margin-top: 10px; font-weight: bold;">Processing...</div>
                </div>
            </div>
        `;
    }

    /**
     * Initialize file upload with progress
     */
    static initFileUpload($input, options = {}) {
        const settings = {
            allowedTypes: ['jpg', 'jpeg', 'png', 'pdf', 'doc', 'docx'],
            maxSize: 5 * 1024 * 1024, // 5MB
            showPreview: true,
            onUpload: null,
            ...options
        };

        $input.on('change', function() {
            const file = this.files[0];
            if (!file) return;

            // Validate file type
            const fileExtension = file.name.split('.').pop().toLowerCase();
            if (!settings.allowedTypes.includes(fileExtension)) {
                if (window.Pharma263) {
                    window.Pharma263.showToast(
                        `File type not allowed. Allowed types: ${settings.allowedTypes.join(', ')}`,
                        'error'
                    );
                }
                $(this).val('');
                return;
            }

            // Validate file size
            if (file.size > settings.maxSize) {
                if (window.Pharma263) {
                    window.Pharma263.showToast(
                        `File size too large. Maximum size: ${(settings.maxSize / 1024 / 1024).toFixed(1)}MB`,
                        'error'
                    );
                }
                $(this).val('');
                return;
            }

            // Show preview for images
            if (settings.showPreview && ['jpg', 'jpeg', 'png'].includes(fileExtension)) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    const preview = $input.siblings('.file-preview');
                    if (preview.length === 0) {
                        $input.after('<div class="file-preview mt-2"></div>');
                    }
                    $input.siblings('.file-preview').html(
                        `<img src="${e.target.result}" class="img-thumbnail" style="max-width: 200px; max-height: 200px;">`
                    );
                };
                reader.readAsDataURL(file);
            }

            if (settings.onUpload) {
                settings.onUpload(file);
            }
        });
    }

    /**
     * Create a dynamic table with add/remove rows
     */
    static createDynamicTable(containerId, config) {
        const container = $(`#${containerId}`);
        const settings = {
            columns: [],
            addButtonText: 'Add Row',
            removeButtonText: 'Remove',
            minRows: 1,
            maxRows: 10,
            onRowAdd: null,
            onRowRemove: null,
            ...config
        };

        // Create table structure
        const tableHtml = `
            <div class="dynamic-table-container">
                <div class="table-responsive">
                    <table class="table table-bordered dynamic-table">
                        <thead>
                            <tr>
                                ${settings.columns.map(col => `<th>${col.title}</th>`).join('')}
                                <th width="80">Actions</th>
                            </tr>
                        </thead>
                        <tbody class="dynamic-table-body">
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="${settings.columns.length + 1}">
                                    <button type="button" class="btn btn-sm btn-success add-row-btn">
                                        <i class="fas fa-plus"></i> ${settings.addButtonText}
                                    </button>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        `;

        container.html(tableHtml);

        // Add row functionality
        container.on('click', '.add-row-btn', function() {
            const tbody = container.find('.dynamic-table-body');
            if (tbody.children().length >= settings.maxRows) {
                if (window.Pharma263) {
                    window.Pharma263.showToast(`Maximum ${settings.maxRows} rows allowed`, 'warning');
                }
                return;
            }

            const rowIndex = tbody.children().length;
            const rowHtml = `
                <tr data-row-index="${rowIndex}">
                    ${settings.columns.map(col => {
                        let inputHtml = '';
                        switch (col.type) {
                            case 'text':
                                inputHtml = `<input type="text" name="${col.name}[${rowIndex}]" class="form-control" placeholder="${col.placeholder || ''}" ${col.required ? 'required' : ''}>`;
                                break;
                            case 'number':
                                inputHtml = `<input type="number" name="${col.name}[${rowIndex}]" class="form-control" placeholder="${col.placeholder || ''}" ${col.required ? 'required' : ''} ${col.min ? `min="${col.min}"` : ''} ${col.max ? `max="${col.max}"` : ''}>`;
                                break;
                            case 'select':
                                const options = col.options || [];
                                inputHtml = `<select name="${col.name}[${rowIndex}]" class="form-control" ${col.required ? 'required' : ''}>
                                    <option value="">Select...</option>
                                    ${options.map(opt => `<option value="${opt.value}">${opt.text}</option>`).join('')}
                                </select>`;
                                break;
                            case 'date':
                                inputHtml = `<input type="text" name="${col.name}[${rowIndex}]" class="form-control date-picker" placeholder="${col.placeholder || 'dd/mm/yyyy'}" ${col.required ? 'required' : ''}>`;
                                break;
                        }
                        return `<td>${inputHtml}</td>`;
                    }).join('')}
                    <td>
                        <button type="button" class="btn btn-sm btn-danger remove-row-btn">
                            <i class="fas fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `;

            tbody.append(rowHtml);

            // Initialize components in new row
            const newRow = tbody.children().last();
            if (window.Pharma263Utils) {
                window.Pharma263Utils.initFieldComponents(newRow);
            }

            if (settings.onRowAdd) {
                settings.onRowAdd(newRow, rowIndex);
            }
        });

        // Remove row functionality
        container.on('click', '.remove-row-btn', function() {
            const tbody = container.find('.dynamic-table-body');
            if (tbody.children().length <= settings.minRows) {
                if (window.Pharma263) {
                    window.Pharma263.showToast(`Minimum ${settings.minRows} row(s) required`, 'warning');
                }
                return;
            }

            const row = $(this).closest('tr');
            const rowIndex = row.data('row-index');

            if (settings.onRowRemove) {
                settings.onRowRemove(row, rowIndex);
            }

            row.remove();

            // Re-index remaining rows
            container.find('.dynamic-table-body tr').each(function(index) {
                $(this).attr('data-row-index', index);
                $(this).find('input, select, textarea').each(function() {
                    const name = $(this).attr('name');
                    if (name) {
                        const newName = name.replace(/\[\d+\]/, `[${index}]`);
                        $(this).attr('name', newName);
                    }
                });
            });
        });

        // Add initial row if minRows > 0
        if (settings.minRows > 0) {
            for (let i = 0; i < settings.minRows; i++) {
                container.find('.add-row-btn').click();
            }
        }

        return container.find('.dynamic-table');
    }

    /**
     * Create autocomplete search field
     */
    static createAutocomplete($input, options) {
        const settings = {
            source: '',
            minLength: 2,
            delay: 300,
            displayField: 'name',
            valueField: 'id',
            onSelect: null,
            extraFields: [],
            ...options
        };

        if (!$.fn.autocomplete) {
            console.warn('jQuery UI Autocomplete not available');
            return;
        }

        $input.autocomplete({
            minLength: settings.minLength,
            delay: settings.delay,
            source: function(request, response) {
                $.ajax({
                    url: settings.source,
                    data: { term: request.term },
                    success: function(data) {
                        const items = data.map(item => ({
                            label: item[settings.displayField],
                            value: item[settings.displayField],
                            data: item
                        }));
                        response(items);
                    },
                    error: function() {
                        response([]);
                    }
                });
            },
            select: function(event, ui) {
                // Fill hidden field with ID
                const hiddenField = $input.siblings(`input[name="${$input.attr('name')}Id"]`);
                if (hiddenField.length) {
                    hiddenField.val(ui.item.data[settings.valueField]);
                }

                // Fill extra fields
                settings.extraFields.forEach(field => {
                    const target = $(`#${field.target}`);
                    if (target.length && ui.item.data[field.source]) {
                        target.val(ui.item.data[field.source]);
                    }
                });

                if (settings.onSelect) {
                    settings.onSelect(ui.item.data);
                }
            }
        });
    }

    /**
     * Create collapsible form sections
     */
    static createCollapsibleSection(containerId, title, content, options = {}) {
        const settings = {
            collapsed: false,
            icon: 'fas fa-chevron-down',
            ...options
        };

        const sectionHtml = `
            <div class="form-section collapsible-section">
                <div class="section-header" role="button" data-toggle="collapse" data-target="#${containerId}-content">
                    <h4 class="section-title">
                        <i class="${settings.icon} section-icon"></i>
                        ${title}
                    </h4>
                </div>
                <div id="${containerId}-content" class="section-content collapse ${settings.collapsed ? '' : 'show'}">
                    ${content}
                </div>
            </div>
        `;

        $(`#${containerId}`).html(sectionHtml);

        // Handle icon rotation
        $(`#${containerId} .section-header`).on('click', function() {
            const icon = $(this).find('.section-icon');
            setTimeout(() => {
                if ($(`#${containerId}-content`).hasClass('show')) {
                    icon.removeClass('fa-chevron-down').addClass('fa-chevron-up');
                } else {
                    icon.removeClass('fa-chevron-up').addClass('fa-chevron-down');
                }
            }, 100);
        });
    }

    /**
     * Create a confirmation modal
     */
    static createConfirmationModal(options) {
        const settings = {
            title: 'Confirm Action',
            message: 'Are you sure you want to proceed?',
            confirmText: 'Yes, Continue',
            cancelText: 'Cancel',
            confirmClass: 'btn-danger',
            onConfirm: null,
            onCancel: null,
            ...options
        };

        const modalId = 'confirmModal-' + Date.now();
        const modalHtml = `
            <div class="modal fade" id="${modalId}" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">${settings.title}</h4>
                            <button type="button" class="close" data-dismiss="modal">
                                <span>&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <p>${settings.message}</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                ${settings.cancelText}
                            </button>
                            <button type="button" class="btn ${settings.confirmClass} confirm-btn">
                                ${settings.confirmText}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        $('body').append(modalHtml);
        const modal = $(`#${modalId}`);

        modal.find('.confirm-btn').on('click', function() {
            if (settings.onConfirm) {
                settings.onConfirm();
            }
            modal.modal('hide');
        });

        modal.on('hidden.bs.modal', function() {
            if (settings.onCancel) {
                settings.onCancel();
            }
            modal.remove();
        });

        modal.modal('show');
        return modal;
    }

    /**
     * Initialize form wizard
     */
    static createFormWizard(containerId, steps) {
        const container = $(`#${containerId}`);
        
        const wizardHtml = `
            <div class="form-wizard">
                <div class="wizard-steps">
                    ${steps.map((step, index) => `
                        <div class="wizard-step ${index === 0 ? 'active' : ''}" data-step="${index}">
                            <div class="step-number">${index + 1}</div>
                            <div class="step-title">${step.title}</div>
                        </div>
                    `).join('')}
                </div>
                <div class="wizard-content">
                    ${steps.map((step, index) => `
                        <div class="wizard-pane ${index === 0 ? 'active' : ''}" data-pane="${index}">
                            ${step.content}
                        </div>
                    `).join('')}
                </div>
                <div class="wizard-actions">
                    <button type="button" class="btn btn-default wizard-prev" disabled>
                        <i class="fas fa-chevron-left"></i> Previous
                    </button>
                    <button type="button" class="btn btn-primary wizard-next">
                        Next <i class="fas fa-chevron-right"></i>
                    </button>
                    <button type="submit" class="btn btn-success wizard-submit" style="display: none;">
                        <i class="fas fa-check"></i> Submit
                    </button>
                </div>
            </div>
        `;

        container.html(wizardHtml);

        let currentStep = 0;

        // Navigation
        container.on('click', '.wizard-next', function() {
            if (currentStep < steps.length - 1) {
                // Validate current step if validator exists
                const currentPane = container.find(`.wizard-pane[data-pane="${currentStep}"]`);
                const form = currentPane.closest('form');
                
                if (form.length && $.fn.validate) {
                    const validator = form.validate();
                    const valid = validator.element(currentPane.find('input, select, textarea'));
                    if (!valid) return;
                }

                currentStep++;
                updateWizard();
            }
        });

        container.on('click', '.wizard-prev', function() {
            if (currentStep > 0) {
                currentStep--;
                updateWizard();
            }
        });

        function updateWizard() {
            // Update steps
            container.find('.wizard-step').removeClass('active completed');
            container.find('.wizard-step').each(function(index) {
                if (index < currentStep) {
                    $(this).addClass('completed');
                } else if (index === currentStep) {
                    $(this).addClass('active');
                }
            });

            // Update panes
            container.find('.wizard-pane').removeClass('active');
            container.find(`.wizard-pane[data-pane="${currentStep}"]`).addClass('active');

            // Update buttons
            container.find('.wizard-prev').prop('disabled', currentStep === 0);
            
            if (currentStep === steps.length - 1) {
                container.find('.wizard-next').hide();
                container.find('.wizard-submit').show();
            } else {
                container.find('.wizard-next').show();
                container.find('.wizard-submit').hide();
            }
        }

        return {
            goToStep: function(step) {
                if (step >= 0 && step < steps.length) {
                    currentStep = step;
                    updateWizard();
                }
            },
            getCurrentStep: function() {
                return currentStep;
            }
        };
    }
}

// Global access
window.Pharma263Forms = Pharma263Forms;