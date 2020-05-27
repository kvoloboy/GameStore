const SELECT_CONTAINERS = document.querySelectorAll('.select-container');
SELECT_CONTAINERS.forEach(container => container.addEventListener('click', selectClickHandler));

function selectClickHandler(e) {
    const SELECT_CONTAINER = e.currentTarget;
    const SELECTED_OPTION = SELECT_CONTAINER.querySelector('.selected-option');
    const OPTIONS_CONTAINER = SELECT_CONTAINER.querySelector(".options-container");
    const ARROW = SELECT_CONTAINER.querySelector('.select-arrow');

    if (e.target.classList.contains('form-sender')) {
        SELECTED_OPTION.innerHTML = e.target.innerHTML;
        OPTIONS_CONTAINER.classList.remove("active");
        ARROW.classList.remove("rotate");
    } else {
        OPTIONS_CONTAINER.classList.toggle("active");
        ARROW.classList.toggle("rotate");
    }
}