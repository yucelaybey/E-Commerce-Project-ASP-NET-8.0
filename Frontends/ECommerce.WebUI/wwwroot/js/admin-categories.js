document.addEventListener('DOMContentLoaded', function () {
    // Resim yükleme alanları
    const categoryImageInput = document.getElementById('categoryImage');
    const categoryImagePreviewImg = document.getElementById('categoryImagePreviewImg');
    const categoryImagePreview = document.getElementById('categoryImagePreview');
    const categoryImageUploadContent = document.querySelector('#categoryImageUpload .image-upload-content');

    const editCategoryImageInput = document.getElementById('editCategoryImage');
    const editCategoryImagePreviewImg = document.getElementById('editCategoryImagePreviewImg');
    const editCategoryImagePreview = document.getElementById('editCategoryImagePreview');
    const editCategoryImageUploadContent = document.querySelector('#editCategoryImageUpload .image-upload-content');

    // Kaydet ve Güncelle butonları
    const saveCategoryBtn = document.getElementById('saveCategoryBtn');
    const updateCategoryBtn = document.getElementById('updateCategoryBtn');

    // Modal'lar
    const addCategoryModal = document.getElementById('addCategoryModal');
    const editCategoryModal = document.getElementById('editCategoryModal');

    // Modal açma/kapatma butonları
    const addCategoryBtn = document.getElementById('addCategoryBtn');
    const closeModalButtons = document.querySelectorAll('.close-modal');

    // Modal açma/kapatma işlemleri
    if (addCategoryBtn) {
        addCategoryBtn.addEventListener('click', () => {
            showModal(addCategoryModal);

            // Formu resetle
            document.getElementById('addCategoryForm').reset();
            categoryImageUploadContent.style.display = 'block';
            categoryImagePreview.style.display = 'none';
        });
    }

    closeModalButtons.forEach(button => {
        button.addEventListener('click', () => {
            closeAllModals();
        });
    });

    window.addEventListener('click', (event) => {
        if (event.target.classList.contains('modal')) {
            closeAllModals();
        }
    });

    function showModal(modal) {
        closeAllModals();
        modal.classList.add('active');
        document.body.style.overflow = 'hidden';
    }

    function closeAllModals() {
        document.querySelectorAll('.modal').forEach(modal => {
            modal.classList.remove('active');
            // Formları da resetleyelim
            const forms = modal.querySelectorAll('form');
            forms.forEach(form => form.reset());
        });
        document.body.style.overflow = 'auto';

        // Resim önizleme alanlarını da sıfırlayalım
        categoryImageUploadContent.style.display = 'block';
        categoryImagePreview.style.display = 'none';
        editCategoryImageUploadContent.style.display = 'block';
        editCategoryImagePreview.style.display = 'none';
    }

    // Resim yükleme işlemleri
    function handleImageUpload(input, previewImg, previewContainer, uploadContent) {
        input.addEventListener('change', function (e) {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewImg.src = e.target.result;
                    uploadContent.style.display = 'none';
                    previewContainer.style.display = 'block';
                }
                reader.readAsDataURL(this.files[0]);
            }
        });

        // Önizleme alanına tıklayarak yeni resim yükleme
        previewContainer?.addEventListener('click', function () {
            input.click();
        });
    }

    // Form doğrulama
    function validateForm(formId) {
        let isValid = true;
        const form = document.getElementById(formId);
        const requiredFields = form.querySelectorAll('[required]');

        requiredFields.forEach(field => {
            const validationElement = document.getElementById(`${field.id}Validation`);

            if (!field.value.trim()) {
                field.style.borderColor = 'var(--danger)';
                if (validationElement) {
                    validationElement.textContent = 'Bu alan zorunludur';
                    validationElement.style.display = 'block';
                }
                isValid = false;
            } else {
                field.style.borderColor = '';
                if (validationElement) validationElement.style.display = 'none';
            }
        });

        // Özel kontroller
        if (formId === 'addCategoryForm' && (!categoryImageInput.files || !categoryImageInput.files[0])) {
            document.getElementById('categoryImageValidation').textContent = 'Lütfen bir resim seçin';
            document.getElementById('categoryImageValidation').style.display = 'block';
            isValid = false;
        }

        return isValid;
    }

    // Kategori ekleme fonksiyonu
    async function saveCategory() {
        if (!validateForm('addCategoryForm')) return;

        const formData = new FormData();
        formData.append('Name', document.getElementById('categoryName').value);
        formData.append('Description', document.getElementById('categoryDescription').value);
        // Resim dosyasını kontrol et
        if (categoryImageInput.files[0]) {
            formData.append('Image', categoryImageInput.files[0]);
        }

        try {
            closeAllModals();

            Swal.fire({
                title: 'İşlem Yürütülüyor',
                html: 'Kategori ekleniyor...',
                allowOutsideClick: false,
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            const response = await fetch('/YoneticiPaneli/KategoriEkle', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (!result.success) throw new Error(result.message || 'Kategori eklenemedi');

            await Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: 'Kategori başarıyla eklendi!',
                confirmButtonText: 'Tamam'
            }).then(() => {
                location.reload();
            });

        } catch (error) {
            console.error('Hata:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: `Kategori eklenirken hata: ${error.message}`
            });
        }
    }

    // Kategori güncelleme fonksiyonu
    async function updateCategory() {
        if (!validateForm('editCategoryForm')) return;

        const formData = new FormData();
        formData.append('Id', document.getElementById('editCategoryId').value);
        formData.append('Name', document.getElementById('editCategoryName').value);
        formData.append('Description', document.getElementById('editCategoryDescription').value);

        if (editCategoryImageInput.files[0]) {
            formData.append('Image', editCategoryImageInput.files[0]);
        } else if (editCategoryImagePreviewImg.src &&
            !editCategoryImagePreviewImg.src.includes('data:image')) {
            // Base64 verisi değilse (yani mevcut bir URL ise)
            formData.append('ImageUrl', editCategoryImagePreviewImg.src);
        }

        try {
            closeAllModals();

            Swal.fire({
                title: 'İşlem Yürütülüyor',
                html: 'Kategori bilgileri güncelleniyor...',
                allowOutsideClick: false,
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            const response = await fetch('/YoneticiPaneli/KategoriGuncelle', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (!result.success) throw new Error(result.message || 'Kategori güncellenemedi');

            await Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: 'Kategori bilgileri başarıyla güncellendi!',
                confirmButtonText: 'Tamam'
            }).then(() => {
                location.reload();
            });

        } catch (error) {
            console.error('Hata:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: `Kategori güncellenirken hata: ${error.message}`
            });
        }
    }

    // Kategori silme fonksiyonu
    async function deleteCategory(categoryId) {
        try {
            const result = await Swal.fire({
                title: 'Emin misiniz?',
                text: "Bu kategoriyi silmek istediğinize emin misiniz? Bu işlem geri alınamaz!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, sil!',
                cancelButtonText: 'İptal'
            });

            if (result.isConfirmed) {
                Swal.fire({
                    title: 'İşlem Yürütülüyor',
                    html: 'Kategori siliniyor...',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });

                const response = await fetch(`/YoneticiPaneli/KategoriSil/${categoryId}`, {
                    method: 'POST'
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const contentType = response.headers.get('content-type');
                if (contentType && contentType.includes('application/json')) {
                    const responseData = await response.json();
                    Swal.fire({
                        icon: 'success',
                        title: 'Silindi!',
                        text: 'Kategori başarıyla silindi.',
                        confirmButtonText: 'Tamam'
                    }).then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'success',
                        title: 'Silindi!',
                        text: 'Kategori başarıyla silindi.',
                        confirmButtonText: 'Tamam'
                    }).then(() => {
                        location.reload();
                    });
                }
            }
        } catch (error) {
            console.error('Hata:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: `Kategori silinirken hata oluştu: ${error.message}`
            });
        }
    }

    // Düzenle butonlarına event listener ekleme
    document.querySelectorAll('.btn-edit').forEach(button => {
        button.addEventListener('click', async function () {
            const categoryId = this.getAttribute('data-id');

            Swal.fire({
                title: 'Yükleniyor...',
                html: 'Kategori bilgileri getiriliyor...',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            try {
                const response = await fetch(`/YoneticiPaneli/KategoriGetir/${categoryId}`);
                if (!response.ok) throw new Error('Kategori bilgileri alınamadı');

                const categoryData = await response.json();

                Swal.close();
                showModal(editCategoryModal);

                setEditCategoryData(categoryData);

            } catch (error) {
                console.error('Hata:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: `Kategori bilgileri alınırken hata: ${error.message}`
                });
            }
        });
    });

    // Sil butonlarına event listener ekleme
    document.querySelectorAll('.btn-delete').forEach(button => {
        button.addEventListener('click', function () {
            const categoryId = this.getAttribute('data-id');
            deleteCategory(categoryId);
        });
    });

    // Event Listeners
    handleImageUpload(categoryImageInput, categoryImagePreviewImg, categoryImagePreview, categoryImageUploadContent);
    handleImageUpload(editCategoryImageInput, editCategoryImagePreviewImg, editCategoryImagePreview, editCategoryImageUploadContent);

    saveCategoryBtn.addEventListener('click', saveCategory);
    updateCategoryBtn.addEventListener('click', updateCategory);

    // Düzenleme modalını açarken çağrılacak fonksiyon
    window.setEditCategoryData = function (categoryData) {
        document.getElementById('editCategoryId').value = categoryData.categoryID;
        document.getElementById('editCategoryName').value = categoryData.name;
        document.getElementById('editCategoryDescription').value = categoryData.description;

        if (categoryData.imageUrl) {
            editCategoryImagePreviewImg.src = categoryData.imageUrl;
            editCategoryImageUploadContent.style.display = 'none';
            editCategoryImagePreview.style.display = 'block';
        } else {
            editCategoryImageUploadContent.style.display = 'block';
            editCategoryImagePreview.style.display = 'none';
        }
    };
});