document.addEventListener('DOMContentLoaded', function () {
    // DOM Elements
    const orderStatusFilter = document.getElementById('orderStatus');
    const orderDateFilter = document.getElementById('orderDate');
    const ordersContainer = document.querySelector('.orders-list');

    // Tüm sipariş kartlarını al
    const allOrders = Array.from(ordersContainer.querySelectorAll('.order-card'));

    // Filtreleme fonksiyonu
    function applyFilters() {
        const statusFilter = orderStatusFilter.value;
        const dateFilter = orderDateFilter.value;

        // Tüm siparişleri gizle
        allOrders.forEach(order => order.style.display = 'none');

        // Filtreleme işlemi
        const filteredOrders = allOrders.filter(order => {
            const statusMatch = checkStatusFilter(order, statusFilter);
            const dateMatch = checkDateFilter(order, dateFilter);
            return statusMatch && dateMatch;
        });

        // Filtrelenmiş siparişleri göster
        filteredOrders.forEach(order => order.style.display = 'block');

        // Sıralama (seçilen filtreye göre önceliklendirme)
        sortOrders(filteredOrders, statusFilter);
    }

    // Durum filtresi kontrolü
    function checkStatusFilter(order, statusFilter) {
        if (statusFilter === 'all') return true;

        const statusElement = order.querySelector('.order-status');
        if (!statusElement) return false;

        const statusText = statusElement.textContent.toLowerCase();

        switch (statusFilter) {
            case 'pending': return statusText.includes('onay bekleniyor');
            case 'preparing': return statusText.includes('hazırlanıyor');
            case 'shipped': return statusText.includes('kargoda');
            case 'delivered': return statusText.includes('teslim edildi');
            case 'cancelled': return statusText.includes('iptal edildi');
            default: return true;
        }
    }

    // Tarih filtresi kontrolü
    function checkDateFilter(order, dateFilter) {
        if (dateFilter === 'all') return true;

        const dateElement = order.querySelector('.order-date');
        if (!dateElement) return false;

        const dateText = dateElement.textContent.trim();
        const orderDate = parseTurkishDate(dateText);
        const today = new Date();

        switch (dateFilter) {
            case 'month':
                const oneMonthAgo = new Date(today);
                oneMonthAgo.setMonth(oneMonthAgo.getMonth() - 1);
                return orderDate >= oneMonthAgo;

            case '3months':
                const threeMonthsAgo = new Date(today);
                threeMonthsAgo.setMonth(threeMonthsAgo.getMonth() - 3);
                return orderDate >= threeMonthsAgo;

            case 'year':
                const oneYearAgo = new Date(today);
                oneYearAgo.setFullYear(oneYearAgo.getFullYear() - 1);
                return orderDate >= oneYearAgo;

            default: return true;
        }
    }

    // Türkçe tarihi Date objesine çevirme
    function parseTurkishDate(dateStr) {
        const months = {
            'Ocak': 0, 'Şubat': 1, 'Mart': 2, 'Nisan': 3, 'Mayıs': 4, 'Haziran': 5,
            'Temmuz': 6, 'Ağustos': 7, 'Eylül': 8, 'Ekim': 9, 'Kasım': 10, 'Aralık': 11
        };

        const parts = dateStr.split(' ');
        const day = parseInt(parts[0]);
        const month = months[parts[1]];
        const year = parseInt(parts[2]);

        return new Date(year, month, day);
    }

    // Siparişleri sıralama (seçilen filtreye göre önceliklendirme)
    function sortOrders(orders, statusFilter) {
        if (statusFilter === 'all') {
            // Tüm siparişlerde en yeniden en eskiye sırala
            orders.sort((a, b) => {
                const dateA = parseTurkishDate(a.querySelector('.order-date').textContent.trim());
                const dateB = parseTurkishDate(b.querySelector('.order-date').textContent.trim());
                return dateB - dateA;
            });
        } else {
            // Seçilen filtreye uyanları en üste, diğerlerini tarihe göre sırala
            orders.sort((a, b) => {
                const aMatches = checkStatusFilter(a, statusFilter);
                const bMatches = checkStatusFilter(b, statusFilter);

                if (aMatches && !bMatches) return -1;
                if (!aMatches && bMatches) return 1;

                const dateA = parseTurkishDate(a.querySelector('.order-date').textContent.trim());
                const dateB = parseTurkishDate(b.querySelector('.order-date').textContent.trim());
                return dateB - dateA;
            });
        }

        // DOM'da sıralamayı güncelle
        orders.forEach(order => {
            ordersContainer.appendChild(order);
        });
    }

    // Event listeners
    orderStatusFilter.addEventListener('change', applyFilters);
    orderDateFilter.addEventListener('change', applyFilters);

    // Sayfa yüklendiğinde filtreleme yap
    applyFilters();
});