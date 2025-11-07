# Form Standardization Guide

## Overview
All forms in Pharma263 MVC use a standardized AJAX submission pattern powered by the `Pharma263Core` class. This ensures consistent behavior, error handling, validation, and user feedback across the application.

## Standard Form Pattern

### Basic Form Structure

```html
@using (Html.BeginForm("ActionName", "ControllerName", FormMethod.Post,
    new {
        @class = "ajax-form",
        data_ajax = "true",
        data_validate = "true",
        data_ajax_options = "{\"showSuccess\": true, \"resetForm\": true, \"redirect\": \"/Controller/Action\"}"
    }))
{
    @Html.AntiForgeryToken()

    <div class="form-group">
        @Html.LabelFor(x => x.Name)
        @Html.TextBoxFor(x => x.Name, new { @class = "form-control", @required = "required" })
        @Html.ValidationMessageFor(x => x.Name, "", new { @class = "text-danger" })
    </div>

    <button type="submit" class="btn btn-primary">
        <i class="fas fa-save"></i> Submit
    </button>
}
```

### Key Attributes Explained

#### `class="ajax-form"` (Optional but recommended)
- Semantic class for styling AJAX forms
- Helps identify AJAX forms visually in the code

#### `data-ajax="true"` (Required)
- Enables AJAX submission via `Pharma263Core`
- Form will submit via AJAX instead of traditional postback
- Automatically handled by `$(document).on('submit', 'form[data-ajax="true"]', ...)`

#### `data-validate="true"` (Optional, default: true)
- Enables client-side validation before submission
- Uses `Pharma263Forms.validateForm()` if available
- Falls back to jQuery validation

#### `data-ajax-options` (Optional)
JSON object with submission options:

```javascript
{
    "showSuccess": true,        // Show success toast notification
    "resetForm": true,          // Reset form after successful submission
    "redirect": "/Path/To/Page", // Redirect after success (optional)
    "onSuccess": "functionName", // Custom success callback (optional)
    "onError": "functionName"    // Custom error callback (optional)
}
```

## Form Submission Flow

### 1. User Submits Form
- User clicks submit button or presses Enter

### 2. Event Intercepted
- `Pharma263Core.initAjaxForms()` intercepts the submit event
- Prevents default browser submission

### 3. Validation (if enabled)
- Runs `Pharma263Forms.validateForm($form)` or jQuery validation
- If validation fails, stops submission and shows errors
- If validation passes, proceeds to submission

### 4. Loading State
- Adds `form-loading` class to form
- Disables submit button
- Shows loading indicator
- Form becomes non-interactive (pointer-events: none)

### 5. AJAX Request
- Creates `FormData` object from form
- Sends AJAX request using jQuery.ajax
- Includes anti-forgery token automatically
- Uses form's `action` and `method` attributes

### 6. Response Handling

#### Success Response (2xx status):
- Hides loading state
- Shows success toast (if `showSuccess: true`)
- Resets form (if `resetForm: true`)
- Redirects (if `redirect` specified)
- Calls custom `onSuccess` callback if provided

#### Validation Error (400 status):
- Displays field-specific validation errors
- Shows error toast with message
- Keeps form data intact
- Highlights invalid fields with `.is-invalid` class

#### Authentication Error (401/403 status):
- Shows "Session expired" warning
- Redirects to login after 2 seconds

#### Server Error (500 status):
- Shows error toast
- Calls custom `onError` callback if provided
- Logs error to console

### 7. Form Reset
- Removes loading state
- Re-enables submit button
- Clears validation states
- Ready for next submission

## Examples

### Example 1: Simple Form with Success Message

```html
@using (Html.BeginForm("AddCustomer", "Customer", FormMethod.Post,
    new {
        @class = "ajax-form",
        data_ajax = "true",
        data_ajax_options = "{\"showSuccess\": true}"
    }))
{
    @Html.AntiForgeryToken()

    <div class="form-group">
        @Html.LabelFor(x => x.Name)
        @Html.TextBoxFor(x => x.Name, new { @class = "form-control", @required = "required" })
    </div>

    <button type="submit" class="btn btn-success">Add Customer</button>
}
```

**Controller Action:**
```csharp
[HttpPost]
public IActionResult AddCustomer(CreateCustomerRequest request)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState); // Returns 400 with validation errors

    var result = await _customerService.CreateAsync(request);

    return Ok(new {
        success = true,
        message = "Customer added successfully!",
        data = result
    });
}
```

### Example 2: Form with Reset and Redirect

```html
@using (Html.BeginForm("AddMedicine", "Medicine", FormMethod.Post,
    new {
        @class = "ajax-form",
        data_ajax = "true",
        data_ajax_options = "{\"showSuccess\": true, \"resetForm\": true, \"redirect\": \"/Medicine/Index\"}"
    }))
{
    @Html.AntiForgeryToken()

    <div class="form-group">
        @Html.LabelFor(x => x.Name)
        @Html.TextBoxFor(x => x.Name, new { @class = "form-control" })
    </div>

    <button type="submit" class="btn btn-primary">Save Medicine</button>
}
```

