// Tüm sepete ekle butonları için event listener ekleme
['.slider-add-to-cart-btn', '.add-to-cart-btn-category', '.add-to-cart-btn-trend', '.add-to-cart-btn', '.add-to-cart'].forEach(selector => {
    document.querySelectorAll(selector).forEach(button => {
        button.addEventListener('click', function (e) {
            // user-info div'i yoksa hiçbir şey yapma (sessizce çık)
            if (!document.querySelector('.user-info')) return;

            e.preventDefault();
            e.stopPropagation();
            const productId = this.getAttribute('data-product-id');
            const refresh = this.getAttribute('data-refresh');
            const quantity = parseInt(this.getAttribute('data-quantity-id')) || 1;

            Swal.fire({
                title: 'Lütfen Bekleyin',
                html: 'Ürün sepete ekleniyor...',
                allowOutsideClick: false,
                didOpen: () => Swal.showLoading()
            });

            fetch('/ProductShoppingCard/AddToCart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({ productId, quantity })
            })
                .then(response => {
                    if (!response.ok) throw new Error('Hata oluştu');
                    return response.json();
                })
                .then(data => {
                    Swal.fire({
                        icon: 'success',
                        title: 'Başarılı!',
                        text: data.message || 'Ürün sepete eklendi',
                        timer: 2000,
                        showConfirmButton: false
                    });

                    const cartCountElement = document.querySelector('.cart-count');
                    if (cartCountElement) {
                        cartCountElement.textContent = (parseInt(cartCountElement.textContent) || 0) + quantity;
                        cartCountElement.classList.add('cart-pulse');
                        setTimeout(() => cartCountElement.classList.remove('cart-pulse'), 500);
                    }

                    if (refresh === 'refresh') {
                        window.location.reload();
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata!',
                        text: 'Ürün sepete eklenirken bir hata oluştu',
                        timer: 2000
                    });
                });
        });
    });
});