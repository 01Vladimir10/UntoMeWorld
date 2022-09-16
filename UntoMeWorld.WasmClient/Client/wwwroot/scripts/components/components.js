const Components = {
    enableLogging: true,
    log(componentName, log, params) {
        if (this.enableLogging) {
            console.log(`${componentName}:${log} - ${JSON.stringify(params)}`);
        }
    }

};

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => {
            func.apply(this, args);
        }, timeout);
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
        container = createNode('div', {id: 'toastsContainer', class: 'toasts-container'});
        const parent = document.querySelector('.page') ?? document.body;
        parent.appendChild(container);
        this.container = container;
    },
    iconsByType: {
        'ERROR': `ph-warning-circle-bold`,
        'INFO': `ph-info-bold`,
        'WARNING': `ph-warning-bold`,
        'SUCCESS': `ph-check-circle-bold`,
    },
    /***
     * Displays a toast
     * @param title {string}
     * @param subtitle {string}
     * @param {('error'|'success'|'warning'|'info')} type
     * @param additionalCssClass {string|Array<string>}
     * @param duration {number}
     */
    create: function (type, title, subtitle, additionalCssClass = '', duration = 2000) {
        console.log('creating toast ', {type, title, subtitle, additionalCssClass})
        this._init();

        const icon = this.iconsByType[type.toUpperCase()]
        const cssClasses = (Array.isArray(additionalCssClass) ? additionalCssClass : [additionalCssClass]).join(' ')
        const closeButton = createNode('button', {class: 'ph-x-bold toast-close-button'})
        const toast = createNode(
            'div', {class: `toast ${type} ${cssClasses || ''}`},
            createNode('i', {class: `toast-icon ${icon}`}),
            createNode('div', {class: 'd-inline-flex flex-column align-items-stretch'},
                createNode('span', {class: 'toast-title'}, title),
                createNode('span', {class: 'toast-subtitle'}, subtitle),
            ),
            closeButton
        )
        const closeToast = () => {
            if (toast.hasAttribute('data-closed'))
                return;
            toast.setAttribute('data-closed', '');
            toast.classList.remove('show')
            setTimeout(() => this.container.removeChild(toast), 500);
        }
        closeButton.addEventListener('click', () => closeToast())

        this.container.appendChild(toast);

        setTimeout(() => toast.classList.add('show'), 1);
        setTimeout(() => closeToast(), duration);
    }
}

function createToast(type, title, subtitle, cssClass, duration) {
    Toast.create(type, title, subtitle, cssClass, duration);
}

function renderIframe(id, url, cssClass = '') {
    Components.log('RenderIframe', 'Rendering iframe ', {id, url, cssClass})
    const item = new pym.Parent(id, url, {})
    if (cssClass.trim() !=='') {
        item.iframe.classList.add(cssClass)
    }
}