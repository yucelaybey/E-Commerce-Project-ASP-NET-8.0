document.addEventListener('DOMContentLoaded', function () {
    // Sidebar Toggle Elements
    const allCategoriesBtn = document.querySelector('.all-categories-btn');
    const sidebar = document.querySelector('.sidebar');
    const closeSidebar = document.querySelector('.close-sidebar');
    const overlay = document.createElement('div');
    overlay.className = 'sidebar-overlay';
    document.body.appendChild(overlay);

    // Mobile Sidebar Toggle Elements
    const mobileSidebar = document.querySelector('.mobile-sidebar');
    const closeMobileSidebar = document.querySelector('.close-mobile-sidebar');

    // Kategori dropdownları
    const categoryHeaders = document.querySelectorAll('.category-header');

    // User Profile Dropdown
    const userProfile = document.querySelector('.user-profile');
    const userInfo = document.querySelector('.user-info');
    const dropdownMenu = document.querySelector('.dropdown-menu');

    // Desktop Sidebar Toggle
    if (allCategoriesBtn) {
        allCategoriesBtn.addEventListener('click', function (e) {
            e.preventDefault();
            sidebar.classList.add('active');
            overlay.classList.add('active');
            document.body.style.overflow = 'hidden';
        });
    }

    if (closeSidebar) {
        closeSidebar.addEventListener('click', function () {
            sidebar.classList.remove('active');
            overlay.classList.remove('active');
            document.body.style.overflow = '';
        });
    }

    // Mobile Sidebar Toggle
    if (allCategoriesBtn) {
        allCategoriesBtn.addEventListener('click', function (e) {
            if (window.innerWidth <= 768) {
                e.preventDefault();
                mobileSidebar.classList.add('active');
                overlay.classList.add('active');
                document.body.style.overflow = 'hidden';
            }
        });
    }

    if (closeMobileSidebar) {
        closeMobileSidebar.addEventListener('click', function () {
            mobileSidebar.classList.remove('active');
            overlay.classList.remove('active');
            document.body.style.overflow = '';
        });
    }

    // Overlay Click
    overlay.addEventListener('click', function () {
        sidebar.classList.remove('active');
        mobileSidebar.classList.remove('active');
        overlay.classList.remove('active');
        document.body.style.overflow = '';
    });

    // Kategori dropdown toggle
    categoryHeaders.forEach(header => {
        header.addEventListener('click', function () {
            const content = this.nextElementSibling;
            const arrow = this.querySelector('.category-arrow');

            content.classList.toggle('active');
            arrow.classList.toggle('rotate');

            if (content.classList.contains('active')) {
                arrow.style.transform = 'rotate(180deg)';
            } else {
                arrow.style.transform = 'rotate(0deg)';
            }
        });

    });
});