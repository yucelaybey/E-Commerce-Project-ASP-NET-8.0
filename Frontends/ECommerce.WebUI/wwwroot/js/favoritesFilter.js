document.addEventListener('DOMContentLoaded', function () {
    const sortDropdown = document.querySelector('.sort-dropdown select');
    const favoritesItemsContainer = document.querySelector('.favorites-items');

    // Sadece favorilerim bölümündeki ürün kartlarını seç
    const getFavoritesProductCards = () => {
        return Array.from(favoritesItemsContainer.querySelectorAll('.product-card'));
    };

    sortDropdown.addEventListener('change', function () {
        const sortValue = this.value;
        sortFavoritesProducts(sortValue);
    });

    function sortFavoritesProducts(sortValue) {
        const productCards = getFavoritesProductCards();

        // Animasyon için opacity ayarı
        favoritesItemsContainer.style.opacity = '0.5';
        favoritesItemsContainer.style.transition = 'opacity 0.3s ease';

        // Küçük bir gecikme ile sıralama işlemi
        setTimeout(() => {
            productCards.sort((a, b) => {
                const aName = a.querySelector('.product-title').textContent.toLowerCase();
                const bName = b.querySelector('.product-title').textContent.toLowerCase();

                // Fiyatları al (indirimli veya normal fiyat)
                const getPrice = (element) => {
                    const priceText = element.querySelector('.current-price').textContent;
                    return parseFloat(
                        priceText.replace('₺', '')
                            .replace(/\./g, '')
                            .replace(',', '.')
                    );
                };

                const aPrice = getPrice(a);
                const bPrice = getPrice(b);

                switch (sortValue) {
                    case 'price-asc':
                        return aPrice - bPrice;
                    case 'price-desc':
                        return bPrice - aPrice;
                    case 'name':
                        return aName.localeCompare(bName);
                    case 'normal':
                    default:
                        // Orjinal sıralama için data attribute kullanılabilir
                        // Eğer data-original-order attribute'u yoksa 0 döndür
                        const aOrder = parseInt(a.getAttribute('data-original-order')) || 0;
                        const bOrder = parseInt(b.getAttribute('data-original-order')) || 0;
                        return aOrder - bOrder;
                }
            });

            // Sıralanmış ürünleri yeniden ekle
            productCards.forEach((card, index) => {
                favoritesItemsContainer.appendChild(card);
                // Orjinal sıra bilgisini sakla
                card.setAttribute('data-original-order', index);
            });

            // Animasyonu tamamla
            setTimeout(() => {
                favoritesItemsContainer.style.opacity = '1';
            }, 50);

        }, 300);
    }

    // Sayfa yüklendiğinde orjinal sıralamayı kaydet
    getFavoritesProductCards().forEach((card, index) => {
        card.setAttribute('data-original-order', index);
    });
});