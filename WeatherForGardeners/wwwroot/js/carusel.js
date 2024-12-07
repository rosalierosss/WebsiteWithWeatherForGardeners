document.addEventListener("DOMContentLoaded", () => {
    const container = document.querySelector(".scrolling-content");
    const items = Array.from(container.children);
    const itemWidth = items[0].offsetWidth;
    let currentIndex = 0;

    function updateCarousel() {
        container.style.transform = `translateX(-${currentIndex * itemWidth}px)`;

        currentIndex++;
        if (currentIndex >= items.length - 2) {
            currentIndex = 0;
        }
    }

    setInterval(updateCarousel, 5000);
});