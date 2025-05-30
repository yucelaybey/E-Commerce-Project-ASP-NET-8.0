document.addEventListener('DOMContentLoaded', function () {
    // 1. Yükleme Ekranı Yönetimi
    setTimeout(function () {
        document.querySelector('.loading-screen').style.display = 'none';
    }, 3000);

    // 2. Kart Detayları Görünürlük Yönetimi
    const cardManager = {
        init() {
            this.processAllCards();
            const showDetailsCheckbox = document.getElementById('showDetails');
            if (showDetailsCheckbox) {
                showDetailsCheckbox.addEventListener('change', (e) => {
                    document.querySelectorAll('.saved-card').forEach(card => {
                        e.target.checked ? this.showCardDetails(card) : this.hideCardDetails(card);
                    });
                });
            }
        },
        processAllCards() {
            document.querySelectorAll('.saved-card').forEach(card => {
                const numberEl = card.querySelector('.card-number');
                const cvvEl = card.querySelector('.card-cvv-number');
                if (!numberEl.dataset.full) numberEl.dataset.full = numberEl.textContent.trim();
                if (!cvvEl.dataset.full) cvvEl.dataset.full = cvvEl.textContent.trim();
                this.hideCardDetails(card);
            });
        },
        hideCardDetails(card) {
            const numberEl = card.querySelector('.card-number');
            const cvvEl = card.querySelector('.card-cvv-number');
            if (numberEl.dataset.full) {
                const parts = numberEl.dataset.full.split(' ');
                const lastFour = parts.length > 0 ? parts[parts.length - 1] : '';
                numberEl.textContent = '•••• •••• •••• ' + lastFour;
            }
            if (cvvEl.dataset.full) cvvEl.textContent = '•••';
        },
        showCardDetails(card) {
            const numberEl = card.querySelector('.card-number');
            const cvvEl = card.querySelector('.card-cvv-number');
            if (numberEl.dataset.full) numberEl.textContent = numberEl.dataset.full;
            if (cvvEl.dataset.full) cvvEl.textContent = cvvEl.dataset.full;
        }
    };

    cardManager.init();

    // 3. Sekme Değiştirme İşlevselliği
    const tabBtns = document.querySelectorAll('.tab-btn');
    tabBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            const tabId = this.getAttribute('data-tab');
            tabBtns.forEach(b => b.classList.remove('active'));
            document.querySelectorAll('.tab-content').forEach(t => t.classList.remove('active'));
            this.classList.add('active');
            document.getElementById(tabId).classList.add('active');
            cardManager.processAllCards();
        });
    });


    // 5. Input Formatlama - GÜNCELLENMİŞ VERSİYON
    document.querySelectorAll('input[placeholder="1234 5678 9012 3456"]').forEach(input => {
        input.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\s+/g, '').replace(/[^0-9]/gi, '');
            let formatted = '';
            for (let i = 0; i < value.length; i++) {
                if (i > 0 && i % 4 === 0) formatted += ' ';
                formatted += value[i];
            }
            e.target.value = formatted.substring(0, 19);
        });
    });

    document.querySelectorAll('input[placeholder="AA/YY"]').forEach(input => {
        input.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\//g, '').replace(/[^0-9]/gi, '');

            // Sadece sayısal değerleri kabul et
            e.target.value = value.replace(/[^0-9]/g, '');

            // AA/YY formatına dönüştür
            if (value.length > 2) {
                value = value.substring(0, 2) + '/' + value.substring(2, 4);
            }
            e.target.value = value.substring(0, 5);

            // Geçerlilik kontrolü
            const currentDate = new Date();
            const currentYear = currentDate.getFullYear() % 100;
            const currentMonth = currentDate.getMonth() + 1;

            if (value.length >= 4) {
                const month = parseInt(value.substring(0, 2));
                const year = parseInt(value.substring(2, 4));

                // Ay kontrolü (1-12 arası)
                if (month < 1 || month > 12) {
                    e.target.setCustomValidity('Geçerli bir ay giriniz (01-12)');
                    return;
                }

                // Tarih geçerlilik kontrolü
                if (year < currentYear || (year === currentYear && month < currentMonth)) {
                    e.target.setCustomValidity('Kartın son kullanma tarihi geçmiş');
                    return;
                }

                e.target.setCustomValidity('');
            }
        });
    });

    document.querySelectorAll('input[placeholder="•••"]').forEach(input => {
        // CVV göster/gizle butonu ekle
        const wrapper = document.createElement('div');
        wrapper.style.position = 'relative';
        input.parentNode.insertBefore(wrapper, input);
        wrapper.appendChild(input);

        const toggleBtn = document.createElement('button');
        toggleBtn.type = 'button';
        toggleBtn.innerHTML = '<i class="far fa-eye"></i>';
        toggleBtn.style.position = 'absolute';
        toggleBtn.style.right = '10px';
        toggleBtn.style.top = '50%';
        toggleBtn.style.transform = 'translateY(-50%)';
        toggleBtn.style.background = 'none';
        toggleBtn.style.border = 'none';
        toggleBtn.style.cursor = 'pointer';
        toggleBtn.style.color = '#666';
        wrapper.appendChild(toggleBtn);

        // CVV göster/gizle işlevi
        toggleBtn.addEventListener('click', function () {
            if (input.type === 'password') {
                input.type = 'text';
                toggleBtn.innerHTML = '<i class="far fa-eye-slash"></i>';
            } else {
                input.type = 'password';
                toggleBtn.innerHTML = '<i class="far fa-eye"></i>';
            }
        });

        // CVV formatlama
        input.addEventListener('input', function (e) {
            e.target.value = e.target.value.replace(/[^0-9]/g, '').substring(0, 3);
        });
    });

    document.querySelectorAll('input[placeholder="Ad Soyad"]').forEach(input => {
        input.addEventListener('input', function (e) {
            // Sadece harf ve boşluk karakterlerine izin ver
            e.target.value = e.target.value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
        });
    });

    document.querySelectorAll('.paypal-input, .apple-pay-input').forEach(input => {
        input.addEventListener('input', function (e) {
            e.target.value = e.target.value.replace(/[^0-9]/gi, '').substring(0, 10);
        });
    });

    // 6. Varsayılan Kart Değiştirme İşlemi
    document.querySelectorAll('input[type="radio"][name="saved-card"]').forEach(radio => {
        radio.addEventListener('change', async function () {
            if (!this.checked) return;
            const cardId = this.getAttribute('data-card-id');
            if (!cardId) return;

            const formData = new FormData();
            formData.append('paymentCardId', parseInt(cardId));

            try {
                const response = await fetch('/CreditCard/DefaultCreditCard', {
                    method: 'POST',
                    body: formData
                });
                const result = await response.json();
                if (!result.success) {
                    Swal.fire({ icon: 'error', title: 'Hata', text: result.message });
                    document.querySelector('input[type="radio"][name="saved-card"]:checked').checked = true;
                }
            } catch (error) {
                Swal.fire({ icon: 'error', title: 'Hata', text: 'İşlem sırasında bir hata oluştu' });
                document.querySelector('input[type="radio"][name="saved-card"]:checked').checked = true;
            }
        });
    });
});
document.querySelector('.complete-payment').addEventListener('click', async function (e) {
    e.preventDefault();

    // First check credit card form
    const creditCardTab = document.getElementById('credit-card');
    const creditCardTabcomputedStyle = window.getComputedStyle(creditCardTab);
    const paypalTab = document.getElementById('paypal');
    const paypalTabcomputedStyle = window.getComputedStyle(paypalTab);
    const applePayTab = document.getElementById('apple-pay');
    const applePaycomputedStyle = window.getComputedStyle(applePayTab);
    const savedCardsTab = document.getElementById('saved-cards');
    const computedStyle = window.getComputedStyle(savedCardsTab);

    // Seçili adresi al
    const selectedAddress = document.querySelector('.address-card.selected');

    if (!selectedAddress) {
        Swal.fire('Hata', 'Lütfen bir adres seçiniz', 'error');
        return;
    }

    const addressId = selectedAddress.dataset.addressId;

    let paymentData = null;
    let paymentMethod = null;
    
    // Check which payment method is active
    if (creditCardTabcomputedStyle.display === 'block') {
        paymentData = validateCreditCard();
        paymentMethod = 'Kredi/Banka Kartı';
    } else if (paypalTabcomputedStyle.display === 'block') {
        paymentData = validatePaypal();
        paymentMethod = 'Paypal';
    } else if (applePaycomputedStyle.display === 'block') {
        paymentData = validateApplePay();
        paymentMethod = 'ApplePay';
    } else if (computedStyle.display === 'block') {
        paymentData = validateSavedCard();
        paymentMethod = 'Kredi/Banka Kartı';
    }

    if (!paymentData) {
        return; // Validation failed, errors already shown
    }

    try {
        // Get cart data
        const cartResponse = await fetch('/Anasayfa/Sepetim/GetCartData');
        const cartData = await cartResponse.json();

        if (!cartData.success) {
            Swal.fire('Hata', 'Sepet bilgileri alınamadı', 'error');
            return;
        }

        // Prepare order data
        const orderData = {
            paymentMethod: paymentMethod,
            paymentData: paymentData,
            cartData: cartData.data,
            selectedAddressId: parseInt(addressId)
        };

        // Submit the order
        const response = await fetch('/Anasayfa/Sepetim/PaymentOrderSave', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(orderData)
        }).then(response => response.json())
            .then(data => {
                if (data.success) {
                    window.location.href = `/Anasayfa/Sepetim/SiparisOnaylandi?orderId=${data.orderId}`;
                }
            });
    } catch (error) {
        Swal.fire('Hata', 'Bir hata oluştu: ' + error.message, 'error');
    }
});

