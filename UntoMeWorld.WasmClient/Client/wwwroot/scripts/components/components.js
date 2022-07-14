initExpandableItems = (itemId) => {
    const item = document.getElementById(itemId);
    if (item.classList.contains('initialized'))
        return;
    
    const header = item.querySelector(".expandable-list-item__header");
    header.addEventListener("click", () => item.classList.toggle("expanded"));
    const contentHeight = item.querySelector('.expandable-list-item__content')?.getBoundingClientRect()?.height ?? 0;
    item.style.setProperty('--max-content-height', `${contentHeight}px`);
    item.classList.add('initialized');
}
