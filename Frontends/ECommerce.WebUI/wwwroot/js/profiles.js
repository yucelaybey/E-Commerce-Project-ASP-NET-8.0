document.addEventListener('DOMContentLoaded', function () {
    // Simulate loading
    setTimeout(() => {
        document.querySelector('.loading-screen').style.display = 'none';
    }, 1000);

    // Boş adres ve kart dizileri
    const addresses = [];
    const creditCards = [];

    // District data
    const districts = {
        istanbul: ['Beyoğlu', 'Kadıköy', 'Beşiktaş', 'Şişli', 'Üsküdar'],
        ankara: ['Çankaya', 'Keçiören', 'Yenimahalle', 'Etimesgut', 'Sincan'],
        izmir: ['Bornova', 'Karşıyaka', 'Konak', 'Buca', 'Bayraklı']
    };

    // Initialize the page
    renderAddresses();

    // Modal functionality
    const modals = {
        addAddress: document.getElementById('addAddressModal'),
        editAddress: document.getElementById('editAddressModal'),
        addCard: document.getElementById('addCardModal'),
        editCard: document.getElementById('editCardModal')
    };

    const modalBtns = {
        addAddress: document.getElementById('addAddressBtn'),
        addCard: document.getElementById('addCardBtn')
    };

    // Open modals
    modalBtns.addAddress.addEventListener('click', () => openModal('addAddress'));
    modalBtns.addCard.addEventListener('click', () => openModal('addCard'));

    // Close modals
    document.querySelectorAll('.close-modal').forEach(btn => {
        btn.addEventListener('click', closeAllModals);
    });

    // Close modal when clicking outside
    window.addEventListener('click', (e) => {
        if (e.target.classList.contains('modal')) {
            closeAllModals();
        }
    });

    // Form submissions
    document.getElementById('addAddressForm').addEventListener('submit', handleAddAddress);
    document.getElementById('editCardForm').addEventListener('submit', handleUpdateCard);

    // Delete buttons
    document.getElementById('deleteCardBtn').addEventListener('click', handleDeleteCard);

    // Toggle password visibility
    document.querySelectorAll('.toggle-password').forEach(btn => {
        btn.addEventListener('click', togglePasswordVisibility);
    });

    function renderAddresses() {

        // Event listeners
        document.querySelectorAll('.edit-address-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const addressId = e.target.closest('.edit-address-btn').dataset.id;
                openEditAddressModal(addressId);
            });
        });

        document.querySelectorAll('.delete-address-btn').forEach(btn => {
            btn.addEventListener('click', function (e) {
                e.preventDefault();
                const addressId = this.getAttribute('data-id');
                deleteAddress(addressId);
            });
        });

        document.querySelectorAll('.delete-address-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const addressId = e.target.closest('.address-card').dataset.id;

            });
        });
    }


    function setDefaultAddress(addressId) {
        addresses.forEach(addr => {
            addr.isDefault = addr.id === addressId;
        });
        renderAddresses();
        alert('Varsayılan adres başarıyla güncellendi.');
    }

    function setDefaultCard(cardId) {
        creditCards.forEach(card => {
            card.isDefault = card.id === cardId;
        });
        alert('Varsayılan kart başarıyla güncellendi.');
    }

    function openModal(modalName) {
        closeAllModals();
        modals[modalName].style.display = 'flex';
        document.body.style.overflow = 'hidden';
    }

    function closeAllModals() {
        Object.values(modals).forEach(modal => {
            modal.style.display = 'none';
        });
        document.body.style.overflow = 'auto';
    }

    function togglePasswordVisibility(e) {
        const input = e.target.previousElementSibling;
        const icon = e.target;

        if (input.type === 'password') {
            input.type = 'text';
            icon.classList.remove('fa-eye');
            icon.classList.add('fa-eye-slash');
        } else {
            input.type = 'password';
            icon.classList.remove('fa-eye-slash');
            icon.classList.add('fa-eye');
        }
    }

    function handleAddAddress(e) {
        e.preventDefault();

        const newAddress = {
            id: 'addr' + (addresses.length + 1),
            title: document.getElementById('addressTitle').value,
            fullName: document.getElementById('addressFullName').value,
            phone: document.getElementById('addressPhone').value,
            city: document.getElementById('addressCity').value,
            district: document.getElementById('addressDistrict').value,
            neighborhood: document.getElementById('addressNeighborhood').value,
            details: document.getElementById('addressDetails').value,
            isDefault: document.getElementById('setDefaultAddress').checked
        };

        if (newAddress.isDefault) {
            addresses.forEach(addr => addr.isDefault = false);
        }

        addresses.push(newAddress);
        renderAddresses();
        closeAllModals();
    }


    // Adres düzenleme modalını aç
    async function openEditAddressModal(addressId) {
        let swalInstance;
        try {
            // Loading göster
            swalInstance = Swal.fire({
                title: 'Yükleniyor...',
                html: 'Adres bilgileri getiriliyor',
                allowOutsideClick: false,
                didOpen: () => Swal.showLoading()
            });

            // 1. ÖNCEKİ EVENT LİSTENER'LARI TEMİZLE
            const form = document.getElementById('editAddressForm');
            if (form) form.replaceWith(form.cloneNode(true));

            // Adres verilerini getir
            const response = await fetch(`/Addresses/GetAddressForEdit?id=${addressId}`);
            if (!response.ok) throw new Error('Adres bilgileri alınamadı');
            const address = await response.json();

            const element = document.querySelector('span[data-id]');
            // Formu doldur
            element.dataset.id = addressId;
            document.getElementById('editAddressTitle').value = address.addressTitle || '';
            document.getElementById('editAddressFullName').value = address.nameSurname || '';
            document.getElementById('editAddressPhone').value = address.phoneNumber || '';
            document.getElementById('editAddressDetails').value = address.addressDetails || '';
            document.getElementById('editSetDefaultAddress').checked = address.isDefault || false;

            // 2. SELECT EVENT LİSTENER'LARINI YENİDEN EKLE
            setupAddressSelectEvents();

            // 3. ŞEHİR SEÇİMİ VE DİĞER ALANLARI DOLDUR
            const citySelect = document.getElementById('editAddressCity');
            const cityOption = Array.from(citySelect.options).find(opt => opt.text === address.city);

            if (cityOption) {
                citySelect.value = cityOption.value;
                await loadLocationOptions('town', cityOption.value, address.town);

                if (address.town) {
                    const townSelect = document.getElementById('editAddressTown');
                    const townOption = Array.from(townSelect.options).find(opt => opt.text === address.town);
                    if (townOption) {
                        townSelect.value = townOption.value;
                        await loadLocationOptions('quarter', townOption.value, address.quarter);

                        if (address.quarter) {
                            const quarterSelect = document.getElementById('editAddressQuarter');
                            const quarterOption = Array.from(quarterSelect.options).find(opt => opt.text === address.quarter);
                            if (quarterOption) {
                                quarterSelect.value = quarterOption.value;
                                await loadLocationOptions('district', quarterOption.value, address.district);
                            }
                        }
                    }
                }
            }

            // 4. FORM SUBMİT EVENT'İ EKLE
            document.getElementById('editAddressForm').addEventListener('submit', handleAddressFormSubmit);

            // 🔥 TELEFON VE AD SOYAD VALİDASYONLARI BURAYA EKLENECEK
            const phoneInput = document.getElementById('editAddressPhone');
            const nameInput = document.getElementById('editAddressFullName');

            // 1. TELEFON FORMATLAMA
            phoneInput.value = address.phoneNumber || '+90 ';
            phoneInput.addEventListener('input', function (e) {
                let value = this.value.replace(/\D/g, '');
                if (!value.startsWith('90')) value = '90' + value;

                let formatted = '+90';
                if (value.length > 2) formatted += ' ' + value.substring(2, 5);
                if (value.length > 5) formatted += ' ' + value.substring(5, 8);
                if (value.length > 8) formatted += ' ' + value.substring(8, 12);

                this.value = formatted;
            });

            // 2. AD SOYAD KONTROLÜ (SADECE HARF)
            nameInput.addEventListener('input', function () {
                this.value = this.value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
            });

            // Modalı aç
            await swalInstance.close();
            document.getElementById('editAddressModal').style.display = 'flex';

        } catch (error) {
            if (swalInstance) await swalInstance.close();
            Swal.fire({
                icon: 'error',
                title: 'Hata',
                text: error.message || 'Adres bilgileri yüklenirken bir hata oluştu'
            });
            console.error('Adres yükleme hatası:', error);
        }
    }

    // SELECT EVENT LİSTENER'LARINI AYARLA
    function setupAddressSelectEvents() {
        // Şehir değişince ilçeleri yükle
        document.getElementById('editAddressCity').addEventListener('change', async function () {
            if (this.value) {
                await loadLocationOptions('town', this.value);
            } else {
                resetDependentSelects('editAddressTown');
            }
        });

        // İlçe değişince semtleri yükle
        document.getElementById('editAddressTown').addEventListener('change', async function () {
            if (this.value) {
                await loadLocationOptions('quarter', this.value);
            } else {
                resetDependentSelects('editAddressQuarter');
            }
        });

        // Semt değişince mahalleleri yükle
        document.getElementById('editAddressQuarter').addEventListener('change', async function () {
            if (this.value) {
                await loadLocationOptions('district', this.value);
            } else {
                resetDependentSelects('editAddressDistrict');
            }
        });
    }

    // FORM SUBMİT İŞLEMİ
    async function handleAddressFormSubmit(e) {
        e.preventDefault();
        const submitBtn = e.target.querySelector('button[type="submit"]');
        const originalText = submitBtn.innerHTML;

        try {
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Kaydediliyor...';

            const element = document.querySelector('span[data-id]');
            const dataId = element.getAttribute('data-id');

            const formData = {
                addressID: dataId,
                addressTitle: document.getElementById('editAddressTitle').value,
                nameSurname: document.getElementById('editAddressFullName').value,
                phoneNumber: document.getElementById('editAddressPhone').value,
                city: document.getElementById('editAddressCity').options[document.getElementById('editAddressCity').selectedIndex].text,
                town: document.getElementById('editAddressTown').options[document.getElementById('editAddressTown').selectedIndex].text,
                quarter: document.getElementById('editAddressQuarter').options[document.getElementById('editAddressQuarter').selectedIndex].text,
                district: document.getElementById('editAddressDistrict').options[document.getElementById('editAddressDistrict').selectedIndex].text,
                addressDetails: document.getElementById('editAddressDetails').value,
                isDefault: document.getElementById('editSetDefaultAddress').checked
            };

            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
            const response = await fetch('/Addresses/UpdateAddress', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token || ''
                },
                body: JSON.stringify(formData)
            });

            if (!response.ok) throw new Error('Güncelleme başarısız');
            Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: 'Adres güncellendi',
                confirmButtonText: 'Tamam'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Kullanıcı "Tamam" butonuna bastığında
                    document.getElementById('editAddressModal').style.display = 'none';
                    location.reload();
                }
            });

        } catch (error) {
            console.error('Hata:', error);
            Swal.fire('Hata!', error.message, 'error');
        } finally {
            submitBtn.disabled = false;
            submitBtn.innerHTML = originalText;
        }
    }

    // Ortak lokasyon yükleme fonksiyonu
    async function loadLocationOptions(type, parentId, selectedValue) {
        const config = {
            town: {
                selectId: 'editAddressTown',
                endpoint: `/Addresses/GetTowns?cityId=${parentId}`,
                nextSelect: 'editAddressQuarter'
            },
            quarter: {
                selectId: 'editAddressQuarter',
                endpoint: `/Addresses/GetQuarters?townId=${parentId}`,
                nextSelect: 'editAddressDistrict'
            },
            district: {
                selectId: 'editAddressDistrict',
                endpoint: `/Addresses/GetDistricts?quarterId=${parentId}`,
                nextSelect: null
            }
        }[type];

        if (!config) return;

        const select = document.getElementById(config.selectId);
        select.disabled = true;
        select.innerHTML = `<option value="">Yükleniyor...</option>`;

        try {
            const response = await fetch(config.endpoint);
            if (!response.ok) throw new Error(`${type} verileri alınamadı`);
            const items = await response.json();

            select.innerHTML = `<option value="">${type === 'town' ? 'İlçe' : type === 'quarter' ? 'Semt' : 'Mahalle'} seçiniz</option>`;

            items.forEach(item => {
                const option = new Option(item[`${type}Name`], item[`${type}Id`]);
                option.dataset.name = item[`${type}Name`];
                select.add(option);
            });

            // Seçili değeri ayarla (düzenleme modu için)
            if (selectedValue) {
                const selectedOption = Array.from(select.options).find(opt => opt.text === selectedValue);
                if (selectedOption) select.value = selectedOption.value;
            }

            // Sonraki select'i resetle
            if (config.nextSelect) {
                resetDependentSelects(config.nextSelect);
            }

        } catch (error) {
            console.error(`${type} yükleme hatası:`, error);
            select.innerHTML = `<option value="">Yükleme hatası</option>`;
        } finally {
            select.disabled = false;
        }
    }

    // Bağımlı select'leri resetle
    function resetDependentSelects(startFromId) {
        const selectHierarchy = ['editAddressTown', 'editAddressQuarter', 'editAddressDistrict'];
        const startIndex = selectHierarchy.indexOf(startFromId);

        if (startIndex >= 0) {
            selectHierarchy.slice(startIndex).forEach(selectId => {
                const select = document.getElementById(selectId);
                select.innerHTML = `<option value="">${selectId === 'editAddressTown' ? 'İlçe' :
                    selectId === 'editAddressQuarter' ? 'Semt' : 'Mahalle'
                    } seçiniz</option>`;
                select.disabled = (selectId !== startFromId);
            });
        }
    }

    // Adres silme butonu - ÇALIŞAN VERSİYON
    async function deleteAddress(addressId) {
        const { isConfirmed } = await Swal.fire({
            title: 'Emin misiniz?',
            text: 'Bu adres kalıcı olarak silinecek!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Evet, sil',
            cancelButtonText: 'Vazgeç'
        });

        if (!isConfirmed) return;

        try {
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            const response = await fetch(`/Addresses/DeleteAddress?id=${addressId}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': token,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.message || 'Silme işlemi başarısız');
            }

            await Swal.fire({
                icon: 'success',
                title: 'Silindi!',
                text: 'Adres başarıyla kaldırıldı',
                confirmButtonText: 'Tamam'
            });

            location.reload(); // Sayfayı yenile

        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: error.message || 'Bir hata oluştu',
                confirmButtonText: 'Tamam'
            });
        }
    }
});
document.addEventListener('DOMContentLoaded', function () {
    // City select change event
    document.getElementById('addressCity').addEventListener('change', async function () {
        const cityId = this.value;
        const cityName = this.options[this.selectedIndex].text;

        // Reset dependent selects
        const districtSelect = document.getElementById('addressDistrict');
        const neighborhoodSelect = document.getElementById('addressNeighborhood');
        const quarterSelect = document.getElementById('addressQuarter');

        districtSelect.innerHTML = '<option value="">İlçe seçiniz</option>';
        neighborhoodSelect.innerHTML = '<option value="">Semt seçiniz</option>';
        quarterSelect.innerHTML = '<option value="">Mahalle seçiniz</option>';

        districtSelect.disabled = true;
        neighborhoodSelect.disabled = true;
        quarterSelect.disabled = true;

        if (cityId) {
            try {
                const response = await fetch(`/Addresses/GetTowns?cityId=${cityId}`);
                if (response.ok) {
                    const towns = await response.json();

                    districtSelect.innerHTML = '<option value="">İlçe seçiniz</option>';
                    towns.forEach(town => {
                        const option = document.createElement('option');
                        option.value = town.townId;
                        option.textContent = town.townName;
                        option.setAttribute('data-name', town.townName);
                        districtSelect.appendChild(option);
                    });

                    districtSelect.disabled = false;
                }
            } catch (error) {
                console.error('Error fetching towns:', error);
            }
        }
    });

    // District (Town) select change event
    document.getElementById('addressDistrict').addEventListener('change', async function () {
        const townId = this.value;
        const townName = this.options[this.selectedIndex].getAttribute('data-name');

        // Reset dependent selects
        const neighborhoodSelect = document.getElementById('addressNeighborhood');
        const quarterSelect = document.getElementById('addressQuarter');

        neighborhoodSelect.innerHTML = '<option value="">Semt seçiniz</option>';
        quarterSelect.innerHTML = '<option value="">Mahalle seçiniz</option>';

        neighborhoodSelect.disabled = true;
        quarterSelect.disabled = true;

        if (townId) {
            try {
                const response = await fetch(`/Addresses/GetQuarters?townId=${townId}`);
                if (response.ok) {
                    const quarters = await response.json();

                    neighborhoodSelect.innerHTML = '<option value="">Semt seçiniz</option>';
                    quarters.forEach(quarter => {
                        const option = document.createElement('option');
                        option.value = quarter.quarterId;
                        option.textContent = quarter.quarterName;
                        option.setAttribute('data-name', quarter.quarterName);
                        neighborhoodSelect.appendChild(option);
                    });

                    neighborhoodSelect.disabled = false;
                }
            } catch (error) {
                console.error('Error fetching quarters:', error);
            }
        }
    });

    // Neighborhood (Quarter) select change event
    document.getElementById('addressNeighborhood').addEventListener('change', async function () {
        const quarterId = this.value;
        const quarterName = this.options[this.selectedIndex].getAttribute('data-name');

        // Reset dependent select
        const quarterSelect = document.getElementById('addressQuarter');
        quarterSelect.innerHTML = '<option value="">Mahalle seçiniz</option>';
        quarterSelect.disabled = true;

        if (quarterId) {
            try {
                const response = await fetch(`/Addresses/GetDistricts?quarterId=${quarterId}`);
                if (response.ok) {
                    const districts = await response.json();

                    quarterSelect.innerHTML = '<option value="">Mahalle seçiniz</option>';
                    districts.forEach(district => {
                        const option = document.createElement('option');
                        option.value = district.districtId;
                        option.textContent = district.districtName;
                        option.setAttribute('data-name', district.districtName);
                        quarterSelect.appendChild(option);
                    });

                    quarterSelect.disabled = false;
                }
            } catch (error) {
                console.error('Error fetching districts:', error);
            }
        }
    });

    // Phone number formatting
    document.getElementById('addressPhone').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');
        if (!value.startsWith('90') && !value.startsWith('+90')) {
            value = '90' + value;
        }
        if (value.length > 2) {
            value = value.substring(0, 2) + ' ' + value.substring(2);
        }
        if (value.length > 6) {
            value = value.substring(0, 6) + ' ' + value.substring(6);
        }
        if (value.length > 10) {
            value = value.substring(0, 10) + ' ' + value.substring(10, 14);
        }
        e.target.value = '+' + value;
    });

    // Name validation (only letters)
    document.getElementById('addressFullName').addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
    });

    // Form submission
    document.getElementById('addAddressForm').addEventListener('submit', async function (e) {
        e.preventDefault();

        const swalLoading = Swal.fire({
            title: 'Lütfen bekleyin...',
            html: 'Adres kaydediliyor',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        const citySelect = document.getElementById('addressCity');
        const districtSelect = document.getElementById('addressDistrict');
        const neighborhoodSelect = document.getElementById('addressNeighborhood');
        const quarterSelect = document.getElementById('addressQuarter');

        const cityName = citySelect.options[citySelect.selectedIndex].text;
        const townName = districtSelect.options[districtSelect.selectedIndex].getAttribute('data-name');
        const quarterName = neighborhoodSelect.options[neighborhoodSelect.selectedIndex].getAttribute('data-name');
        const districtName = quarterSelect.options[quarterSelect.selectedIndex].getAttribute('data-name');

        const addressData = {
            AddressTitle: document.getElementById('addressTitle').value,
            AddressDetails: document.getElementById('addressDetails').value,
            NameSurname: document.getElementById('addressFullName').value,
            PhoneNumber: document.getElementById('addressPhone').value.replace(/\s/g, ''),
            City: cityName,
            Town: townName,
            Quarter: quarterName,
            District: districtName,
            IsDefault: document.getElementById('setDefaultAddress').checked
        };

        try {
            const response = await fetch('/Addresses/CreateAddress', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(addressData)
            });

            // Yükleme mesajını kapat
            await swalLoading.close();

            if (response.ok) {
                Swal.fire({
                    icon: 'success',
                    title: 'Başarılı',
                    text: 'Adres başarıyla kaydedildi!',
                    confirmButtonText: 'Tamam'
                }).then(() => {
                    setTimeout(() => {
                        location.reload();
                    }, 500);
                });
            } else {
                throw new Error('Adres kaydedilirken bir hata oluştu');
            }
        } catch (error) {
            await swalLoading.close();
            Swal.fire({
                icon: 'error',
                title: 'Hata',
                text: error.message,
                confirmButtonText: 'Tamam'
            });
        }
    });
});

// Varsayılan adres ayarlama fonksiyonu
async function setDefaultAddress(addressId) {
    try {
        const addressCard = document.querySelector(`.address-card[data-id="${addressId}"]`);
        const setDefaultBtn = addressCard.querySelector('.set-default-address-btn');
        const originalHtml = setDefaultBtn?.innerHTML;

        if (setDefaultBtn) {
            setDefaultBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> İşleniyor...';
            setDefaultBtn.disabled = true;
        }

        // CSRF token'ı al
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const response = await fetch('/Addresses/SetDefaultAddress', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify({ addressId: addressId })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.json();

        if (result.success) {
            // 1. Tüm adreslerden default classını ve badge'leri kaldır
            document.querySelectorAll('.address-card').forEach(card => {
                card.classList.remove('default');
                const badge = card.querySelector('.default-badge');
                if (badge) badge.remove();
            });

            // 2. Seçilen adresi default yap
            addressCard.classList.add('default');
            const titleDiv = addressCard.querySelector('.address-card-title');
            titleDiv.insertAdjacentHTML('beforeend', '<span class="default-badge">Varsayılan</span>');

            // 3. Seçilen adresteki butonu kaldır
            const btnToRemove = addressCard.querySelector('.set-default-address-btn');
            if (btnToRemove) btnToRemove.remove();

            // 4. Diğer tüm adreslere buton ekle (eğer yoksa)
            document.querySelectorAll('.address-card:not(.default)').forEach(card => {
                const actionsDiv = card.querySelector('.address-card-actions');
                if (actionsDiv && !actionsDiv.querySelector('.set-default-address-btn')) {
                    const newBtn = createDefaultButton(card.dataset.id);

                    // Sil butonundan önce ekle
                    const deleteBtn = actionsDiv.querySelector('.delete-address-btn');
                    if (deleteBtn) {
                        actionsDiv.insertBefore(newBtn, deleteBtn);
                    } else {
                        actionsDiv.appendChild(newBtn);
                    }
                }
            });

            // Başarı mesajı
            showToast('success', 'Adres varsayılan olarak ayarlandı');
        } else {
            throw new Error(result.message || 'İşlem başarısız oldu');
        }
    } catch (error) {
        console.error('Error:', error);
        showToast('error', error.message || 'Adres varsayılan yapılırken bir hata oluştu');
    } finally {
        const setDefaultBtn = document.querySelector(`.address-card[data-id="${addressId}"] .set-default-address-btn`);
        if (setDefaultBtn) {
            setDefaultBtn.innerHTML = '<i class="fas fa-check-circle"></i> Varsayılan Yap';
            setDefaultBtn.disabled = false;
        }
    }
}

// Buton oluşturma fonksiyonu
function createDefaultButton(addressId) {
    const newBtn = document.createElement('button');
    newBtn.className = 'address-card-btn set-default-address-btn';
    newBtn.innerHTML = '<i class="fas fa-check-circle"></i> Varsayılan Yap';
    newBtn.dataset.addressId = addressId;
    return newBtn;
}

// Toast mesaj fonksiyonu
function showToast(type, message) {
    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    toast.textContent = message;
    document.body.appendChild(toast);

    setTimeout(() => {
        toast.remove();
    }, 3000);
}

// EVENT DELEGATION ile tıklama olayını yakala
document.addEventListener('click', function (e) {
    // Varsayılan Yap butonuna tıklanırsa
    if (e.target.closest('.set-default-address-btn')) {
        const button = e.target.closest('.set-default-address-btn');
        const addressId = button.closest('.address-card').dataset.id;
        setDefaultAddress(addressId);
    }
});

// Sayfa yüklendiğinde butonları kontrol et
document.addEventListener('DOMContentLoaded', function () {
    // Varsayılan olmayan adreslere buton ekle
    document.querySelectorAll('.address-card:not(.default)').forEach(card => {
        const actionsDiv = card.querySelector('.address-card-actions');
        if (actionsDiv && !actionsDiv.querySelector('.set-default-address-btn')) {
            const newBtn = createDefaultButton(card.dataset.id);

            const deleteBtn = actionsDiv.querySelector('.delete-address-btn');
            if (deleteBtn) {
                actionsDiv.insertBefore(newBtn, deleteBtn);
            } else {
                actionsDiv.appendChild(newBtn);
            }
        }
    });

    // Varsayılan adresteki butonu kaldır
    document.querySelectorAll('.address-card.default .set-default-address-btn').forEach(btn => {
        btn.remove();
    });
});

document.addEventListener('DOMContentLoaded', function () {
    // Elementler
    const cardNumberInput = document.getElementById('cardNumber');
    const cardExpiryInput = document.getElementById('cardExpiry');
    const cardCvvInput = document.getElementById('cardCvv');
    const cardHolderInput = document.getElementById('cardHolder');
    const addCardForm = document.getElementById('addCardForm');
    const setDefaultCheckbox = document.getElementById('setDefaultCard');
    const toggleCvvBtn = document.getElementById('toggleCvv');

    // CVV göster/gizle butonu
    toggleCvvBtn.addEventListener('click', function () {
        const type = cardCvvInput.getAttribute('type') === 'password' ? 'text' : 'password';
        cardCvvInput.setAttribute('type', type);
        this.classList.toggle('fa-eye-slash');
        this.classList.toggle('fa-eye');
    });

    // Kart numarası formatlama (16 haneli, 4'lü gruplar)
    cardNumberInput.addEventListener('input', function (e) {
        let value = this.value.replace(/\D/g, '');
        if (value.length > 16) value = value.substring(0, 16);

        let formatted = '';
        for (let i = 0; i < value.length; i++) {
            if (i > 0 && i % 4 === 0) formatted += ' ';
            formatted += value[i];
        }
        this.value = formatted;
    });

    // Son kullanma tarihi formatı (AA/YY)
    cardExpiryInput.addEventListener('input', function (e) {
        let value = this.value.replace(/\D/g, '');
        if (value.length > 2) {
            value = value.substring(0, 2) + '/' + value.substring(2, 4);
        }
        this.value = value;
    });

    // CVV formatlama (sadece 3 rakam)
    cardCvvInput.addEventListener('input', function (e) {
        this.value = this.value.replace(/\D/g, '').substring(0, 3);
    });

    // Kart sahibi adı (sadece harf ve boşluk)
    cardHolderInput.addEventListener('input', function (e) {
        this.value = this.value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
    });

    // Form gönderim işlemi
    addCardForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        // Form elemanları
        const cardNumber = document.getElementById('cardNumber').value.replace(/\s/g, '');
        const cardHolderName = document.getElementById('cardHolder').value.trim();
        const expiry = document.getElementById('cardExpiry').value;
        const cvv = document.getElementById('cardCvv').value;
        const isDefault = document.getElementById('setDefaultCard').checked;

        // Validasyon fonksiyonu
        const showError = async (message) => {
            await Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: message,
                confirmButtonColor: '#3085d6'
            });
            return false;
        };

        // Validasyonlar
        if (cardNumber.length !== 16) return await showError('16 haneli kart numarası giriniz');
        if (!expiry.match(/^\d{2}\/\d{2}$/)) return await showError('AA/YY formatında tarih giriniz');
        if (cvv.length !== 3) return await showError('3 haneli CVV giriniz');
        if (cardHolderName.length < 3) return await showError('Kart sahibi adı en az 3 karakter olmalı');

        // Tarih validasyonu
        const [inputMonth, inputYear] = expiry.split('/');
        const month = parseInt(inputMonth);
        const year = parseInt(inputYear);

        // Şuanki tarih bilgisi
        const now = new Date();
        const currentYear = now.getFullYear() % 100;
        const currentMonth = now.getMonth() + 1;

        // Ay kontrolü (01-12 arası olmalı)
        if (month < 1 || month > 12) {
            return await showError('Geçersiz ay. 01 ile 12 arasında bir değer giriniz.');
        }

        // Yıl kontrolü (geçerli yıldan küçük olamaz)
        if (year < currentYear) {
            return await showError('Kartın son kullanma tarihi geçmiş. Geçerli bir yıl giriniz.');
        }

        // Aynı yıl ise ay kontrolü (geçerli aydan küçük olamaz)
        if (year === currentYear && month < currentMonth) {
            return await showError('Kartın son kullanma tarihi geçmiş. Geçerli bir ay giriniz.');
        }

        try {
            // Loading göster
            Swal.fire({
                title: 'Lütfen bekleyin...',
                allowOutsideClick: false,
                didOpen: () => Swal.showLoading()
            });

            const requestData = {
                CardNumber: cardNumber,
                CardHolderName: cardHolderName,
                ExpirationDate: `20${year}-${month.toString().padStart(2, '0')}-01`,
                CVV: cvv,
                IsDefault: isDefault
            };

            // API isteği
            const response = await fetch('/CreditCard/CreatePaymentCard', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(requestData)
            });

            // Yanıtı işle
            const result = await response.json();

            Swal.close();
            if (result.success) {
                await Swal.fire({
                    icon: 'success',
                    title: 'Başarılı!',
                    text: result.message,
                    confirmButtonColor: '#3085d6'
                });
                window.location.reload();
            } else {
                await Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: result.message,
                    confirmButtonColor: '#3085d6'
                });
            }
        } catch (error) {
            await Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: 'Bir şeyler yanlış gitti',
                confirmButtonColor: '#3085d6'
            });
            console.error('Hata:', error);
        }
    });
});
document.addEventListener('DOMContentLoaded', function () {
    // Kart silme işlemleri
    document.querySelectorAll('.delete-card-btn').forEach(button => {
        button.addEventListener('click', async function (e) {
            e.preventDefault();
            const cardId = this.getAttribute('data-card-id');

            // SweetAlert ile onay iste
            const result = await Swal.fire({
                title: 'Emin misiniz?',
                text: "Bu kartı silmek istediğinize emin misiniz? Bu işlem geri alınamaz!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, Sil!',
                cancelButtonText: 'İptal'
            });

            if (result.isConfirmed) {
                try {
                    // Loading göster
                    Swal.fire({
                        title: 'Siliniyor...',
                        allowOutsideClick: false,
                        didOpen: () => Swal.showLoading()
                    });

                    // Controller'a istek gönder
                    const response = await fetch(`/CreditCard/DeleteCreditCard?id=${cardId}`, {
                        method: 'POST',
                        headers: {
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                        }
                    });

                    const data = await response.json();

                    if (data.success) {
                        await Swal.fire({
                            icon: 'success',
                            title: 'Başarılı!',
                            text: 'Kart başarıyla silindi',
                            confirmButtonColor: '#3085d6'
                        });
                        window.location.reload();
                    } else {
                        throw new Error(data.message || 'Silme işlemi başarısız oldu');
                    }
                } catch (error) {
                    await Swal.fire({
                        icon: 'error',
                        title: 'Hata!',
                        text: error.message || 'Kart silinirken bir hata oluştu',
                        confirmButtonColor: '#3085d6'
                    });
                }
            }
        });
    });
});
// Kart düzenleme modalını açma ve verileri yükleme
document.querySelectorAll('.edit-card-btn').forEach(button => {
    button.addEventListener('click', function () {
        const cardId = this.getAttribute('data-card-id');
        const cardContainer = this.closest('.card-container');

        // Kart bilgilerini al
        const cardNumber = cardContainer.querySelector('.card-number').textContent.replace(/\s/g, '');
        const cardHolder = cardContainer.querySelector('.card-holder-name').textContent;
        const expiry = cardContainer.querySelector('.card-expiry').textContent;
        const cvv = cardContainer.querySelector('.card-cvv-number').textContent;
        const isDefault = cardContainer.querySelector('.card-inner').classList.contains('default-card');

        // Kart numarasını formatla (4'lü gruplar halinde)
        let formattedCardNumber = '';
        for (let i = 0; i < cardNumber.length; i++) {
            if (i > 0 && i % 4 === 0) formattedCardNumber += ' ';
            formattedCardNumber += cardNumber[i];
        }

        // Modalı doldur
        document.getElementById('editCardId').value = cardId;
        document.getElementById('editCardNumber').value = formattedCardNumber;
        document.getElementById('editCardExpiry').value = expiry;
        document.getElementById('editCardCvv').value = cvv;
        document.getElementById('editCardHolder').value = cardHolder;
        document.getElementById('editSetDefaultCard').checked = isDefault;

        // Modalı aç (flex olarak göster)
        document.getElementById('editCardModal').style.display = 'flex';
    });
});

// Modal kapatma
document.querySelector('.close-modal').addEventListener('click', function () {
    document.getElementById('editCardModal').style.display = 'none';
});

// CVV göster/gizle butonu için HTML'i oluştur
const cvvContainer = document.querySelector('#editCardForm .form-row .form-group:last-child');
cvvContainer.innerHTML = `
    <label>CVV</label>
    <div class="cvv-input-container">
        <input type="password" id="editCardCvv" placeholder="•••" maxlength="3" pattern="\\d*" required>
        <i class="fas fa-eye toggle-cvv" id="toggleEditCvv"></i>
    </div>
`;

// CVV göster/gizle butonu
document.querySelectorAll('.edit-card-btn').forEach(button => {
    button.addEventListener('click', function () {
        const cardContainer = this.closest('.card-container');
        const realCvv = cardContainer.querySelector('.card-cvv-number').getAttribute('data-cvv') ||
            cardContainer.querySelector('.card-cvv-number').textContent.trim();

        // Input'a gerçek CVV'yi data attribute olarak sakla
        document.getElementById('editCardCvv').setAttribute('data-real-cvv', realCvv);

        // Input değerini başlangıçta '•••' olarak ayarla ama type'ını text yap
        document.getElementById('editCardCvv').value = realCvv;
        document.getElementById('editCardCvv').setAttribute('type', 'password');

        // Göz ikonunu sıfırla
        document.getElementById('toggleEditCvv').classList.remove('fa-eye-slash');
        document.getElementById('toggleEditCvv').classList.add('fa-eye');
    });
});

// CVV göster/gizle butonu
document.getElementById('toggleEditCvv').addEventListener('click', function () {
    const cvvInput = document.getElementById('editCardCvv');
    const type = cvvInput.getAttribute('type') === 'password' ? 'text' : 'password';
    cvvInput.setAttribute('type', type);

    // İkonu değiştir
    this.classList.toggle('fa-eye-slash');
    this.classList.toggle('fa-eye');
});

// Kart numarası formatlama (16 haneli, 4'lü gruplar)
document.getElementById('editCardNumber').addEventListener('input', function (e) {
    let value = this.value.replace(/\D/g, '');
    if (value.length > 16) value = value.substring(0, 16);

    let formatted = '';
    for (let i = 0; i < value.length; i++) {
        if (i > 0 && i % 4 === 0) formatted += ' ';
        formatted += value[i];
    }
    this.value = formatted;
});

// Son kullanma tarihi formatı (AA/YY)
document.getElementById('editCardExpiry').addEventListener('input', function (e) {
    let value = this.value.replace(/\D/g, '');
    if (value.length > 2) {
        value = value.substring(0, 2) + '/' + value.substring(2, 4);
    }
    this.value = value;
});

// CVV formatlama (sadece 3 rakam)
document.getElementById('editCardCvv').addEventListener('input', function (e) {
    this.value = this.value.replace(/\D/g, '').substring(0, 3);
});

// Kart sahibi adı (sadece harf ve boşluk)
document.getElementById('editCardHolder').addEventListener('input', function (e) {
    this.value = this.value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
});

// Form gönderim işlemi
document.getElementById('editCardForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    // Form elemanları
    const cardId = document.getElementById('editCardId').value;
    const cardNumber = document.getElementById('editCardNumber').value.replace(/\s/g, '');
    const cardHolderName = document.getElementById('editCardHolder').value.trim();
    const expiry = document.getElementById('editCardExpiry').value;
    const cvv = document.getElementById('editCardCvv').value;
    const isDefault = document.getElementById('editSetDefaultCard').checked;

    // Validasyon fonksiyonu
    const showError = async (message) => {
        await Swal.fire({
            icon: 'error',
            title: 'Hata!',
            text: message,
            confirmButtonColor: '#3085d6'
        });
        return false;
    };

    // Validasyonlar
    if (cardNumber.length !== 16) return await showError('16 haneli kart numarası giriniz');
    if (!expiry.match(/^\d{2}\/\d{2}$/)) return await showError('AA/YY formatında tarih giriniz');
    if (cvv.length !== 3) return await showError('3 haneli CVV giriniz');
    if (cardHolderName.length < 3) return await showError('Kart sahibi adı en az 3 karakter olmalı');

    // Tarih validasyonu
    const [inputMonth, inputYear] = expiry.split('/');
    const month = parseInt(inputMonth);
    const year = parseInt(inputYear);

    // Şuanki tarih bilgisi
    const now = new Date();
    const currentYear = now.getFullYear() % 100;
    const currentMonth = now.getMonth() + 1;

    // Ay kontrolü (01-12 arası olmalı)
    if (month < 1 || month > 12) {
        return await showError('Geçersiz ay. 01 ile 12 arasında bir değer giriniz.');
    }

    // Yıl kontrolü (geçerli yıldan küçük olamaz)
    if (year < currentYear) {
        return await showError('Kartın son kullanma tarihi geçmiş. Geçerli bir yıl giriniz.');
    }

    // Aynı yıl ise ay kontrolü (geçerli aydan küçük olamaz)
    if (year === currentYear && month < currentMonth) {
        return await showError('Kartın son kullanma tarihi geçmiş. Geçerli bir ay giriniz.');
    }

    try {
        // Loading göster
        Swal.fire({
            title: 'Kart bilgileri güncelleniyor...',
            allowOutsideClick: false,
            didOpen: () => Swal.showLoading()
        });

        const requestData = {
            PaymentCardID: cardId,
            CardNumber: cardNumber,
            CardHolderName: cardHolderName,
            ExpirationDate: `20${year}-${month.toString().padStart(2, '0')}-01`,
            CVV: cvv,
            IsDefault: isDefault
        };

        // Controller'a istek gönder
        const response = await fetch('/CreditCard/UpdateCreditCard', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(requestData)
        });

        const result = await response.json();

        Swal.close();
        if (result.success) {
            await Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: 'Kart bilgileri başarıyla güncellendi',
                confirmButtonColor: '#3085d6'
            });
            document.getElementById('editCardModal').style.display = 'none';
            window.location.reload();
        } else {
            throw new Error(result.message || 'Güncelleme işlemi başarısız oldu');
        }
    } catch (error) {
        await Swal.fire({
            icon: 'error',
            title: 'Hata!',
            text: error.message || 'Kart güncellenirken bir hata oluştu',
            confirmButtonColor: '#3085d6'
        });
    }
});
// CVV göster/gizle toggle işlevi
document.getElementById('showCvvToggle').addEventListener('change', function () {
    const showCvv = this.checked;
    const cvvElements = document.querySelectorAll('.card-cvv-number');

    cvvElements.forEach(element => {
        if (showCvv) {
            // Gerçek CVV'yi göster (data-cvv attribute'unda saklıyoruz)
            element.textContent = element.getAttribute('data-cvv');
        } else {
            // CVV'yi gizle ve yıldız koy
            element.textContent = '•••';
        }
    });
});

// Sayfa yüklendiğinde CVV'leri gizle ve gerçek değerleri data attribute'unda sakla
document.addEventListener('DOMContentLoaded', function () {
    const cvvElements = document.querySelectorAll('.card-cvv-number');

    cvvElements.forEach(element => {
        // Gerçek CVV'yi data attribute'unda sakla
        const realCvv = element.textContent.trim();
        element.setAttribute('data-cvv', realCvv);

        // Başlangıçta yıldız göster
        element.textContent = '•••';
    });
});

document.addEventListener('DOMContentLoaded', function () {
    // Varsayılan kart butonuna tıklanınca
    document.querySelectorAll('.set-default-card-btn').forEach(button => {
        button.addEventListener('click', function () {
            const cardId = this.getAttribute('data-card-id');
            setDefaultCard(cardId);
        });
    });
});

function setDefaultCard(cardId) {
    const formData = new FormData();
    formData.append('paymentCardId', cardId);

    fetch('/CreditCard/DefaultCreditCard', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Tüm kartlardan default-card classını kaldır
                document.querySelectorAll('.card-inner').forEach(card => {
                    card.classList.remove('default-card');
                });

                // Tüm kartlara "Varsayılan Yap" butonunu ekle (eğer yoksa)
                document.querySelectorAll('.card-container').forEach(container => {
                    const cardId = container.querySelector('.card').getAttribute('data-card-id');
                    const defaultBtn = container.querySelector('.set-default-card-btn');

                    if (!defaultBtn) {
                        const btnHtml = `
                        <button class="card-btn set-default-card-btn" data-card-id="${cardId}">
                            <i class="fas fa-check-circle"></i> Varsayılan Yap
                        </button>
                    `;

                        // Sil butonundan önce ekle
                        const deleteBtn = container.querySelector('.delete-card-btn');
                        deleteBtn.insertAdjacentHTML('beforebegin', btnHtml);

                        // Yeni eklenen butona event listener ekle
                        const newBtn = container.querySelector(`.set-default-card-btn[data-card-id="${cardId}"]`);
                        newBtn.addEventListener('click', function () {
                            setDefaultCard(cardId);
                        });
                    }
                });

                // Seçilen karta default-card classını ekle
                const selectedCard = document.querySelector(`.card[data-card-id="${cardId}"] .card-inner`);
                if (selectedCard) {
                    selectedCard.classList.add('default-card');

                    // Seçilen kartın "Varsayılan Yap" butonunu kaldır
                    const cardContainer = selectedCard.closest('.card-container');
                    const defaultBtn = cardContainer.querySelector('.set-default-card-btn');
                    if (defaultBtn) {
                        defaultBtn.remove();
                    }
                }

                // Başarı mesajı göster
                toastr.success('Varsayılan kart başarıyla ayarlandı');
            } else {
                toastr.error(data.message || 'Varsayılan kart ayarlanamadı');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            toastr.error('Bir hata oluştu');
        });
}

document.addEventListener('DOMContentLoaded', function () {
    // Modal elementleri
    const changePasswordModal = document.getElementById('changePasswordModal');
    const changePasswordForm = document.getElementById('changePasswordForm');
    const closeModalBtn = document.querySelector('#changePasswordModal .close-modal');

    // Şifre göster/gizle butonları
    const togglePasswordBtns = document.querySelectorAll('#changePasswordModal .toggle-password');

    // Modal açıldığında çalışacak fonksiyon
    function setupPasswordModal() {
        // Formu ve şifre alanlarını resetle
        changePasswordForm.reset();

        // Tüm şifre alanlarını gizle ve ikonları ayarla
        document.querySelectorAll('#changePasswordModal input[type="password"], #changePasswordModal input[type="text"]').forEach(input => {
            input.type = 'password';
            const icon = input.nextElementSibling;
            if (icon) {
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            }
        });
    }

    // Şifre göster/gizle işlevi
    togglePasswordBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            const input = this.previousElementSibling;
            const isPassword = input.type === 'password';

            input.type = isPassword ? 'text' : 'password';
            this.classList.toggle('fa-eye-slash');
            this.classList.toggle('fa-eye');
        });
    });

    // Modal açma fonksiyonu
    function openChangePasswordModal() {
        setupPasswordModal();
        changePasswordModal.style.display = 'flex';
        document.body.style.overflow = 'hidden';
    }

    // Modal kapatma fonksiyonu
    function closeChangePasswordModal() {
        changePasswordModal.style.display = 'none';
        document.body.style.overflow = 'auto';
    }

    // Modal dışına tıklayarak kapatma
    changePasswordModal.addEventListener('click', function (e) {
        if (e.target === changePasswordModal) {
            closeChangePasswordModal();
        }
    });

    // Kapat butonu
    closeModalBtn.addEventListener('click', closeChangePasswordModal);

    // Şifre doğrulama fonksiyonu
    function validatePassword(password) {
        const minLength = 8;
        const hasUpperCase = /[A-ZĞÜŞİÖÇ]/.test(password);
        const hasLowerCase = /[a-zğüşıöç]/.test(password);
        const hasNumber = /[0-9]/.test(password);
        const hasSpecialChar = /[*+.]/.test(password);

        if (password.length < minLength) {
            return { valid: false, message: 'Şifre en az 8 karakter olmalıdır' };
        }
        if (!hasUpperCase) {
            return { valid: false, message: 'Şifre en az 1 büyük harf içermelidir' };
        }
        if (!hasLowerCase) {
            return { valid: false, message: 'Şifre en az 1 küçük harf içermelidir' };
        }
        if (!hasNumber) {
            return { valid: false, message: 'Şifre en az 1 rakam içermelidir' };
        }
        if (!hasSpecialChar) {
            return { valid: false, message: 'Şifre en az 1 özel karakter (*, +, .) içermelidir' };
        }

        return { valid: true };
    }

    // Form gönderme işlemi
    changePasswordForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        const currentPassword = document.getElementById('currentPassword').value;
        const newPassword = document.getElementById('newPassword').value;
        const confirmPassword = document.getElementById('confirmPassword').value;

        // Yeni şifrelerin eşleştiğini kontrol et
        if (newPassword !== confirmPassword) {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: 'Girdiğiniz yeni şifreler eşleşmiyor!',
                confirmButtonText: 'Tamam'
            });
            return;
        }

        // Şifre kurallarını kontrol et
        const passwordValidation = validatePassword(newPassword);
        if (!passwordValidation.valid) {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: passwordValidation.message,
                confirmButtonText: 'Tamam'
            });
            return;
        }

        // Şifre değiştirme işlemi
        Swal.fire({
            title: 'Lütfen Bekleyin',
            html: 'Şifreniz değiştiriliyor...',
            allowOutsideClick: false,
            didOpen: () => { Swal.showLoading(); }
        });

        // Burada gerçek bir API isteği yapılacak
        try {
            // Örnek API isteği (gerçek uygulamada token vb. eklemeler yapılmalı)
            const response = await fetch('/api/auth/change-password', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                },
                body: JSON.stringify({
                    currentPassword: currentPassword,
                    newPassword: newPassword,
                    confirmPassword: confirmPassword
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || 'Şifre değiştirme başarısız oldu');
            }

            // Başarılı yanıt
            Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: 'Şifreniz başarıyla güncellendi',
                confirmButtonText: 'Tamam'
            }).then(() => {
                // Modalı kapat ve formu temizle
                closeChangePasswordModal();
                setupPasswordModal();
            });

        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: error.message || 'Şifre değiştirilirken bir hata oluştu',
                confirmButtonText: 'Tamam'
            });
        }
    });

    // Modal açma butonuna event listener ekleme (örnek)
    const changePasswordBtn = document.getElementById('changePasswordBtn');
    if (changePasswordBtn) {
        changePasswordBtn.addEventListener('click', openChangePasswordModal);
    }
});

document.addEventListener('DOMContentLoaded', function () {
    // Elementleri seç
    const editProfileModal = document.getElementById('editProfileModal');
    const editProfileBtn = document.getElementById('editProfileBtn');
    const editProfileForm = document.getElementById('editProfileForm');
    const closeModalBtn = editProfileModal.querySelector('.close-modal');

    // Profil bilgilerini forma yükleme fonksiyonu
    // Profil bilgilerini forma yükleme fonksiyonu (Güncellenmiş versiyon)
    function loadProfileDataToModal() {
        try {
            // Ad soyad bilgisini al ve ayır
            const userFullName = document.getElementById('userFullName').textContent.trim();
            const nameParts = userFullName.split(' ');
            const firstName = nameParts[0] || '';
            const lastName = nameParts.slice(1).join(' ') || '';

            // E-posta bilgisini al (doğrulama badge'ini tamamen kaldır)
            const emailElement = document.getElementById('userEmail');
            let email = emailElement.textContent.trim();

            // Doğrulama durumunu kontrol etmek için ayrı bir işlem yap
            const isEmailVerified = emailElement.querySelector('.verified-badge') !== null;

            // E-posta adresini temizle (doğrulama durumunu kaldır)
            email = email.replace('Doğrulanmış', '')
                .replace('Doğrulanmamış', '')
                .replace(/\s+/g, ' ') // Fazla boşlukları temizle
                .trim();

            // Telefon numarasını al
            let phone = document.getElementById('userPhone').textContent.trim();

            // Doğum tarihini al ve formatla (DD.MM.YYYY -> YYYY-MM-DD)
            let birthDateText = document.getElementById('userBirthDate').textContent.trim();
            let birthDate = '';

            if (birthDateText) {
                const dateParts = birthDateText.split('.');
                if (dateParts.length === 3) {
                    const day = dateParts[0].padStart(2, '0');
                    const month = dateParts[1].padStart(2, '0');
                    const year = dateParts[2];
                    birthDate = `${year}-${month}-${day}`;
                }
            }

            // Form alanlarını doldur
            document.getElementById('editFirstName').value = firstName;
            document.getElementById('editLastName').value = lastName;
            document.getElementById('editEmail').value = email;
            document.getElementById('editPhone').value = phone;
            document.getElementById('editBirthDate').value = birthDate;

            return true;
        } catch (error) {
            console.error('Profil bilgileri yüklenirken hata:', error);
            return false;
        }
    }

    // Telefon numarası formatlama fonksiyonu
    function formatPhoneNumber(phone) {
        if (!phone) return '+90 ';

        // +90 eklenmemişse ekle
        if (!phone.startsWith('+90')) {
            phone = '+90' + phone.replace(/\D/g, '');
        }

        // Boşlukları kaldır
        phone = phone.replace(/\s/g, '');

        // Format: +90 XXX XXX XXXX
        let formatted = phone.substring(0, 3); // +90
        if (phone.length > 3) formatted += ' ' + phone.substring(3, 6);
        if (phone.length > 6) formatted += ' ' + phone.substring(6, 9);
        if (phone.length > 9) formatted += ' ' + phone.substring(9, 12);

        return formatted;
    }

    // Modal açma fonksiyonu
    function openEditProfileModal() {
        // Profil bilgilerini forma yükle
        if (loadProfileDataToModal()) {
            editProfileModal.style.display = 'flex';
            document.body.style.overflow = 'hidden';
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Hata',
                text: 'Profil bilgileri yüklenirken bir hata oluştu',
                confirmButtonText: 'Tamam'
            });
        }
    }

    // Modal kapatma fonksiyonu
    function closeEditProfileModal() {
        editProfileModal.style.display = 'none';
        document.body.style.overflow = 'auto';
    }

    // Telefon numarası input formatlama
    document.getElementById('editPhone').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        // +90 eklenmemişse ekle
        if (!value.startsWith('90') && !value.startsWith('+90')) {
            value = '90' + value;
        }

        // Formatlama
        let formatted = '+90';
        if (value.length > 2) formatted += ' ' + value.substring(2, 5);
        if (value.length > 5) formatted += ' ' + value.substring(5, 8);
        if (value.length > 8) formatted += ' ' + value.substring(8, 12);

        e.target.value = formatted;
    });

    // Ad ve soyad için sadece harf kontrolü
    document.getElementById('editFirstName').addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
    });

    document.getElementById('editLastName').addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
    });

    // Form gönderme işlemi
    editProfileForm.addEventListener('submit', function (e) {
        e.preventDefault();
        handleProfileUpdate(e);
    });

    // Event listener'ları ekle
    editProfileBtn.addEventListener('click', openEditProfileModal);
    closeModalBtn.addEventListener('click', closeEditProfileModal);

    // Modal dışına tıklayarak kapatma
    editProfileModal.addEventListener('click', function (e) {
        if (e.target === editProfileModal) {
            closeEditProfileModal();
        }
    });

    // Doğum tarihi için flatpickr ayarı
    if (document.getElementById('editBirthDate')) {
        flatpickr("#editBirthDate", {
            dateFormat: "Y-m-d",
            maxDate: new Date().fp_incr(-18 * 365), // 18 yaşından büyük olmalı
            locale: "tr"
        });
    }
});

// Profil güncelleme fonksiyonu
function handleProfileUpdate(e) {
    e.preventDefault();

    const firstName = document.getElementById('editFirstName').value.trim();
    const lastName = document.getElementById('editLastName').value.trim();
    const email = document.getElementById('editEmail').value.trim();
    const phone = document.getElementById('editPhone').value.trim();
    const birthDate = document.getElementById('editBirthDate').value;

    // Validasyonlar
    if (!isValidName(firstName) || !isValidName(lastName)) {
        Swal.fire({
            icon: 'error',
            title: 'Hata',
            text: 'Ad ve soyad sadece harflerden oluşmalıdır',
            confirmButtonText: 'Tamam'
        });
        return;
    }

    if (!isValidEmail(email)) {
        Swal.fire({
            icon: 'error',
            title: 'Hata',
            text: 'Geçerli bir e-posta adresi giriniz',
            confirmButtonText: 'Tamam'
        });
        return;
    }

    if (!isValidPhone(phone)) {
        Swal.fire({
            icon: 'error',
            title: 'Hata',
            text: 'Geçerli bir telefon numarası giriniz (+90 ile başlayan 10 haneli)',
            confirmButtonText: 'Tamam'
        });
        return;
    }

    let originalEmail = document.getElementById('userEmail').textContent.trim();
    const badges = document.getElementById('userEmail').querySelectorAll('.verified-badge, .not-verified-badge');
    badges.forEach(badge => {
        originalEmail = originalEmail.replace(/\s*(Doğrulanmış|Doğrulanmamış)\s*/g, '').trim();
    });

    const isEmailVerified = document.getElementById('userEmail').querySelector('.verified-badge') !== null;
    const emailChanged = email !== originalEmail;

    Swal.fire({
        title: 'Lütfen Bekleyin',
        html: 'Profil bilgileriniz güncelleniyor...',
        allowOutsideClick: false,
        didOpen: () => { Swal.showLoading(); }
    });

    const updateProfileDto = {
        NameSurname: firstName + ' ' + lastName,
        Email: email,
        EmailCheck: emailChanged ? false : isEmailVerified,
        PhoneNumber: phone.replace(/\s/g, ''),
        BirtDay: birthDate
    };

    fetch('/api/auth/update-profile', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        },
        body: JSON.stringify(updateProfileDto)
    })
        .then(async response => {
            if (!response.ok) {
                let errorText;
                try {
                    const errorData = await response.json();
                    errorText = errorData.message || response.statusText;
                } catch {
                    errorText = response.statusText;
                }
                throw new Error(errorText || 'İstek başarısız oldu');
            }
            return response.json();
        })
        .then(data => {
            Swal.fire({
                icon: 'success',
                title: 'Başarılı',
                text: data.message || 'Profil bilgileriniz başarıyla güncellendi!',
                confirmButtonText: 'Tamam'
            }).then(() => {
                location.reload();
            });
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                icon: 'error',
                title: 'Hata',
                text: error.message || 'Profil güncellenirken bir hata oluştu',
                confirmButtonText: 'Tamam'
            });
        });
}

// Validasyon fonksiyonları
function isValidEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

function isValidPhone(phone) {
    const re = /^\+90\s?\d{3}\s?\d{3}\s?\d{4}$/;
    return re.test(phone);
}

function isValidName(name) {
    const re = /^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$/;
    return re.test(name);
}