/**
 * Navigation Module
 * Handles top navigation dropdown and mobile navigation interactions
 * Extracted from site2.js for better modularization
 */

// Modern Top Navigation Dropdown Handler
document.addEventListener('DOMContentLoaded', function() {
    // User dropdown functionality
    const userDropdownToggle = document.querySelector('.user-dropdown-toggle');
    const userMenu = document.querySelector('.user-menu');
    const userDropdownMenu = document.querySelector('.user-dropdown-menu');

    if (userDropdownToggle && userMenu && userDropdownMenu) {
        // Toggle dropdown on click
        userDropdownToggle.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();

            const isOpen = userMenu.classList.contains('open');

            // Close all other dropdowns first
            document.querySelectorAll('.user-menu.open').forEach(menu => {
                menu.classList.remove('open');
                const toggle = menu.querySelector('.user-dropdown-toggle');
                if (toggle) {
                    toggle.setAttribute('aria-expanded', 'false');
                }
            });

            // Toggle current dropdown
            if (!isOpen) {
                userMenu.classList.add('open');
                userDropdownToggle.setAttribute('aria-expanded', 'true');
            } else {
                userMenu.classList.remove('open');
                userDropdownToggle.setAttribute('aria-expanded', 'false');
            }
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', function(e) {
            if (!userMenu.contains(e.target)) {
                userMenu.classList.remove('open');
                userDropdownToggle.setAttribute('aria-expanded', 'false');
            }
        });

        // Close dropdown on escape key
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape') {
                userMenu.classList.remove('open');
                userDropdownToggle.setAttribute('aria-expanded', 'false');
            }
        });

        // Handle dropdown item navigation
        const dropdownItems = userDropdownMenu.querySelectorAll('.dropdown-item');
        dropdownItems.forEach((item, index) => {
            item.addEventListener('keydown', function(e) {
                if (e.key === 'ArrowDown') {
                    e.preventDefault();
                    const nextItem = dropdownItems[index + 1];
                    if (nextItem) nextItem.focus();
                }
                if (e.key === 'ArrowUp') {
                    e.preventDefault();
                    const prevItem = dropdownItems[index - 1];
                    if (prevItem) {
                        prevItem.focus();
                    } else {
                        userDropdownToggle.focus();
                    }
                }
            });
        });
    }

    // Mobile navigation toggle (if needed in future)
    const mobileToggle = document.querySelector('.mobile-nav-toggle');
    if (mobileToggle) {
        mobileToggle.addEventListener('click', function() {
            const isExpanded = this.getAttribute('aria-expanded') === 'true';
            this.setAttribute('aria-expanded', !isExpanded);
            // Mobile menu functionality can be added here later
        });
    }
});