function validateCreditCard() {
    const cardNumber = document.querySelector('#credit-card input[placeholder="1234 5678 9012 3456"]').value.replace(/\s/g, '');
    const expiryDate = document.querySelector('#credit-card input[placeholder="AA/YY"]').value;
    const cvv = document.querySelector('#credit-card input[placeholder="•••"]').value;
    const cardName = document.querySelector('#credit-card input[placeholder="Ad Soyad"]').value;

    // Validate card number (16 digits)
    if (cardNumber.length !== 16 || !/^\d+$/.test(cardNumber)) {
        Swal.fire('Hata', 'Geçersiz kart numarası. Lütfen 16 haneli kart numaranızı girin.', 'error');
        return null;
    }

    // Validate expiry date (MM/YY format and not expired)
    if (!/^\d{2}\/\d{2}$/.test(expiryDate)) {
        Swal.fire('Hata', 'Geçersiz son kullanma tarihi formatı. AA/YY formatında girin.', 'error');
        return null;
    }

    const [month, year] = expiryDate.split('/').map(Number);
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear() % 100;
    const currentMonth = currentDate.getMonth() + 1;

    if (month < 1 || month > 12) {
        Swal.fire('Hata', 'Geçersiz ay bilgisi. Ay 01-12 arasında olmalıdır.', 'error');
        return null;
    }

    if (year < currentYear || (year === currentYear && month < currentMonth)) {
        Swal.fire('Hata', 'Kartın son kullanma tarihi geçmiş.', 'error');
        return null;
    }

    // Validate CVV (3 digits)
    if (cvv.length !== 3 || !/^\d+$/.test(cvv)) {
        Swal.fire('Hata', 'Geçersiz CVV numarası. Lütfen 3 haneli CVV numaranızı girin.', 'error');
        return null;
    }

    // Validate card name
    if (!cardName.trim()) {
        Swal.fire('Hata', 'Lütfen kart üzerindeki ismi girin.', 'error');
        return null;
    }

    return {
        cardNumber: cardNumber,
        expiryDate: expiryDate,
        cvv: cvv,
        cardHolderName: cardName,
        saveCard: document.querySelector('#credit-card #saveCard').checked
    };
}

function validatePaypal() {
    const paypalNumber = document.querySelector('#paypal input[placeholder="PayPal numarası"]').value;

    if (paypalNumber.length !== 10 || !/^\d+$/.test(paypalNumber)) {
        Swal.fire('Hata', 'Geçersiz PayPal numarası. Lütfen 10 haneli PayPal numaranızı girin.', 'error');
        return null;
    }

    return {
        paymentNumber: paypalNumber
    };
}

function validateApplePay() {
    const applePayNumber = document.querySelector('#apple-pay input[placeholder="Apple Pay numarası"]').value;

    if (applePayNumber.length !== 10 || !/^\d+$/.test(applePayNumber)) {
        Swal.fire('Hata', 'Geçersiz Apple Pay numarası. Lütfen 10 haneli Apple Pay numaranızı girin.', 'error');
        return null;
    }

    return {
        paymentNumber: applePayNumber
    };
}

function validateSavedCard() {
    const selectedCard = document.querySelector('#saved-cards input[name="saved-card"]:checked');

    if (!selectedCard) {
        Swal.fire('Hata', 'Lütfen bir kart seçiniz.', 'error');
        return null;
    }

    return {
        cardId: selectedCard.dataset.cardId
    };
}