document.addEventListener('DOMContentLoaded', function () {
    // DOM Elements
    const loadingScreen = document.querySelector('.loading-screen');
    const orderDetailModal = document.getElementById('orderDetailModal');
    const orderDetailContent = document.querySelector('.order-detail-content');

    // Hide loading screen after 1 second
    setTimeout(hideLoadingScreen, 1000);

    function hideLoadingScreen() {
        if (loadingScreen) {
            loadingScreen.style.display = 'none';
            loadingScreen.style.opacity = '0';
            loadingScreen.style.visibility = 'hidden';
        }
    }

    // Order detail modal functionality
    document.querySelectorAll('.order-detail-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            const orderCard = this.closest('.order-card');
            if (!orderCard) return;

            const orderId = orderCard.querySelector('.order-id')?.textContent || '';
            const orderDateText = orderCard.querySelector('.order-date')?.textContent || '';
            const orderDate = parseOrderDate(orderDateText.trim());
            const orderStatus = orderCard.querySelector('.order-status')?.textContent || '';
            const orderTotal = orderCard.querySelector('.total-amount')?.textContent || '';
            const statusClass = orderCard.querySelector('.order-status')?.className.replace('order-status ', '') || '';
            const discountAmount = orderCard.querySelector('.private-discount-amount')?.textContent || '₺0,00';
            const totalPrice = orderCard.querySelector('.private-total-price')?.textContent || '₺0,00';

            const products = [];
            orderCard.querySelectorAll('.order-product').forEach(product => {
                products.push({
                    name: product.querySelector('.product-name')?.textContent || '',
                    description: product.querySelector('.product-sku')?.textContent || '',
                    quantity: product.querySelector('.product-quantity')?.textContent || '',
                    price: product.querySelector('.product-price')?.textContent || '',
                    image: product.querySelector('.product-image img')?.src || ''
                });
            });

            renderOrderModal({
                orderId,
                orderDate,
                orderStatus,
                orderTotal,
                statusClass,
                products,
                discountAmount,
                totalPrice
            });
        });
    });

    function parseOrderDate(dateText) {
        // Example: "15 Mayıs 2023" -> Date object
        const turkishMonths = {
            'Ocak': 0, 'Şubat': 1, 'Mart': 2, 'Nisan': 3, 'Mayıs': 4, 'Haziran': 5,
            'Temmuz': 6, 'Ağustos': 7, 'Eylül': 8, 'Ekim': 9, 'Kasım': 10, 'Aralık': 11
        };

        const parts = dateText.split(' ');
        if (parts.length < 3) return new Date(); // Fallback to current date if format is wrong

        const day = parseInt(parts[0]);
        const monthName = parts[1];
        const year = parseInt(parts[2]);
        const month = turkishMonths[monthName] || 0;

        return new Date(year, month, day);
    }

    function renderOrderModal(data) {
        if (!orderDetailContent || !orderDetailModal) return;

        // Calculate timeline dates (1 day between each step)
        const orderCreatedDate = new Date(data.orderDate);
        const paymentApprovedDate = new Date(orderCreatedDate);
        paymentApprovedDate.setDate(orderCreatedDate.getDate() + 1);

        const preparingDate = new Date(paymentApprovedDate);
        preparingDate.setDate(paymentApprovedDate.getDate() + 1);

        const shippedDate = new Date(preparingDate);
        shippedDate.setDate(preparingDate.getDate() + 1);

        const deliveredDate = new Date(shippedDate);
        deliveredDate.setDate(shippedDate.getDate() + 1);

        const cancelledDate = new Date(orderCreatedDate);
        cancelledDate.setDate(orderCreatedDate.getDate() + 1);

        // Determine which statuses are active/completed
        const isCancelled = data.orderStatus.includes('İptal Edildi');
        const isDelivered = data.orderStatus.includes('Teslim Edildi');
        const isShipped = data.orderStatus.includes('Kargoda') || isDelivered;
        const isPreparing = data.orderStatus.includes('Hazırlanıyor') || isShipped || isDelivered;
        const isPending = data.orderStatus.includes('Onay Bekleniyor');

        // Generate timeline HTML
        let timelineHTML = `
        <div class="timeline-item ${isPending ? 'active' : 'completed'}">
            <div class="timeline-date">${formatDateTime(orderCreatedDate)}</div>
            <div class="timeline-status">Sipariş Oluşturuldu</div>
            <div class="timeline-description">Siparişiniz başarıyla alındı.</div>
        </div>`;

        if (isCancelled) {
            timelineHTML += `
        <div class="timeline-item completed">
            <div class="timeline-date">${formatDateTime(cancelledDate)}</div>
            <div class="timeline-status">Sipariş İptal Edildi</div>
            <div class="timeline-description">Siparişiniz iptal edildi.</div>
        </div>`;
        } else {
            timelineHTML += `
        <div class="timeline-item ${isPreparing || isShipped || isDelivered ? 'completed' : (isPending ? '' : 'active')}">
            <div class="timeline-date">${formatDateTime(paymentApprovedDate)}</div>
            <div class="timeline-status">Ödeme Onaylandı</div>
            <div class="timeline-description">Ödemeniz başarıyla alındı.</div>
        </div>
        
        <div class="timeline-item ${isPreparing || isShipped || isDelivered ? 'completed' : (isPending ? '' : 'active')}">
            <div class="timeline-date">${formatDateTime(preparingDate)}</div>
            <div class="timeline-status">Sipariş Hazırlanıyor</div>
            <div class="timeline-description">Siparişiniz hazırlanma aşamasında.</div>
        </div>
        
        <div class="timeline-item ${isShipped || isDelivered ? 'completed' : (isPreparing ? 'active' : '')}">
            <div class="timeline-date">${formatDateTime(shippedDate)}</div>
            <div class="timeline-status">Kargoya Verildi</div>
            <div class="timeline-description">Siparişiniz kargoya verildi.</div>
        </div>`;

            if (isDelivered) {
                timelineHTML += `
            <div class="timeline-item completed">
                <div class="timeline-date">${formatDateTime(deliveredDate)}</div>
                <div class="timeline-status">Teslim Edildi</div>
                <div class="timeline-description">Siparişiniz teslim edildi.</div>
            </div>`;
            }
        }

        orderDetailContent.innerHTML = `
            <div class="order-detail-header">
                <div class="detail-order-id">${data.orderId}</div>
                <div class="detail-order-date"><i class="far fa-calendar-alt"></i> ${formatDate(data.orderDate)}</div>
                <div class="detail-order-status ${data.statusClass}">
                    ${data.orderStatus}
                </div>
            </div>
            
            <div class="order-timeline">
                ${timelineHTML}
            </div>
            
            <div class="order-detail-products">
                <h3><i class="fas fa-box-open"></i> Ürünler (${data.products.length})</h3>
                ${data.products.map(product => `
                    <div class="detail-product">
                        <div class="detail-product-image">
                            <img src="${product.image}" alt="${product.name}" onerror="this.src='https://via.placeholder.com/80'">
                        </div>
                        <div class="detail-product-info">
                            <h4>${product.name}</h4>
                            <div>${product.description}</div>
                            <div>${product.quantity}</div>
                            <div class="detail-product-price">${product.price}</div>
                        </div>
                    </div>
                `).join('')}
            </div>
            
            <div class="order-detail-summary">
                <h3><i class="fas fa-receipt"></i> Sipariş Özeti</h3>
                <div class="summary-row">
                    <span>Ürünler Toplamı:</span>
                    <span>${data.totalPrice}</span>
                </div>
                <div class="summary-row">
                    <span>Kargo Ücreti:</span>
                    <span>₺0,00</span>
                </div>
                <div class="summary-row">
                    <span>İndirim:</span>
                    <span>-${data.discountAmount}</span>
                </div>
                <div class="summary-row total">
                    <span>Toplam:</span>
                    <span>${data.orderTotal}</span>
                </div>
            </div>
            
            <div class="order-detail-actions">
                <button class="order-detail-btn close"><i class="fas fa-times"></i> Kapat</button>
            </div>
        `;

        orderDetailModal.style.display = 'flex';

        // Add event listener to close button
        const closeBtn = orderDetailContent.querySelector('.close');
        if (closeBtn) {
            closeBtn.addEventListener('click', () => {
                orderDetailModal.style.display = 'none';
            });
        }
    }

    function formatDate(date) {
        if (!(date instanceof Date)) return '';
        const options = { day: 'numeric', month: 'long', year: 'numeric' };
        return date.toLocaleDateString('tr-TR', options);
    }

    function formatDateTime(date) {
        if (!(date instanceof Date)) return '';
        const options = {
            day: 'numeric',
            month: 'long',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        };
        return date.toLocaleDateString('tr-TR', options);
    }

    // Modal close functionality
    document.querySelectorAll('.close-modal').forEach(btn => {
        btn.addEventListener('click', () => {
            if (orderDetailModal) orderDetailModal.style.display = 'none';
        });
    });

    window.addEventListener('click', (event) => {
        if (event.target === orderDetailModal) {
            orderDetailModal.style.display = 'none';
        }
    });
});