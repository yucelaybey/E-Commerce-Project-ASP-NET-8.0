document.addEventListener("DOMContentLoaded", function () {
    const slider = document.querySelector('.slider');
    const sliderWrapper = document.querySelector('.slider-wrapper');
    const prevBtn = document.querySelector('.slider-arrow-left');
    const nextBtn = document.querySelector('.slider-arrow-right');
    const sliderItems = document.querySelectorAll('.slider-item');
    let autoSlideInterval;
    let visibleItemsCount;
    let itemWidth;
    let currentIndex = 0;

    function calculateLayout() {
        const wrapperWidth = sliderWrapper.offsetWidth;
        const minItemWidth = 200;

        if (wrapperWidth <= 400) {
            visibleItemsCount = Math.min(Math.floor(wrapperWidth / minItemWidth), sliderItems.length);
            visibleItemsCount = Math.max(visibleItemsCount, 1); // En az 1 olsun
        } else {
            visibleItemsCount = Math.min(Math.floor(wrapperWidth / minItemWidth), sliderItems.length);
        }

        itemWidth = wrapperWidth / visibleItemsCount;

        // 400px altýnda slider geniþliðini yarýya düþür
        const sliderTotalWidth = wrapperWidth <= 400 ?
            ((itemWidth * sliderItems.length) / 2) :
            itemWidth * sliderItems.length;

        slider.style.width = `${sliderTotalWidth}px`;

        // Slider item geniþlikleri normal hesaplamayla ayný kalýyor
        sliderItems.forEach(item => {
            item.style.width = `${itemWidth}px`;
        });

        updateSliderPosition();
        updateControls();
    }

    function updateSliderPosition() {
        const isMobile = sliderWrapper.offsetWidth <= 400;
        const multiplier = isMobile ? 0.5 : 1; // 400px altýnda yarý hýz

        const offset = -(currentIndex * itemWidth * multiplier);
        slider.style.transform = `translateX(${offset}px)`;
    }

    function updateControls() {
        prevBtn.style.display = currentIndex === 0 ? 'none' : 'block';
        nextBtn.style.display = sliderWrapper.offsetWidth <= 400 ? currentIndex >= sliderItems.length - visibleItemsCount - 1 ? 'none' : 'block' : currentIndex >= sliderItems.length - visibleItemsCount ? 'none' : 'block';
    }

    function nextSlide() {
        if (currentIndex < (sliderItems.length - visibleItemsCount) - (sliderWrapper.offsetWidth <= 400 ? 1 : 0)) {
            currentIndex++;
            updateSliderPosition();
        } else {
            currentIndex = 0;
            updateSliderPosition();
        }
        updateControls();
    }

    function prevSlide() {
        if (currentIndex > 0) {
            currentIndex--;
            updateSliderPosition();
        } else {
            currentIndex = sliderItems.length - visibleItemsCount;
            updateSliderPosition();
        }
        updateControls();
    }

    function startAutoSlide() {
        autoSlideInterval = setInterval(nextSlide, 3000);
    }

    // Event listeners
    nextBtn.addEventListener('click', function () {
        clearInterval(autoSlideInterval);
        nextSlide();
        startAutoSlide();
    });

    prevBtn.addEventListener('click', function () {
        clearInterval(autoSlideInterval);
        prevSlide();
        startAutoSlide();
    });

    // Fare üzerindeyken duraklat
    sliderWrapper.addEventListener('mouseenter', function () {
        clearInterval(autoSlideInterval);
    });

    sliderWrapper.addEventListener('mouseleave', startAutoSlide);

    // Dokunmatik destek
    let touchStartX = 0;
    let touchEndX = 0;

    slider.addEventListener('touchstart', function (e) {
        touchStartX = e.changedTouches[0].screenX;
        clearInterval(autoSlideInterval);
    }, { passive: true });

    slider.addEventListener('touchend', function (e) {
        touchEndX = e.changedTouches[0].screenX;
        handleSwipe();
        startAutoSlide();
    }, { passive: true });

    function handleSwipe() {
        const diff = touchStartX - touchEndX;
        if (diff > 50) nextSlide();
        if (diff < -50) prevSlide();
    }

    // Pencere boyutu deðiþtiðinde yeniden hesapla
    window.addEventListener('resize', function () {
        calculateLayout();
    });

    // Baþlangýçta hesapla ve baþlat
    calculateLayout();
    startAutoSlide();
});