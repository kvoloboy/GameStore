const genresContainer = document.querySelector('#genres');
genresContainer.addEventListener('change', treeContainerChangeHandler);

function treeContainerChangeHandler(e) {
    const target = e.target;
    target.checked ? handleCheck(target) : handleUncheck(target);
}

function handleCheck(checkbox) {
    const dataParentId = checkbox.getAttribute('data-parent-id');
    if (dataParentId !== null) {
        const parentId = `#${dataParentId}`;
        const parent = document.querySelector(parentId);
        parent.checked = true;
        handleCheck(parent);
    }
}

function handleUncheck(checkbox) {
    const id = checkbox.getAttribute("id");
    const children = document.querySelectorAll(`input[data-parent-id=${id}]`);
    for (const child of children) {
        child.checked = false;
        handleUncheck(child);
    }
}