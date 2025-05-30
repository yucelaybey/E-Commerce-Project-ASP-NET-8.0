// Modal Açma/Kapatma
const addModal = document.getElementById('addModal');
const updateModal = document.getElementById('updateModal');
const openAddModal = document.getElementById('openAddModal');
const openUpdateModal = document.getElementById('openUpdateModal');
const closeModals = document.querySelectorAll('.close-modal');

openAddModal.addEventListener('click', () => {
    addModal.style.display = 'flex';
});

openUpdateModal.addEventListener('click', () => {
    updateModal.style.display = 'flex';
});

closeModals.forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.modal').forEach(modal => {
            modal.style.display = 'none';
        });
    });
});
// Onay İstemeden Sil Kutucuğu
const deleteWithoutConfirmation = document.getElementById('deleteWithoutConfirmation');

// Kutucuğun durumunu localStorage'dan yükle
deleteWithoutConfirmation.checked = localStorage.getItem('deleteWithoutConfirmation') === 'true';

// Kutucuk değiştiğinde localStorage'a kaydet
deleteWithoutConfirmation.addEventListener('change', function () {
    localStorage.setItem('deleteWithoutConfirmation', this.checked);
});

// Sil Butonu İşlevselliği
document.querySelectorAll('.delete-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        const productId = this.getAttribute('data-product-id');
        const productName = this.getAttribute('data-product-name');
        const productImage = this.getAttribute('data-product-image');

        if (deleteWithoutConfirmation.checked) {
            // Direkt sil
            if (confirm(`"${productName}" adlı ürünü silmek istediğinize emin misiniz?`)) {
                alert(`Ürün silindi: ${productName}`);
                // Burada silme işlemi gerçekleştirilecek
            }
        } else {
            // Onay modalını aç
            const deleteModal = document.getElementById('deleteModal');
            document.getElementById('deleteProductImage').src = productImage;
            document.getElementById('deleteProductName').textContent = productName;
            deleteModal.style.display = 'flex';

            // Onay butonu
            document.querySelector('.confirm-delete-btn').addEventListener('click', function () {
                alert(`Ürün silindi: ${productName}`);
                deleteModal.style.display = 'none';
                // Burada silme işlemi gerçekleştirilecek
            });

            // İptal butonu
            document.querySelector('.cancel-btn').addEventListener('click', function () {
                deleteModal.style.display = 'none';
            });
        }
    });
});
// Resim Yükleme ve Önizleme
const productImage = document.getElementById('productImage');
const previewImage = document.getElementById('previewImage');
const updateProductImage = document.getElementById('updateProductImage');
const updatePreviewImage = document.getElementById('updatePreviewImage');

// Ürün Ekle Modalı için resim yükleme
document.getElementById('imagePreview').addEventListener('click', function () {
    productImage.click();
});

productImage.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const imagePreview = document.getElementById('imagePreview');
            imagePreview.innerHTML = `
                <img src="${e.target.result}" alt="Resim Önizleme" id="previewImage">
                <div class="upload-overlay">
                    <span>Yeni Resim Yükle</span>
                </div>
            `;
        };
        reader.readAsDataURL(file);
    }
});

// Ürün Güncelle Modalı için resim yükleme
document.getElementById('updateImagePreview').addEventListener('click', function () {
    updateProductImage.click();
});

updateProductImage.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const updateImagePreview = document.getElementById('updateImagePreview');
            updateImagePreview.innerHTML = `
                <img src="${e.target.result}" alt="Resim Önizleme" id="updatePreviewImage">
                <div class="upload-overlay">
                    <span>Yeni Resim Yükle</span>
                </div>
            `;
        };
        reader.readAsDataURL(file);
    }
});
