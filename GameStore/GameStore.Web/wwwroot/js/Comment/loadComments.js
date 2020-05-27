const link = document.querySelector("#open-comments");
let commentsContainer = document.querySelector("#comments-container");
link.addEventListener('click', openCommentsHandler);

function openCommentsHandler(e) {
    e.preventDefault();

    if (commentsContainer) {
        hideComments();
    } else {
        showComments();
    }
}

function showComments() {
    const request = new XMLHttpRequest();
    const gameKey = link.getAttribute("data-game-key");
    const url = `/games/${gameKey}/comments`;

    request.open("GET", url, true);
    request.onreadystatechange = function () {
        
        if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
            const container = link.parentNode;
            commentsContainer = createContainer('comments-container', this.responseText);
            container.appendChild(commentsContainer);
            link.textContent = "Hide comments";
        }
        
    };
    request.send();
}

function hideComments() {
    link.textContent = "Open comments";
    commentsContainer.remove();
    commentsContainer = null;
}

function createContainer(id, html) {
    const container = document.createElement('div');
    container.setAttribute('id', id);
    container.innerHTML = html;
    
    return container;
}