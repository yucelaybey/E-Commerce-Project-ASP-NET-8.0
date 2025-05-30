window.addEventListener("load", function () {
    const sliderContainer = document.querySelector(".slider-container");
    const slider = document.querySelector(".slider");
    let cards = Array.from(document.querySelectorAll(".slider-card"));
    const cardGap = 20;

    let cardWidth, visibleCards;
    let position = 0;
    let direction = "right";
    let interval;

    function updateDimensions() {
        cardWidth = cards[0].offsetWidth;
        visibleCards = Math.floor(sliderContainer.clientWidth / (cardWidth + cardGap));
        return { cardWidth, visibleCards };
    }

    function showSlide(index) {
        const offset = (sliderContainer.clientWidth - (visibleCards * (cardWidth + cardGap))) / 2;
        slider.style.transform = `translateX(-${index * (cardWidth + cardGap)}px)`;
        slider.style.transition = "transform 0.5s ease-in-out";
    }

    function autoSlide() {
        if (cards.length <= visibleCards) {
            clearInterval(interval);
            return;
        }

        if (direction === "right") {
            position = (position + 1) % cards.length;
            if (position + visibleCards >= cards.length) {
                direction = "left";
            }
        } else {
            position = (position - 1 + cards.length) % cards.length;
            if (position === 0) {
                direction = "right";
            }
        }
        showSlide(position);
    }

    function startAutoSlide() {
        if (cards.length > visibleCards) {
            interval = setInterval(autoSlide, 3000);
        }
    }

    sliderContainer.addEventListener("mouseenter", () => clearInterval(interval));
    sliderContainer.addEventListener("mouseleave", startAutoSlide);

    window.addEventListener("resize", () => {
        let dims = updateDimensions();
        cardWidth = dims.cardWidth;
        visibleCards = dims.visibleCards;

        if (cards.length <= visibleCards) {
            clearInterval(interval);
        } else {
            showSlide(position);
        }
    });

    ({ cardWidth, visibleCards } = updateDimensions());
    if (cards.length > visibleCards) {
        startAutoSlide();
    }
});
