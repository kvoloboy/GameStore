const RATING_CONTAINERS = document.querySelectorAll(".rating-container");

RATING_CONTAINERS.forEach(container => container.addEventListener('click', ratingClickHandler));

function ratingClickHandler(e) {
    e.preventDefault();

    if (!e.target.classList.contains('star')) {
        return;
    }

    const STAR_LINK = e.target.parentNode;
    const PATH = STAR_LINK.getAttribute('href');

    fetch(PATH)
        .then(function (response) {
                if (!response.ok) {
                    return;
                }

                return response.text().then(function (text) {
                    RATING_CONTAINERS.forEach(container => container.innerHTML = text);
                })
            }
        )
}