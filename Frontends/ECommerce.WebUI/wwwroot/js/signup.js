    function togglePassword(fieldId, iconElement) {
        const passwordField = document.getElementById(fieldId);
    if (passwordField.type === 'password') {
        passwordField.type = 'text';
    iconElement.classList.remove('fa-lock');
    iconElement.classList.add('fa-unlock');
        } else {
        passwordField.type = 'password';
    iconElement.classList.remove('fa-unlock');
    iconElement.classList.add('fa-lock');
        }
    }

    function formatPhoneNumber(input) {
        let value = input.value.replace(/\D/g, '');
        
        if (!value.startsWith('90') && value.length > 0) {
        value = '90' + value;
        }
        
        if (value.length > 2) {
        value = '+90 ' + value.substring(2);
        }
        if (value.length > 7) {
        value = value.substring(0, 7) + ' ' + value.substring(7);
        }
        if (value.length > 11) {
        value = value.substring(0, 11) + ' ' + value.substring(11, 15);
        }
        
        if (value.length > 16) {
        value = value.substring(0, 16);
        }

    input.value = value;
    }

    function checkPasswordRules() {
        const password = document.getElementById('registerPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

        // Kuralları kontrol et
        const lengthValid = password.length >= 8;
    const upperValid = /[A-ZĞÜŞİÖÇ]/.test(password);
    const lowerValid = /[a-zğüşıöç]/.test(password);
    const numberValid = /[0-9]/.test(password);
    const specialValid = /[*+.]/.test(password);
    const matchValid = password === confirmPassword && password !== '';

    // İkonları güncelle
    document.getElementById('lengthIcon').className = lengthValid ? 'fas fa-check-circle rule-valid' : 'fas fa-times-circle rule-invalid';
    document.getElementById('upperIcon').className = upperValid ? 'fas fa-check-circle rule-valid' : 'fas fa-times-circle rule-invalid';
    document.getElementById('lowerIcon').className = lowerValid ? 'fas fa-check-circle rule-valid' : 'fas fa-times-circle rule-invalid';
    document.getElementById('numberIcon').className = numberValid ? 'fas fa-check-circle rule-valid' : 'fas fa-times-circle rule-invalid';
    document.getElementById('specialIcon').className = specialValid ? 'fas fa-check-circle rule-valid' : 'fas fa-times-circle rule-invalid';
    document.getElementById('matchIcon').className = matchValid ? 'fas fa-check-circle rule-valid' : 'fas fa-times-circle rule-invalid';

    return lengthValid && upperValid && lowerValid && numberValid && specialValid && matchValid;
    }

    function validateStep1() {
        const fullName = document.getElementById('fullName').value.trim();
    const email = document.getElementById('registerEmail').value.trim();
    const birthDay = document.getElementById('birthDay').value;
    const phoneNumber = document.getElementById('phoneNumber').value.trim();

    // Validation checks
    if (!fullName) {
        Swal.fire({
            title: 'Hata!',
            text: 'Lütfen ad soyad bilgilerinizi giriniz.',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        }

    if (!email) {
        Swal.fire({
            title: 'Hata!',
            text: 'Lütfen e-posta adresinizi giriniz.',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        } else if (!/^\S+@\S+\.\S+$/.test(email)) {
        Swal.fire({
            title: 'Hata!',
            text: 'Lütfen geçerli bir e-posta adresi giriniz.',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        }

    if (!birthDay) {
        Swal.fire({
            title: 'Hata!',
            text: 'Lütfen doğum tarihinizi seçiniz.',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        }

    if (!phoneNumber) {
        Swal.fire({
            title: 'Hata!',
            text: 'Lütfen telefon numaranızı giriniz.',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        } else if (!/^\+90 \d{3} \d{3} \d{4}$/.test(phoneNumber)) {
        Swal.fire({
            title: 'Hata!',
            text: 'Lütfen geçerli bir telefon numarası giriniz. (+90 555 123 4567 formatında)',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        }

    // Değerleri hidden alanlara ata
    document.getElementById('hiddenName').value = fullName;
    document.getElementById('hiddenEmail').value = email;
    document.getElementById('hiddenBirthDay').value = birthDay;
    document.getElementById('hiddenPhoneNumber').value = phoneNumber;

    // Adımları değiştir
    document.getElementById('step1Form').style.display = 'none';
    document.getElementById('step2Form').style.display = 'block';
    document.getElementById('step1Indicator').classList.remove('active');
    document.getElementById('step2Indicator').classList.add('active');
    }

    function prevStep() {
        // Adımları değiştir
        document.getElementById('step1Form').style.display = 'block';
    document.getElementById('step2Form').style.display = 'none';
    document.getElementById('step1Indicator').classList.add('active');
    document.getElementById('step2Indicator').classList.remove('active');
    }

    function validateStep2() {
        const password = document.getElementById('registerPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    const termsChecked = document.getElementById('terms').checked;

    // Şifre kurallarını kontrol et
    const passwordValid = checkPasswordRules();

    if (!passwordValid) {
        Swal.fire({
            title: 'Hata!',
            html: 'Lütfen tüm şifre kurallarını sağladığınızdan emin olun:<br><br>' +
                '- Minimum 8 karakter<br>' +
                '- En az 1 büyük harf<br>' +
                '- En az 1 küçük harf<br>' +
                '- En az 1 rakam<br>' +
                '- En az 1 özel karakter (*, +, .)<br>' +
                '- Şifrelerin eşleşmesi',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        }

    if (!termsChecked) {
        Swal.fire({
            title: 'Hata!',
            text: 'Kayıt olabilmek için kullanım koşullarını kabul etmelisiniz.',
            icon: 'error',
            confirmButtonText: 'Tamam'
        });
    return;
        }

    // Loading ekranını göster
    document.getElementById('loadingOverlay').style.display = 'flex';

    // Formu gönder
        setTimeout(function () {
            document.getElementById('registerForm').submit();
        }, 2000);
    }