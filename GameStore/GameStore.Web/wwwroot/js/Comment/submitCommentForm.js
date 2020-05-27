document.addEventListener('click', createCommentHandler);

function createCommentHandler(e) {
    const clickedElement = e.target;
    const commentButtonId = "new-comment";

    if (clickedElement.getAttribute("id") !== commentButtonId) {
        return;
    }

    e.preventDefault();
    const formContainer = document.querySelector('#form-container');
    const form = document.querySelector('#create-comment');
    const formData = new FormData(form);
    const url = form.getAttribute("action");
    const request = new XMLHttpRequest();

    request.open("POST", url, true);
    request.send(formData);
    request.onreadystatechange = function () {

        if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
            const response = this.responseText;

            if (response.includes("id=\"create-comment\"")) {
                formContainer.innerHTML = response;
            } else {
                formContainer.remove();
                insertComment(response, clickedElement);
                updateCommentsCount()
            }
        }
    }
}

function insertComment(comment, button) {
    const block = document.createElement('div');
    const parentId = button.dataset.parentHtmlId;
    let containerToInsert;
    block.innerHTML = comment;

    if (parentId) {
        block.classList.add("reply");
        containerToInsert = document.getElementById(parentId);
        containerToInsert.append(block);

        return;
    }

    block.classList.add('comment-wrapper');
    containerToInsert = document.querySelector(".comments");
    containerToInsert.prepend(block);
}

function updateCommentsCount() {
    document.querySelectorAll('.comments-count').forEach(count => {
        count.textContent = ++count.textContent;
    })
}