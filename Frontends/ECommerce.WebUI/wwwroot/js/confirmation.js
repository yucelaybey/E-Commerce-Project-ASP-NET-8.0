document.addEventListener('DOMContentLoaded', function() {
    // Show loading screen initially
    const loadingScreen = document.querySelector('.loading-screen');
    const mainContent = document.querySelector('.confirmation-container');
    
    // Hide main content initially
    mainContent.style.display = 'none';
    
    // After 3 seconds, hide loading and show main content
    setTimeout(function() {
        loadingScreen.style.opacity = '0';
        loadingScreen.style.pointerEvents = 'none';
        
        // Show main content with fade in effect
        mainContent.style.display = 'block';
        setTimeout(() => {
            mainContent.style.opacity = '1';
        }, 50);
        
        // Remove loading screen from DOM after animation completes
        setTimeout(() => {
            loadingScreen.remove();
        }, 500);
    }, 3000);
    
    // Mobile menu toggle
    const mobileMenuBtn = document.querySelector('.mobile-menu-btn');
    const mobileMenuClose = document.querySelector('.mobile-menu-close');
    const mobileMenu = document.querySelector('.mobile-menu');
    const mobileMenuOverlay = document.querySelector('.mobile-menu-overlay');
    
    if(mobileMenuBtn) {
        mobileMenuBtn.addEventListener('click', function() {
            mobileMenu.classList.add('active');
            mobileMenuOverlay.classList.add('active');
            document.body.style.overflow = 'hidden';
        });
    }
    
    if(mobileMenuClose) {
        mobileMenuClose.addEventListener('click', function() {
            mobileMenu.classList.remove('active');
            mobileMenuOverlay.classList.remove('active');
            document.body.style.overflow = '';
        });
    }
    
    if(mobileMenuOverlay) {
        mobileMenuOverlay.addEventListener('click', function() {
            mobileMenu.classList.remove('active');
            mobileMenuOverlay.classList.remove('active');
            document.body.style.overflow = '';
        });
    }
});