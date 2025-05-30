const emailInput = document.getElementById('emailInput');
const emailIcon = document.getElementById('emailIcon');
const submitButton = document.getElementById('submitButton');

emailInput.addEventListener('input', function () {
    const email = emailInput.value.trim();
    const isValid = validateEmail(email);

    if (isValid) {
        emailIcon.classList.remove('fa-times-circle', 'error-icon');
        emailIcon.classList.add('fa-check-circle', 'success-icon');
        submitButton.disabled = false;
    } else {
        emailIcon.classList.remove('fa-check-circle', 'success-icon');
        emailIcon.classList.add('fa-times-circle', 'error-icon');
        submitButton.disabled = true;
    }
});

function validateEmail(email) {
    const turkishChars = /[çÇğĞıİöÖşŞüÜ]/;
    const atIndex = email.indexOf('@');

    // @ işareti yoksa veya birden fazla @ varsa geçersiz
    if (atIndex === -1 || email.indexOf('@', atIndex + 1) !== -1) {
        return false;
    }

    // @ işaretinin solunda veya sağında Türkçe karakter varsa geçersiz
    if (turkishChars.test(email.substring(0, atIndex)) || turkishChars.test(email.substring(atIndex + 1))) {
        return false;
    }

    // @ işaretinin solunda ve sağında en az bir karakter olmalı
    if (atIndex === 0 || atIndex === email.length - 1) {
        return false;
    }

    return true;
}