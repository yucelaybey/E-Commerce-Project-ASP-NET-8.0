document.addEventListener('DOMContentLoaded', function () {
    const addressModal = document.getElementById('addressModal');
    if (!addressModal) return;

    // Modal işlemleri
    const openModal = () => {
        addressModal.style.display = 'flex';
        document.body.style.overflow = 'hidden';
    };

    const closeModal = () => {
        addressModal.style.display = 'none';
        document.body.style.overflow = 'auto';
    };

    // Tüm adres değiştir butonlarını yakala
    document.addEventListener('click', function (e) {
        if (e.target.id === 'changeAddressBtn' || e.target.classList.contains('change-address')) {
            openModal();
        }
    });

    // Modal kapatma
    document.querySelector('.close-modal')?.addEventListener('click', closeModal);
    addressModal.addEventListener('click', (e) => e.target === addressModal && closeModal());

    // Adres seçim işlemi
    document.querySelectorAll('.address-option').forEach(option => {
        option.addEventListener('click', function () {
            document.querySelectorAll('.address-option').forEach(opt => {
                opt.classList.remove('selected');
                const radio = opt.querySelector('input[type="radio"]');
                if (radio) radio.checked = false;
            });
            this.classList.add('selected');
            const radio = this.querySelector('input[type="radio"]');
            if (radio) radio.checked = true;
        });
    });

    // Adres onaylama
    document.querySelector('.select-address-btn')?.addEventListener('click', async function () {
        const selectedOption = document.querySelector('.address-option.selected');
        if (!selectedOption) {
            Swal.fire('Hata', 'Lütfen bir adres seçiniz', 'error');
            return;
        }

        const addressId = selectedOption.dataset.id;
        if (!addressId) return;

        try {
            // API isteği
            const response = await fetch('/Addresses/SetDefaultAddress', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ addressId: parseInt(addressId) })
            });

            if (!response.ok) throw new Error('İşlem başarısız');

            // Seçilen adresin bilgilerini al
            const name = selectedOption.querySelector('.address-details h3')?.textContent.trim() || '';
            const addressDetails = selectedOption.querySelector('.address-details p:nth-of-type(1)')?.textContent || '';
            const townCity = selectedOption.querySelector('.address-details p:nth-of-type(2)')?.textContent || '';
            const phone = selectedOption.querySelector('.address-details p:nth-of-type(3)')?.textContent || '';

            // Delivery section'ı bul
            const deliverySection = document.querySelector('.delivery-section');
            if (!deliverySection) return;

            // Eğer "varsayılan adres yok" mesajı varsa onu kaldır
            const noAddressMsg = deliverySection.querySelector('p');
            if (noAddressMsg && noAddressMsg.textContent.includes('Varsayılan bir adresiniz bulunmamaktadır')) {
                deliverySection.removeChild(noAddressMsg);
                const changeAddressBtn = deliverySection.querySelector('.change-address');
                if (changeAddressBtn) deliverySection.removeChild(changeAddressBtn);
            }

            // Yeni adres kartı HTML'i oluştur
            const newAddressHTML = `
                <div class="address-card selected" data-address-id="${addressId}">
                    <div class="address-header">
                        <h3>${name}</h3>
                        <span class="default-badge">Varsayılan</span>
                    </div>
                    <p>${addressDetails}</p>
                    <p>${townCity}</p>
                    <p>${phone}</p>
                    <button class="change-address" id="changeAddressBtn">Adresi Değiştir</button>
                </div>
            `;

            // Mevcut address-card'ı bul veya yeni oluştur
            const existingCard = deliverySection.querySelector('.address-card');
            if (existingCard) {
                existingCard.outerHTML = newAddressHTML;
            } else {
                deliverySection.insertAdjacentHTML('beforeend', newAddressHTML);
            }

            closeModal();
            Swal.fire('Başarılı', 'Adres güncellendi', 'success');
        } catch (error) {
            Swal.fire('Hata', error.message, 'error');
        }
    });
});