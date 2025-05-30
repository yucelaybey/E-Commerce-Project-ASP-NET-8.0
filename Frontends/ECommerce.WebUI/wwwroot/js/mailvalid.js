// Sayfa yüklendiğinde
document.addEventListener('DOMContentLoaded', function () {
    const loadingScreen = document.getElementById('loadingScreen');
    const content = document.getElementById('content');
    const verifyBtn = document.getElementById('verifyBtn');
    const codeInput = document.getElementById('verificationCode');

    // Loading ekranını kapat
    setTimeout(() => {
        loadingScreen.style.display = 'none';
        content.style.display = 'block';
        showMailNotification();
    }, 2000);

    // Kod giriş kontrolü
    codeInput.addEventListener('input', function () {
        this.value = this.value.replace(/\D/g, '');
        verifyBtn.disabled = this.value.length !== 6;
    });

    // Doğrulama butonu
    verifyBtn.addEventListener('click', function () {
        const enteredCode = codeInput.value;
        const tempCode = "@TempData["TempCode"]";

        if (enteredCode === tempCode) {
            // Form oluştur ve gönder
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = '/Login/MailCodeValid';

            const codeInput = document.createElement('input');
            codeInput.type = 'hidden';
            codeInput.name = 'EnteredCode';
            codeInput.value = enteredCode;
            form.appendChild(codeInput);

            const actionInput = document.createElement('input');
            actionInput.type = 'hidden';
            actionInput.name = 'actionA';
            actionInput.value = 'VerifyCode';
            form.appendChild(actionInput);

            document.body.appendChild(form);
            form.submit();
        } else {
            showError('Girdiğiniz kod hatalı!');
        }
    });

    // Yeni kod butonu
    document.getElementById('resendBtn').addEventListener('click', function () {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '/Login/MailCodeValid';

        const actionInput = document.createElement('input');
        actionInput.type = 'hidden';
        actionInput.name = 'actionA';
        actionInput.value = 'ResendCode';
        form.appendChild(actionInput);

        document.body.appendChild(form);
        form.submit();
    });
});

// Mail bildirimini göster
function showMailNotification() {
    const notif = document.getElementById('mailNotification');
    notif.style.opacity = '1';
    notif.style.transform = 'translateX(-50%) translateY(0)';

    setTimeout(() => {
        notif.style.opacity = '0';
        notif.style.transform = 'translateX(-50%) translateY(-100%)';
    }, 3000);
}

// Hata göster
function showError(message) {
    const errorNotif = document.getElementById('errorNotification');
    document.getElementById('errorMessage').textContent = message;

    errorNotif.style.opacity = '1';
    errorNotif.style.transform = 'translateX(-50%) translateY(0)';

    setTimeout(() => {
        errorNotif.style.opacity = '0';
        errorNotif.style.transform = 'translateX(-50%) translateY(-100%)';
    }, 3000);
}