const CONTAINER = document.querySelector(".main-page");
const PAGE_NUMBER_QUERY_NAME = "PageNumber";
const SORT_QUERY_NAME = "SortOption";
const PAGE_SIZE_QUERY_NAME = "PageSize";

CONTAINER.addEventListener("click", sendFilterForm);

function sendFilterForm(e) {
    const CLICKED_ELEMENT = e.target.tagName == "IMG" ? e.target.parentNode : e.target;
    const IS_FORM_SENDER = CLICKED_ELEMENT.classList.contains('form-sender');

    let requestUrl;

    if (!IS_FORM_SENDER) {
        return;
    }

    e.preventDefault();

    if (CLICKED_ELEMENT.tagName == "A") {
        requestUrl = `${window.location.origin}${CLICKED_ELEMENT.getAttribute('href')}`;
    } else {
        const FORM = document.querySelector('#filter');
        const FORM_DATA = new FormData(FORM);
        const QUERY = new URLSearchParams(FORM_DATA);
        requestUrl = `${FORM.action}?${QUERY.toString()}`;
    }

    fetch(requestUrl)
        .then(handleResponse);

    updatePaths(SORT_QUERY_NAME, "sort-option", requestUrl);
    updatePaths(PAGE_SIZE_QUERY_NAME, "page-size-option", requestUrl);
    updatePaths(PAGE_NUMBER_QUERY_NAME, "page-number", requestUrl, true);
}

function handleResponse(response) {
    if (!response.ok) {
        return;
    }

    return response.text()
        .then(function (text) {
            document.querySelector('.filtered-games').innerHTML = text;
        });
}

function updatePaths(queryName, className, url, isPagination) {
    const OPTIONS = document.querySelectorAll(`.${className}`);
    const CURRENT_URL = new URL(url);
    const QUERY_PARAMS = new URLSearchParams(CURRENT_URL.search);
    const SELECTED_OPTION = QUERY_PARAMS.get(queryName);

    OPTIONS.forEach(option => {
        const OPTION_VALUE = getQueryValue(option, queryName);
        QUERY_PARAMS.delete(PAGE_NUMBER_QUERY_NAME);
        QUERY_PARAMS.set(queryName, OPTION_VALUE);
        option.setAttribute('href', `${CURRENT_URL.pathname}?${QUERY_PARAMS.toString()}`);

        if (!isPagination) {
            if (OPTION_VALUE == SELECTED_OPTION) {
                option.classList.add("orange");
            } else {
                option.classList.remove("orange");
            }
        }
    });
    if (!isPagination) {

        document.querySelector('input[name=' + queryName + ']').value = SELECTED_OPTION;
    }
}

function getQueryValue(a, queryName) {
    const CURRENT_LINK_PATH = `${window.location.origin}${decodeURI(a.getAttribute('href'))}`;
    const CURRENT_URL = new URL(CURRENT_LINK_PATH);
    const QUERY_PARAMS = new URLSearchParams(CURRENT_URL.search);
    return QUERY_PARAMS.get(queryName);
}