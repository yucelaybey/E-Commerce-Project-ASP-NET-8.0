document.addEventListener('DOMContentLoaded', function () {
    // Sidebar aç/kapa
    const sidebar = document.querySelector('.sidebar');
    const closeSidebarBtn = document.querySelector('.close-sidebar');
    const allCategoriesBtn = document.querySelector('.all-categories-btn');

    if (allCategoriesBtn) {
        allCategoriesBtn.addEventListener('click', function () {
            sidebar.classList.add('active');
        });
    }

    if (closeSidebarBtn) {
        closeSidebarBtn.addEventListener('click', function () {
            sidebar.classList.remove('active');
        });
    }

    // Thumbnail resimlerini büyük resme yansıt
    const mainImage = document.querySelector('.main-image img');
    const thumbnailImages = document.querySelectorAll('.thumbnail-images img');

    if (mainImage && thumbnailImages.length > 0) {
        thumbnailImages.forEach(thumb => {
            thumb.addEventListener('click', function () {
                mainImage.src = thumb.src;
            });
        });
    }

    // Ürün miktar seçici işlevselliği
    const minusBtn = document.querySelector('.minus-btn');
    const plusBtn = document.querySelector('.plus-btn');
    const quantityInput = document.querySelector('.quantity-input');
    const stockCount = document.querySelector('.stock-count');
    const addToCartBtn = document.querySelector('.add-to-cart-btn');

    if (minusBtn && plusBtn && quantityInput && stockCount) {
        // Maksimum stok miktarını al
        const maxStock = parseInt(stockCount.textContent);

        // Miktar azaltma butonu
        minusBtn.addEventListener('click', function () {
            let currentValue = parseInt(quantityInput.value);
            if (currentValue > 1) {
                quantityInput.value = currentValue - 1;
                updateAddToCartButton();
            }
        });

        // Miktar artırma butonu
        plusBtn.addEventListener('click', function () {
            let currentValue = parseInt(quantityInput.value);
            if (currentValue < maxStock) {
                quantityInput.value = currentValue + 1;
                updateAddToCartButton();
            } else {
                alert(`Maksimum stok adeti ${maxStock}'dir.`);
            }
        });

        // Input alanından direkt miktar girme
        quantityInput.addEventListener('change', function () {
            let enteredValue = parseInt(this.value);

            // Geçersiz girişleri kontrol et
            if (isNaN(enteredValue)) {
                this.value = 1;
            } else if (enteredValue < 1) {
                this.value = 1;
            } else if (enteredValue > maxStock) {
                this.value = maxStock;
                alert(`Maksimum stok adeti ${maxStock}'dir.`);
            }

            updateAddToCartButton();
        });

        // Sepete ekle butonunu güncelleme fonksiyonu
        function updateAddToCartButton() {
            if (addToCartBtn) {
                const quantity = parseInt(quantityInput.value);
                addToCartBtn.setAttribute('data-quantity-id', quantity);
            }
        }

        // Sayfa yüklendiğinde butonu güncelle
        updateAddToCartButton();
    }

    // Kategori aç/kapa işlevselliği
    const categoryItems = document.querySelectorAll('.category-item');

    if (categoryItems.length > 0) {
        categoryItems.forEach(item => {
            item.addEventListener('click', function () {
                const parent = this.closest('.category-section');
                const subCategories = parent.querySelector('.sub-categories');
                const icon = this.querySelector('i');

                this.classList.toggle('active');
                if (subCategories) {
                    subCategories.style.display = subCategories.style.display === 'block' ? 'none' : 'block';
                }
                if (icon) {
                    icon.style.transform = icon.style.transform === 'rotate(180deg)' ? 'rotate(0deg)' : 'rotate(180deg)';
                }
            });
        });
    }
});