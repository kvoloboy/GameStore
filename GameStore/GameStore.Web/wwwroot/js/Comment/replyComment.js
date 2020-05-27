document.addEventListener('click', openReplyForm);

function openReplyForm(event) {
    const replyButtonClass = "reply-btn";
    const quoteButtonClass = "quote-btn";
    const clickedElement = event.target;
    const isReplyBtn = clickedElement.classList.contains(replyButtonClass);
    const isQuoteBtn = clickedElement.classList.contains(quoteButtonClass);

    if (!isReplyBtn && !isQuoteBtn) {
        return;
    }

    event.preventDefault();
    const parentId = clickedElement.getAttribute('data-parent-id');
    const isQuote = clickedElement.getAttribute('data-quote');
    let urlParam = `?parentId=${parentId}`;

    if (isQuote) {
        urlParam += `&isQuote=${isQuote}`;
    }

    const targetUrl = `${window.location.pathname}/reply`;
    const request = new XMLHttpRequest();

    request.open("GET", targetUrl + urlParam, true);
    request.send();
    request.onreadystatechange = function () {

        if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
            const container = clickedElement.parentNode.parentNode.parentNode;
            removeExistingForm();
            createReplyContainer(container, this.responseText)
        }
    }
}

function removeExistingForm() {
    const formContainer = document.querySelector('#form-container');

    if (formContainer) {
        formContainer.remove();
    }
}

function createReplyContainer(container, html) {
    const formContainer = document.createElement('div');
    formContainer.setAttribute('id', 'form-container');
    formContainer.innerHTML = html;
    container.appendChild(formContainer);
}