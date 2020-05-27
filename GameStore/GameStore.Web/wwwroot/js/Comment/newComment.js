document.addEventListener('click', openCommentForm);

function openCommentForm(event) {
    const targetId = "open-form";
    const clickedElement = event.target;
    const formContainerWrapper = document.querySelector('#new-comment-container');

    if (clickedElement.getAttribute('id') !== targetId) {
        return;
    }

    event.preventDefault();

    if (formContainerWrapper.childElementCount > 0) {
        return;
    }

    const currentUrl = `${window.location.pathname}`;
    const targetUrl = `${currentUrl}/comment`;
    const request = new XMLHttpRequest();

    request.open("GET", targetUrl, true);
    request.send();
    request.onreadystatechange = function () {

        if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
            const formContainer = createFormContainer();
            removeReplyForm();
            formContainer.innerHTML = this.responseText;
            formContainerWrapper.appendChild(formContainer);
        }
    }
}

function removeReplyForm() {
    const replyContainer = document.querySelector('#form-container');

    if (replyContainer) {
        replyContainer.remove();
    }
}

function createFormContainer() {
    const container = document.createElement('div');
    container.setAttribute('id', 'form-container')

    return container;
}