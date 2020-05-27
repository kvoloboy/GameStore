const SLIDES = document.querySelectorAll('.slide');
const DEFAULT_SLIDE = 1;
const LAST_SLIDE = SLIDES.length;
const PREVIOUS_BUTTON = document.querySelector('.previous');
const NEXT_BUTTON = document.querySelector('.next');

let currentSlide = DEFAULT_SLIDE;

PREVIOUS_BUTTON.addEventListener('click', leftScroll);
NEXT_BUTTON.addEventListener('click', rightScroll);

const SLIDER_BUTTONS = document.querySelectorAll('.slider-nav-button');
SLIDER_BUTTONS.forEach(button => button.addEventListener('click', () => {
    currentSlide = +button.getAttribute('value');
    enableSlide();
}));

function enableSlide() {
    for (let slideIndex = 0; slideIndex < LAST_SLIDE; slideIndex++) {
        if (currentSlide == slideIndex + 1) {
            SLIDES[slideIndex].classList.add('slide-active');
        } else {
            SLIDES[slideIndex].classList.remove('slide-active');
        }
    }
}

function leftScroll() {
    if (currentSlide == DEFAULT_SLIDE) {
        currentSlide = LAST_SLIDE;
    } else {
        --currentSlide;
    }

    enableSlide()
}

function rightScroll() {
    if (currentSlide == LAST_SLIDE) {
        currentSlide = DEFAULT_SLIDE
    } else {
        ++currentSlide;
    }

    enableSlide();
}

(function initSlider() {
    enableSlide();
})();