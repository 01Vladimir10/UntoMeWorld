const Components = {
    enableLogging: true,
    log(componentName, log, params) {
        if (this.enableLogging) {
            console.log(`${componentName}:${log} - ${JSON.stringify(params)}`);
        }
    }
    
};
function debounce(func, timeout = 300){
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

function createNode(tag, attributes = {}, ...children) {
    const element = document.createElement(tag)
    for (const attribute in attributes) {
        if (attributes.hasOwnProperty(attribute)) {
            element.setAttribute(attribute, attributes[attribute])
        }
    }
    const fragment = document.createDocumentFragment()
    children.forEach((child) => {
        if (typeof child === 'string') {
            child = document.createTextNode(child)
        }
        fragment.appendChild(child)
    })
    element.appendChild(fragment)
    return element
}

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

const Toast = {
    _init: function () {
        let container = document.querySelector('#toastsContainer');
        if (container) {
            this.container = container;
            return;
        }
        container = createNode('div', { id: 'toastsContainer', class: 'toasts-container' });
        const parent = document.querySelector('.page') ?? document.body;
        parent.appendChild(container);
        this.container = container;
    },
    create: function (text, iconClass = null, cssClass = '', duration = 200) {
        this._init();
        Components.log('Toasts',`showing toast`, {text, iconClass, cssClass, duration})

        const toast = createNode(
            'div', { class: `toast ${cssClass || ''}` },
            createNode('div', { class: 'toast-content' }, text)
        )
        if (iconClass) {
            toast.insertBefore(createNode('i', { class: `toast-icon ${iconClass}` }), toast.firstChild)
        }
        
        this.container.appendChild(toast);
        setTimeout(() =>toast.classList.add('show'), 1);
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => this.container.removeChild(toast), 500);
        }, duration);
    },
    createDebounce: debounce((text, iconClass = null, cssClass = '', duration = 200) => {
        Toast.create(text, iconClass, cssClass, duration);
    }, 500),
}

function createToast (text, cssClass = '', duration = 200, iconClass = null) {
    Toast.create(text, cssClass, duration, iconClass);
}