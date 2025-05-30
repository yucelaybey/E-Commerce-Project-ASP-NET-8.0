document.addEventListener('DOMContentLoaded', function () {
    const productCards = document.querySelectorAll('.product-card');

    function processCards(index = 0) {
        if (index >= productCards.length) return;

        const card = productCards[index];
        const existingLink = card.querySelector('a.product-card-link');

        if (existingLink) {
            const productUrl = existingLink.href;
            const linkContent = existingLink.innerHTML;

            // Yeni link yapısı oluştur
            const newLink = document.createElement('a');
            newLink.href = productUrl;
            newLink.className = 'product-card-link';
            newLink.style.cssText = `
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                z-index: 1;
                text-decoration: none;
            `;

            // İçerik container'ı (pointer-events: none)
            const contentContainer = document.createElement('div');
            contentContainer.innerHTML = linkContent;
            contentContainer.style.cssText = `
                position: relative;
                z-index: 2;
                pointer-events: none;
                height: 100%;
            `;

            // Sepete Ekle butonlarını bul ve pointer-events: auto yap
            const addToCartButtons = contentContainer.querySelectorAll('.add-to-cart-btn-category, .add-to-cart-btn-trend');
            addToCartButtons.forEach(button => {
                button.style.pointerEvents = 'auto';
                button.style.position = 'relative';
                button.style.zIndex = '3';
            });

            // Favori butonlarını işle
            const favoriteButtons = contentContainer.querySelectorAll('.slider-favorite-btn');
            favoriteButtons.forEach(button => {
                button.style.pointerEvents = 'auto';
                button.style.position = 'relative';
                button.style.zIndex = '3';
            });

            // Yeni yapıyı ekle
            card.style.position = 'relative';
            newLink.appendChild(contentContainer);
            card.insertBefore(newLink, card.firstChild);

            // Eski linki kaldır
            existingLink.remove();
        }

        requestAnimationFrame(() => processCards(index + 1));
    }

    processCards();
});