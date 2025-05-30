document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.querySelector('.search-input');
    const searchBtn = document.querySelector('.search-btn');

    // Arama fonksiyonu
    function performSearch() {
        const searchTerm = searchInput.value.trim();
        if (searchTerm.length > 0) {
            window.location.href = `/Search/Anasayfa?search=${encodeURIComponent(searchTerm)}`;
        }
    }

    // Enter tuşu dinleyicisi
    searchInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            performSearch();
        }
    });

    // Arama butonu dinleyicisi
    searchBtn.addEventListener('click', performSearch);
});