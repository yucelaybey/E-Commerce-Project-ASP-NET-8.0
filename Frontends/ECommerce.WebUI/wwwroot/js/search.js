document.addEventListener('DOMContentLoaded', function () {
    // Arama Fonksiyonları
    const searchInput = document.querySelector('.search-input');
    const searchBtn = document.querySelector('.search-btn');
    const searchResultsContainer = document.querySelector('.search-results-container');
    const searchResults = document.querySelector('.search-results');
    const loadingIndicator = document.querySelector('.loading-indicator');
    const navSearch = document.querySelector('.nav-search');

    let searchTimeout;
    const minSearchLength = 2;
    const maxResults = 10;
    const apiBaseUrl = '/Search/'; // Controller route'unuz

    // Arama input eventleri
    searchInput.addEventListener('input', function (e) {
        clearTimeout(searchTimeout);
        const searchTerm = e.target.value.trim();

        if (searchTerm.length >= minSearchLength) {
            showLoading();
            searchTimeout = setTimeout(() => performSearch(searchTerm), 500);
        } else {
            hideResults();
        }
    });

    // Arama butonu eventi
    searchBtn.addEventListener('click', function () {
        const searchTerm = searchInput.value.trim();
        if (searchTerm.length >= minSearchLength) {
            showLoading();
            performSearch(searchTerm);
        }
    });

    // Input focus olduğunda
    searchInput.addEventListener('focus', function () {
        if (searchInput.value.trim().length >= minSearchLength) {
            searchResultsContainer.classList.add('active');
        }
    });

    // Dışarı tıklandığında sonuçları gizle
    document.addEventListener('click', function (e) {
        if (!e.target.closest('.nav-search')) {
            hideResults();
        }
    });

    // NavSearch içindeki tıklamalar
    navSearch.addEventListener('click', function (e) {
        e.stopPropagation();
    });

    function showLoading() {
        searchResultsContainer.classList.add('active');
        loadingIndicator.style.display = 'flex';
        searchResults.style.display = 'none';
    }

    function hideResults() {
        searchResultsContainer.classList.remove('active');
        loadingIndicator.style.display = 'none';
        searchResults.style.display = 'none';
    }

    async function performSearch(searchTerm) {
        try {
            // API isteklerini paralel olarak yap
            const [productsResponse, categoriesResponse] = await Promise.all([
                fetch(`${apiBaseUrl}SearchProduct?searchTerm=${encodeURIComponent(searchTerm)}`),
                fetch(`${apiBaseUrl}SearchCategory?searchTerm=${encodeURIComponent(searchTerm)}`)
            ]);

            // HTTP hatalarını kontrol et
            if (!productsResponse.ok || !categoriesResponse.ok) {
                throw new Error('API request failed');
            }

            // JSON verilerini al
            const [products, categories] = await Promise.all([
                productsResponse.json(),
                categoriesResponse.json()
            ]);

            displayResults(products, categories);
        } catch (error) {
            console.error('Search error:', error);
            displayError();
        }
    }

    function displayResults(products, categories) {
        searchResults.innerHTML = '';

        // API'den gelen verileri birleştir ve sınırla
        const allResults = [
            ...(products || []).slice(0, 7).map(p => ({
                ...p,
                type: 'product',
                id: p.productId || p.id,
                name: p.productName || p.name,
                price: p.price,
                salePrice: p.salePrice,
                imageUrl: p.imageUrl || p.imagePath,
                categoryName: p.categoryName
            })),
            ...(categories || []).slice(0, 3).map(c => ({
                ...c,
                type: 'category',
                id: c.categoryId || c.id,
                name: c.categoryName || c.name
            }))
        ].slice(0, maxResults);

        if (allResults.length === 0) {
            searchResults.innerHTML = '<div class="no-results">Sonuç bulunamadı</div>';
            loadingIndicator.style.display = 'none';
            searchResults.style.display = 'block';
            return;
        }

        // Sonuçları ekranda göster
        allResults.forEach(item => {
            const resultItem = document.createElement('div');
            resultItem.className = 'result-item';

            if (item.type === 'product') {
                resultItem.innerHTML = `
        <a href="/Anasayfa/Kategoriler/${FormatForUrl(item.categoryName)}/${FormatForUrl(item.name)}?productId=${item.productID}" style="text-decoration: none; color: inherit;">
            <div class="product-result" style="display: flex; gap: 15px; padding: 12px; border-radius: 8px; border: 1px solid #e0e0e0; margin-bottom: 10px; background: white;">
                <img src="${item.imageUrl || '/images/default-product.png'}" 
                     alt="${item.name}" 
                     style="width: 80px; height: 80px; object-fit: contain; border-radius: 4px;">
                <div class="product-info" style="flex: 1;">
                    <div class="product-name" style="font-size: 16px; font-weight: 600; color: #333; margin-bottom: 5px;">${escapeHtml(item.name)}</div>
                    <div class="product-price" style="margin-bottom: 5px;">
                        <span class="current-price" style="font-size: 15px; font-weight: 700; color: #d32f2f;">${formatPrice(item.salePrice > 0 ? item.salePrice : item.price)}</span>
                        ${item.salePrice > 0 ? `<span class="old-price" style="font-size: 13px; color: #757575; text-decoration: line-through; margin-left: 6px;">${formatPrice(item.price)}</span>` : ''}
                    </div>
                    <div class="result-type" style="font-size: 12px; color: #666;">Ürün • ${escapeHtml(item.categoryName || 'Kategori yok')}</div>
                </div>
            </div>
        </a>
    `;
            } else {
                resultItem.innerHTML = `
        <a href="/Anasayfa/Kategoriler/${FormatForUrl(item.name)}" style="text-decoration: none; color: inherit;">
            <div class="category-result" style="padding: 12px; border-radius: 8px; border: 1px solid #e0e0e0; margin-bottom: 10px; background: white;">
                <div class="category-name" style="font-size: 16px; font-weight: 600; color: #1976d2;">${escapeHtml(item.name)}</div>
                <div class="result-type" style="font-size: 12px; color: #666;">Kategori</div>
            </div>
        </a>
    `;
            }

            resultItem.addEventListener('click', () => {
                window.location.href = item.type === 'product'
                    ? `/products/${item.id}`
                    : `/categories/${item.id}`;
            });

            searchResults.appendChild(resultItem);
        });

        loadingIndicator.style.display = 'none';
        searchResults.style.display = 'block';
    }

    // URL formatlama fonksiyonu
    function FormatForUrl(text) {
        if (!text) return '';

        // Türkçe karakter dönüşümü
        const trMap = {
            'ı': 'i',
            'ğ': 'g',
            'ü': 'u',
            'ş': 's',
            'ö': 'o',
            'ç': 'c',
            'İ': 'i',
            'Ğ': 'g',
            'Ü': 'u',
            'Ş': 's',
            'Ö': 'o',
            'Ç': 'c'
        };

        text = text.toString().toLowerCase();
        for (const key in trMap) {
            text = text.replace(new RegExp(key, 'g'), trMap[key]);
        }

        // Özel karakterleri temizle
        text = text.replace(/[^a-z0-9\s-]/g, '')
            .replace(/\s+/g, '-')
            .replace(/-+/g, '-')
            .trim('-');

        return text;
    }

    function displayError() {
        searchResults.innerHTML = '<div class="no-results">Arama sırasında bir hata oluştu</div>';
        loadingIndicator.style.display = 'none';
        searchResults.style.display = 'block';
    }

    // Helper functions
    function escapeHtml(unsafe) {
        if (!unsafe) return '';
        return unsafe.toString()
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    function formatPrice(price) {
        return parseFloat(price || 0).toFixed(2) + '₺';
    }
});