### Example 3: Form with Custom Callbacks

```html
@using (Html.BeginForm("ProcessReturn", "Return", FormMethod.Post,
    new {
        @class = "ajax-form",
        data_ajax = "true",
        data_ajax_options = "{\"showSuccess\": false, \"onSuccess\": \"handleReturnSuccess\", \"onError\": \"handleReturnError\"}"
    }))
{
    @Html.AntiForgeryToken()

    <!-- Form fields here -->

    <button type="submit" class="btn btn-warning">Process Return</button>
}

@section scripts {
    <script>
        function handleReturnSuccess(response, $form) {
            // Custom success handling
            Swal.fire({
                icon: 'success',
                title: 'Return Processed',
                text: 'Return has been processed and stock updated.',
                timer: 3000
            });

            // Reload specific section instead of full page
            $('#returns-list').load('/Return/GetRecentReturns');
        }

        function handleReturnError(xhr, $form) {
            // Custom error handling
            console.error('Return processing failed:', xhr.responseJSON);

            Swal.fire({
                icon: 'error',
                title: 'Processing Failed',
                text: xhr.responseJSON?.message || 'An error occurred'
            });
        }
    </script>
}
```

### Example 4: Complex Form with Validation

```html
@using (Html.BeginForm("AddStock", "Stock", FormMethod.Post,
    new {
        @class = "ajax-form",
        data_ajax = "true",
        data_validate = "true",
        data_ajax_options = "{\"showSuccess\": true, \"resetForm\": true}"
    }))
{
    @Html.AntiForgeryToken()

    <div class="form-group">
        @Html.LabelFor(x => x.MedicineName)
        @Html.TextBoxFor(x => x.MedicineName, new { @class = "form-control", @required = "required" })
        @Html.ValidationMessageFor(x => x.MedicineName, "", new { @class = "text-danger" })
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                @Html.LabelFor(x => x.BuyingPrice)
                @Html.TextBoxFor(x => x.BuyingPrice, new {
                    @class = "form-control",
                    @type = "number",
                    @step = "0.01",
                    @min = "0",
                    @required = "required"
                })
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                @Html.LabelFor(x => x.SellingPrice)
                @Html.TextBoxFor(x => x.SellingPrice, new {
                    @class = "form-control",
                    @type = "number",
                    @step = "0.01",
                    @min = "0",
                    @required = "required"
                })
            </div>
        </div>
    </div>

    <button type="submit" class="btn btn-success">
        <i class="fas fa-plus"></i> Add Stock
    </button>
}
```

## Validation

### Client-Side Validation

Forms with `data-validate="true"` automatically validate before submission:

**HTML5 Validation:**
```html
<input type="text" required />
<input type="email" required />
<input type="number" min="0" max="100" required />
<input type="tel" pattern="[\+]?[0-9]{6,15}" />
```

**Custom Validation Rules:**
```html
<input type="text" data-rule-phone="true" />
<input type="text" data-rule-vat="true" />
<input type="text" data-rule-mcaz="true" />
```

Available custom validators (defined in `pharma263.core.js`):
- `phone`: Validates phone numbers (6-15 digits)
- `vat`: Validates VAT numbers (alphanumeric)
- `mcaz`: Validates MCAZ license numbers

### Server-Side Validation

**Controller Response for Validation Errors:**
```csharp
[HttpPost]
public IActionResult AddCustomer(CreateCustomerRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new {
            title = "Validation failed",
            errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            )
        });
    }

    // Process request...
}
```

The `Pharma263Core.submitForm()` automatically handles this response:
- Parses the `errors` dictionary
- Highlights invalid fields with `.is-invalid` class
- Shows field-specific error messages
- Shows general error toast

## Error Handling

### Automatic Error Handling

All forms automatically handle:

1. **Validation Errors (400)**
   - Field-level errors displayed
   - Toast notification shown
   - Form remains filled
   - Invalid fields highlighted

2. **Authentication Errors (401/403)**
   - Session expired warning
   - Auto-redirect to login
   - 2-second delay for user awareness

3. **Server Errors (500)**
   - Error toast notification
   - Error logged to console
   - Form remains filled for correction

4. **Network Errors**
   - Connection error toast
   - Form re-enabled for retry

### Custom Error Handling

Use `onError` callback for custom handling:

```javascript
function myErrorHandler(xhr, $form) {
    const error = xhr.responseJSON;

    if (error.code === 'DUPLICATE_ENTRY') {
        // Handle specific error
        Swal.fire({
            icon: 'warning',
            title: 'Duplicate Entry',
            text: 'This record already exists.'
        });
    }
}
```

