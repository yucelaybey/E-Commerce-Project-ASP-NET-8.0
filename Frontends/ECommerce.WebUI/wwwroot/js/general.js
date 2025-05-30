document.addEventListener('DOMContentLoaded', function () {
    // Şu anki URL'yi al
    const currentUrl = window.location.pathname;

    // Tüm nav-item'ları seç
    const navItems = document.querySelectorAll('.nav-item');

    // Her bir nav-item için kontrol yap
    navItems.forEach(item => {
        // Nav-item'ın href özelliğini al
        const itemHref = item.getAttribute('href');

        // Eğer şu anki URL, bu nav-item'ın href'i ile eşleşiyorsa
        if (currentUrl === itemHref) {
            // Diğer tüm nav-item'lardan active class'ını ve indicator'ı kaldır
            navItems.forEach(nav => {
                nav.classList.remove('active');
                const indicator = nav.querySelector('.active-indicator');
                if (indicator) {
                    nav.removeChild(indicator);
                }
            });

            // Bu nav-item'a active class'ını ekle
            item.classList.add('active');

            // Eğer zaten bir indicator yoksa, yeni bir indicator oluştur ve ekle
            if (!item.querySelector('.active-indicator')) {
                const indicator = document.createElement('div');
                indicator.className = 'active-indicator';
                item.appendChild(indicator);
            }
        }
    });
});