# JavaScript Architecture

## Overview
The JavaScript codebase has been modularized for better maintainability, performance, and code organization.

## Bundle Structure

### Core Bundle (`/js/core-bundle.js`) - **Loaded on ALL pages**
Contains essential functionality needed across the application:
- `pharma263.core.js` (27KB) - Core functionality, AJAX helpers, global error handlers
- `utility.js` (12KB) - Utility functions, date formatting, validation helpers
- `navigation.js` (3KB) - Top navigation dropdown handler, mobile navigation

**Usage:** Automatically loaded in `_Layout.cshtml`, no action required.

### Forms Bundle (`/js/forms-bundle.js`) - **Load on form pages**
Contains functionality for sales, purchase, and quotation forms:
- `pharma263.forms.js` (27KB) - Form handling, AJAX form submission, validation
- `pharma263.calculations.js` (15KB) - Price calculations, discount logic, totals

**Usage:** Add to form pages (Sales, Purchase, Quotation):
```html
@section scripts {
    <script src="~/js/forms-bundle.js" asp-append-version="true"></script>
}
```

### Reports Bundle (`/js/reports-bundle.js`) - **Load on report pages**
Contains report-specific functionality:
- `reports.js` (14KB) - Report generation, PDF export, Chart.js integration

**Usage:** Add to report pages:
```html
@section scripts {
    <script src="~/js/reports-bundle.js" asp-append-version="true"></script>
}
```

## Vendor Files

### site.js (380KB, 9154 lines)
- **Content:** Moment.js 2.2.1 (date/time library)
- **Status:** Legacy vendor library
- **Action:** Consider upgrading to Moment.js 2.29+ or migrating to Day.js (2KB alternative)

### site2.js (195KB, 4308 lines)
- **Content:** jQuery plugins (toggles, subMenu, niceScroll, etc.)
- **Status:** Vendor plugins with minimal application code
- **Note:** Navigation dropdown handler extracted to `navigation.js` (Phase 2)

## File Sizes and Line Counts

### Application Code (Modularized)
| File | Size | Lines | Bundle | Purpose |
|------|------|-------|--------|---------|
| pharma263.core.js | 27KB | ~1,000 | core | Core functionality |
| utility.js | 12KB | ~400 | core | Utilities |
| navigation.js | 3KB | ~90 | core | Navigation |
| pharma263.forms.js | 27KB | ~1,000 | forms | Form handling |
| pharma263.calculations.js | 15KB | ~500 | forms | Calculations |
| reports.js | 14KB | ~500 | reports | Reports |
| **Total Application Code** | **98KB** | **~3,490** | - | - |

### Vendor Code (Legacy)
| File | Size | Lines | Purpose |
|------|------|-------|---------|
| site.js | 380KB | 9,154 | Moment.js 2.2.1 |
| site2.js | 195KB | 4,308 | jQuery plugins |
| **Total Vendor Code** | **575KB** | **13,462** | - |

## Performance Benefits

### Before Phase 2:
- Single bundle.js (113KB) loaded on all pages
- All functionality loaded regardless of page needs

### After Phase 2:
- **Core Bundle** (42KB) - Loaded on all pages
- **Forms Bundle** (42KB) - Only on form pages (Sales, Purchase, Quotation)
- **Reports Bundle** (14KB) - Only on report pages

### Example Savings:
- Dashboard page: Loads only Core Bundle = **42KB** (saved 71KB)
- Report page: Loads Core + Reports = **56KB** (saved 57KB)
- Sales form: Loads Core + Forms = **84KB** (saved 29KB)

## Future Improvements (Phase 3+)

1. **Upgrade Moment.js**
   - Current: Moment.js 2.2.1 (2013, 380KB)
   - Recommended: Day.js (2KB, modern API) or Moment.js 2.29+

2. **Audit site2.js Plugins**
   - Identify which plugins are actually used
   - Remove unused jQuery plugins
   - Consider modern vanilla JS alternatives

3. **Feature-Specific Modules**
   - Extract inventory-specific code to `inventory.js`
   - Extract customer-specific code to `customers.js`
   - Load only on relevant pages

4. **ES6 Modules**
   - Migrate to ES6 module syntax
   - Use import/export instead of globals
   - Enable tree-shaking for smaller bundles

## How to Add New JavaScript

### For Application-Wide Functionality:
1. Add code to appropriate module:
   - Core logic → `pharma263.core.js`
   - Utilities → `utility.js`
   - Navigation → `navigation.js`

2. Code will automatically be included in core-bundle.js

### For Feature-Specific Functionality:
1. Create new module file (e.g., `inventory.js`)
2. Add to WebOptimizer in `Program.cs`:
```csharp
pipeline.AddJavaScriptBundle("/js/inventory-bundle.js",
    "/js/inventory.js"
).MinifyJavaScript();
```
3. Load in relevant views:
```html
@section scripts {
    <script src="~/js/inventory-bundle.js" asp-append-version="true"></script>
}
```

## Testing After Changes

1. **Clear browser cache** (Ctrl+Shift+R)
2. **Check browser console** for errors
3. **Test functionality** on affected pages
4. **Verify bundles load** in Network tab

## Debugging

### If bundle.js not found:
- Old reference in view - update to `core-bundle.js`
- Check WebOptimizer configuration in `Program.cs`
- Verify file exists in `/wwwroot/js/`

### If functionality broken after modularization:
- Check browser console for errors
- Verify correct bundle loaded on page
- Check for global variable conflicts
- Test with bundles disabled (load individual files)

## Migration Status

- ✅ Phase 1: CSS Cleanup (Completed)
- ✅ Phase 2: JavaScript Modularization (Completed)
- ⏳ Phase 3: Form Standardization
- ⏳ Phase 4: Accessibility
- ⏳ Phase 5: Performance Optimization