## Loading States

Forms automatically show loading state during submission:

**CSS Classes Applied:**
- `.form-loading` - Applied to form element
  - `pointer-events: none` - Disables interaction
  - `opacity: 0.7` - Visual feedback

**Submit Button:**
- Disabled during submission
- Re-enabled after completion

**Custom Loading Indicators:**
You can add custom loading indicators:

```html
<button type="submit" class="btn btn-primary">
    <span class="button-text">Submit</span>
    <span class="button-spinner" style="display:none;">
        <i class="fas fa-spinner fa-spin"></i>
    </span>
</button>

<style>
.form-loading .button-text { display: none; }
.form-loading .button-spinner { display: inline; }
</style>
```

## Migration from Manual Forms

### Before (Manual fetch):

```javascript
form.addEventListener('submit', function(e) {
    e.preventDefault();

    fetch('/Stock/AddStock', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('Success!');
            form.reset();
        } else {
            alert('Error: ' + data.message);
        }
    })
    .catch(error => {
        alert('An error occurred');
    });
});
```

### After (Standard Pattern):

```html
@using (Html.BeginForm("AddStock", "Stock", FormMethod.Post,
    new {
        @class = "ajax-form",
        data_ajax = "true",
        data_ajax_options = "{\"showSuccess\": true, \"resetForm\": true}"
    }))
{
    @Html.AntiForgeryToken()

    <!-- Form fields -->

    <button type="submit" class="btn btn-success">Submit</button>
}
```

**Benefits:**
- ✅ No JavaScript code needed
- ✅ Automatic validation
- ✅ Consistent error handling
- ✅ Loading states handled
- ✅ Toast notifications
- ✅ Form reset after success
- ✅ Anti-forgery token automatic

## Best Practices

### 1. Always Include Anti-Forgery Token
```html
@Html.AntiForgeryToken()
```

### 2. Use Semantic HTML5 Input Types
```html
<input type="email" />      <!-- Email validation -->
<input type="tel" />        <!-- Phone number -->
<input type="number" />     <!-- Numeric values -->
<input type="date" />       <!-- Date picker -->
```

### 3. Provide Clear Labels
```html
@Html.LabelFor(x => x.Email)  <!-- Accessible label -->
<label for="Email">Email Address</label>
```

### 4. Include Validation Messages
```html
@Html.ValidationMessageFor(x => x.Email, "", new { @class = "text-danger" })
```

### 5. Use Appropriate Button Types
```html
<button type="submit">Submit</button>  <!-- Triggers form submission -->
<button type="button">Cancel</button>  <!-- Does not submit -->
<button type="reset">Clear</button>    <!-- Resets form -->
```

### 6. Group Related Fields
```html
<div class="row">
    <div class="col-md-6">
        <!-- First Name -->
    </div>
    <div class="col-md-6">
        <!-- Last Name -->
    </div>
</div>
```

### 7. Provide Helpful Placeholders
```html
<input type="text" placeholder="e.g., John Doe" />
<input type="email" placeholder="email@example.com" />
```

### 8. Use Consistent Button Styling
```html
<button type="submit" class="btn btn-primary">Save</button>
<a href="/Back" class="btn btn-default">Cancel</a>
```

## Troubleshooting

### Form Not Submitting via AJAX

**Check:**
1. ✓ `data-ajax="true"` attribute present
2. ✓ `pharma263.core.js` loaded
3. ✓ jQuery loaded before Pharma263Core
4. ✓ `Pharma263.initAjaxForms()` called

**Console Error:**
```
Uncaught ReferenceError: $ is not defined
```
**Solution:** Ensure jQuery is loaded before pharma263.core.js

### Validation Not Working

**Check:**
1. ✓ `data-validate="true"` attribute
2. ✓ `pharma263.forms.js` loaded
3. ✓ Fields have validation attributes (`required`, `type`, etc.)

### Success Message Not Showing

**Check:**
1. ✓ `data-ajax-options` includes `"showSuccess": true`
2. ✓ Controller returns proper response format
3. ✓ Response status is 2xx

### Form Not Resetting

**Check:**
1. ✓ `data-ajax-options` includes `"resetForm": true`
2. ✓ Submission was successful
3. ✓ No custom `onSuccess` callback preventing default behavior

### Redirect Not Working

**Check:**
1. ✓ `data-ajax-options` includes `"redirect": "/Path"`
2. ✓ Path is valid
3. ✓ Submission was successful

## Summary

The standardized form pattern provides:
- ✅ Consistent UX across all forms
- ✅ Automatic validation
- ✅ Standardized error handling
- ✅ Loading states
- ✅ Success/error notifications
- ✅ Less code to maintain
- ✅ Better accessibility
- ✅ Improved security (anti-forgery tokens)

**Always use the standard pattern for new forms and migrate existing manual forms when possible.**
