// Banner animasyonu için
document.addEventListener("DOMContentLoaded", function () {
    const bannerContent = document.querySelector('.banner-content');
    bannerContent.style.opacity = '1';
});
// Tümü Butonu İşlevselliği
document.querySelector('.all-categories-btn').addEventListener('click', function () {
    if (window.innerWidth > 768) {
        // Masaüstü için sidebar'ı aç/kapat
        document.querySelector('.sidebar').classList.toggle('active');
    } else {
        // Mobil için mobil menüyü aç/kapat
        document.querySelector('.mobile-menu').classList.toggle('active');
    }
});

// Sidebar ve Mobil Menü Kapatma
document.querySelectorAll('.close-sidebar, .close-mobile-menu').forEach(btn => {
    btn.addEventListener('click', function () {
        document.querySelector('.sidebar').classList.remove('active');
        document.querySelector('.mobile-menu').classList.remove('active');
    });
});

// Kategori Açma/Kapatma
document.querySelectorAll('.category-item').forEach(item => {
    item.addEventListener('click', function () {
        this.classList.toggle('active');
        this.nextElementSibling.classList.toggle('active');
    });
});

// Sepet Modalını Açma
document.querySelector('.nav-cart').addEventListener('click', function () {
    const cartModal = document.querySelector('.cart-modal');
    cartModal.style.display = 'flex';

    // Sepet durumunu kontrol et (örnek: giriş yapılmış mı, sepette ürün var mı)
    const isLoggedIn = false; // Bu dinamik olarak ayarlanacak
    const cartItems = []; // Bu dinamik olarak ayarlanacak

    if (isLoggedIn && cartItems.length > 0) {
        // Sepette ürünler varsa
        document.querySelector('.cart-items').style.display = 'block';
        document.querySelector('.empty-cart-message').style.display = 'none';
    } else {
        // Sepet boş veya giriş yapılmamışsa
        document.querySelector('.cart-items').style.display = 'none';
        document.querySelector('.empty-cart-message').style.display = 'block';
    }
});

// Sepet Modalını Kapatma
document.querySelector('.close-cart-modal').addEventListener('click', function () {
    document.querySelector('.cart-modal').style.display = 'none';
});

// Modal dışına tıklayarak kapatma
window.addEventListener('click', function (event) {
    const cartModal = document.querySelector('.cart-modal');
    if (event.target === cartModal) {
        cartModal.style.display = 'none';
    }
});