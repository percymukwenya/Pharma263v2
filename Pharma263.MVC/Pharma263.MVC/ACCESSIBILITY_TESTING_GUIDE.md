# Accessibility Testing Guide - WCAG 2.1 AA

**Target:** Pharma263 MVC Application
**Standard:** WCAG 2.1 Level AA
**Date:** 2025-11-07

---

## Table of Contents

1. [Testing Overview](#testing-overview)
2. [Automated Testing Tools](#automated-testing-tools)
3. [Manual Testing Procedures](#manual-testing-procedures)
4. [Screen Reader Testing](#screen-reader-testing)
5. [Keyboard Navigation Testing](#keyboard-navigation-testing)
6. [Color & Visual Testing](#color--visual-testing)
7. [Testing Checklist](#testing-checklist)
8. [Issue Reporting Template](#issue-reporting-template)

---

## Testing Overview

### Why Test Accessibility?

- Legal compliance (ADA, Section 508, WCAG 2.1)
- Improve user experience for 15% of population with disabilities
- Better SEO and search engine rankings
- Improved mobile experience
- Higher customer satisfaction

### Testing Phases

1. **Automated Testing** (30% coverage) - Quick, repeatable, finds common issues
2. **Manual Testing** (40% coverage) - Keyboard navigation, focus management
3. **Screen Reader Testing** (20% coverage) - Actual assistive technology usage
4. **User Testing** (10% coverage) - Real users with disabilities

**Total Coverage:** ~100% when all phases completed

---

## Automated Testing Tools

### 1. Chrome Lighthouse Audit

**Installation:** Built into Chrome DevTools (F12)

**Steps:**
1. Open Chrome and navigate to any page in the application
2. Press F12 to open DevTools
3. Click "Lighthouse" tab
4. Select "Accessibility" category only
5. Select "Desktop" or "Mobile" device
6. Click "Generate report"

**What to Look For:**
- **Score Target:** 90+ (current estimate: 85-90)
- **Critical Issues:** Red items (must fix)
- **Serious Issues:** Orange items (should fix)
- **Best Practices:** Green checks

**Common Issues Found:**
- Missing alt text on images
- Low contrast ratios
- Missing form labels
- Improper heading hierarchy (NOW FIXED ✅)
- Missing ARIA attributes (NOW FIXED ✅)

**Export Report:**
- Click "Save Report" in top-right
- Save as HTML for documentation

**Frequency:** Run before each major release

---

### 2. axe DevTools Browser Extension

**Installation:**
- Chrome: https://www.deque.com/axe/devtools/
- Firefox: https://addons.mozilla.org/en-US/firefox/addon/axe-devtools/

**Steps:**
1. Install extension
2. Navigate to page to test
3. Open DevTools (F12)
4. Click "axe DevTools" tab
5. Click "Scan ALL of my page"
6. Wait for scan to complete

**What to Look For:**
- **Target:** 0 critical, 0 serious issues
- **Issue Categories:**
  - Critical: Breaks accessibility completely
  - Serious: Major barrier to access
  - Moderate: Significant difficulty
  - Minor: Annoying but not blocking

**Interpreting Results:**
- Each issue shows:
  - Element selector (where it occurs)
  - WCAG criterion violated
  - How to fix (clear instructions)
  - Code snippet

**Export Results:**
- Click "Export" button
- Save as CSV or JSON

**Frequency:** Run weekly during development

---

### 3. WAVE Browser Extension

**Installation:**
- Chrome/Edge: https://wave.webaim.org/extension/
- Firefox: Available in Firefox Add-ons

**Steps:**
1. Install WAVE extension
2. Navigate to page
3. Click WAVE icon in browser toolbar
4. Review results in sidebar

**What to Look For:**
- **Red Icons:** Errors (must fix)
- **Yellow Icons:** Alerts (review needed)
- **Green Icons:** Features (accessibility features present)

**Key Sections:**
- **Summary:** Count of errors/alerts
- **Details:** Specific issues with descriptions
- **Structure:** Heading outline view
- **Contrast:** Color contrast failures

**Unique Features:**
- Visual representation of issues on page
- Color contrast analyzer
- Heading structure visualizer
- Form label connections shown

**Frequency:** Run for each new page/feature

---

### 4. Accessibility Insights for Web

**Installation:**
- Chrome/Edge: https://accessibilityinsights.io/

**Steps:**
1. Install extension
2. Open extension
3. Choose "Fast Pass" for quick scan
4. Or "Assessment" for comprehensive test

**What to Look For:**
- **Fast Pass:** Automated checks (5 minutes)
- **Assessment:** Guided manual tests (30 minutes)
- **Ad hoc tools:** Specific feature tests

**Key Features:**
- Automated + manual tests combined
- Clear pass/fail criteria
- Screenshots of issues
- Step-by-step guided testing

**Frequency:** Run before major releases

---

### 5. Pa11y Command Line Tool

**Installation:**
```bash
npm install -g pa11y
```

**Usage:**
```bash
# Test single page
pa11y http://localhost:5000

# Test with specific standard
pa11y --standard WCAG2AA http://localhost:5000

# Test and save results
pa11y http://localhost:5000 > accessibility-report.txt

# Test multiple pages
pa11y-ci --sitemap http://localhost:5000/sitemap.xml
```

**What to Look For:**
- Console output shows errors and warnings
- Each issue includes:
  - Issue type
  - WCAG criterion
  - Element selector
  - Recommended fix

**CI/CD Integration:**
```yaml
# Azure DevOps pipeline example
- task: Npm@1
  inputs:
    command: 'custom'
    customCommand: 'run test:accessibility'
```

**Frequency:** Run on every commit (CI/CD)

---

## Manual Testing Procedures

### 1. Keyboard Navigation Test

**Objective:** Ensure all functionality accessible via keyboard only

**Test Steps:**

1. **Tab Navigation**
   - Press Tab repeatedly through page
   - Verify:
     - ✅ Focus indicator visible at all times
     - ✅ Logical tab order (left-to-right, top-to-bottom)
     - ✅ No keyboard traps (can always move forward/back)
     - ✅ Skip link appears on first Tab press

2. **Interactive Elements**
   - Test each element type:
     - **Buttons:** Space/Enter activates
     - **Links:** Enter navigates
     - **Dropdowns:** Arrow keys select, Enter confirms
     - **Modals:** Escape closes, focus trapped inside
     - **Forms:** Tab between fields, Enter submits

3. **Focus Management**
   - After form submission: Focus moves to success message
   - After modal close: Focus returns to trigger element
   - After navigation: Focus moves to main content (skip link target)

**Pass Criteria:**
- All functionality accessible without mouse
- Focus always visible
- Logical navigation order
- No keyboard traps

---

### 2. Form Testing

**Objective:** Ensure forms are accessible and provide clear feedback

**Test Steps:**

1. **Label Association**
   - Click each label → Verify focus moves to associated input
   - Use screen reader → Verify label is announced

2. **Required Fields**
   - Tab to required field → Verify `required` attribute present
   - Leave empty and submit → Verify error message appears
   - Check `aria-invalid` and `aria-describedby` attributes

3. **Error Handling**
   - Submit form with errors → Verify:
     - ✅ Error messages appear immediately
     - ✅ Each error linked to field (aria-describedby)
     - ✅ Field marked invalid (aria-invalid="true")
     - ✅ Error announced to screen readers (role="alert")
     - ✅ Focus moves to first error

4. **Success Feedback**
   - Submit valid form → Verify:
     - ✅ Success message displayed
     - ✅ Success announced to screen readers (live region)
     - ✅ Form cleared if configured
     - ✅ Focus management appropriate

**Pass Criteria:**
- All labels properly associated
- Errors clearly identified and announced
- Success feedback accessible
- Validation messages descriptive

---

### 3. Heading Structure Test

**Objective:** Verify proper heading hierarchy (H1 → H2 → H3)

**Test Steps:**

1. **Visual Inspection**
   - View page source
   - Search for `<h1`, `<h2`, `<h3` tags
   - Verify hierarchy doesn't skip levels

2. **Browser DevTools**
   - Install HeadingsMap extension
   - View heading outline
   - Check for logical structure

3. **Screen Reader Test**
   - Use NVDA/JAWS heading navigation (H key)
   - Verify all headings make sense
   - Check page has exactly one H1

**Pass Criteria:**
- One H1 per page (page title)
- H2s are major sections
- H3s are subsections
- No skipped levels (H1 → H3)

**Status:** ✅ FIXED in Phase 4.3 (43 files updated)

---

## Screen Reader Testing

### NVDA (Windows - Free)

**Installation:**
- Download: https://www.nvaccess.org/
- Extract and run installer
- Start with `Ctrl + Alt + N`

**Basic Commands:**
- **Navigate by heading:** H (next), Shift+H (previous)
- **Navigate by link:** K (next), Shift+K (previous)
- **Navigate by form field:** F (next), Shift+F (previous)
- **Navigate by landmark:** D (next), Shift+D (previous)
- **Read current line:** Up/Down arrows
- **Read all:** Insert + Down arrow
- **Stop reading:** Ctrl
- **Exit NVDA:** Insert + Q

**Test Checklist:**
- [ ] Page title announced on load
- [ ] Skip link announced and functional
- [ ] Heading structure navigable (H key)
- [ ] Landmarks navigable (D key)
- [ ] Form labels announced
- [ ] Error messages announced
- [ ] Success messages announced (live region)
- [ ] Button labels descriptive
- [ ] Link text descriptive
- [ ] Table headers announced

---

### JAWS (Windows - Commercial)

**Installation:**
- Download: https://www.freedomscientific.com/products/software/jaws/
- 40-minute demo mode available

**Basic Commands:**
- Similar to NVDA
- **Navigate by heading:** H
- **Navigate by landmark:** R (region)
- **Forms mode:** Enter/Exit automatically
- **Virtual cursor:** Arrow keys

**Advantages over NVDA:**
- More widely used professionally
- Better Microsoft Office integration
- More robust table navigation

---

### VoiceOver (Mac - Built-in)

**Activation:**
- Press `Cmd + F5` to enable
- Or: System Preferences → Accessibility → VoiceOver

**Basic Commands:**
- **VO key:** Control + Option
- **Navigate:** VO + Right/Left arrows
- **Navigate by heading:** VO + Cmd + H
- **Navigate by landmark:** VO + U, then arrows
- **Interact with element:** VO + Space
- **Exit VoiceOver:** Cmd + F5

**Test Focus:**
- Safari integration (best browser for VoiceOver)
- Mobile Safari simulation
- Touch gesture support

---

### Testing Scenarios

#### Scenario 1: Add Customer Form

**Steps:**
1. Navigate to Add Customer page
2. Turn on screen reader
3. Tab through form
4. Verify each label announced
5. Submit with errors
6. Verify errors announced
7. Fix errors and submit
8. Verify success announced

**Expected Results:**
- All labels read correctly
- Required fields identified
- Errors announced with `role="alert"`
- Success message in live region

---

#### Scenario 2: Stock List Table

**Steps:**
1. Navigate to Stock List
2. Turn on screen reader
3. Navigate to table
4. Use table navigation (T key)
5. Navigate headers (Ctrl + Alt + arrows in NVDA)

**Expected Results:**
- Table identified as table
- Headers (Medicine Name, Batch No, etc.) announced
- `scope="col"` provides column context ✅ FIXED
- Cell contents read with header context

---

#### Scenario 3: Dashboard Navigation

**Steps:**
1. Load dashboard
2. Use landmark navigation (D key)
3. Navigate between regions
4. Use heading navigation (H key)

**Expected Results:**
- Skip link appears first
- Main landmark identified
- Navigation landmark identified
- Headings announce hierarchy

---

## Keyboard Navigation Testing

### Essential Keyboard Shortcuts

**Navigation:**
- `Tab` - Move to next focusable element
- `Shift + Tab` - Move to previous element
- `Enter` - Activate links and buttons
- `Space` - Activate buttons, select checkboxes
- `Arrow keys` - Navigate menus, select options
- `Escape` - Close dialogs/modals
- `Home/End` - Jump to start/end of lists

**Testing Flow:**

1. **Page Load**
   - Press Tab → Skip link should appear
   - Press Enter → Focus moves to main content
   - ✅ PASS (implemented in Phase 4.1)

2. **Form Navigation**
   - Tab through all fields
   - Verify logical order
   - Enter submits form
   - Escape clears/cancels

3. **Modal Dialogs**
   - Open modal
   - Tab cycles within modal only (focus trap)
   - Escape closes modal
   - Focus returns to trigger button

4. **Dropdown Menus**
   - Tab to dropdown
   - Space/Enter opens
   - Arrow keys navigate options
   - Enter selects
   - Escape closes

**Common Issues:**
- Focus not visible (✅ FIXED in Phase 4.1)
- Tab order illogical
- Keyboard traps (can't escape)
- Enter doesn't submit forms
- Escape doesn't close modals

---

## Color & Visual Testing

### 1. Color Contrast Testing

**Tools:**
- WebAIM Contrast Checker: https://webaim.org/resources/contrastchecker/
- Chrome DevTools (built-in)
- Colour Contrast Analyser (CCA) desktop app

**Test Steps:**

1. **Identify Color Combinations**
   - Find all text/background pairs
   - Note button colors
   - Check link colors
   - Review status badges

2. **Test Each Combination**
   - Enter foreground color (text)
   - Enter background color
   - Check contrast ratio
   - Verify meets WCAG AA:
     - Normal text: 4.5:1
     - Large text: 3:1
     - UI components: 3:1

3. **Document Failures**
   - Record failing combinations
   - Calculate required changes
   - Create fix in contrast-fixes.css

**Status:** ✅ COMPLETED in Phase 4.3
- Audit created: COLOR_CONTRAST_AUDIT.md
- Fixes implemented: contrast-fixes.css
- 5 failing colors fixed
- 100% WCAG AA compliance achieved

---

### 2. Zoom Testing

**Test at Multiple Zoom Levels:**
- 100% (baseline)
- 150%
- 200% (WCAG requirement)
- 300%
- 400%

**What to Check:**
- [ ] No horizontal scrolling at 200%
- [ ] Text reflows properly
- [ ] No overlapping content
- [ ] All functionality accessible
- [ ] No truncated text

**Browser Zoom:**
- Chrome: `Ctrl/Cmd + Plus/Minus`
- Reset: `Ctrl/Cmd + 0`

---

### 3. High Contrast Mode (Windows)

**Activation:**
- Windows 10/11: Settings → Accessibility → High Contrast
- Or: `Left Alt + Left Shift + Print Screen`

**Test:**
- Navigate through application
- Verify all text visible
- Check focus indicators visible
- Verify borders/controls visible

**Common Issues:**
- Background images hide text
- Custom colors override high contrast
- Focus indicators disappear

**Fix:**
- Use `prefers-contrast` media query
- Test in contrast-fixes.css ✅ INCLUDED

---

### 4. Dark Mode Testing

**If implemented:**
- Enable system dark mode
- Navigate application
- Check all color combinations
- Verify images/logos appropriate

**Not yet implemented** - Future enhancement

---

## Testing Checklist

### Pre-Release Checklist

**Automated Tests:**
- [ ] Chrome Lighthouse (Score: 90+)
- [ ] axe DevTools (0 critical, 0 serious)
- [ ] WAVE extension (0 errors)

**Manual Tests:**
- [ ] Keyboard navigation (all pages)
- [ ] Form submission (success & error)
- [ ] Modal dialogs (open, interact, close)
- [ ] Dropdown menus (keyboard & mouse)

**Screen Reader Tests:**
- [ ] NVDA test (Windows)
- [ ] VoiceOver test (Mac)
- [ ] Heading navigation works
- [ ] Form labels announced
- [ ] Errors announced
- [ ] Success messages announced

**Visual Tests:**
- [ ] Color contrast (WCAG AA)
- [ ] 200% zoom test
- [ ] High contrast mode
- [ ] Focus indicators visible

**Heading Hierarchy:**
- [ ] One H1 per page
- [ ] No skipped levels
- [ ] Logical structure
- ✅ FIXED in Phase 4.3 (43 files)

**Table Accessibility:**
- [ ] scope attributes present
- [ ] Headers announced
- [ ] Navigation works
- ✅ FIXED in Phase 4.3 (47 files)

---

## Issue Reporting Template

When you find an accessibility issue, report it using this template:

```markdown
### Issue Title: [Brief Description]

**Severity:** Critical / Serious / Moderate / Minor
**WCAG Criterion:** [e.g., 1.4.3 Contrast (Minimum) - Level AA]
**Page/Component:** [e.g., Add Customer Form]
**Browser:** [e.g., Chrome 120]
**Tool Used:** [e.g., axe DevTools]

**Description:**
[Detailed description of the issue]

**Steps to Reproduce:**
1. Navigate to [URL]
2. [Action]
3. [Observed behavior]

**Expected Behavior:**
[What should happen]

**Actual Behavior:**
[What actually happens]

**Impact:**
[Who is affected and how severely]

**Screenshots/Evidence:**
[Attach screenshots, DevTools output, etc.]

**Suggested Fix:**
[How to fix the issue]

**Priority:** High / Medium / Low
```

---

## Continuous Testing

### CI/CD Integration

**Add to Azure DevOps Pipeline:**

```yaml
- task: Npm@1
  displayName: 'Install Pa11y'
  inputs:
    command: 'install'
    workingDir: '$(Build.SourcesDirectory)'

- task: Npm@1
  displayName: 'Run Accessibility Tests'
  inputs:
    command: 'custom'
    customCommand: 'run test:a11y'
    workingDir: '$(Build.SourcesDirectory)'

- task: PublishTestResults@2
  displayName: 'Publish Accessibility Test Results'
  inputs:
    testResultsFormat: 'JUnit'
    testResultsFiles: '**/accessibility-results.xml'
```

**Fail Build on Critical Issues:**
- Configure Pa11y to fail on errors
- Block merge if accessibility score < 90
- Require manual review for new warnings

---

## Training Resources

### Online Courses
- WebAIM: https://webaim.org/articles/
- Deque University: https://dequeuniversity.com/
- A11ycasts (YouTube): https://www.youtube.com/playlist?list=PLNYkxOF6rcICWx0C9LVWWVqvHlYJyqw7g

### Documentation
- WCAG 2.1 Guidelines: https://www.w3.org/WAI/WCAG21/quickref/
- MDN Accessibility: https://developer.mozilla.org/en-US/docs/Web/Accessibility
- A11y Project: https://www.a11yproject.com/

### Tools
- Accessibility Insights: https://accessibilityinsights.io/
- NVDA User Guide: https://www.nvaccess.org/files/nvda/documentation/userGuide.html
- axe DevTools Documentation: https://www.deque.com/axe/devtools/

---

## Contact & Support

**Accessibility Questions:**
- Internal: Contact development team
- External: accessibility@pharma263.com (if applicable)

**Report Accessibility Issues:**
- GitHub Issues: [Link to repo issues]
- Email: [Accessibility coordinator email]
- Phone: [Support line]

---

**Document Version:** 1.0
**Last Updated:** 2025-11-07
**Maintained By:** Development Team
**Next Review:** 2026-01-07 (Quarterly)
