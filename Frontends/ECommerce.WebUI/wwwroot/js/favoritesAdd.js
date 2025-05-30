document.addEventListener('DOMContentLoaded', function () {
    // Tüm favori butonlarına click eventi ekle
    document.querySelectorAll('.slider-favorite-btn, .save-for-later').forEach(button => {
        button.addEventListener('click', async function (event) {
            event.preventDefault();
            if (!document.querySelector('.user-info')) return;

            // Data attribute'larını al
            let favoritesCardItemID = this.getAttribute('data-favoritesCardItemID');
            let favoritesCardID = this.getAttribute('data-favoritesCardID');
            const productID = this.getAttribute('data-productID');
            const spanElement = this.querySelector('.shopping');
            const favorites = this.getAttribute('data-favorites'); // 'favorites' veya null
            const recommended = this.getAttribute('data-recommended'); // 'recommended' veya null
            const icon = this.querySelector('i');

            try {
                let url, body;

                // Favori durumunu kontrol et (ikon class'ı veya data-favorites attribute'u)
                const isFavorite = icon.classList.contains('fas');

                // Eğer favori DEĞİLSE ekleme işlemi
                if (!isFavorite) {
                    url = '/Anasayfa/Favoriler/FavoriEkle';
                    body = {
                        favoritesCardItemID: favoritesCardItemID || null,
                        favoritesCardID: favoritesCardID || null,
                        productID: parseInt(productID),
                        favorites: favorites || null,
                        recommended: recommended || null
                    };
                }
                // Eğer favori İSE çıkarma işlemi
                else {
                    url = '/Anasayfa/Favoriler/FavoriSil';
                    body = {
                        favoritesCardItemID: favoritesCardItemID,
                        favoritesCardID: favoritesCardID,
                        productID: parseInt(productID),
                        favorites: favorites,
                        recommended: recommended || null
                    };
                }

                // Fetch API ile istek yap
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify(body)
                });

                // Response kontrolü
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const result = await response.json();

                // Başarılıysa
                if (result.success) {
                    // Eğer ürün zaten favorilerdeyse özel mesaj göster
                    if (result.isAlreadyFavorite) {
                        Swal.fire({
                            icon: 'info',
                            title: 'Bilgi!',
                            text: result.message || 'Ürün zaten favorilerinizde',
                            timer: 2000,
                            showConfirmButton: false
                        });

                        // Tüm verileri güncelle
                        this.setAttribute('data-favoritesCardItemID', result.favoritesCardItemID || '');
                        this.setAttribute('data-favoritesCardID', result.favoritesCardID || '');
                        this.setAttribute('data-favorites', 'favorites');
                        spanElement.textContent = 'Favorilerden Çıkar';
                        icon.classList.remove('far');
                        icon.classList.add('fas');
                        return;
                    }

                    // Normal ekleme/çıkarma işlemi için SweetAlert göster
                    await Swal.fire({
                        icon: 'success',
                        title: 'Başarılı!',
                        text: !isFavorite ? 'Ürün favorilere eklendi.' : 'Ürün favorilerden çıkarıldı.',
                        timer: 2000,
                        showConfirmButton: false
                    });

                    // İkonu ve data attribute'ları güncelle
                    icon.classList.toggle('far');
                    icon.classList.toggle('fas');

                    // ID'leri güncelle (varsa)
                    if (result.favoritesCardItemID) {
                        this.setAttribute('data-favoritesCardItemID', result.favoritesCardItemID);
                    }
                    if (result.favoritesCardID) {
                        this.setAttribute('data-favoritesCardID', result.favoritesCardID);
                    }

                    // Eğer recommended varsa veya favorites varsa sayfayı yenile
                    if ((recommended === 'recommended' || favorites === 'favorites' || favorites === 'shopping') && result.success) {
                        window.location.reload();
                    }
                } else {
                    throw new Error(result.message || 'İşlem başarısız oldu.');
                }
            } catch (error) {
                console.error('Hata detayı:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: error.message || 'Bir şeyler yanlış gitti. Lütfen konsolu kontrol edin.'
                });
            }
        });
    });
});