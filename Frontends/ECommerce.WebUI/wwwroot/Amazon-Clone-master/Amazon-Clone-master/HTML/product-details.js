document.addEventListener('DOMContentLoaded', function() {
    const addToCartBtn = document.querySelector('.add-to-cart-btn');
    const cartCountElement = document.querySelector('.cart-count');
    const stockCountElement = document.querySelector('.stock-count');

    let cartCount = 0;
    let stockCount = parseInt(stockCountElement.textContent);

    addToCartBtn.addEventListener('click', function() {
        if (stockCount > 0) {
            cartCount++;
            stockCount--;
            cartCountElement.textContent = cartCount;
            stockCountElement.textContent = stockCount;
        } else {
            alert('Stokta yeterli ürün bulunmamaktadır.');
        }
    });

    // Sidebar aç/kapa
    const sidebar = document.querySelector('.sidebar');
    const closeSidebarBtn = document.querySelector('.close-sidebar');
    const allCategoriesBtn = document.querySelector('.all-categories-btn');

    allCategoriesBtn.addEventListener('click', function() {
        sidebar.classList.add('active');
    });

    closeSidebarBtn.addEventListener('click', function() {
        sidebar.classList.remove('active');
    });

    // Thumbnail resimlerini büyük resme yansıt
    const mainImage = document.querySelector('.main-image img');
    const thumbnailImages = document.querySelectorAll('.thumbnail-images img');

    thumbnailImages.forEach(thumb => {
        thumb.addEventListener('click', function() {
            mainImage.src = thumb.src;
        });
    });
});
