document.addEventListener('DOMContentLoaded', function () {
    const sortOptions = document.getElementById('sort-options');
    const productsGrid = document.querySelector('.products-grid');
    let originalOrder = []; // Orijinal sıralamayı saklamak için

    // Sayfa yüklendiğinde orijinal sıralamayı kaydet
    if (productsGrid) {
        originalOrder = Array.from(document.querySelectorAll('.product-card'));
    }

    if (sortOptions && productsGrid) {
        sortOptions.addEventListener('change', function () {
            const selectedValue = this.value;
            sortProducts(selectedValue);
        });
    }

    function sortProducts(sortType) {
        const productCards = Array.from(document.querySelectorAll('.product-card'));

        // Animasyon için opacity ayarla
        productsGrid.style.opacity = '0.5';
        productsGrid.style.transition = 'opacity 0.3s ease';

        setTimeout(() => {
            if (sortType === 'default') {
                // Varsayılan için orijinal sıralamayı geri yükle
                restoreOriginalOrder();
            } else {
                // Diğer sıralama seçenekleri
                productCards.sort((a, b) => {
                    const priceA = getPriceFromCard(a);
                    const priceB = getPriceFromCard(b);

                    if (sortType === 'price-asc') {
                        return priceA - priceB;
                    } else if (sortType === 'price-desc') {
                        return priceB - priceA;
                    }
                    return 0;
                });

                // Ürünleri yeniden yerleştir (animasyonlu)
                animateReorder(productCards);
            }

            // Opacity'i eski haline getir
            productsGrid.style.opacity = '1';
        }, 300);
    }

    function restoreOriginalOrder() {
        // Tüm mevcut kartları kaldır
        while (productsGrid.firstChild) {
            productsGrid.removeChild(productsGrid.firstChild);
        }

        // Orijinal sırayla yeniden ekle (animasyonlu)
        animateReorder(originalOrder);
    }

    function animateReorder(sortedCards) {
        // Önce tüm kartlara animasyon class'ı ekle
        sortedCards.forEach(card => {
            card.style.transition = 'transform 0.5s ease, opacity 0.5s ease';
            card.style.transform = 'translateY(20px)';
            card.style.opacity = '0';
        });

        // Küçük bir gecikmeyle pozisyonları güncelle
        setTimeout(() => {
            // Tüm mevcut kartları kaldır
            while (productsGrid.firstChild) {
                productsGrid.removeChild(productsGrid.firstChild);
            }

            // Yeniden sıralanmış kartları ekle
            sortedCards.forEach((card, index) => {
                productsGrid.appendChild(card);

                // Kartları tek tek animasyonla göster
                setTimeout(() => {
                    card.style.transform = 'translateY(0)';
                    card.style.opacity = '1';
                }, index * 50); // Sırayla animasyon için
            });
        }, 50);
    }

    function getPriceFromCard(card) {
        // Satış fiyatı varsa onu al, yoksa normal fiyatı al
        const salePriceElement = card.querySelector('del');
        const priceElement = salePriceElement
            ? card.querySelector('span:not(.cart-count-effect)')
            : card.querySelector('.product-price span');

        const priceText = priceElement.textContent
            .replace('₺', '')
            .replace('.', '')
            .replace(',', '.');

        return parseFloat(priceText);
    }
});