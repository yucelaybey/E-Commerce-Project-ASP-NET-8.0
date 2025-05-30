document.addEventListener('DOMContentLoaded', function () {
    const filterBtn = document.getElementById('filterBtn');
    const filterPanel = document.getElementById('filterPanel');
    const sortSelect = document.getElementById('sortSelect');
    const minPriceInput = document.getElementById('minPrice');
    const maxPriceInput = document.getElementById('maxPrice');
    const clearFilterBtn = document.getElementById('clearFilter');
    const filterIndicator = document.getElementById('filterIndicator');
    const productsGrid = document.querySelector('.products-grid');

    let activeFilters = 0;
    let products = [];

    // Initialize products array with proper price parsing
    function initializeProducts() {
        products = Array.from(document.querySelectorAll('.product-card')).map(productCard => {
            // Find the correct price element (handles sale prices)
            const priceElement = productCard.querySelector('.product-price span:last-child');
            const priceText = priceElement.textContent.replace('₺', '').trim().replace(/\./g, '').replace(',', '.');
            const price = parseFloat(priceText);

            // Parse cart count correctly
            const cartCountText = productCard.querySelector('.cart-count-effect').textContent;
            const cartCount = parseInt(cartCountText.replace(/[^\d]/g, ''));

            return {
                element: productCard,
                price: price,
                cartCount: cartCount,
                originalPosition: Array.from(productsGrid.children).indexOf(productCard)
            };
        });
    }

    // Toggle filter panel
    filterBtn.addEventListener('click', function (e) {
        e.stopPropagation();
        filterPanel.classList.toggle('active');
    });

    // Close filter panel when clicking outside
    document.addEventListener('click', function () {
        filterPanel.classList.remove('active');
    });

    filterPanel.addEventListener('click', function (e) {
        e.stopPropagation();
    });

    // Apply filters and sorting when inputs change
    [minPriceInput, maxPriceInput].forEach(input => {
        input.addEventListener('input', function () {
            // Add slight delay for better performance
            setTimeout(applyFiltersAndSorting, 300);
        });
    });

    sortSelect.addEventListener('change', applyFiltersAndSorting);

    // Clear filters
    clearFilterBtn.addEventListener('click', function () {
        minPriceInput.value = '';
        maxPriceInput.value = '';
        sortSelect.value = 'popular';
        applyFiltersAndSorting();
        updateFilterIndicator(0);
    });

    function applyFiltersAndSorting() {
        const minPrice = minPriceInput.value ? parseFloat(minPriceInput.value.replace(/[^\d.,]/g, '').replace(',', '.')) : 0;
        const maxPrice = maxPriceInput.value ? parseFloat(maxPriceInput.value.replace(/[^\d.,]/g, '').replace(',', '.')) : Infinity;
        const sortOption = sortSelect.value;

        // Count active filters
        activeFilters = 0;
        if (minPrice > 0) activeFilters++;
        if (maxPrice < Infinity) activeFilters++;
        updateFilterIndicator(activeFilters);

        // Initialize products if not done yet
        if (products.length === 0) {
            initializeProducts();
        }

        // Filter products by price
        const filteredProducts = products.filter(product => {
            return product.price >= minPrice && product.price <= maxPrice;
        });

        // Sort products
        const sortedProducts = [...filteredProducts];
        switch (sortOption) {
            case 'price-high-to-low':
                sortedProducts.sort((a, b) => b.price - a.price);
                break;
            case 'price-low-to-high':
                sortedProducts.sort((a, b) => a.price - b.price);
                break;
            case 'popular':
            default:
                sortedProducts.sort((a, b) => b.cartCount - a.cartCount);
                break;
        }

        // Update the DOM without animation if no products
        if (sortedProducts.length === 0) {
            productsGrid.innerHTML = '<p class="no-products">Filtreleme kriterlerinize uygun ürün bulunamadı.</p>';
            return;
        }

        // Simple fade animation for sorting
        productsGrid.style.opacity = '0';

        setTimeout(() => {
            productsGrid.innerHTML = '';
            sortedProducts.forEach(product => {
                productsGrid.appendChild(product.element);
            });

            productsGrid.style.opacity = '1';
        }, 300);
    }

    function updateFilterIndicator(count) {
        filterIndicator.textContent = count;
        filterIndicator.style.display = count > 0 ? 'block' : 'none';
    }

    // Initialize
    initializeProducts();
    applyFiltersAndSorting();
});