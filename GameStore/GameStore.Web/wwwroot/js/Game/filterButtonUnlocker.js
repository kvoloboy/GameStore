const form = document.querySelector("#filter");
form.addEventListener("change", handleFormChange);

function handleFormChange() {
    const filterButton = document.querySelector('#filter-btn');
    filterButton.removeAttribute('disabled');
}