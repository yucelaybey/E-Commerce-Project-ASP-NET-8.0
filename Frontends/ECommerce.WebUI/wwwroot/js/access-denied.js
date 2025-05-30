document.addEventListener('DOMContentLoaded', function() {
    // Mobil Menü İşlemleri
    const mobileMenuBtn = document.querySelector('.mobile-menu-btn');
    const mobileMenu = document.querySelector('.mobile-menu');
    const mobileMenuOverlay = document.querySelector('.mobile-menu-overlay');
    const mobileMenuClose = document.querySelector('.mobile-menu-close');

    if (mobileMenuBtn) {
        mobileMenuBtn.addEventListener('click', function() {
            mobileMenu.classList.add('active');
            mobileMenuOverlay.classList.add('active');
            document.body.style.overflow = 'hidden';
        });
    }

    if (mobileMenuClose) {
        mobileMenuClose.addEventListener('click', closeMenu);
    }

    if (mobileMenuOverlay) {
        mobileMenuOverlay.addEventListener('click', closeMenu);
    }

    function closeMenu() {
        mobileMenu.classList.remove('active');
        mobileMenuOverlay.classList.remove('active');
        document.body.style.overflow = '';
    }

    // Canlı Destek Butonu İşlemleri
    const liveChatBtn = document.querySelector('.live-chat-btn');
    
    if (liveChatBtn) {
        liveChatBtn.addEventListener('click', function(e) {
            e.preventDefault();
            // Burada gerçek uygulamada canlı destek modülünü açacak kod olabilir
        });
    }

    // Erişim Reddedildi Sayfası Animasyonu
    const accessDeniedContent = document.querySelector('.access-denied-content');
    
    if (accessDeniedContent) {
        accessDeniedContent.style.opacity = '0';
        accessDeniedContent.style.transform = 'translateY(20px)';
        accessDeniedContent.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
        
        setTimeout(function() {
            accessDeniedContent.style.opacity = '1';
            accessDeniedContent.style.transform = 'translateY(0)';
        }, 200);
    }

    // Otomatik yönlendirme süre sayacı (isteğe bağlı)
    let redirectTime = 60; // 60 saniye sonra yönlendirme
    const showRedirectTimer = false; // Bu özelliği aktif etmek istersen true yap
    
    if (showRedirectTimer) {
        const timerElement = document.createElement('div');
        timerElement.className = 'redirect-timer';
        timerElement.innerHTML = `<p>Anasayfaya yönlendiriliyorsunuz: <span id="redirect-count">${redirectTime}</span> saniye</p>`;
        
        accessDeniedContent.appendChild(timerElement);
        
        const countElement = document.getElementById('redirect-count');
        
        const countdownInterval = setInterval(function() {
            redirectTime--;
            if (countElement) {
                countElement.textContent = redirectTime;
            }
            
            if (redirectTime <= 0) {
                clearInterval(countdownInterval);
                window.location.href = 'index.html';
            }
        }, 1000);
    }
});