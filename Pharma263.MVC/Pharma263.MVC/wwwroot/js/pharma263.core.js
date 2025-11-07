/**
 * Pharma263 Core JavaScript Library
 * Provides core functionality, Ajax forms, validation, and utilities
 * Requires jQuery 3.6+ and jQuery UI
 */

class Pharma263Core {
    constructor() {
        this.config = {
            apiBaseUrl: '',
            defaultTimeout: 30000,
            validationDelay: 500,
            toastDuration: 3000
        };
        this.validators = new Map();
        this.activeRequests = new Map();
        this.init();
    }

    /**
     * Initialize core functionality
     */
    init() {
        this.setupAjaxDefaults();
        this.setupGlobalErrorHandlers();
        this.setupDatePickers();
        this.setupDataTables();
        this.setupFormValidation();
        console.log('Pharma263 Core initialized');
    }

    /**
     * Setup jQuery AJAX defaults
     */
    setupAjaxDefaults() {
        $.ajaxSetup({
            timeout: this.config.defaultTimeout,
            beforeSend: (xhr, settings) => {
                // Add anti-forgery token to all AJAX requests
                const token = $('input[name="__RequestVerificationToken"]').val();
                if (token && (settings.type === 'POST' || settings.type === 'PUT' || settings.type === 'DELETE')) {
                    xhr.setRequestHeader('RequestVerificationToken', token);
                }
                
                // Only show global loading for non-form requests
                // Form requests handle their own loading states
                if (!settings.formSubmission) {
                    this.showGlobalLoading();
                }
            },
            complete: (xhr, settings) => {
                if (!settings.formSubmission) {
                    this.hideGlobalLoading();
                }
            },
            error: (xhr, status, error) => {
                this.handleAjaxError(xhr, status, error);
            }
        });
    }

    /**
     * Setup global error handlers
     */
    setupGlobalErrorHandlers() {
        // Global AJAX error handler for authentication
        $(document).ajaxError((event, xhr, settings) => {
            if (xhr.status === 401 || xhr.status === 403) {
                this.showToast('Your session has expired. Please login again.', 'warning');
                setTimeout(() => {
                    window.location.href = '/Auth/Login';
                }, 2000);
            }
        });

        // Global error handler for uncaught exceptions
        window.addEventListener('error', (event) => {
            console.error('Global error:', event.error);
            this.showToast('An unexpected error occurred. Please refresh the page.', 'error');
        });
    }

    /**
     * Setup date pickers with consistent formatting
     */
    setupDatePickers() {
        if ($.datepicker) {
            $.datepicker.setDefaults({
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                yearRange: "-50:+10",
                showButtonPanel: true,
                beforeShow: (input, inst) => {
                    setTimeout(() => {
                        inst.dpDiv.css({
                            position: "absolute",
                            top: $(input).offset().top + $(input).outerHeight(),
                            left: $(input).offset().left
                        });
                    }, 0);
                }
            });

            // Auto-initialize date pickers
            $(document).on('focus', '.date-picker, .mydatepicker', function() {
                if (!$(this).hasClass('hasDatepicker')) {
                    $(this).datepicker({
                        dateFormat: "dd/mm/yy",
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-50:+10"
                    });
                }
            });
        }
    }

    /**
     * Setup DataTables with consistent configuration
     */
    setupDataTables() {
        if ($.fn.DataTable) {
            // Initialize DataTables when they become visible
            $(document).on('shown.bs.tab shown.bs.modal', () => {
                $('table[data-table]').each((index, table) => {
                    if (!$.fn.DataTable.isDataTable(table)) {
                        this.initializeDataTable($(table));
                    }
                });
            });

            // Auto-initialize visible tables
            setTimeout(() => {
                $('table[data-table]:visible, #datatable:visible').each((index, table) => {
                    if (!$.fn.DataTable.isDataTable(table)) {
                        this.initializeDataTable($(table));
                    }
                });
            }, 100);
        }
    }

    /**
     * Initialize a DataTable with standard configuration
     */
    initializeDataTable($table) {
        const config = {
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            "responsive": true,
            "pageLength": 25,
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            "dom": '<"row"<"col-sm-6"l><"col-sm-6"f>>' +
                   '<"row"<"col-sm-12"tr>>' +
                   '<"row"<"col-sm-5"i><"col-sm-7"p>>',
            "language": {
                "searchPlaceholder": "Search anything...",
                "search": "",
                "lengthMenu": "Show _MENU_ entries",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "paginate": {
                    "first": "First",
                    "last": "Last",
                    "next": "Next",
                    "previous": "Previous"
                }
            }
        };

        // Merge with any data attributes
        const tableConfig = $table.data('table-config');
        if (tableConfig) {
            Object.assign(config, tableConfig);
        }

        return $table.DataTable(config);
    }

