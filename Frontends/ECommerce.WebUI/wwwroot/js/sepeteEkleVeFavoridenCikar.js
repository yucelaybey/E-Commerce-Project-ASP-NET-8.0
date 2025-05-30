document.querySelectorAll('.add-to-cart-remove').forEach(button => {
    button.addEventListener('click', async function () {
        const productId = this.getAttribute('data-product-id');
        const favoritesId = this.getAttribute('data-favorites-id');

        // CSRF Token'ı al (ASP.NET Core için)
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        try {
            // 1. Sepete Ekleme İsteği
            const addToCartResponse = await fetch('/ProductShoppingCard/AddToCart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ productId: productId, quantity: 1 })
            });

            if (!addToCartResponse.ok) {
                const error = await addToCartResponse.text();
                throw new Error(error || 'Sepete ekleme başarısız');
            }

            // 2. Favorilerden Kaldırma İsteği
            const removeFavoritesResponse = await fetch('/Anasayfa/Favoriler/FavoriSil', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ favoritesCardItemID: favoritesId })
            });

            if (!removeFavoritesResponse.ok) {
                const error = await removeFavoritesResponse.text();
                throw new Error(error || 'Favorilerden kaldırma başarısız');
            }

            // Başarılı Mesajı ve sayfa yenileme
            Swal.fire({
                icon: 'success',
                title: 'İşlem Tamam!',
                text: 'Ürün sepete eklendi ve favorilerden kaldırıldı.',
                confirmButtonText: 'Tamam'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.reload();
                }
            });

        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: error.message || 'Beklenmeyen bir hata oluştu',
                confirmButtonText: 'Tamam'
            });
        }
    });
});