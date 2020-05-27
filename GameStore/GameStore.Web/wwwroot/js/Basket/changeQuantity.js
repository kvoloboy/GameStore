const QUANTITY_CONTROLS = document.querySelectorAll('.details-changer');
QUANTITY_CONTROLS.forEach(control => control.addEventListener('click', changeQuantityHandler));

function changeQuantityHandler(e) {
    e.preventDefault();

    const CLICKED_ELEMENT = e.currentTarget;
    const CLICKED_ELEMENT_CONTAINER = e.currentTarget.parentNode;
    const IS_INCREMENT = CLICKED_ELEMENT_CONTAINER.classList.contains('increment-button');
    const REQUEST_PATH = CLICKED_ELEMENT_CONTAINER.action;

    fetch(REQUEST_PATH, {
        method: "POST"
    })
        .then(function (response) {
            if (!response.ok) {
                return;
            }

            return response.text()
                .then(function (text) {
                    const QUANTITY = updateDetailsQuantity(CLICKED_ELEMENT, IS_INCREMENT);
                    document.querySelector("#total-summary").innerHTML = text;

                    updateForms(CLICKED_ELEMENT_CONTAINER, QUANTITY);
                })
        });
}

function updateDetailsQuantity(clickedElement, isIncrement) {
    const DETAILS_QUANTITY_CONTAINER = clickedElement.parentNode.parentNode.querySelector('.quantity');
    const DETAILS_QUANTITY_VALUE = isIncrement
        ? +DETAILS_QUANTITY_CONTAINER.textContent + 1
        : +DETAILS_QUANTITY_CONTAINER.textContent - 1;

    const IS_DELETE = DETAILS_QUANTITY_VALUE == 0 || clickedElement.classList.contains('button-remove');

    if (IS_DELETE) {
        const CONTAINER_ID = clickedElement.dataset.containerId;
        document.getElementById(`${CONTAINER_ID}`).remove();
        updateTotalQuantity(isIncrement, +DETAILS_QUANTITY_CONTAINER.textContent);

        return;
    }

    DETAILS_QUANTITY_CONTAINER.textContent = DETAILS_QUANTITY_VALUE;
    updateTotalQuantity(isIncrement);

    return DETAILS_QUANTITY_VALUE;
}

function updateTotalQuantity(isIncrement, value) {
    const TOTAL_QUANTITY_CONTAINERS = document.querySelectorAll(".total-items");
    const TOTAL_QUANTITY = +TOTAL_QUANTITY_CONTAINERS[0].textContent;

    if (isIncrement) {
        TOTAL_QUANTITY_CONTAINERS.forEach(quantity => quantity.textContent = TOTAL_QUANTITY + 1);
    } else {
        value = value || 1;
        TOTAL_QUANTITY_CONTAINERS.forEach(quantity => quantity.textContent = TOTAL_QUANTITY - value);
    }
}

function updateForms(clickedElementContainer, quantity) {
    const INCREMENT_QUANTITY_QUERY_VALUE = quantity + 1;
    const DECREMENT_QUANTITY_QUERY_VALUE = quantity - 1;

    let incrementForm;
    let decrementForm;

    for (const node of clickedElementContainer.parentNode.children) {
        if (node.classList.contains('decrement-button')) {
            decrementForm = node;
        }

        if (node.classList.contains("increment-button")) {
            incrementForm = node;
        }
    }
    updateFormAction(incrementForm, INCREMENT_QUANTITY_QUERY_VALUE);
    updateFormAction(decrementForm, DECREMENT_QUANTITY_QUERY_VALUE);
}

function updateFormAction(form, quantity) {
    const QUANTITY_PARAM_NAME = "quantity";
    let url = new URL(form.action);
    const PARAMS = new URLSearchParams(url.search);
    PARAMS.set(QUANTITY_PARAM_NAME, quantity);
    form.action = `${url.pathname}?${PARAMS.toString()}`;
}