    /**
     * Setup form validation
     */
    setupFormValidation() {
        // Initialize our enhanced validation system
        if (window.Pharma263Forms && typeof Pharma263Forms.initFormValidation === 'function') {
            // Auto-initialize validation for forms with data-validate attribute
            $('form[data-validate="true"]').each(function() {
                Pharma263Forms.initFormValidation($(this));
            });
        }

        // Setup jQuery validation as fallback
        if ($.validator) {
            // Extend jQuery validator with custom rules
            $.validator.addMethod('phone', function(value, element) {
                return this.optional(element) || /^[\+]?[0-9][\d]{6,15}$/.test(value);
            }, 'Please enter a valid phone number');

            $.validator.addMethod('vat', function(value, element) {
                return this.optional(element) || /^[A-Z0-9]+$/.test(value);
            }, 'Please enter a valid VAT number');

            $.validator.addMethod('mcaz', function(value, element) {
                return this.optional(element) || /^[A-Z0-9\-\/]+$/.test(value);
            }, 'Please enter a valid MCAZ license number');

            // Set default validation options
            $.validator.setDefaults({
                errorClass: 'is-invalid',
                validClass: 'is-valid',
                errorElement: 'div',
                errorPlacement: function(error, element) {
                    error.addClass('invalid-feedback');

                    // Add role="alert" for screen readers (WCAG 4.1.3)
                    error.attr('role', 'alert');

                    // Create unique error ID for aria-describedby
                    const elementId = element.attr('id') || element.attr('name');
                    if (elementId) {
                        const errorId = 'error-' + elementId;
                        error.attr('id', errorId);
                    }

                    if (element.parent('.input-group').length) {
                        error.insertAfter(element.parent());
                    } else {
                        error.insertAfter(element);
                    }
                },
                highlight: function(element) {
                    const $element = $(element);
                    $element.removeClass('is-valid').addClass('is-invalid');

                    // Add ARIA invalid state (WCAG 3.3.1)
                    $element.attr('aria-invalid', 'true');

                    // Link to error message with aria-describedby
                    const elementId = $element.attr('id') || $element.attr('name');
                    if (elementId) {
                        const errorId = 'error-' + elementId;
                        $element.attr('aria-describedby', errorId);
                    }
                },
                unhighlight: function(element) {
                    const $element = $(element);
                    $element.removeClass('is-invalid').addClass('is-valid');

                    // Remove ARIA invalid state (WCAG 3.3.1)
                    $element.attr('aria-invalid', 'false');

                    // Remove aria-describedby when valid
                    $element.removeAttr('aria-describedby');
                },
                success: function(label, element) {
                    const $element = $(element);
                    $element.removeClass('is-invalid').addClass('is-valid');

                    // Remove ARIA invalid state (WCAG 3.3.1)
                    $element.attr('aria-invalid', 'false');
                    $element.removeAttr('aria-describedby');

                    $(label).remove();
                }
            });
        }
    }

