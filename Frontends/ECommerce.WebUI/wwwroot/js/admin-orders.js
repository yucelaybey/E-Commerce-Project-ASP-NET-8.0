document.addEventListener('DOMContentLoaded', function () {
    // Modal elements
    const modal = document.getElementById('orderDetailModal');
    const modalContent = modal.querySelector('.modal-content');
    const closeModalButtons = document.querySelectorAll('.close-modal');

    // Open modal with animation when view button is clicked
    document.querySelectorAll('.btn-view').forEach(button => {
        button.addEventListener('click', function () {
            const orderId = this.getAttribute('data-id');
            showLoadingAlert();
            loadOrderDetails(orderId)
                .then(() => {
                    // Add active class for animation
                    modal.classList.add('active');
                    modal.style.display = 'flex';

                    // Small delay for display to take effect before animation
                    setTimeout(() => {
                        modalContent.style.transform = 'scale(1)';
                        modalContent.style.opacity = '1';
                    }, 10);

                    Swal.close();
                })
                .catch(error => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata!',
                        text: 'Sipariş detayları yüklenirken bir hata oluştu: ' + error.message,
                        confirmButtonText: 'Tamam'
                    });
                });
        });
    });

    // Close modal with animation
    function closeModal() {
        // Start animation
        modalContent.style.transform = 'scale(0.9)';
        modalContent.style.opacity = '0';

        // Wait for animation to finish before hiding
        setTimeout(() => {
            modal.classList.remove('active');
            modal.style.display = 'none';
        }, 300);
    }

    // Close modal when X or close button is clicked
    closeModalButtons.forEach(button => {
        button.addEventListener('click', closeModal);
    });

    // Close modal when clicking outside the modal content
    modal.addEventListener('click', function (event) {
        if (event.target === modal) {
            closeModal();
        }
    });

    // Save order status button click handler
    document.getElementById('saveOrderStatusBtn').addEventListener('click', function () {
        updateOrderStatus();
    });

    // Function to show loading alert
    function showLoadingAlert() {
        Swal.fire({
            title: 'Bilgiler getiriliyor...',
            html: 'Lütfen bekleyiniz',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
    }

    // Function to load order details
    async function loadOrderDetails(orderId) {
        try {
            // Show loading state in modal
            document.getElementById('modalOrderNumber').textContent = 'Yükleniyor...';
            const numericOrderId = parseInt(orderId);

            // First, get the order details
            const orderResponse = await fetch(`/YoneticiPaneli/SiparisleriGetir`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ orderId: numericOrderId })
            });

            if (!orderResponse.ok) {
                throw new Error('Sipariş bilgileri alınamadı');
            }

            const orderData = await orderResponse.json();

            // Set the current order ID
            document.getElementById('currentOrderId').value = orderId;

            // Display basic order info
            document.getElementById('modalOrderNumber').textContent = orderData.order.orderNumber;
            document.getElementById('orderDate').textContent = new Date(orderData.order.orderDate).toLocaleString('tr-TR');
            document.getElementById('orderTotal').textContent = formatCurrency(orderData.order.totalSalePrice);
            document.getElementById('orderDiscount').textContent = formatCurrency(orderData.order.discountAmount);
            document.getElementById('paymentMethod').textContent = orderData.order.paymentMethod;

            // Set order status dropdown
            document.getElementById('orderStatus').value = orderData.order.orderStatus;

            // Display customer info
            document.getElementById('customerName').textContent = orderData.customer.nameSurname;
            document.getElementById('customerPhone').textContent = orderData.customer.phoneNumber;
            document.getElementById('customerEmail').textContent = orderData.customer.email;

            // Format and display address
            const address = orderData.customer.getOrderPaymentAddressDto;
            const formattedAddress = `${address.addressTitle}, ${address.city}/${address.town}, ${address.district}, ${address.addressDetails}`;
            document.getElementById('customerAddress').textContent = formattedAddress;

            // Display order items
            const orderItemsTable = document.getElementById('orderItems');
            orderItemsTable.innerHTML = '';

            orderData.orderItems.forEach(item => {
                const row = document.createElement('tr');

                const productNameCell = document.createElement('td');
                productNameCell.textContent = item.name;

                const quantityCell = document.createElement('td');
                quantityCell.textContent = item.quantity;

                const priceCell = document.createElement('td');
                const unitPrice = item.saleSeason ? item.salePrice : item.price;
                priceCell.textContent = formatCurrency(unitPrice);

                const totalCell = document.createElement('td');
                totalCell.textContent = formatCurrency(unitPrice * item.quantity);

                row.appendChild(productNameCell);
                row.appendChild(quantityCell);
                row.appendChild(priceCell);
                row.appendChild(totalCell);

                orderItemsTable.appendChild(row);
            });

            return Promise.resolve();

        } catch (error) {
            console.error('Error loading order details:', error);
            return Promise.reject(error);
        }
    }

    // Function to update order status
    async function updateOrderStatus() {
        const orderId = document.getElementById('currentOrderId').value;
        const newStatus = document.getElementById('orderStatus').value;

        const numericOrderId = parseInt(orderId);

        closeModal();

        if (!orderId) {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: 'Sipariş ID bulunamadı',
                confirmButtonText: 'Tamam'
            });
            return;
        }

        try {
            Swal.fire({
                title: 'Durum güncelleniyor...',
                html: 'Lütfen bekleyiniz',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            const response = await fetch('/YoneticiPaneli/SiparisDurumuGuncelle', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    orderId: numericOrderId,
                    newStatus: newStatus
                })
            });

            if (!response.ok) {
                throw new Error('Durum güncellenemedi');
            }

            const result = await response.json();

            Swal.close();

            if (result.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Başarılı!',
                    text: 'Sipariş durumu başarıyla güncellendi',
                    confirmButtonText: 'Tamam'
                }).then(() => {
                    window.location.reload();
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: 'Sipariş durumu güncellenirken bir hata oluştu: ' + result.message,
                    confirmButtonText: 'Tamam'
                });
            }
        } catch (error) {
            console.error('Error updating order status:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: 'Sipariş durumu güncellenirken bir hata oluştu: ' + error.message,
                confirmButtonText: 'Tamam'
            });
        }
    }

    // Helper function to format currency
    function formatCurrency(amount) {
        return new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(amount);
    }
});