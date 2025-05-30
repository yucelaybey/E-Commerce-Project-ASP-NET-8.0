document.addEventListener('DOMContentLoaded', function () {
    // Sidebar Toggle Functionality
    const menuToggle = document.querySelector('.menu-toggle');
    const closeSidebar = document.querySelector('.close-sidebar');
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.overlay');

    menuToggle.addEventListener('click', function () {
        sidebar.classList.add('active');
        overlay.classList.add('active');
        document.body.style.overflow = 'hidden';
    });

    closeSidebar.addEventListener('click', function () {
        sidebar.classList.remove('active');
        overlay.classList.remove('active');
        document.body.style.overflow = '';
    });

    overlay.addEventListener('click', function () {
        sidebar.classList.remove('active');
        overlay.classList.remove('active');
        document.body.style.overflow = '';
    });

    // Window Resize Event
    function handleResize() {
        if (window.innerWidth > 768) {
            sidebar.classList.remove('active');
            overlay.classList.remove('active');
            document.body.style.overflow = '';
        }
    }

    window.addEventListener('resize', handleResize);
    handleResize(); // Initialize

    // Table Row Click Event
    const tableRows = document.querySelectorAll('tbody tr');
    tableRows.forEach(row => {
        row.addEventListener('click', function (e) {
            if (!e.target.closest('.btn-icon')) {
                const orderId = this.cells[0].textContent;
                console.log('Order selected:', orderId);
                // You can implement a modal or page redirect here
            }
        });
    });

    // Button Hover Effects
    const buttons = document.querySelectorAll('.btn, .btn-icon');
    buttons.forEach(button => {
        button.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-2px)';
        });

        button.addEventListener('mouseleave', function () {
            this.style.transform = '';
        });
    });

    // Active Nav Item
    const navItems = document.querySelectorAll('.nav-item');
    navItems.forEach(item => {
        item.addEventListener('click', function () {
            navItems.forEach(nav => nav.classList.remove('active'));
            this.classList.add('active');

            // Close sidebar on mobile after selection
            if (window.innerWidth <= 768) {
                sidebar.classList.remove('active');
                overlay.classList.remove('active');
                document.body.style.overflow = '';
            }
        });
    });

    // Refresh Button with Loading State
    const refreshBtn = document.querySelector('.card-header .btn');
    if (refreshBtn) {
        refreshBtn.addEventListener('click', function () {
            const originalContent = this.innerHTML;
            this.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Yenileniyor...';
            this.disabled = true;

            // Simulate API call
            setTimeout(() => {
                this.innerHTML = originalContent;
                this.disabled = false;
                window.location.reload();
                showNotification('Tablo verileri başarıyla güncellendi!', 'success');
            }, 1500);
        });
    }

    // Notification System
    function showNotification(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `notification ${type}`;
        notification.textContent = message;
        document.body.appendChild(notification);

        setTimeout(() => {
            notification.classList.add('show');
        }, 10);

        setTimeout(() => {
            notification.classList.remove('show');
            setTimeout(() => {
                document.body.removeChild(notification);
            }, 300);
        }, 3000);
    }

    // Add notification styles dynamically
    const style = document.createElement('style');
    style.textContent = `
        .notification {
            position: fixed;
            bottom: 20px;
            left: 50%;
            transform: translateX(-50%);
            background-color: var(--primary-color);
            color: white;
            padding: 12px 24px;
            border-radius: var(--border-radius);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
            opacity: 0;
            transition: opacity 0.3s ease, transform 0.3s ease;
            z-index: 1100;
            max-width: 90%;
            text-align: center;
        }
        
        .notification.show {
            opacity: 1;
            transform: translateX(-50%) translateY(-10px);
        }
        
        .notification.success {
            background-color: var(--success);
        }
        
        .notification.warning {
            background-color: var(--warning);
        }
        
        .notification.danger {
            background-color: var(--danger);
        }
    `;
    document.head.appendChild(style);

    // Logout Button
    const logoutBtn = document.querySelector('.logout-btn');
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function () {
            showNotification('Çıkış yapılıyor...', 'warning');
            // You can implement logout logic here
        });
    }
});