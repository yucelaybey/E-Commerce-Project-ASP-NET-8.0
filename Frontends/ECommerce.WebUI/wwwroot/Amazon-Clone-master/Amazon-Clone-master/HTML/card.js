document.addEventListener('DOMContentLoaded', function () {
    const selectAllCheckbox = document.getElementById('select-all-checkbox');
    const itemCheckboxes = document.querySelectorAll('.item-checkbox');
    const cartItems = document.querySelectorAll('.cart-item');
    const quantityControls = document.querySelectorAll('.quantity-controls');
    const removeButtons = document.querySelectorAll('.remove-item');
    const totalPriceElement = document.querySelector('.total-price');
    const shippingFeeElement = document.querySelector('.shipping-fee');
    const discountAmountElement = document.querySelector('.discount-amount');
    const grandTotalPriceElement = document.querySelector('.grand-total-price');
    const freeShippingInfoElement = document.getElementById('free-shipping-info');
    const remainingAmountElement = document.querySelector('.remaining-amount');

    let totalPrice = 250; // Sepetteki toplam ürün fiyatı
    const shippingFee = 44.90; // Sabit kargo ücreti
    const freeShippingThreshold = 250; // Ücretsiz kargo eşiği

    // Tümünü Seç Fonksiyonu
    selectAllCheckbox.addEventListener('change', function () {
        itemCheckboxes.forEach(checkbox => {
            checkbox.checked = selectAllCheckbox.checked;
        });
    });

    // Her bir ürünün seçim kutusu için olay dinleyicisi
    itemCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            const allChecked = Array.from(itemCheckboxes).every(cb => cb.checked);
            selectAllCheckbox.checked = allChecked;
        });
    });

    // Ürün Sayısını Artırma/Azaltma
    quantityControls.forEach(control => {
        const decreaseBtn = control.querySelector('.decrease');
        const increaseBtn = control.querySelector('.increase');
        const quantitySpan = control.querySelector('.quantity');

        decreaseBtn.addEventListener('click', () => {
            let quantity = parseInt(quantitySpan.textContent);
            if (quantity > 1) {
                quantitySpan.textContent = quantity - 1;
            }
        });

        increaseBtn.addEventListener('click', () => {
            let quantity = parseInt(quantitySpan.textContent);
            quantitySpan.textContent = quantity + 1;
        });
    });

    // Ürün Silme
    removeButtons.forEach(button => {
        button.addEventListener('click', function () {
            this.closest('.cart-item').remove();
        });
    });

    // Toplam Fiyat Hesaplama ve Güncelleme
    function updateTotalPrice() {
        totalPriceElement.textContent = `₺${totalPrice.toFixed(2)}`;
        const grandTotal = totalPrice >= freeShippingThreshold ? totalPrice : totalPrice + shippingFee;
        grandTotalPriceElement.textContent = `₺${grandTotal.toFixed(2)}`;

        if (totalPrice >= freeShippingThreshold) {
            discountAmountElement.textContent = `-₺${shippingFee.toFixed(2)}`;
            freeShippingInfoElement.style.display = 'none';
        } else {
            discountAmountElement.textContent = '₺0.00';
            freeShippingInfoElement.style.display = 'block';
            const remainingAmount = freeShippingThreshold - totalPrice;
            remainingAmountElement.textContent = `₺${remainingAmount.toFixed(2)}`;
        }
    }

    updateTotalPrice();
});