document.addEventListener('DOMContentLoaded', function () {
    const addCardBtn = document.getElementById('add-card-btn');
    const closeModalBtn = document.getElementById('close-modal');
    const modal = document.getElementById('add-card-modal');
    const cardForm = document.getElementById('card-form');
    const cardNumberInput = document.getElementById('card-number');
    const cardExpiryInput = document.getElementById('card-expiry');
    const cardCvvInput = document.getElementById('card-cvv');

    // Modal açma/kapatma
    addCardBtn.addEventListener('click', () => {
        modal.style.display = 'flex';
    });

    closeModalBtn.addEventListener('click', () => {
        modal.style.display = 'none';
    });

    window.addEventListener('click', (event) => {
        if (event.target === modal) {
            modal.style.display = 'none';
        }
    });

    // Kart numarası formatlama (16 haneyi 4'lü gruplara ayırma)
    cardNumberInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, ''); // Sadece rakamları al
        value = value.match(/.{1,4}/g)?.join(' ') || ''; // 4'lü gruplara ayır
        e.target.value = value;
    });

    // Son kullanma tarihi formatlama (AA/YY)
    cardExpiryInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, ''); // Sadece rakamları al
        if (value.length > 2) {
            value = value.slice(0, 2) + '/' + value.slice(2, 4); // AA/YY formatı
        }
        e.target.value = value;
    });

    // CVV formatlama (max 3 hane)
    cardCvvInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, ''); // Sadece rakamları al
        e.target.value = value.slice(0, 3); // Max 3 hane
    });

    // Kart formu gönderimi
    cardForm.addEventListener('submit', function (e) {
        e.preventDefault();
        alert('Kart başarıyla kaydedildi!');
        modal.style.display = 'none';
    });
});