    /**
     * Submit form via AJAX
     */
    submitForm(form, options = {}) {
        const $form = $(form);
        const formId = $form.attr('id') || 'form-' + Date.now();
        
        // Cancel any existing request for this form
        if (this.activeRequests.has(formId)) {
            this.activeRequests.get(formId).abort();
        }

        const settings = {
            url: $form.attr('action'),
            method: $form.attr('method') || 'POST',
            showLoading: true,
            showSuccess: true,
            resetForm: false,
            redirect: null,
            onSuccess: null,
            onError: null,
            ...options
        };

        // Validate form if validation is enabled
        if ($form.data('validate') !== false) {
            // Use our enhanced validation system if available
            if (window.Pharma263Forms && typeof Pharma263Forms.validateForm === 'function') {
                if (!Pharma263Forms.validateForm($form)) {
                    return Promise.reject('Validation failed');
                }
            }
            // Fallback to jQuery validation if available
            else if ($.fn.validate && !$form.valid()) {
                return Promise.reject('Validation failed');
            }
        }

        const formData = new FormData($form[0]);

        // Show loading state
        if (settings.showLoading) {
            this.showFormLoading($form);
        }

        const xhr = $.ajax({
            url: settings.url,
            method: settings.method,
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            formSubmission: true, // Flag to prevent global loading
            statusCode: {
                400: function(response) {
                    // Handle validation errors specifically
                    return response;
                }
            }
        });

        this.activeRequests.set(formId, xhr);

        return xhr.then(
            (response, textStatus, jqXHR) => {
                this.activeRequests.delete(formId);
                this.hideFormLoading($form);

                // Check if this is a successful submission (only 2xx status codes)
                const isSuccess = jqXHR.status >= 200 && jqXHR.status < 300;
                const isRedirect = jqXHR.getResponseHeader('Location') || settings.redirect;
                
                // Check for validation errors in JSON response (like .NET Core validation)
                // Note: jQuery sometimes treats 400 responses as "success" if they return valid JSON
                if (jqXHR.status === 400 || (response && typeof response === 'object' && (response.errors || response.status === 400 || response.title === "One or more validation errors occurred."))) {
                    // This is a validation error response from API
                    let errorMessage = response.title || 'Validation errors occurred';
                    
                    if (response.errors) {
                        this.showValidationErrors($form, response.errors);
                    }
                    
                    this.showToast(errorMessage, 'error');
                    
                    if (settings.onError) {
                        settings.onError({ responseJSON: response }, $form);
                    }
                    
                    return Promise.reject(response);
                }
                
                // For successful JSON responses (like from API)
                if (isSuccess && response && typeof response === 'object' && response.success !== false) {
                    let successMessage = response.message || 'Operation completed successfully';
                    
                    if (settings.showSuccess) {
                        this.showToast(successMessage, 'success');
                    }
                } 
                // For successful HTML responses (like redirect or success page)
                else if (isSuccess && typeof response === 'string') {
                    // Extract success message from TempData or HTML
                    let successMessage = 'Operation completed successfully';
                    
                    // Try to extract TempData success message
                    const tempDataMatch = response.match(/TempData\["success"\]\s*=\s*"([^"]+)"/);
                    if (tempDataMatch) {
                        successMessage = tempDataMatch[1];
                    }
                    
                    // Check if this looks like an error response (has validation errors or error content)
                    const hasValidationErrors = response.includes('field-validation-error') || 
                                              response.includes('validation-summary-errors') ||
                                              response.includes('alert-danger') ||
                                              response.includes('TempData["error"]');
                    
                    if (hasValidationErrors) {
                        // This is actually an error response, handle it as such
                        return this.handleValidationErrorResponse(response, $form, settings);
                    }
                    
                    if (settings.showSuccess) {
                        this.showToast(successMessage, 'success');
                    }
                }
                // If we get here with a non-2xx status, treat as error
                else if (!isSuccess) {
                    return Promise.reject({ 
                        status: jqXHR.status, 
                        responseText: typeof response === 'string' ? response : JSON.stringify(response),
                        responseJSON: typeof response === 'object' ? response : null
                    });
                }

                // Common success actions (only for actual success cases)
                if (isSuccess) {
                    if (settings.resetForm) {
                        $form[0].reset();
                        $form.find('.is-valid, .is-invalid').removeClass('is-valid is-invalid');
                    }

                    if (settings.redirect || isRedirect) {
                        const redirectUrl = settings.redirect || jqXHR.getResponseHeader('Location');
                        if (redirectUrl) {
                            setTimeout(() => {
                                window.location.href = redirectUrl;
                            }, 1000);
                        }
                    }

                    if (settings.onSuccess) {
                        settings.onSuccess(response, $form);
                    }
                }

                return response;
            },
            (xhr) => {
                this.activeRequests.delete(formId);
                this.hideFormLoading($form);

                // Handle different types of error responses
                if (xhr.status === 400 && xhr.responseText) {
                    // This might be a validation error with HTML content
                    return this.handleValidationErrorResponse(xhr.responseText, $form, settings);
                }

                let errorMessage = 'An error occurred while processing your request.';
                
                if (xhr.responseJSON) {
                    errorMessage = xhr.responseJSON.message || xhr.responseJSON.title || errorMessage;
                    
                    // Handle validation errors
                    if (xhr.responseJSON.errors) {
                        this.showValidationErrors($form, xhr.responseJSON.errors);
                    }
                } else if (xhr.responseText && !xhr.responseText.includes('<!DOCTYPE')) {
                    errorMessage = xhr.responseText;
                }

                this.showToast(errorMessage, 'error');

                if (settings.onError) {
                    settings.onError(xhr, $form);
                }

                return Promise.reject(xhr);
            }
        );
    }

    /**
     * Handle validation error response (HTML with form errors)
     */
    handleValidationErrorResponse(responseHtml, $form, settings) {
        try {
            // Parse the HTML response to extract validation errors
            const $responseDoc = $(responseHtml);
            
            // Extract TempData error message
            let errorMessage = null;
            const errorMatch = responseHtml.match(/TempData\["error"\]\s*=\s*"([^"]+)"/);
            if (errorMatch) {
                errorMessage = errorMatch[1];
            }
            
            // Look for validation summary errors
            const $validationSummary = $responseDoc.find('.validation-summary-errors, .alert-danger');
            if ($validationSummary.length && !errorMessage) {
                errorMessage = $validationSummary.text().trim();
            }
            
            // Look for field-specific validation errors
            const validationErrors = {};
            $responseDoc.find('.field-validation-error, .invalid-feedback').each((index, element) => {
                const $error = $(element);
                const $field = $error.prev('input, select, textarea').first();
                if ($field.length) {
                    const fieldName = $field.attr('name');
                    if (fieldName) {
                        validationErrors[fieldName] = [$error.text().trim()];
                    }
                }
            });
            
            // Also check for validation attributes on inputs in the response
            $responseDoc.find('input[data-val-required], input[data-val-regex]').each((index, element) => {
                const $input = $(element);
                const fieldName = $input.attr('name');
                const $correspondingInput = $form.find(`[name="${fieldName}"]`);
                
                if ($correspondingInput.length) {
                    // Check if this field has validation errors in response
                    const $nextError = $input.next('.field-validation-error, .invalid-feedback');
                    if ($nextError.length && $nextError.text().trim()) {
                        validationErrors[fieldName] = [$nextError.text().trim()];
                    }
                }
            });
            
            // Show validation errors on the form
            if (Object.keys(validationErrors).length > 0) {
                this.showValidationErrors($form, validationErrors);
            }
            
            // Show general error message
            if (errorMessage) {
                this.showToast(errorMessage, 'error');
            } else if (Object.keys(validationErrors).length === 0) {
                this.showToast('Please correct the errors and try again.', 'error');
            }
            
            // Call error callback if provided
            if (settings.onError) {
                settings.onError({ responseText: responseHtml, validationErrors }, $form);
            }
            
        } catch (error) {
            console.error('Error parsing validation response:', error);
            this.showToast('An error occurred while processing the form.', 'error');
        }
        
        return Promise.reject('Validation failed');
    }

    /**
     * Show validation errors on form
     */
    showValidationErrors($form, errors) {
        // Clear existing validation states
        $form.find('.is-invalid').removeClass('is-invalid');
        $form.find('.invalid-feedback').remove();

        // Show field-specific errors
        Object.keys(errors).forEach(field => {
            const $field = $form.find(`[name="${field}"]`);
            if ($field.length) {
                $field.addClass('is-invalid');
                const errorDiv = $('<div class="invalid-feedback"></div>').text(errors[field][0]);
                
                if ($field.parent('.input-group').length) {
                    errorDiv.insertAfter($field.parent());
                } else {
                    errorDiv.insertAfter($field);
                }
            }
        });
    }

    /**
     * Show form loading state
     */
    showFormLoading($form) {
        const $submitBtn = $form.find('button[type="submit"], input[type="submit"]');
        $submitBtn.prop('disabled', true);
        
        // Store original text and show loading
        $submitBtn.each(function() {
            const $btn = $(this);
            if (!$btn.data('original-text')) {
                $btn.data('original-text', $btn.html());
                $btn.html('<i class="fas fa-spinner fa-spin"></i> Processing...');
            }
        });

        // Add subtle loading overlay to form instead of global loader
        if (!$form.find('.form-loading-overlay').length) {
            $form.css('position', 'relative').append(`
                <div class="form-loading-overlay" style="
                    position: absolute; 
                    top: 0; 
                    left: 0; 
                    right: 0; 
                    bottom: 0; 
                    background: rgba(248, 249, 250, 0.8); 
                    z-index: 10; 
                    pointer-events: none;
                    border-radius: 4px;
                "></div>
            `);
        }

        $form.addClass('form-loading');
    }

    /**
     * Hide form loading state
     */
    hideFormLoading($form) {
        const $submitBtn = $form.find('button[type="submit"], input[type="submit"]');
        $submitBtn.prop('disabled', false);
        
        // Restore original text
        $submitBtn.each(function() {
            const $btn = $(this);
            const originalText = $btn.data('original-text');
            if (originalText) {
                $btn.html(originalText);
                $btn.removeData('original-text');
            }
        });

        // Remove loading overlay
        $form.find('.form-loading-overlay').remove();
        $form.removeClass('form-loading');
    }

    /**
     * Show global loading indicator
     */
    showGlobalLoading() {
        if (!$('#global-loading').length) {
            $('body').append(`
                <div id="global-loading" class="global-loading" style="display: none;">
                    <div class="loading-backdrop"></div>
                    <div class="loading-content">
                        <i class="fas fa-spinner fa-spin fa-2x"></i>
                        <div class="loading-text">Loading...</div>
                    </div>
                </div>
            `);
        }
        $('#global-loading').fadeIn(200);
    }

    /**
     * Hide global loading indicator
     */
    hideGlobalLoading() {
        $('#global-loading').fadeOut(200);
    }

    /**
     * Show toast notification
     */
    showToast(message, type = 'info', options = {}) {
        if (typeof toastr !== 'undefined') {
            const toastrOptions = {
                timeOut: this.config.toastDuration,
                closeButton: true,
                progressBar: true,
                positionClass: 'toast-top-right',
                ...options
            };

            toastr.options = toastrOptions;
            toastr[type](message);
        } else {
            // Fallback to browser alert if toastr is not available
            alert(`${type.toUpperCase()}: ${message}`);
        }

        // Announce to screen readers (WCAG 4.1.3 Status Messages)
        this.announceToScreenReader(message);
    }

    /**
     * Announce message to screen readers via live region
     */
    announceToScreenReader(message) {
        const announcer = document.getElementById('screen-reader-announcements');
        if (announcer) {
            // Clear previous message
            announcer.textContent = '';
            // Set new message (triggers screen reader announcement)
            setTimeout(() => {
                announcer.textContent = message;
            }, 100);
        }
    }

    /**
     * Handle AJAX errors
     */
    handleAjaxError(xhr, status, error) {
        if (xhr.status === 0 && status === 'abort') {
            return; // Request was aborted, ignore
        }

        let message = 'An error occurred while processing your request.';
        
        if (xhr.responseJSON && xhr.responseJSON.message) {
            message = xhr.responseJSON.message;
        } else if (xhr.status === 404) {
            message = 'The requested resource was not found.';
        } else if (xhr.status === 500) {
            message = 'A server error occurred. Please try again later.';
        } else if (status === 'timeout') {
            message = 'The request timed out. Please try again.';
        }

        this.showToast(message, 'error');
        console.error('AJAX Error:', { xhr, status, error });
    }

    /**
     * Auto-initialize forms with AJAX submission
     */
    initAjaxForms() {
        $(document).on('submit', 'form[data-ajax="true"]', (e) => {
            e.preventDefault();
            const $form = $(e.target);
            const options = $form.data('ajax-options') || {};
            this.submitForm($form, options);
        });
    }

    /**
     * Utility: Debounce function
     */
    debounce(func, wait, immediate) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                timeout = null;
                if (!immediate) func.apply(this, args);
            };
            const callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) func.apply(this, args);
        };
    }

    /**
     * Utility: Format currency
     */
    formatCurrency(amount, currency = '$') {
        return currency + parseFloat(amount).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    }

    /**
     * Utility: Validate form fields in real-time
     */
    enableRealtimeValidation($form) {
        if (!$.fn.validate) return;

        const validator = $form.validate();
        
        $form.find('input, select, textarea').on('blur change', this.debounce(function() {
            validator.element(this);
        }, this.config.validationDelay));
    }
}

// Initialize Pharma263 Core when document is ready
$(document).ready(function() {
    window.Pharma263 = new Pharma263Core();
    window.Pharma263.initAjaxForms();
});

// CSS for loading states and notifications
const coreStyles = `
<style>
.global-loading {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 9999;
}

.loading-backdrop {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.3);
}

.loading-content {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background: white;
    padding: 30px;
    border-radius: 8px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
    text-align: center;
    min-width: 200px;
}

.loading-text {
    margin-top: 15px;
    font-size: 14px;
    color: #666;
}

.form-loading {
    position: relative;
    pointer-events: none;
    opacity: 0.7;
}

.is-invalid {
    border-color: #dc3545 !important;
    box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25) !important;
}

.is-valid {
    border-color: #28a745 !important;
    box-shadow: 0 0 0 0.2rem rgba(40, 167, 69, 0.25) !important;
}

.invalid-feedback {
    display: block;
    width: 100%;
    margin-top: 0.25rem;
    font-size: 0.875em;
    color: #dc3545;
}
</style>
`;

// Inject styles
$('head').append(coreStyles);