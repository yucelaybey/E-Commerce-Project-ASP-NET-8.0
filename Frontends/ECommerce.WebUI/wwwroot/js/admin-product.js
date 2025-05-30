document.addEventListener('DOMContentLoaded', function () {
    // Kategorileri tutacak global değişken
    let categories = [];

    // Resim yükleme alanları
    const productImageInput = document.getElementById('productImage');
    const productImagePreviewImg = document.getElementById('productImagePreviewImg');
    const productImagePreview = document.getElementById('productImagePreview');
    const productImageUploadContent = document.querySelector('#productImageUpload .image-upload-content');

    const editProductImageInput = document.getElementById('editProductImage');
    const editProductImagePreviewImg = document.getElementById('editProductImagePreviewImg');
    const editProductImagePreview = document.getElementById('editProductImagePreview');
    const editProductImageUploadContent = document.querySelector('#editProductImageUpload .image-upload-content');

    // Toggle switch'ler
    const discountToggle = document.getElementById('discountToggle');
    const editDiscountToggle = document.getElementById('editDiscountToggle');
    const discountFields = document.querySelector('.discount-fields');
    const editDiscountFields = document.querySelector('.edit-discount-fields');

    // Fiyat alanları
    const productPrice = document.getElementById('productPrice');
    const discountedPrice = document.getElementById('discountedPrice');
    const discountRate = document.getElementById('discountRate');

    const editProductPrice = document.getElementById('editProductPrice');
    const editDiscountedPrice = document.getElementById('editDiscountedPrice');
    const editDiscountRate = document.getElementById('editDiscountRate');

    // Kaydet ve Güncelle butonları
    const saveProductBtn = document.getElementById('saveProductBtn');
    const updateProductBtn = document.getElementById('updateProductBtn');

    // Kategori dropdown'ları
    const productCategoryDropdown = document.getElementById('productCategory');
    const editProductCategoryDropdown = document.getElementById('editProductCategory');

    // Modal'lar
    const addProductModal = document.getElementById('addProductModal');
    const editProductModal = document.getElementById('editProductModal');

    // Modal açma/kapatma butonları
    const addProductBtn = document.getElementById('addProductBtn');
    const closeModalButtons = document.querySelectorAll('.close-modal');

    // Sayfa yüklendiğinde kategorileri çek
    fetchCategories();

    // Modal açma/kapatma işlemleri
    if (addProductBtn) {
        addProductBtn.addEventListener('click', () => {
            showModal(addProductModal);

            // Formu resetle
            document.getElementById('addProductForm').reset();
            productImageUploadContent.style.display = 'block';
            productImagePreview.style.display = 'none';
            discountToggle.checked = false;
            discountFields.style.display = 'none';
            updateDiscountFieldsRequired();
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
        productImageUploadContent.style.display = 'block';
        productImagePreview.style.display = 'none';
        editProductImageUploadContent.style.display = 'block';
        editProductImagePreview.style.display = 'none';
    }

    // Kategorileri getiren fonksiyon
    async function fetchCategories() {
        try {
            Swal.fire({
                title: 'Yükleniyor...',
                html: 'Kategoriler getiriliyor...',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            const response = await fetch('/YoneticiPaneli/KategoriGetir');
            if (!response.ok) throw new Error('Kategoriler alınamadı');

            categories = await response.json();
            populateCategoryDropdowns(categories);

            Swal.close();
        } catch (error) {
            console.error('Kategori yükleme hatası:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: 'Kategoriler yüklenirken bir hata oluştu: ' + error.message
            });
        }
    }

    function populateCategoryDropdowns(categories) {
        // Ekleme formu için
        productCategoryDropdown.innerHTML = '<option value="">Seçiniz</option>';
        categories.forEach(category => {
            productCategoryDropdown.innerHTML += `<option value="${category.categoryID}">${category.name}</option>`;
        });

        // Düzenleme formu için
        editProductCategoryDropdown.innerHTML = '<option value="">Seçiniz</option>';
        categories.forEach(category => {
            editProductCategoryDropdown.innerHTML += `<option value="${category.categoryID}">${category.name}</option>`;
        });
    }

    // Düzenleme modalını açarken kategori seçimini ayarla
    function setEditCategory(categoryId) {
        editProductCategoryDropdown.value = categoryId;
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

    // İndirim alanlarının görünürlüğünü kontrol etme
    function toggleDiscountFields(toggle, fields) {
        toggle.addEventListener('change', function () {
            fields.style.display = this.checked ? 'block' : 'none';
            updateDiscountFieldsRequired();
        });
    }

    // İndirim alanlarının zorunluluğunu güncelleme
    function updateDiscountFieldsRequired() {
        [discountToggle, editDiscountToggle].forEach((toggle, index) => {
            const requiredFields = document.querySelectorAll(index === 0 ? '.discount-required' : '.edit-discount-required');
            const priceField = index === 0 ? discountedPrice : editDiscountedPrice;
            const rateField = index === 0 ? discountRate : editDiscountRate;

            if (toggle.checked) {
                requiredFields.forEach(field => field.style.display = 'inline');
                priceField.setAttribute('required', '');
                rateField.setAttribute('required', '');
            } else {
                requiredFields.forEach(field => field.style.display = 'none');
                priceField.removeAttribute('required');
                rateField.removeAttribute('required');
            }
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
        if (formId === 'addProductForm' && (!productImageInput.files || !productImageInput.files[0])) {
            document.getElementById('productImageValidation').textContent = 'Lütfen bir resim seçin';
            document.getElementById('productImageValidation').style.display = 'block';
            isValid = false;
        }

        // İndirim kontrolü
        const price = parseFloat(formId === 'addProductForm' ? productPrice.value : editProductPrice.value);
        const discountedPriceValue = parseFloat(formId === 'addProductForm' ? discountedPrice.value : editDiscountedPrice.value);
        const discountRateValue = parseFloat(formId === 'addProductForm' ? discountRate.value : editDiscountRate.value);
        const isDiscountActive = formId === 'addProductForm' ? discountToggle.checked : editDiscountToggle.checked;

        if (isDiscountActive) {
            if (isNaN(discountedPriceValue)) {
                document.getElementById(`${formId === 'addProductForm' ? 'discountedPrice' : 'editDiscountedPrice'}Validation`)
                    .textContent = 'Geçerli bir indirimli fiyat girin';
                document.getElementById(`${formId === 'addProductForm' ? 'discountedPrice' : 'editDiscountedPrice'}Validation`)
                    .style.display = 'block';
                isValid = false;
            } else if (discountedPriceValue >= price) {
                document.getElementById(`${formId === 'addProductForm' ? 'discountedPrice' : 'editDiscountedPrice'}Validation`)
                    .textContent = 'İndirimli fiyat normal fiyattan düşük olmalı';
                document.getElementById(`${formId === 'addProductForm' ? 'discountedPrice' : 'editDiscountedPrice'}Validation`)
                    .style.display = 'block';
                isValid = false;
            }

            if (isNaN(discountRateValue) || discountRateValue <= 0) {
                document.getElementById(`${formId === 'addProductForm' ? 'discountRate' : 'editDiscountRate'}Validation`)
                    .textContent = 'Geçerli bir indirim oranı girin (0\'dan büyük)';
                document.getElementById(`${formId === 'addProductForm' ? 'discountRate' : 'editDiscountRate'}Validation`)
                    .style.display = 'block';
                isValid = false;
            }
        }

        // Sayısal alan kontrolü
        const numericFields = form.querySelectorAll('input[type="number"]');
        numericFields.forEach(field => {
            if (field.value && isNaN(field.value)) {
                document.getElementById(`${field.id}Validation`)
                    .textContent = 'Lütfen geçerli bir sayı girin';
                document.getElementById(`${field.id}Validation`)
                    .style.display = 'block';
                isValid = false;
            }
        });

        return isValid;
    }

    // İndirim oranı hesaplama
    function calculateDiscountRate(priceField, discountedPriceField, rateField) {
        const price = parseFloat(priceField.value);
        const discountedPrice = parseFloat(discountedPriceField.value);

        if (price && discountedPrice && discountedPrice < price) {
            rateField.value = Math.round(((price - discountedPrice) / price) * 100);
        } else {
            rateField.value = '';
        }
    }

    // Ürün ekleme fonksiyonu
    async function saveProduct() {
        if (!validateForm('addProductForm')) return;

        const formData = new FormData();
        formData.append('Name', document.getElementById('productName').value);
        formData.append('Description', document.getElementById('productDescription').value);
        formData.append('CategoryID', document.getElementById('productCategory').value);
        formData.append('Stock', document.getElementById('productStock').value);
        formData.append('Price', document.getElementById('productPrice').value);
        formData.append('ImageFile', productImageInput.files[0]);

        if (discountToggle.checked) {
            formData.append('SalePrice', discountedPrice.value);
            formData.append('DiscountRate', discountRate.value);
            formData.append('SaleSeason', discountToggle.checked);
        } else {
            formData.append('SalePrice', '0');
            formData.append('DiscountRate', '0');
            formData.append('SaleSeason', false);

        }

        try {
            closeAllModals();

            Swal.fire({
                title: 'İşlem Yürütülüyor',
                html: 'Ürün ekleniyor...',
                allowOutsideClick: false,
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            const response = await fetch('/YoneticiPaneli/UrunEkle', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (!result.success) throw new Error(result.message || 'Ürün eklenemedi');

            await Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: 'Ürün başarıyla eklendi!',
                confirmButtonText: 'Tamam'
            }).then(() => {
                location.reload();
            });

        } catch (error) {
            console.error('Hata:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: `Ürün eklenirken hata: ${error.message}`
            });
        }
    }

    // Ürün güncelleme fonksiyonu
    async function updateProduct() {
        if (!validateForm('editProductForm')) return;

        const formData = new FormData();
        formData.append('ProductID', document.getElementById('editProductId').value);
        formData.append('Name', document.getElementById('editProductName').value);
        formData.append('Description', document.getElementById('editProductDescription').value);
        formData.append('CategoryID', document.getElementById('editProductCategory').value);
        formData.append('Stock', document.getElementById('editProductStock').value);
        formData.append('Price', document.getElementById('editProductPrice').value);

        if (editProductImageInput.files[0]) {
            formData.append('ImageFile', editProductImageInput.files[0]);
        } else {
            formData.append('ImageUrl', editProductImagePreviewImg.src);
        }

        if (editDiscountToggle.checked) {
            formData.append('SaleSeason', true);
            formData.append('SalePrice', editDiscountedPrice.value);
            formData.append('DiscountRate', editDiscountRate.value);
        } else {
            formData.append('SaleSeason', false);
            // Eğer değerler varsa onları gönder, yoksa 0 gönder
            formData.append('SalePrice', editDiscountedPrice.value || '0');
            formData.append('DiscountRate', editDiscountRate.value || '0');
        }

        try {
            closeAllModals();

            Swal.fire({
                title: 'İşlem Yürütülüyor',
                html: 'Ürün bilgileri güncelleniyor...',
                allowOutsideClick: false,
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            const response = await fetch('/YoneticiPaneli/UrunGuncelle', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (!result.success) throw new Error(result.message || 'Ürün güncellenemedi');


            await Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: 'Ürün bilgileri başarıyla güncellendi!',
                confirmButtonText: 'Tamam'
            }).then(() => {
                location.reload();
            });

        } catch (error) {
            console.error('Hata:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: `Ürün güncellenirken hata: ${error.message}`
            });
        }
    }

    // Ürün silme fonksiyonu
    async function deleteProduct(productId) {
        try {
            const result = await Swal.fire({
                title: 'Emin misiniz?',
                text: "Bu ürünü silmek istediğinize emin misiniz? Bu işlem geri alınamaz!",
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
                    html: 'Ürün siliniyor...',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });

                const response = await fetch(`/YoneticiPaneli/UrunSil/${productId}`, {
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
                        text: 'Ürün başarıyla silindi.',
                        confirmButtonText: 'Tamam'
                    }).then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'success',
                        title: 'Silindi!',
                        text: 'Ürün başarıyla silindi.',
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
                text: `Ürün silinirken hata oluştu: ${error.message}`
            });
        }
    }

    // Düzenle butonlarına event listener ekleme
    document.querySelectorAll('.btn-edit').forEach(button => {
        button.addEventListener('click', async function () {
            const productId = this.getAttribute('data-id');

            Swal.fire({
                title: 'Yükleniyor...',
                html: 'Ürün bilgileri getiriliyor...',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            try {
                const response = await fetch(`/YoneticiPaneli/UrunGetir/${productId}`);
                if (!response.ok) throw new Error('Ürün bilgileri alınamadı');

                const productData = await response.json();

                Swal.close();
                showModal(editProductModal);

                setEditProductData(productData);
                
            } catch (error) {
                console.error('Hata:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: `Ürün bilgileri alınırken hata: ${error.message}`
                });
            }
        });
    });

    // Sil butonlarına event listener ekleme
    document.querySelectorAll('.btn-delete').forEach(button => {
        button.addEventListener('click', function () {
            const productId = this.getAttribute('data-id');
            deleteProduct(productId);
        });
    });

    // Event Listeners
    handleImageUpload(productImageInput, productImagePreviewImg, productImagePreview, productImageUploadContent);
    handleImageUpload(editProductImageInput, editProductImagePreviewImg, editProductImagePreview, editProductImageUploadContent);

    toggleDiscountFields(discountToggle, discountFields);
    toggleDiscountFields(editDiscountToggle, editDiscountFields);

    discountedPrice.addEventListener('input', () => calculateDiscountRate(productPrice, discountedPrice, discountRate));
    editDiscountedPrice.addEventListener('input', () => calculateDiscountRate(editProductPrice, editDiscountedPrice, editDiscountRate));

    saveProductBtn.addEventListener('click', saveProduct);
    updateProductBtn.addEventListener('click', updateProduct);

    // Sayısal alanlara sadece rakam girişi kontrolü
    document.querySelectorAll('input[type="number"]').forEach(input => {
        input.addEventListener('keypress', function (e) {
            if (isNaN(String.fromCharCode(e.which))) {
                e.preventDefault();
            }
        });
    });

    // Düzenleme modalını açarken çağrılacak fonksiyon
    window.setEditProductData = function (productData) {
        document.getElementById('editProductId').value = productData.productID;
        document.getElementById('editProductName').value = productData.name;
        document.getElementById('editProductDescription').value = productData.description;
        setEditCategory(productData.categoryID);
        document.getElementById('editProductStock').value = productData.stock;
        document.getElementById('editProductPrice').value = productData.price;

        if (productData.imageUrl) {
            editProductImagePreviewImg.src = productData.imageUrl;
            editProductImageUploadContent.style.display = 'none';
            editProductImagePreview.style.display = 'block';
        } else {
            editProductImageUploadContent.style.display = 'block';
            editProductImagePreview.style.display = 'none';
        }

        // İndirim bilgilerini ayarla
        if (productData.salePrice > 0 && productData.discountRate > 0) {
            editDiscountToggle.checked = productData.saleSeason;
            editDiscountFields.style.display = 'block';
            document.getElementById('editDiscountedPrice').value = productData.salePrice;
            document.getElementById('editDiscountRate').value = productData.discountRate;
        } else {
            editDiscountToggle.checked = false;
            editDiscountFields.style.display = 'none';
            document.getElementById('editDiscountedPrice').value = '';
            document.getElementById('editDiscountRate').value = '';
        }
        updateDiscountFieldsRequired();
    };
});