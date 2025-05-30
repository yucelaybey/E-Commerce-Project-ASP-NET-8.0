// Modal Açma/Kapatma
const updateModal = document.getElementById('updateModal');
const openUpdateModal = document.getElementById('openUpdateModal');
const closeModals = document.querySelectorAll('.close-modal');

openUpdateModal.addEventListener('click', () => {
    updateModal.style.display = 'flex';
});

closeModals.forEach(btn => {
    btn.addEventListener('click', () => {
        updateModal.style.display = 'none';
    });
});

// Form Gönderme İşlevselliği
document.getElementById('updateAccountForm').addEventListener('submit', function (e) {
    e.preventDefault();

    // Güncellenen bilgileri al
    const firstName = document.getElementById('updateFirstName').value;
    const lastName = document.getElementById('updateLastName').value;
    const email = document.getElementById('updateEmail').value;
    const phone = document.getElementById('updatePhone').value;
    const birthDate = document.getElementById('updateBirthDate').value;

    // Bilgileri güncelle
    document.getElementById('firstName').textContent = firstName;
    document.getElementById('lastName').textContent = lastName;
    document.getElementById('email').textContent = email;
    document.getElementById('phone').textContent = phone;
    document.getElementById('birthDate').textContent = birthDate;

    // Modalı kapat
    updateModal.style.display = 'none';

    alert('Bilgileriniz başarıyla güncellendi!');
});