/*!
* sweetalert2 v11.17.2
* Released under the MIT License.
*/
function _assertClassBrand(e, t, n) {
  if ("function" == typeof e ? e === t : e.has(t)) return arguments.length < 3 ? t : n;
  throw new TypeError("Private element is not present on this object");
}
function _checkPrivateRedeclaration(e, t) {
  if (t.has(e)) throw new TypeError("Cannot initialize the same private elements twice on an object");
}
function _classPrivateFieldGet2(s, a) {
  return s.get(_assertClassBrand(s, a));
}
function _classPrivateFieldInitSpec(e, t, a) {
  _checkPrivateRedeclaration(e, t), t.set(e, a);
}
function _classPrivateFieldSet2(s, a, r) {
  return s.set(_assertClassBrand(s, a), r), r;
}

const RESTORE_FOCUS_TIMEOUT = 100;

/** @type {GlobalState} */
const globalState = {};
const focusPreviousActiveElement = () => {
  if (globalState.previousActiveElement instanceof HTMLElement) {
    globalState.previousActiveElement.focus();
    globalState.previousActiveElement = null;
  } else if (document.body) {
    document.body.focus();
  }
};

/**
 * Restore previous active (focused) element
 *
 * @param {boolean} returnFocus
 * @returns {Promise<void>}
 */
const restoreActiveElement = returnFocus => {
  return new Promise(resolve => {
    if (!returnFocus) {
      return resolve();
    }
    const x = window.scrollX;
    const y = window.scrollY;
    globalState.restoreFocusTimeout = setTimeout(() => {
      focusPreviousActiveElement();
      resolve();
    }, RESTORE_FOCUS_TIMEOUT); // issues/900

    window.scrollTo(x, y);
  });
};

const swalPrefix = 'swal2-';

/**
 * @typedef {Record<SwalClass, string>} SwalClasses
 */

/**
 * @typedef {'success' | 'warning' | 'info' | 'question' | 'error'} SwalIcon
 * @typedef {Record<SwalIcon, string>} SwalIcons
 */

/** @type {SwalClass[]} */
const classNames = ['container', 'shown', 'height-auto', 'iosfix', 'popup', 'modal', 'no-backdrop', 'no-transition', 'toast', 'toast-shown', 'show', 'hide', 'close', 'title', 'html-container', 'actions', 'confirm', 'deny', 'cancel', 'default-outline', 'footer', 'icon', 'icon-content', 'image', 'input', 'file', 'range', 'select', 'radio', 'checkbox', 'label', 'textarea', 'inputerror', 'input-label', 'validation-message', 'progress-steps', 'active-progress-step', 'progress-step', 'progress-step-line', 'loader', 'loading', 'styled', 'top', 'top-start', 'top-end', 'top-left', 'top-right', 'center', 'center-start', 'center-end', 'center-left', 'center-right', 'bottom', 'bottom-start', 'bottom-end', 'bottom-left', 'bottom-right', 'grow-row', 'grow-column', 'grow-fullscreen', 'rtl', 'timer-progress-bar', 'timer-progress-bar-container', 'scrollbar-measure', 'icon-success', 'icon-warning', 'icon-info', 'icon-question', 'icon-error', 'draggable', 'dragging'];
const swalClasses = classNames.reduce((acc, className) => {
  acc[className] = swalPrefix + className;
  return acc;
}, /** @type {SwalClasses} */{});

/** @type {SwalIcon[]} */
const icons = ['success', 'warning', 'info', 'question', 'error'];
const iconTypes = icons.reduce((acc, icon) => {
  acc[icon] = swalPrefix + icon;
  return acc;
}, /** @type {SwalIcons} */{});

const consolePrefix = 'SweetAlert2:';

/**
 * Capitalize the first letter of a string
 *
 * @param {string} str
 * @returns {string}
 */
const capitalizeFirstLetter = str => str.charAt(0).toUpperCase() + str.slice(1);

/**
 * Standardize console warnings
 *
 * @param {string | string[]} message
 */
const warn = message => {
  console.warn(`${consolePrefix} ${typeof message === 'object' ? message.join(' ') : message}`);
};

/**
 * Standardize console errors
 *
 * @param {string} message
 */
const error = message => {
  console.error(`${consolePrefix} ${message}`);
};

/**
 * Private global state for `warnOnce`
 *
 * @type {string[]}
 * @private
 */
const previousWarnOnceMessages = [];

/**
 * Show a console warning, but only if it hasn't already been shown
 *
 * @param {string} message
 */
const warnOnce = message => {
  if (!previousWarnOnceMessages.includes(message)) {
    previousWarnOnceMessages.push(message);
    warn(message);
  }
};

/**
 * Show a one-time console warning about deprecated params/methods
 *
 * @param {string} deprecatedParam
 * @param {string?} useInstead
 */
const warnAboutDeprecation = function (deprecatedParam) {
  let useInstead = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : null;
  warnOnce(`"${deprecatedParam}" is deprecated and will be removed in the next major release.${useInstead ? ` Use "${useInstead}" instead.` : ''}`);
};

/**
 * If `arg` is a function, call it (with no arguments or context) and return the result.
 * Otherwise, just pass the value through
 *
 * @param {Function | any} arg
 * @returns {any}
 */
const callIfFunction = arg => typeof arg === 'function' ? arg() : arg;

/**
 * @param {any} arg
 * @returns {boolean}
 */
const hasToPromiseFn = arg => arg && typeof arg.toPromise === 'function';

/**
 * @param {any} arg
 * @returns {Promise<any>}
 */
const asPromise = arg => hasToPromiseFn(arg) ? arg.toPromise() : Promise.resolve(arg);

/**
 * @param {any} arg
 * @returns {boolean}
 */
const isPromise = arg => arg && Promise.resolve(arg) === arg;

/**
 * Gets the popup container which contains the backdrop and the popup itself.
 *
 * @returns {HTMLElement | null}
 */
const getContainer = () => document.body.querySelector(`.${swalClasses.container}`);

/**
 * @param {string} selectorString
 * @returns {HTMLElement | null}
 */
const elementBySelector = selectorString => {
  const container = getContainer();
  return container ? container.querySelector(selectorString) : null;
};

/**
 * @param {string} className
 * @returns {HTMLElement | null}
 */
const elementByClass = className => {
  return elementBySelector(`.${className}`);
};

/**
 * @returns {HTMLElement | null}
 */
const getPopup = () => elementByClass(swalClasses.popup);

/**
 * @returns {HTMLElement | null}
 */
const getIcon = () => elementByClass(swalClasses.icon);

/**
 * @returns {HTMLElement | null}
 */
const getIconContent = () => elementByClass(swalClasses['icon-content']);

/**
 * @returns {HTMLElement | null}
 */
const getTitle = () => elementByClass(swalClasses.title);

/**
 * @returns {HTMLElement | null}
 */
const getHtmlContainer = () => elementByClass(swalClasses['html-container']);

/**
 * @returns {HTMLElement | null}
 */
const getImage = () => elementByClass(swalClasses.image);

/**
 * @returns {HTMLElement | null}
 */
const getProgressSteps = () => elementByClass(swalClasses['progress-steps']);

/**
 * @returns {HTMLElement | null}
 */
const getValidationMessage = () => elementByClass(swalClasses['validation-message']);

/**
 * @returns {HTMLButtonElement | null}
 */
const getConfirmButton = () => (/** @type {HTMLButtonElement} */elementBySelector(`.${swalClasses.actions} .${swalClasses.confirm}`));

/**
 * @returns {HTMLButtonElement | null}
 */
const getCancelButton = () => (/** @type {HTMLButtonElement} */elementBySelector(`.${swalClasses.actions} .${swalClasses.cancel}`));

/**
 * @returns {HTMLButtonElement | null}
 */
const getDenyButton = () => (/** @type {HTMLButtonElement} */elementBySelector(`.${swalClasses.actions} .${swalClasses.deny}`));

/**
 * @returns {HTMLElement | null}
 */
const getInputLabel = () => elementByClass(swalClasses['input-label']);

/**
 * @returns {HTMLElement | null}
 */
const getLoader = () => elementBySelector(`.${swalClasses.loader}`);

/**
 * @returns {HTMLElement | null}
 */
const getActions = () => elementByClass(swalClasses.actions);

/**
 * @returns {HTMLElement | null}
 */
const getFooter = () => elementByClass(swalClasses.footer);

/**
 * @returns {HTMLElement | null}
 */
const getTimerProgressBar = () => elementByClass(swalClasses['timer-progress-bar']);

/**
 * @returns {HTMLElement | null}
 */
const getCloseButton = () => elementByClass(swalClasses.close);

// https://github.com/jkup/focusable/blob/master/index.js
const focusable = `
  a[href],
  area[href],
  input:not([disabled]),
  select:not([disabled]),
  textarea:not([disabled]),
  button:not([disabled]),
  iframe,
  object,
  embed,
  [tabindex="0"],
  [contenteditable],
  audio[controls],
  video[controls],
  summary
`;
/**
 * @returns {HTMLElement[]}
 */
const getFocusableElements = () => {
  const popup = getPopup();
  if (!popup) {
    return [];
  }
  /** @type {NodeListOf<HTMLElement>} */
  const focusableElementsWithTabindex = popup.querySelectorAll('[tabindex]:not([tabindex="-1"]):not([tabindex="0"])');
  const focusableElementsWithTabindexSorted = Array.from(focusableElementsWithTabindex)
  // sort according to tabindex
  .sort((a, b) => {
    const tabindexA = parseInt(a.getAttribute('tabindex') || '0');
    const tabindexB = parseInt(b.getAttribute('tabindex') || '0');
    if (tabindexA > tabindexB) {
      return 1;
    } else if (tabindexA < tabindexB) {
      return -1;
    }
    return 0;
  });

  /** @type {NodeListOf<HTMLElement>} */
  const otherFocusableElements = popup.querySelectorAll(focusable);
  const otherFocusableElementsFiltered = Array.from(otherFocusableElements).filter(el => el.getAttribute('tabindex') !== '-1');
  return [...new Set(focusableElementsWithTabindexSorted.concat(otherFocusableElementsFiltered))].filter(el => isVisible$1(el));
};

/**
 * @returns {boolean}
 */
const isModal = () => {
  return hasClass(document.body, swalClasses.shown) && !hasClass(document.body, swalClasses['toast-shown']) && !hasClass(document.body, swalClasses['no-backdrop']);
};

/**
 * @returns {boolean}
 */
const isToast = () => {
  const popup = getPopup();
  if (!popup) {
    return false;
  }
  return hasClass(popup, swalClasses.toast);
};

/**
 * @returns {boolean}
 */
const isLoading = () => {
  const popup = getPopup();
  if (!popup) {
    return false;
  }
  return popup.hasAttribute('data-loading');
};

/**
 * Securely set innerHTML of an element
 * https://github.com/sweetalert2/sweetalert2/issues/1926
 *
 * @param {HTMLElement} elem
 * @param {string} html
 */
const setInnerHtml = (elem, html) => {
  elem.textContent = '';
  if (html) {
    const parser = new DOMParser();
    const parsed = parser.parseFromString(html, `text/html`);
    const head = parsed.querySelector('head');
    if (head) {
      Array.from(head.childNodes).forEach(child => {
        elem.appendChild(child);
      });
    }
    const body = parsed.querySelector('body');
    if (body) {
      Array.from(body.childNodes).forEach(child => {
        if (child instanceof HTMLVideoElement || child instanceof HTMLAudioElement) {
          elem.appendChild(child.cloneNode(true)); // https://github.com/sweetalert2/sweetalert2/issues/2507
        } else {
          elem.appendChild(child);
        }
      });
    }
  }
};

/**
 * @param {HTMLElement} elem
 * @param {string} className
 * @returns {boolean}
 */
const hasClass = (elem, className) => {
  if (!className) {
    return false;
  }
  const classList = className.split(/\s+/);
  for (let i = 0; i < classList.length; i++) {
    if (!elem.classList.contains(classList[i])) {
      return false;
    }
  }
  return true;
};

/**
 * @param {HTMLElement} elem
 * @param {SweetAlertOptions} params
 */
const removeCustomClasses = (elem, params) => {
  Array.from(elem.classList).forEach(className => {
    if (!Object.values(swalClasses).includes(className) && !Object.values(iconTypes).includes(className) && !Object.values(params.showClass || {}).includes(className)) {
      elem.classList.remove(className);
    }
  });
};

/**
 * @param {HTMLElement} elem
 * @param {SweetAlertOptions} params
 * @param {string} className
 */
const applyCustomClass = (elem, params, className) => {
  removeCustomClasses(elem, params);
  if (!params.customClass) {
    return;
  }
  const customClass = params.customClass[(/** @type {keyof SweetAlertCustomClass} */className)];
  if (!customClass) {
    return;
  }
  if (typeof customClass !== 'string' && !customClass.forEach) {
    warn(`Invalid type of customClass.${className}! Expected string or iterable object, got "${typeof customClass}"`);
    return;
  }
  addClass(elem, customClass);
};

/**
 * @param {HTMLElement} popup
 * @param {import('./renderers/renderInput').InputClass | SweetAlertInput} inputClass
 * @returns {HTMLInputElement | null}
 */
const getInput$1 = (popup, inputClass) => {
  if (!inputClass) {
    return null;
  }
  switch (inputClass) {
    case 'select':
    case 'textarea':
    case 'file':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses[inputClass]}`);
    case 'checkbox':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.checkbox} input`);
    case 'radio':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.radio} input:checked`) || popup.querySelector(`.${swalClasses.popup} > .${swalClasses.radio} input:first-child`);
    case 'range':
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.range} input`);
    default:
      return popup.querySelector(`.${swalClasses.popup} > .${swalClasses.input}`);
  }
};

/**
 * @param {HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement} input
 */
const focusInput = input => {
  input.focus();

  // place cursor at end of text in text input
  if (input.type !== 'file') {
    // http://stackoverflow.com/a/2345915
    const val = input.value;
    input.value = '';
    input.value = val;
  }
};

/**
 * @param {HTMLElement | HTMLElement[] | null} target
 * @param {string | string[] | readonly string[] | undefined} classList
 * @param {boolean} condition
 */
const toggleClass = (target, classList, condition) => {
  if (!target || !classList) {
    return;
  }
  if (typeof classList === 'string') {
    classList = classList.split(/\s+/).filter(Boolean);
  }
  classList.forEach(className => {
    if (Array.isArray(target)) {
      target.forEach(elem => {
        if (condition) {
          elem.classList.add(className);
        } else {
          elem.classList.remove(className);
        }
      });
    } else {
      if (condition) {
        target.classList.add(className);
      } else {
        target.classList.remove(className);
      }
    }
  });
};

/**
 * @param {HTMLElement | HTMLElement[] | null} target
 * @param {string | string[] | readonly string[] | undefined} classList
 */
const addClass = (target, classList) => {
  toggleClass(target, classList, true);
};

/**
 * @param {HTMLElement | HTMLElement[] | null} target
 * @param {string | string[] | readonly string[] | undefined} classList
 */
const removeClass = (target, classList) => {
  toggleClass(target, classList, false);
};

/**
 * Get direct child of an element by class name
 *
 * @param {HTMLElement} elem
 * @param {string} className
 * @returns {HTMLElement | undefined}
 */
const getDirectChildByClass = (elem, className) => {
  const children = Array.from(elem.children);
  for (let i = 0; i < children.length; i++) {
    const child = children[i];
    if (child instanceof HTMLElement && hasClass(child, className)) {
      return child;
    }
  }
};

/**
 * @param {HTMLElement} elem
 * @param {string} property
 * @param {*} value
 */
const applyNumericalStyle = (elem, property, value) => {
  if (value === `${parseInt(value)}`) {
    value = parseInt(value);
  }
  if (value || parseInt(value) === 0) {
    elem.style.setProperty(property, typeof value === 'number' ? `${value}px` : value);
  } else {
    elem.style.removeProperty(property);
  }
};

/**
 * @param {HTMLElement | null} elem
 * @param {string} display
 */
const show = function (elem) {
  let display = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 'flex';
  if (!elem) {
    return;
  }
  elem.style.display = display;
};

/**
 * @param {HTMLElement | null} elem
 */
const hide = elem => {
  if (!elem) {
    return;
  }
  elem.style.display = 'none';
};

/**
 * @param {HTMLElement | null} elem
 * @param {string} display
 */
const showWhenInnerHtmlPresent = function (elem) {
  let display = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 'block';
  if (!elem) {
    return;
  }
  new MutationObserver(() => {
    toggle(elem, elem.innerHTML, display);
  }).observe(elem, {
    childList: true,
    subtree: true
  });
};

/**
 * @param {HTMLElement} parent
 * @param {string} selector
 * @param {string} property
 * @param {string} value
 */
const setStyle = (parent, selector, property, value) => {
  /** @type {HTMLElement | null} */
  const el = parent.querySelector(selector);
  if (el) {
    el.style.setProperty(property, value);
  }
};

/**
 * @param {HTMLElement} elem
 * @param {any} condition
 * @param {string} display
 */
const toggle = function (elem, condition) {
  let display = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : 'flex';
  if (condition) {
    show(elem, display);
  } else {
    hide(elem);
  }
};

/**
 * borrowed from jquery $(elem).is(':visible') implementation
 *
 * @param {HTMLElement | null} elem
 * @returns {boolean}
 */
const isVisible$1 = elem => !!(elem && (elem.offsetWidth || elem.offsetHeight || elem.getClientRects().length));

/**
 * @returns {boolean}
 */
const allButtonsAreHidden = () => !isVisible$1(getConfirmButton()) && !isVisible$1(getDenyButton()) && !isVisible$1(getCancelButton());

/**
 * @param {HTMLElement} elem
 * @returns {boolean}
 */
const isScrollable = elem => !!(elem.scrollHeight > elem.clientHeight);

/**
 * borrowed from https://stackoverflow.com/a/46352119
 *
 * @param {HTMLElement} elem
 * @returns {boolean}
 */
const hasCssAnimation = elem => {
  const style = window.getComputedStyle(elem);
  const animDuration = parseFloat(style.getPropertyValue('animation-duration') || '0');
  const transDuration = parseFloat(style.getPropertyValue('transition-duration') || '0');
  return animDuration > 0 || transDuration > 0;
};

/**
 * @param {number} timer
 * @param {boolean} reset
 */
const animateTimerProgressBar = function (timer) {
  let reset = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : false;
  const timerProgressBar = getTimerProgressBar();
  if (!timerProgressBar) {
    return;
  }
  if (isVisible$1(timerProgressBar)) {
    if (reset) {
      timerProgressBar.style.transition = 'none';
      timerProgressBar.style.width = '100%';
    }
    setTimeout(() => {
      timerProgressBar.style.transition = `width ${timer / 1000}s linear`;
      timerProgressBar.style.width = '0%';
    }, 10);
  }
};
const stopTimerProgressBar = () => {
  const timerProgressBar = getTimerProgressBar();
  if (!timerProgressBar) {
    return;
  }
  const timerProgressBarWidth = parseInt(window.getComputedStyle(timerProgressBar).width);
  timerProgressBar.style.removeProperty('transition');
  timerProgressBar.style.width = '100%';
  const timerProgressBarFullWidth = parseInt(window.getComputedStyle(timerProgressBar).width);
  const timerProgressBarPercent = timerProgressBarWidth / timerProgressBarFullWidth * 100;
  timerProgressBar.style.width = `${timerProgressBarPercent}%`;
};

/**
 * Detect Node env
 *
 * @returns {boolean}
 */
const isNodeEnv = () => typeof window === 'undefined' || typeof document === 'undefined';

const sweetHTML = `
 <div aria-labelledby="${swalClasses.title}" aria-describedby="${swalClasses['html-container']}" class="${swalClasses.popup}" tabindex="-1">
   <button type="button" class="${swalClasses.close}"></button>
   <ul class="${swalClasses['progress-steps']}"></ul>
   <div class="${swalClasses.icon}"></div>
   <img class="${swalClasses.image}" />
   <h2 class="${swalClasses.title}" id="${swalClasses.title}"></h2>
   <div class="${swalClasses['html-container']}" id="${swalClasses['html-container']}"></div>
   <input class="${swalClasses.input}" id="${swalClasses.input}" />
   <input type="file" class="${swalClasses.file}" />
   <div class="${swalClasses.range}">
     <input type="range" />
     <output></output>
   </div>
   <select class="${swalClasses.select}" id="${swalClasses.select}"></select>
   <div class="${swalClasses.radio}"></div>
   <label class="${swalClasses.checkbox}">
     <input type="checkbox" id="${swalClasses.checkbox}" />
     <span class="${swalClasses.label}"></span>
   </label>
   <textarea class="${swalClasses.textarea}" id="${swalClasses.textarea}"></textarea>
   <div class="${swalClasses['validation-message']}" id="${swalClasses['validation-message']}"></div>
   <div class="${swalClasses.actions}">
     <div class="${swalClasses.loader}"></div>
     <button type="button" class="${swalClasses.confirm}"></button>
     <button type="button" class="${swalClasses.deny}"></button>
     <button type="button" class="${swalClasses.cancel}"></button>
   </div>
   <div class="${swalClasses.footer}"></div>
   <div class="${swalClasses['timer-progress-bar-container']}">
     <div class="${swalClasses['timer-progress-bar']}"></div>
   </div>
 </div>
`.replace(/(^|\n)\s*/g, '');

/**
 * @returns {boolean}
 */
const resetOldContainer = () => {
  const oldContainer = getContainer();
  if (!oldContainer) {
    return false;
  }
  oldContainer.remove();
  removeClass([document.documentElement, document.body], [swalClasses['no-backdrop'], swalClasses['toast-shown'], swalClasses['has-column']]);
  return true;
};
const resetValidationMessage$1 = () => {
  globalState.currentInstance.resetValidationMessage();
};
const addInputChangeListeners = () => {
  const popup = getPopup();
  const input = getDirectChildByClass(popup, swalClasses.input);
  const file = getDirectChildByClass(popup, swalClasses.file);
  /** @type {HTMLInputElement} */
  const range = popup.querySelector(`.${swalClasses.range} input`);
  /** @type {HTMLOutputElement} */
  const rangeOutput = popup.querySelector(`.${swalClasses.range} output`);
  const select = getDirectChildByClass(popup, swalClasses.select);
  /** @type {HTMLInputElement} */
  const checkbox = popup.querySelector(`.${swalClasses.checkbox} input`);
  const textarea = getDirectChildByClass(popup, swalClasses.textarea);
  input.oninput = resetValidationMessage$1;
  file.onchange = resetValidationMessage$1;
  select.onchange = resetValidationMessage$1;
  checkbox.onchange = resetValidationMessage$1;
  textarea.oninput = resetValidationMessage$1;
  range.oninput = () => {
    resetValidationMessage$1();
    rangeOutput.value = range.value;
  };
  range.onchange = () => {
    resetValidationMessage$1();
    rangeOutput.value = range.value;
  };
};

/**
 * @param {string | HTMLElement} target
 * @returns {HTMLElement}
 */
const getTarget = target => typeof target === 'string' ? document.querySelector(target) : target;

/**
 * @param {SweetAlertOptions} params
 */
const setupAccessibility = params => {
  const popup = getPopup();
  popup.setAttribute('role', params.toast ? 'alert' : 'dialog');
  popup.setAttribute('aria-live', params.toast ? 'polite' : 'assertive');
  if (!params.toast) {
    popup.setAttribute('aria-modal', 'true');
  }
};

/**
 * @param {HTMLElement} targetElement
 */
const setupRTL = targetElement => {
  if (window.getComputedStyle(targetElement).direction === 'rtl') {
    addClass(getContainer(), swalClasses.rtl);
  }
};

/**
 * Add modal + backdrop + no-war message for Russians to DOM
 *
 * @param {SweetAlertOptions} params
 */
const init = params => {
  // Clean up the old popup container if it exists
  const oldContainerExisted = resetOldContainer();
  if (isNodeEnv()) {
    error('SweetAlert2 requires document to initialize');
    return;
  }
  const container = document.createElement('div');
  container.className = swalClasses.container;
  if (oldContainerExisted) {
    addClass(container, swalClasses['no-transition']);
  }
  setInnerHtml(container, sweetHTML);
  container.dataset['swal2Theme'] = params.theme;
  const targetElement = getTarget(params.target);
  targetElement.appendChild(container);
  setupAccessibility(params);
  setupRTL(targetElement);
  addInputChangeListeners();
};

/**
 * @param {HTMLElement | object | string} param
 * @param {HTMLElement} target
 */
const parseHtmlToContainer = (param, target) => {
  // DOM element
  if (param instanceof HTMLElement) {
    target.appendChild(param);
  }

  // Object
  else if (typeof param === 'object') {
    handleObject(param, target);
  }

  // Plain string
  else if (param) {
    setInnerHtml(target, param);
  }
};

/**
 * @param {any} param
 * @param {HTMLElement} target
 */
const handleObject = (param, target) => {
  // JQuery element(s)
  if (param.jquery) {
    handleJqueryElem(target, param);
  }

  // For other objects use their string representation
  else {
    setInnerHtml(target, param.toString());
  }
};

/**
 * @param {HTMLElement} target
 * @param {any} elem
 */
const handleJqueryElem = (target, elem) => {
  target.textContent = '';
  if (0 in elem) {
    for (let i = 0; i in elem; i++) {
      target.appendChild(elem[i].cloneNode(true));
    }
  } else {
    target.appendChild(elem.cloneNode(true));
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderActions = (instance, params) => {
  const actions = getActions();
  const loader = getLoader();
  if (!actions || !loader) {
    return;
  }

  // Actions (buttons) wrapper
  if (!params.showConfirmButton && !params.showDenyButton && !params.showCancelButton) {
    hide(actions);
  } else {
    show(actions);
  }

  // Custom class
  applyCustomClass(actions, params, 'actions');

  // Render all the buttons
  renderButtons(actions, loader, params);

  // Loader
  setInnerHtml(loader, params.loaderHtml || '');
  applyCustomClass(loader, params, 'loader');
};

/**
 * @param {HTMLElement} actions
 * @param {HTMLElement} loader
 * @param {SweetAlertOptions} params
 */
function renderButtons(actions, loader, params) {
  const confirmButton = getConfirmButton();
  const denyButton = getDenyButton();
  const cancelButton = getCancelButton();
  if (!confirmButton || !denyButton || !cancelButton) {
    return;
  }

  // Render buttons
  renderButton(confirmButton, 'confirm', params);
  renderButton(denyButton, 'deny', params);
  renderButton(cancelButton, 'cancel', params);
  handleButtonsStyling(confirmButton, denyButton, cancelButton, params);
  if (params.reverseButtons) {
    if (params.toast) {
      actions.insertBefore(cancelButton, confirmButton);
      actions.insertBefore(denyButton, confirmButton);
    } else {
      actions.insertBefore(cancelButton, loader);
      actions.insertBefore(denyButton, loader);
      actions.insertBefore(confirmButton, loader);
    }
  }
}

/**
 * @param {HTMLElement} confirmButton
 * @param {HTMLElement} denyButton
 * @param {HTMLElement} cancelButton
 * @param {SweetAlertOptions} params
 */
function handleButtonsStyling(confirmButton, denyButton, cancelButton, params) {
  if (!params.buttonsStyling) {
    removeClass([confirmButton, denyButton, cancelButton], swalClasses.styled);
    return;
  }
  addClass([confirmButton, denyButton, cancelButton], swalClasses.styled);

  // Buttons background colors
  if (params.confirmButtonColor) {
    confirmButton.style.backgroundColor = params.confirmButtonColor;
    addClass(confirmButton, swalClasses['default-outline']);
  }
  if (params.denyButtonColor) {
    denyButton.style.backgroundColor = params.denyButtonColor;
    addClass(denyButton, swalClasses['default-outline']);
  }
  if (params.cancelButtonColor) {
    cancelButton.style.backgroundColor = params.cancelButtonColor;
    addClass(cancelButton, swalClasses['default-outline']);
  }
}

/**
 * @param {HTMLElement} button
 * @param {'confirm' | 'deny' | 'cancel'} buttonType
 * @param {SweetAlertOptions} params
 */
function renderButton(button, buttonType, params) {
  const buttonName = /** @type {'Confirm' | 'Deny' | 'Cancel'} */capitalizeFirstLetter(buttonType);
  toggle(button, params[`show${buttonName}Button`], 'inline-block');
  setInnerHtml(button, params[`${buttonType}ButtonText`] || ''); // Set caption text
  button.setAttribute('aria-label', params[`${buttonType}ButtonAriaLabel`] || ''); // ARIA label

  // Add buttons custom classes
  button.className = swalClasses[buttonType];
  applyCustomClass(button, params, `${buttonType}Button`);
}

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderCloseButton = (instance, params) => {
  const closeButton = getCloseButton();
  if (!closeButton) {
    return;
  }
  setInnerHtml(closeButton, params.closeButtonHtml || '');

  // Custom class
  applyCustomClass(closeButton, params, 'closeButton');
  toggle(closeButton, params.showCloseButton);
  closeButton.setAttribute('aria-label', params.closeButtonAriaLabel || '');
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderContainer = (instance, params) => {
  const container = getContainer();
  if (!container) {
    return;
  }
  handleBackdropParam(container, params.backdrop);
  handlePositionParam(container, params.position);
  handleGrowParam(container, params.grow);

  // Custom class
  applyCustomClass(container, params, 'container');
};

/**
 * @param {HTMLElement} container
 * @param {SweetAlertOptions['backdrop']} backdrop
 */
function handleBackdropParam(container, backdrop) {
  if (typeof backdrop === 'string') {
    container.style.background = backdrop;
  } else if (!backdrop) {
    addClass([document.documentElement, document.body], swalClasses['no-backdrop']);
  }
}

/**
 * @param {HTMLElement} container
 * @param {SweetAlertOptions['position']} position
 */
function handlePositionParam(container, position) {
  if (!position) {
    return;
  }
  if (position in swalClasses) {
    addClass(container, swalClasses[position]);
  } else {
    warn('The "position" parameter is not valid, defaulting to "center"');
    addClass(container, swalClasses.center);
  }
}

/**
 * @param {HTMLElement} container
 * @param {SweetAlertOptions['grow']} grow
 */
function handleGrowParam(container, grow) {
  if (!grow) {
    return;
  }
  addClass(container, swalClasses[`grow-${grow}`]);
}

/**
 * This module contains `WeakMap`s for each effectively-"private  property" that a `Swal` has.
 * For example, to set the private property "foo" of `this` to "bar", you can `privateProps.foo.set(this, 'bar')`
 * This is the approach that Babel will probably take to implement private methods/fields
 *   https://github.com/tc39/proposal-private-methods
 *   https://github.com/babel/babel/pull/7555
 * Once we have the changes from that PR in Babel, and our core class fits reasonable in *one module*
 *   then we can use that language feature.
 */

var privateProps = {
  innerParams: new WeakMap(),
  domCache: new WeakMap()
};

/// <reference path="../../../../sweetalert2.d.ts"/>


/** @type {InputClass[]} */
const inputClasses = ['input', 'file', 'range', 'select', 'radio', 'checkbox', 'textarea'];

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderInput = (instance, params) => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  const innerParams = privateProps.innerParams.get(instance);
  const rerender = !innerParams || params.input !== innerParams.input;
  inputClasses.forEach(inputClass => {
    const inputContainer = getDirectChildByClass(popup, swalClasses[inputClass]);
    if (!inputContainer) {
      return;
    }

    // set attributes
    setAttributes(inputClass, params.inputAttributes);

    // set class
    inputContainer.className = swalClasses[inputClass];
    if (rerender) {
      hide(inputContainer);
    }
  });
  if (params.input) {
    if (rerender) {
      showInput(params);
    }
    // set custom class
    setCustomClass(params);
  }
};

/**
 * @param {SweetAlertOptions} params
 */
const showInput = params => {
  if (!params.input) {
    return;
  }
  if (!renderInputType[params.input]) {
    error(`Unexpected type of input! Expected ${Object.keys(renderInputType).join(' | ')}, got "${params.input}"`);
    return;
  }
  const inputContainer = getInputContainer(params.input);
  if (!inputContainer) {
    return;
  }
  const input = renderInputType[params.input](inputContainer, params);
  show(inputContainer);

  // input autofocus
  if (params.inputAutoFocus) {
    setTimeout(() => {
      focusInput(input);
    });
  }
};

/**
 * @param {HTMLInputElement} input
 */
const removeAttributes = input => {
  for (let i = 0; i < input.attributes.length; i++) {
    const attrName = input.attributes[i].name;
    if (!['id', 'type', 'value', 'style'].includes(attrName)) {
      input.removeAttribute(attrName);
    }
  }
};

/**
 * @param {InputClass} inputClass
 * @param {SweetAlertOptions['inputAttributes']} inputAttributes
 */
const setAttributes = (inputClass, inputAttributes) => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  const input = getInput$1(popup, inputClass);
  if (!input) {
    return;
  }
  removeAttributes(input);
  for (const attr in inputAttributes) {
    input.setAttribute(attr, inputAttributes[attr]);
  }
};

/**
 * @param {SweetAlertOptions} params
 */
const setCustomClass = params => {
  if (!params.input) {
    return;
  }
  const inputContainer = getInputContainer(params.input);
  if (inputContainer) {
    applyCustomClass(inputContainer, params, 'input');
  }
};

/**
 * @param {HTMLInputElement | HTMLTextAreaElement} input
 * @param {SweetAlertOptions} params
 */
const setInputPlaceholder = (input, params) => {
  if (!input.placeholder && params.inputPlaceholder) {
    input.placeholder = params.inputPlaceholder;
  }
};

/**
 * @param {Input} input
 * @param {Input} prependTo
 * @param {SweetAlertOptions} params
 */
const setInputLabel = (input, prependTo, params) => {
  if (params.inputLabel) {
    const label = document.createElement('label');
    const labelClass = swalClasses['input-label'];
    label.setAttribute('for', input.id);
    label.className = labelClass;
    if (typeof params.customClass === 'object') {
      addClass(label, params.customClass.inputLabel);
    }
    label.innerText = params.inputLabel;
    prependTo.insertAdjacentElement('beforebegin', label);
  }
};

/**
 * @param {SweetAlertInput} inputType
 * @returns {HTMLElement | undefined}
 */
const getInputContainer = inputType => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  return getDirectChildByClass(popup, swalClasses[(/** @type {SwalClass} */inputType)] || swalClasses.input);
};

/**
 * @param {HTMLInputElement | HTMLOutputElement | HTMLTextAreaElement} input
 * @param {SweetAlertOptions['inputValue']} inputValue
 */
const checkAndSetInputValue = (input, inputValue) => {
  if (['string', 'number'].includes(typeof inputValue)) {
    input.value = `${inputValue}`;
  } else if (!isPromise(inputValue)) {
    warn(`Unexpected type of inputValue! Expected "string", "number" or "Promise", got "${typeof inputValue}"`);
  }
};

/** @type {Record<SweetAlertInput, (input: Input | HTMLElement, params: SweetAlertOptions) => Input>} */
const renderInputType = {};

/**
 * @param {HTMLInputElement} input
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.text = renderInputType.email = renderInputType.password = renderInputType.number = renderInputType.tel = renderInputType.url = renderInputType.search = renderInputType.date = renderInputType['datetime-local'] = renderInputType.time = renderInputType.week = renderInputType.month = /** @type {(input: Input | HTMLElement, params: SweetAlertOptions) => Input} */
(input, params) => {
  checkAndSetInputValue(input, params.inputValue);
  setInputLabel(input, input, params);
  setInputPlaceholder(input, params);
  input.type = params.input;
  return input;
};

/**
 * @param {HTMLInputElement} input
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.file = (input, params) => {
  setInputLabel(input, input, params);
  setInputPlaceholder(input, params);
  return input;
};

/**
 * @param {HTMLInputElement} range
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.range = (range, params) => {
  const rangeInput = range.querySelector('input');
  const rangeOutput = range.querySelector('output');
  checkAndSetInputValue(rangeInput, params.inputValue);
  rangeInput.type = params.input;
  checkAndSetInputValue(rangeOutput, params.inputValue);
  setInputLabel(rangeInput, range, params);
  return range;
};

/**
 * @param {HTMLSelectElement} select
 * @param {SweetAlertOptions} params
 * @returns {HTMLSelectElement}
 */
renderInputType.select = (select, params) => {
  select.textContent = '';
  if (params.inputPlaceholder) {
    const placeholder = document.createElement('option');
    setInnerHtml(placeholder, params.inputPlaceholder);
    placeholder.value = '';
    placeholder.disabled = true;
    placeholder.selected = true;
    select.appendChild(placeholder);
  }
  setInputLabel(select, select, params);
  return select;
};

/**
 * @param {HTMLInputElement} radio
 * @returns {HTMLInputElement}
 */
renderInputType.radio = radio => {
  radio.textContent = '';
  return radio;
};

/**
 * @param {HTMLLabelElement} checkboxContainer
 * @param {SweetAlertOptions} params
 * @returns {HTMLInputElement}
 */
renderInputType.checkbox = (checkboxContainer, params) => {
  const checkbox = getInput$1(getPopup(), 'checkbox');
  checkbox.value = '1';
  checkbox.checked = Boolean(params.inputValue);
  const label = checkboxContainer.querySelector('span');
  setInnerHtml(label, params.inputPlaceholder || params.inputLabel);
  return checkbox;
};

/**
 * @param {HTMLTextAreaElement} textarea
 * @param {SweetAlertOptions} params
 * @returns {HTMLTextAreaElement}
 */
renderInputType.textarea = (textarea, params) => {
  checkAndSetInputValue(textarea, params.inputValue);
  setInputPlaceholder(textarea, params);
  setInputLabel(textarea, textarea, params);

  /**
   * @param {HTMLElement} el
   * @returns {number}
   */
  const getMargin = el => parseInt(window.getComputedStyle(el).marginLeft) + parseInt(window.getComputedStyle(el).marginRight);

  // https://github.com/sweetalert2/sweetalert2/issues/2291
  setTimeout(() => {
    // https://github.com/sweetalert2/sweetalert2/issues/1699
    if ('MutationObserver' in window) {
      const initialPopupWidth = parseInt(window.getComputedStyle(getPopup()).width);
      const textareaResizeHandler = () => {
        // check if texarea is still in document (i.e. popup wasn't closed in the meantime)
        if (!document.body.contains(textarea)) {
          return;
        }
        const textareaWidth = textarea.offsetWidth + getMargin(textarea);
        if (textareaWidth > initialPopupWidth) {
          getPopup().style.width = `${textareaWidth}px`;
        } else {
          applyNumericalStyle(getPopup(), 'width', params.width);
        }
      };
      new MutationObserver(textareaResizeHandler).observe(textarea, {
        attributes: true,
        attributeFilter: ['style']
      });
    }
  });
  return textarea;
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderContent = (instance, params) => {
  const htmlContainer = getHtmlContainer();
  if (!htmlContainer) {
    return;
  }
  showWhenInnerHtmlPresent(htmlContainer);
  applyCustomClass(htmlContainer, params, 'htmlContainer');

  // Content as HTML
  if (params.html) {
    parseHtmlToContainer(params.html, htmlContainer);
    show(htmlContainer, 'block');
  }

  // Content as plain text
  else if (params.text) {
    htmlContainer.textContent = params.text;
    show(htmlContainer, 'block');
  }

  // No content
  else {
    hide(htmlContainer);
  }
  renderInput(instance, params);
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderFooter = (instance, params) => {
  const footer = getFooter();
  if (!footer) {
    return;
  }
  showWhenInnerHtmlPresent(footer);
  toggle(footer, params.footer, 'block');
  if (params.footer) {
    parseHtmlToContainer(params.footer, footer);
  }

  // Custom class
  applyCustomClass(footer, params, 'footer');
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderIcon = (instance, params) => {
  const innerParams = privateProps.innerParams.get(instance);
  const icon = getIcon();
  if (!icon) {
    return;
  }

  // if the given icon already rendered, apply the styling without re-rendering the icon
  if (innerParams && params.icon === innerParams.icon) {
    // Custom or default content
    setContent(icon, params);
    applyStyles(icon, params);
    return;
  }
  if (!params.icon && !params.iconHtml) {
    hide(icon);
    return;
  }
  if (params.icon && Object.keys(iconTypes).indexOf(params.icon) === -1) {
    error(`Unknown icon! Expected "success", "error", "warning", "info" or "question", got "${params.icon}"`);
    hide(icon);
    return;
  }
  show(icon);

  // Custom or default content
  setContent(icon, params);
  applyStyles(icon, params);

  // Animate icon
  addClass(icon, params.showClass && params.showClass.icon);

  // Re-adjust the success icon on system theme change
  const colorSchemeQueryList = window.matchMedia('(prefers-color-scheme: dark)');
  colorSchemeQueryList.addEventListener('change', adjustSuccessIconBackgroundColor);
};

/**
 * @param {HTMLElement} icon
 * @param {SweetAlertOptions} params
 */
const applyStyles = (icon, params) => {
  for (const [iconType, iconClassName] of Object.entries(iconTypes)) {
    if (params.icon !== iconType) {
      removeClass(icon, iconClassName);
    }
  }
  addClass(icon, params.icon && iconTypes[params.icon]);

  // Icon color
  setColor(icon, params);

  // Success icon background color
  adjustSuccessIconBackgroundColor();

  // Custom class
  applyCustomClass(icon, params, 'icon');
};

// Adjust success icon background color to match the popup background color
const adjustSuccessIconBackgroundColor = () => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  const popupBackgroundColor = window.getComputedStyle(popup).getPropertyValue('background-color');
  /** @type {NodeListOf<HTMLElement>} */
  const successIconParts = popup.querySelectorAll('[class^=swal2-success-circular-line], .swal2-success-fix');
  for (let i = 0; i < successIconParts.length; i++) {
    successIconParts[i].style.backgroundColor = popupBackgroundColor;
  }
};
const successIconHtml = `
  <div class="swal2-success-circular-line-left"></div>
  <span class="swal2-success-line-tip"></span> <span class="swal2-success-line-long"></span>
  <div class="swal2-success-ring"></div> <div class="swal2-success-fix"></div>
  <div class="swal2-success-circular-line-right"></div>
`;
const errorIconHtml = `
  <span class="swal2-x-mark">
    <span class="swal2-x-mark-line-left"></span>
    <span class="swal2-x-mark-line-right"></span>
  </span>
`;

/**
 * @param {HTMLElement} icon
 * @param {SweetAlertOptions} params
 */
const setContent = (icon, params) => {
  if (!params.icon && !params.iconHtml) {
    return;
  }
  let oldContent = icon.innerHTML;
  let newContent = '';
  if (params.iconHtml) {
    newContent = iconContent(params.iconHtml);
  } else if (params.icon === 'success') {
    newContent = successIconHtml;
    oldContent = oldContent.replace(/ style=".*?"/g, ''); // undo adjustSuccessIconBackgroundColor()
  } else if (params.icon === 'error') {
    newContent = errorIconHtml;
  } else if (params.icon) {
    const defaultIconHtml = {
      question: '?',
      warning: '!',
      info: 'i'
    };
    newContent = iconContent(defaultIconHtml[params.icon]);
  }
  if (oldContent.trim() !== newContent.trim()) {
    setInnerHtml(icon, newContent);
  }
};

/**
 * @param {HTMLElement} icon
 * @param {SweetAlertOptions} params
 */
const setColor = (icon, params) => {
  if (!params.iconColor) {
    return;
  }
  icon.style.color = params.iconColor;
  icon.style.borderColor = params.iconColor;
  for (const sel of ['.swal2-success-line-tip', '.swal2-success-line-long', '.swal2-x-mark-line-left', '.swal2-x-mark-line-right']) {
    setStyle(icon, sel, 'background-color', params.iconColor);
  }
  setStyle(icon, '.swal2-success-ring', 'border-color', params.iconColor);
};

/**
 * @param {string} content
 * @returns {string}
 */
const iconContent = content => `<div class="${swalClasses['icon-content']}">${content}</div>`;

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderImage = (instance, params) => {
  const image = getImage();
  if (!image) {
    return;
  }
  if (!params.imageUrl) {
    hide(image);
    return;
  }
  show(image, '');

  // Src, alt
  image.setAttribute('src', params.imageUrl);
  image.setAttribute('alt', params.imageAlt || '');

  // Width, height
  applyNumericalStyle(image, 'width', params.imageWidth);
  applyNumericalStyle(image, 'height', params.imageHeight);

  // Class
  image.className = swalClasses.image;
  applyCustomClass(image, params, 'image');
};

let dragging = false;
let mousedownX = 0;
let mousedownY = 0;
let initialX = 0;
let initialY = 0;

/**
 * @param {HTMLElement} popup
 */
const addDraggableListeners = popup => {
  popup.addEventListener('mousedown', down);
  document.body.addEventListener('mousemove', move);
  popup.addEventListener('mouseup', up);
  popup.addEventListener('touchstart', down);
  document.body.addEventListener('touchmove', move);
  popup.addEventListener('touchend', up);
};

/**
 * @param {HTMLElement} popup
 */
const removeDraggableListeners = popup => {
  popup.removeEventListener('mousedown', down);
  document.body.removeEventListener('mousemove', move);
  popup.removeEventListener('mouseup', up);
  popup.removeEventListener('touchstart', down);
  document.body.removeEventListener('touchmove', move);
  popup.removeEventListener('touchend', up);
};

/**
 * @param {MouseEvent | TouchEvent} event
 */
const down = event => {
  const popup = getPopup();
  if (event.target === popup || getIcon().contains(/** @type {HTMLElement} */event.target)) {
    dragging = true;
    const clientXY = getClientXY(event);
    mousedownX = clientXY.clientX;
    mousedownY = clientXY.clientY;
    initialX = parseInt(popup.style.insetInlineStart) || 0;
    initialY = parseInt(popup.style.insetBlockStart) || 0;
    addClass(popup, 'swal2-dragging');
  }
};

/**
 * @param {MouseEvent | TouchEvent} event
 */
const move = event => {
  const popup = getPopup();
  if (dragging) {
    let {
      clientX,
      clientY
    } = getClientXY(event);
    popup.style.insetInlineStart = `${initialX + (clientX - mousedownX)}px`;
    popup.style.insetBlockStart = `${initialY + (clientY - mousedownY)}px`;
  }
};
const up = () => {
  const popup = getPopup();
  dragging = false;
  removeClass(popup, 'swal2-dragging');
};

/**
 * @param {MouseEvent | TouchEvent} event
 * @returns {{ clientX: number, clientY: number }}
 */
const getClientXY = event => {
  let clientX = 0,
    clientY = 0;
  if (event.type.startsWith('mouse')) {
    clientX = /** @type {MouseEvent} */event.clientX;
    clientY = /** @type {MouseEvent} */event.clientY;
  } else if (event.type.startsWith('touch')) {
    clientX = /** @type {TouchEvent} */event.touches[0].clientX;
    clientY = /** @type {TouchEvent} */event.touches[0].clientY;
  }
  return {
    clientX,
    clientY
  };
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderPopup = (instance, params) => {
  const container = getContainer();
  const popup = getPopup();
  if (!container || !popup) {
    return;
  }

  // Width
  // https://github.com/sweetalert2/sweetalert2/issues/2170
  if (params.toast) {
    applyNumericalStyle(container, 'width', params.width);
    popup.style.width = '100%';
    const loader = getLoader();
    if (loader) {
      popup.insertBefore(loader, getIcon());
    }
  } else {
    applyNumericalStyle(popup, 'width', params.width);
  }

  // Padding
  applyNumericalStyle(popup, 'padding', params.padding);

  // Color
  if (params.color) {
    popup.style.color = params.color;
  }

  // Background
  if (params.background) {
    popup.style.background = params.background;
  }
  hide(getValidationMessage());

  // Classes
  addClasses$1(popup, params);
  if (params.draggable && !params.toast) {
    addClass(popup, swalClasses.draggable);
    addDraggableListeners(popup);
  } else {
    removeClass(popup, swalClasses.draggable);
    removeDraggableListeners(popup);
  }
};

/**
 * @param {HTMLElement} popup
 * @param {SweetAlertOptions} params
 */
const addClasses$1 = (popup, params) => {
  const showClass = params.showClass || {};
  // Default Class + showClass when updating Swal.update({})
  popup.className = `${swalClasses.popup} ${isVisible$1(popup) ? showClass.popup : ''}`;
  if (params.toast) {
    addClass([document.documentElement, document.body], swalClasses['toast-shown']);
    addClass(popup, swalClasses.toast);
  } else {
    addClass(popup, swalClasses.modal);
  }

  // Custom class
  applyCustomClass(popup, params, 'popup');
  // TODO: remove in the next major
  if (typeof params.customClass === 'string') {
    addClass(popup, params.customClass);
  }

  // Icon class (#1842)
  if (params.icon) {
    addClass(popup, swalClasses[`icon-${params.icon}`]);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderProgressSteps = (instance, params) => {
  const progressStepsContainer = getProgressSteps();
  if (!progressStepsContainer) {
    return;
  }
  const {
    progressSteps,
    currentProgressStep
  } = params;
  if (!progressSteps || progressSteps.length === 0 || currentProgressStep === undefined) {
    hide(progressStepsContainer);
    return;
  }
  show(progressStepsContainer);
  progressStepsContainer.textContent = '';
  if (currentProgressStep >= progressSteps.length) {
    warn('Invalid currentProgressStep parameter, it should be less than progressSteps.length ' + '(currentProgressStep like JS arrays starts from 0)');
  }
  progressSteps.forEach((step, index) => {
    const stepEl = createStepElement(step);
    progressStepsContainer.appendChild(stepEl);
    if (index === currentProgressStep) {
      addClass(stepEl, swalClasses['active-progress-step']);
    }
    if (index !== progressSteps.length - 1) {
      const lineEl = createLineElement(params);
      progressStepsContainer.appendChild(lineEl);
    }
  });
};

/**
 * @param {string} step
 * @returns {HTMLLIElement}
 */
const createStepElement = step => {
  const stepEl = document.createElement('li');
  addClass(stepEl, swalClasses['progress-step']);
  setInnerHtml(stepEl, step);
  return stepEl;
};

/**
 * @param {SweetAlertOptions} params
 * @returns {HTMLLIElement}
 */
const createLineElement = params => {
  const lineEl = document.createElement('li');
  addClass(lineEl, swalClasses['progress-step-line']);
  if (params.progressStepsDistance) {
    applyNumericalStyle(lineEl, 'width', params.progressStepsDistance);
  }
  return lineEl;
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const renderTitle = (instance, params) => {
  const title = getTitle();
  if (!title) {
    return;
  }
  showWhenInnerHtmlPresent(title);
  toggle(title, params.title || params.titleText, 'block');
  if (params.title) {
    parseHtmlToContainer(params.title, title);
  }
  if (params.titleText) {
    title.innerText = params.titleText;
  }

  // Custom class
  applyCustomClass(title, params, 'title');
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const render = (instance, params) => {
  renderPopup(instance, params);
  renderContainer(instance, params);
  renderProgressSteps(instance, params);
  renderIcon(instance, params);
  renderImage(instance, params);
  renderTitle(instance, params);
  renderCloseButton(instance, params);
  renderContent(instance, params);
  renderActions(instance, params);
  renderFooter(instance, params);
  const popup = getPopup();
  if (typeof params.didRender === 'function' && popup) {
    params.didRender(popup);
  }
  globalState.eventEmitter.emit('didRender', popup);
};

/*
 * Global function to determine if SweetAlert2 popup is shown
 */
const isVisible = () => {
  return isVisible$1(getPopup());
};

/*
 * Global function to click 'Confirm' button
 */
const clickConfirm = () => {
  var _dom$getConfirmButton;
  return (_dom$getConfirmButton = getConfirmButton()) === null || _dom$getConfirmButton === void 0 ? void 0 : _dom$getConfirmButton.click();
};

/*
 * Global function to click 'Deny' button
 */
const clickDeny = () => {
  var _dom$getDenyButton;
  return (_dom$getDenyButton = getDenyButton()) === null || _dom$getDenyButton === void 0 ? void 0 : _dom$getDenyButton.click();
};

/*
 * Global function to click 'Cancel' button
 */
const clickCancel = () => {
  var _dom$getCancelButton;
  return (_dom$getCancelButton = getCancelButton()) === null || _dom$getCancelButton === void 0 ? void 0 : _dom$getCancelButton.click();
};

/** @typedef {'cancel' | 'backdrop' | 'close' | 'esc' | 'timer'} DismissReason */

/** @type {Record<DismissReason, DismissReason>} */
const DismissReason = Object.freeze({
  cancel: 'cancel',
  backdrop: 'backdrop',
  close: 'close',
  esc: 'esc',
  timer: 'timer'
});

/**
 * @param {GlobalState} globalState
 */
const removeKeydownHandler = globalState => {
  if (globalState.keydownTarget && globalState.keydownHandlerAdded) {
    globalState.keydownTarget.removeEventListener('keydown', globalState.keydownHandler, {
      capture: globalState.keydownListenerCapture
    });
    globalState.keydownHandlerAdded = false;
  }
};

/**
 * @param {GlobalState} globalState
 * @param {SweetAlertOptions} innerParams
 * @param {*} dismissWith
 */
const addKeydownHandler = (globalState, innerParams, dismissWith) => {
  removeKeydownHandler(globalState);
  if (!innerParams.toast) {
    globalState.keydownHandler = e => keydownHandler(innerParams, e, dismissWith);
    globalState.keydownTarget = innerParams.keydownListenerCapture ? window : getPopup();
    globalState.keydownListenerCapture = innerParams.keydownListenerCapture;
    globalState.keydownTarget.addEventListener('keydown', globalState.keydownHandler, {
      capture: globalState.keydownListenerCapture
    });
    globalState.keydownHandlerAdded = true;
  }
};

/**
 * @param {number} index
 * @param {number} increment
 */
const setFocus = (index, increment) => {
  var _dom$getPopup;
  const focusableElements = getFocusableElements();
  // search for visible elements and select the next possible match
  if (focusableElements.length) {
    index = index + increment;

    // rollover to first item
    if (index === focusableElements.length) {
      index = 0;

      // go to last item
    } else if (index === -1) {
      index = focusableElements.length - 1;
    }
    focusableElements[index].focus();
    return;
  }
  // no visible focusable elements, focus the popup
  (_dom$getPopup = getPopup()) === null || _dom$getPopup === void 0 || _dom$getPopup.focus();
};
const arrowKeysNextButton = ['ArrowRight', 'ArrowDown'];
const arrowKeysPreviousButton = ['ArrowLeft', 'ArrowUp'];

/**
 * @param {SweetAlertOptions} innerParams
 * @param {KeyboardEvent} event
 * @param {Function} dismissWith
 */
const keydownHandler = (innerParams, event, dismissWith) => {
  if (!innerParams) {
    return; // This instance has already been destroyed
  }

  // Ignore keydown during IME composition
  // https://developer.mozilla.org/en-US/docs/Web/API/Document/keydown_event#ignoring_keydown_during_ime_composition
  // https://github.com/sweetalert2/sweetalert2/issues/720
  // https://github.com/sweetalert2/sweetalert2/issues/2406
  if (event.isComposing || event.keyCode === 229) {
    return;
  }
  if (innerParams.stopKeydownPropagation) {
    event.stopPropagation();
  }

  // ENTER
  if (event.key === 'Enter') {
    handleEnter(event, innerParams);
  }

  // TAB
  else if (event.key === 'Tab') {
    handleTab(event);
  }

  // ARROWS - switch focus between buttons
  else if ([...arrowKeysNextButton, ...arrowKeysPreviousButton].includes(event.key)) {
    handleArrows(event.key);
  }

  // ESC
  else if (event.key === 'Escape') {
    handleEsc(event, innerParams, dismissWith);
  }
};

/**
 * @param {KeyboardEvent} event
 * @param {SweetAlertOptions} innerParams
 */
const handleEnter = (event, innerParams) => {
  // https://github.com/sweetalert2/sweetalert2/issues/2386
  if (!callIfFunction(innerParams.allowEnterKey)) {
    return;
  }
  const input = getInput$1(getPopup(), innerParams.input);
  if (event.target && input && event.target instanceof HTMLElement && event.target.outerHTML === input.outerHTML) {
    if (['textarea', 'file'].includes(innerParams.input)) {
      return; // do not submit
    }
    clickConfirm();
    event.preventDefault();
  }
};

/**
 * @param {KeyboardEvent} event
 */
const handleTab = event => {
  const targetElement = event.target;
  const focusableElements = getFocusableElements();
  let btnIndex = -1;
  for (let i = 0; i < focusableElements.length; i++) {
    if (targetElement === focusableElements[i]) {
      btnIndex = i;
      break;
    }
  }

  // Cycle to the next button
  if (!event.shiftKey) {
    setFocus(btnIndex, 1);
  }

  // Cycle to the prev button
  else {
    setFocus(btnIndex, -1);
  }
  event.stopPropagation();
  event.preventDefault();
};

/**
 * @param {string} key
 */
const handleArrows = key => {
  const actions = getActions();
  const confirmButton = getConfirmButton();
  const denyButton = getDenyButton();
  const cancelButton = getCancelButton();
  if (!actions || !confirmButton || !denyButton || !cancelButton) {
    return;
  }
  /** @type HTMLElement[] */
  const buttons = [confirmButton, denyButton, cancelButton];
  if (document.activeElement instanceof HTMLElement && !buttons.includes(document.activeElement)) {
    return;
  }
  const sibling = arrowKeysNextButton.includes(key) ? 'nextElementSibling' : 'previousElementSibling';
  let buttonToFocus = document.activeElement;
  if (!buttonToFocus) {
    return;
  }
  for (let i = 0; i < actions.children.length; i++) {
    buttonToFocus = buttonToFocus[sibling];
    if (!buttonToFocus) {
      return;
    }
    if (buttonToFocus instanceof HTMLButtonElement && isVisible$1(buttonToFocus)) {
      break;
    }
  }
  if (buttonToFocus instanceof HTMLButtonElement) {
    buttonToFocus.focus();
  }
};

/**
 * @param {KeyboardEvent} event
 * @param {SweetAlertOptions} innerParams
 * @param {Function} dismissWith
 */
const handleEsc = (event, innerParams, dismissWith) => {
  if (callIfFunction(innerParams.allowEscapeKey)) {
    event.preventDefault();
    dismissWith(DismissReason.esc);
  }
};

/**
 * This module contains `WeakMap`s for each effectively-"private  property" that a `Swal` has.
 * For example, to set the private property "foo" of `this` to "bar", you can `privateProps.foo.set(this, 'bar')`
 * This is the approach that Babel will probably take to implement private methods/fields
 *   https://github.com/tc39/proposal-private-methods
 *   https://github.com/babel/babel/pull/7555
 * Once we have the changes from that PR in Babel, and our core class fits reasonable in *one module*
 *   then we can use that language feature.
 */

var privateMethods = {
  swalPromiseResolve: new WeakMap(),
  swalPromiseReject: new WeakMap()
};

// From https://developer.paciellogroup.com/blog/2018/06/the-current-state-of-modal-dialog-accessibility/
// Adding aria-hidden="true" to elements outside of the active modal dialog ensures that
// elements not within the active modal dialog will not be surfaced if a user opens a screen
// reader’s list of elements (headings, form controls, landmarks, etc.) in the document.

const setAriaHidden = () => {
  const container = getContainer();
  const bodyChildren = Array.from(document.body.children);
  bodyChildren.forEach(el => {
    if (el.contains(container)) {
      return;
    }
    if (el.hasAttribute('aria-hidden')) {
      el.setAttribute('data-previous-aria-hidden', el.getAttribute('aria-hidden') || '');
    }
    el.setAttribute('aria-hidden', 'true');
  });
};
const unsetAriaHidden = () => {
  const bodyChildren = Array.from(document.body.children);
  bodyChildren.forEach(el => {
    if (el.hasAttribute('data-previous-aria-hidden')) {
      el.setAttribute('aria-hidden', el.getAttribute('data-previous-aria-hidden') || '');
      el.removeAttribute('data-previous-aria-hidden');
    } else {
      el.removeAttribute('aria-hidden');
    }
  });
};

// @ts-ignore
const isSafariOrIOS = typeof window !== 'undefined' && !!window.GestureEvent; // true for Safari desktop + all iOS browsers https://stackoverflow.com/a/70585394

/**
 * Fix iOS scrolling
 * http://stackoverflow.com/q/39626302
 */
const iOSfix = () => {
  if (isSafariOrIOS && !hasClass(document.body, swalClasses.iosfix)) {
    const offset = document.body.scrollTop;
    document.body.style.top = `${offset * -1}px`;
    addClass(document.body, swalClasses.iosfix);
    lockBodyScroll();
  }
};

/**
 * https://github.com/sweetalert2/sweetalert2/issues/1246
 */
const lockBodyScroll = () => {
  const container = getContainer();
  if (!container) {
    return;
  }
  /** @type {boolean} */
  let preventTouchMove;
  /**
   * @param {TouchEvent} event
   */
  container.ontouchstart = event => {
    preventTouchMove = shouldPreventTouchMove(event);
  };
  /**
   * @param {TouchEvent} event
   */
  container.ontouchmove = event => {
    if (preventTouchMove) {
      event.preventDefault();
      event.stopPropagation();
    }
  };
};

/**
 * @param {TouchEvent} event
 * @returns {boolean}
 */
const shouldPreventTouchMove = event => {
  const target = event.target;
  const container = getContainer();
  const htmlContainer = getHtmlContainer();
  if (!container || !htmlContainer) {
    return false;
  }
  if (isStylus(event) || isZoom(event)) {
    return false;
  }
  if (target === container) {
    return true;
  }
  if (!isScrollable(container) && target instanceof HTMLElement && target.tagName !== 'INPUT' &&
  // #1603
  target.tagName !== 'TEXTAREA' &&
  // #2266
  !(isScrollable(htmlContainer) &&
  // #1944
  htmlContainer.contains(target))) {
    return true;
  }
  return false;
};

/**
 * https://github.com/sweetalert2/sweetalert2/issues/1786
 *
 * @param {*} event
 * @returns {boolean}
 */
const isStylus = event => {
  return event.touches && event.touches.length && event.touches[0].touchType === 'stylus';
};

/**
 * https://github.com/sweetalert2/sweetalert2/issues/1891
 *
 * @param {TouchEvent} event
 * @returns {boolean}
 */
const isZoom = event => {
  return event.touches && event.touches.length > 1;
};
const undoIOSfix = () => {
  if (hasClass(document.body, swalClasses.iosfix)) {
    const offset = parseInt(document.body.style.top, 10);
    removeClass(document.body, swalClasses.iosfix);
    document.body.style.top = '';
    document.body.scrollTop = offset * -1;
  }
};

/**
 * Measure scrollbar width for padding body during modal show/hide
 * https://github.com/twbs/bootstrap/blob/master/js/src/modal.js
 *
 * @returns {number}
 */
const measureScrollbar = () => {
  const scrollDiv = document.createElement('div');
  scrollDiv.className = swalClasses['scrollbar-measure'];
  document.body.appendChild(scrollDiv);
  const scrollbarWidth = scrollDiv.getBoundingClientRect().width - scrollDiv.clientWidth;
  document.body.removeChild(scrollDiv);
  return scrollbarWidth;
};

/**
 * Remember state in cases where opening and handling a modal will fiddle with it.
 * @type {number | null}
 */
let previousBodyPadding = null;

/**
 * @param {string} initialBodyOverflow
 */
const replaceScrollbarWithPadding = initialBodyOverflow => {
  // for queues, do not do this more than once
  if (previousBodyPadding !== null) {
    return;
  }
  // if the body has overflow
  if (document.body.scrollHeight > window.innerHeight || initialBodyOverflow === 'scroll' // https://github.com/sweetalert2/sweetalert2/issues/2663
  ) {
    // add padding so the content doesn't shift after removal of scrollbar
    previousBodyPadding = parseInt(window.getComputedStyle(document.body).getPropertyValue('padding-right'));
    document.body.style.paddingRight = `${previousBodyPadding + measureScrollbar()}px`;
  }
};
const undoReplaceScrollbarWithPadding = () => {
  if (previousBodyPadding !== null) {
    document.body.style.paddingRight = `${previousBodyPadding}px`;
    previousBodyPadding = null;
  }
};

/**
 * @param {SweetAlert} instance
 * @param {HTMLElement} container
 * @param {boolean} returnFocus
 * @param {Function} didClose
 */
function removePopupAndResetState(instance, container, returnFocus, didClose) {
  if (isToast()) {
    triggerDidCloseAndDispose(instance, didClose);
  } else {
    restoreActiveElement(returnFocus).then(() => triggerDidCloseAndDispose(instance, didClose));
    removeKeydownHandler(globalState);
  }

  // workaround for https://github.com/sweetalert2/sweetalert2/issues/2088
  // for some reason removing the container in Safari will scroll the document to bottom
  if (isSafariOrIOS) {
    container.setAttribute('style', 'display:none !important');
    container.removeAttribute('class');
    container.innerHTML = '';
  } else {
    container.remove();
  }
  if (isModal()) {
    undoReplaceScrollbarWithPadding();
    undoIOSfix();
    unsetAriaHidden();
  }
  removeBodyClasses();
}

/**
 * Remove SweetAlert2 classes from body
 */
function removeBodyClasses() {
  removeClass([document.documentElement, document.body], [swalClasses.shown, swalClasses['height-auto'], swalClasses['no-backdrop'], swalClasses['toast-shown']]);
}

/**
 * Instance method to close sweetAlert
 *
 * @param {any} resolveValue
 */
function close(resolveValue) {
  resolveValue = prepareResolveValue(resolveValue);
  const swalPromiseResolve = privateMethods.swalPromiseResolve.get(this);
  const didClose = triggerClosePopup(this);
  if (this.isAwaitingPromise) {
    // A swal awaiting for a promise (after a click on Confirm or Deny) cannot be dismissed anymore #2335
    if (!resolveValue.isDismissed) {
      handleAwaitingPromise(this);
      swalPromiseResolve(resolveValue);
    }
  } else if (didClose) {
    // Resolve Swal promise
    swalPromiseResolve(resolveValue);
  }
}
const triggerClosePopup = instance => {
  const popup = getPopup();
  if (!popup) {
    return false;
  }
  const innerParams = privateProps.innerParams.get(instance);
  if (!innerParams || hasClass(popup, innerParams.hideClass.popup)) {
    return false;
  }
  removeClass(popup, innerParams.showClass.popup);
  addClass(popup, innerParams.hideClass.popup);
  const backdrop = getContainer();
  removeClass(backdrop, innerParams.showClass.backdrop);
  addClass(backdrop, innerParams.hideClass.backdrop);
  handlePopupAnimation(instance, popup, innerParams);
  return true;
};

/**
 * @param {any} error
 */
function rejectPromise(error) {
  const rejectPromise = privateMethods.swalPromiseReject.get(this);
  handleAwaitingPromise(this);
  if (rejectPromise) {
    // Reject Swal promise
    rejectPromise(error);
  }
}

/**
 * @param {SweetAlert} instance
 */
const handleAwaitingPromise = instance => {
  if (instance.isAwaitingPromise) {
    delete instance.isAwaitingPromise;
    // The instance might have been previously partly destroyed, we must resume the destroy process in this case #2335
    if (!privateProps.innerParams.get(instance)) {
      instance._destroy();
    }
  }
};

/**
 * @param {any} resolveValue
 * @returns {SweetAlertResult}
 */
const prepareResolveValue = resolveValue => {
  // When user calls Swal.close()
  if (typeof resolveValue === 'undefined') {
    return {
      isConfirmed: false,
      isDenied: false,
      isDismissed: true
    };
  }
  return Object.assign({
    isConfirmed: false,
    isDenied: false,
    isDismissed: false
  }, resolveValue);
};

/**
 * @param {SweetAlert} instance
 * @param {HTMLElement} popup
 * @param {SweetAlertOptions} innerParams
 */
const handlePopupAnimation = (instance, popup, innerParams) => {
  var _globalState$eventEmi;
  const container = getContainer();
  // If animation is supported, animate
  const animationIsSupported = hasCssAnimation(popup);
  if (typeof innerParams.willClose === 'function') {
    innerParams.willClose(popup);
  }
  (_globalState$eventEmi = globalState.eventEmitter) === null || _globalState$eventEmi === void 0 || _globalState$eventEmi.emit('willClose', popup);
  if (animationIsSupported) {
    animatePopup(instance, popup, container, innerParams.returnFocus, innerParams.didClose);
  } else {
    // Otherwise, remove immediately
    removePopupAndResetState(instance, container, innerParams.returnFocus, innerParams.didClose);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {HTMLElement} popup
 * @param {HTMLElement} container
 * @param {boolean} returnFocus
 * @param {Function} didClose
 */
const animatePopup = (instance, popup, container, returnFocus, didClose) => {
  globalState.swalCloseEventFinishedCallback = removePopupAndResetState.bind(null, instance, container, returnFocus, didClose);
  /**
   * @param {AnimationEvent | TransitionEvent} e
   */
  const swalCloseAnimationFinished = function (e) {
    if (e.target === popup) {
      var _globalState$swalClos;
      (_globalState$swalClos = globalState.swalCloseEventFinishedCallback) === null || _globalState$swalClos === void 0 || _globalState$swalClos.call(globalState);
      delete globalState.swalCloseEventFinishedCallback;
      popup.removeEventListener('animationend', swalCloseAnimationFinished);
      popup.removeEventListener('transitionend', swalCloseAnimationFinished);
    }
  };
  popup.addEventListener('animationend', swalCloseAnimationFinished);
  popup.addEventListener('transitionend', swalCloseAnimationFinished);
};

/**
 * @param {SweetAlert} instance
 * @param {Function} didClose
 */
const triggerDidCloseAndDispose = (instance, didClose) => {
  setTimeout(() => {
    var _globalState$eventEmi2;
    if (typeof didClose === 'function') {
      didClose.bind(instance.params)();
    }
    (_globalState$eventEmi2 = globalState.eventEmitter) === null || _globalState$eventEmi2 === void 0 || _globalState$eventEmi2.emit('didClose');
    // instance might have been destroyed already
    if (instance._destroy) {
      instance._destroy();
    }
  });
};

/**
 * Shows loader (spinner), this is useful with AJAX requests.
 * By default the loader be shown instead of the "Confirm" button.
 *
 * @param {HTMLButtonElement | null} [buttonToReplace]
 */
const showLoading = buttonToReplace => {
  let popup = getPopup();
  if (!popup) {
    new Swal();
  }
  popup = getPopup();
  if (!popup) {
    return;
  }
  const loader = getLoader();
  if (isToast()) {
    hide(getIcon());
  } else {
    replaceButton(popup, buttonToReplace);
  }
  show(loader);
  popup.setAttribute('data-loading', 'true');
  popup.setAttribute('aria-busy', 'true');
  popup.focus();
};

/**
 * @param {HTMLElement} popup
 * @param {HTMLButtonElement | null} [buttonToReplace]
 */
const replaceButton = (popup, buttonToReplace) => {
  const actions = getActions();
  const loader = getLoader();
  if (!actions || !loader) {
    return;
  }
  if (!buttonToReplace && isVisible$1(getConfirmButton())) {
    buttonToReplace = getConfirmButton();
  }
  show(actions);
  if (buttonToReplace) {
    hide(buttonToReplace);
    loader.setAttribute('data-button-to-replace', buttonToReplace.className);
    actions.insertBefore(loader, buttonToReplace);
  }
  addClass([popup, actions], swalClasses.loading);
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const handleInputOptionsAndValue = (instance, params) => {
  if (params.input === 'select' || params.input === 'radio') {
    handleInputOptions(instance, params);
  } else if (['text', 'email', 'number', 'tel', 'textarea'].some(i => i === params.input) && (hasToPromiseFn(params.inputValue) || isPromise(params.inputValue))) {
    showLoading(getConfirmButton());
    handleInputValue(instance, params);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} innerParams
 * @returns {SweetAlertInputValue}
 */
const getInputValue = (instance, innerParams) => {
  const input = instance.getInput();
  if (!input) {
    return null;
  }
  switch (innerParams.input) {
    case 'checkbox':
      return getCheckboxValue(input);
    case 'radio':
      return getRadioValue(input);
    case 'file':
      return getFileValue(input);
    default:
      return innerParams.inputAutoTrim ? input.value.trim() : input.value;
  }
};

/**
 * @param {HTMLInputElement} input
 * @returns {number}
 */
const getCheckboxValue = input => input.checked ? 1 : 0;

/**
 * @param {HTMLInputElement} input
 * @returns {string | null}
 */
const getRadioValue = input => input.checked ? input.value : null;

/**
 * @param {HTMLInputElement} input
 * @returns {FileList | File | null}
 */
const getFileValue = input => input.files && input.files.length ? input.getAttribute('multiple') !== null ? input.files : input.files[0] : null;

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const handleInputOptions = (instance, params) => {
  const popup = getPopup();
  if (!popup) {
    return;
  }
  /**
   * @param {Record<string, any>} inputOptions
   */
  const processInputOptions = inputOptions => {
    if (params.input === 'select') {
      populateSelectOptions(popup, formatInputOptions(inputOptions), params);
    } else if (params.input === 'radio') {
      populateRadioOptions(popup, formatInputOptions(inputOptions), params);
    }
  };
  if (hasToPromiseFn(params.inputOptions) || isPromise(params.inputOptions)) {
    showLoading(getConfirmButton());
    asPromise(params.inputOptions).then(inputOptions => {
      instance.hideLoading();
      processInputOptions(inputOptions);
    });
  } else if (typeof params.inputOptions === 'object') {
    processInputOptions(params.inputOptions);
  } else {
    error(`Unexpected type of inputOptions! Expected object, Map or Promise, got ${typeof params.inputOptions}`);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertOptions} params
 */
const handleInputValue = (instance, params) => {
  const input = instance.getInput();
  if (!input) {
    return;
  }
  hide(input);
  asPromise(params.inputValue).then(inputValue => {
    input.value = params.input === 'number' ? `${parseFloat(inputValue) || 0}` : `${inputValue}`;
    show(input);
    input.focus();
    instance.hideLoading();
  }).catch(err => {
    error(`Error in inputValue promise: ${err}`);
    input.value = '';
    show(input);
    input.focus();
    instance.hideLoading();
  });
};

/**
 * @param {HTMLElement} popup
 * @param {InputOptionFlattened[]} inputOptions
 * @param {SweetAlertOptions} params
 */
function populateSelectOptions(popup, inputOptions, params) {
  const select = getDirectChildByClass(popup, swalClasses.select);
  if (!select) {
    return;
  }
  /**
   * @param {HTMLElement} parent
   * @param {string} optionLabel
   * @param {string} optionValue
   */
  const renderOption = (parent, optionLabel, optionValue) => {
    const option = document.createElement('option');
    option.value = optionValue;
    setInnerHtml(option, optionLabel);
    option.selected = isSelected(optionValue, params.inputValue);
    parent.appendChild(option);
  };
  inputOptions.forEach(inputOption => {
    const optionValue = inputOption[0];
    const optionLabel = inputOption[1];
    // <optgroup> spec:
    // https://www.w3.org/TR/html401/interact/forms.html#h-17.6
    // "...all OPTGROUP elements must be specified directly within a SELECT element (i.e., groups may not be nested)..."
    // check whether this is a <optgroup>
    if (Array.isArray(optionLabel)) {
      // if it is an array, then it is an <optgroup>
      const optgroup = document.createElement('optgroup');
      optgroup.label = optionValue;
      optgroup.disabled = false; // not configurable for now
      select.appendChild(optgroup);
      optionLabel.forEach(o => renderOption(optgroup, o[1], o[0]));
    } else {
      // case of <option>
      renderOption(select, optionLabel, optionValue);
    }
  });
  select.focus();
}

/**
 * @param {HTMLElement} popup
 * @param {InputOptionFlattened[]} inputOptions
 * @param {SweetAlertOptions} params
 */
function populateRadioOptions(popup, inputOptions, params) {
  const radio = getDirectChildByClass(popup, swalClasses.radio);
  if (!radio) {
    return;
  }
  inputOptions.forEach(inputOption => {
    const radioValue = inputOption[0];
    const radioLabel = inputOption[1];
    const radioInput = document.createElement('input');
    const radioLabelElement = document.createElement('label');
    radioInput.type = 'radio';
    radioInput.name = swalClasses.radio;
    radioInput.value = radioValue;
    if (isSelected(radioValue, params.inputValue)) {
      radioInput.checked = true;
    }
    const label = document.createElement('span');
    setInnerHtml(label, radioLabel);
    label.className = swalClasses.label;
    radioLabelElement.appendChild(radioInput);
    radioLabelElement.appendChild(label);
    radio.appendChild(radioLabelElement);
  });
  const radios = radio.querySelectorAll('input');
  if (radios.length) {
    radios[0].focus();
  }
}

/**
 * Converts `inputOptions` into an array of `[value, label]`s
 *
 * @param {Record<string, any>} inputOptions
 * @typedef {string[]} InputOptionFlattened
 * @returns {InputOptionFlattened[]}
 */
const formatInputOptions = inputOptions => {
  /** @type {InputOptionFlattened[]} */
  const result = [];
  if (inputOptions instanceof Map) {
    inputOptions.forEach((value, key) => {
      let valueFormatted = value;
      if (typeof valueFormatted === 'object') {
        // case of <optgroup>
        valueFormatted = formatInputOptions(valueFormatted);
      }
      result.push([key, valueFormatted]);
    });
  } else {
    Object.keys(inputOptions).forEach(key => {
      let valueFormatted = inputOptions[key];
      if (typeof valueFormatted === 'object') {
        // case of <optgroup>
        valueFormatted = formatInputOptions(valueFormatted);
      }
      result.push([key, valueFormatted]);
    });
  }
  return result;
};

/**
 * @param {string} optionValue
 * @param {SweetAlertInputValue} inputValue
 * @returns {boolean}
 */
const isSelected = (optionValue, inputValue) => {
  return !!inputValue && inputValue.toString() === optionValue.toString();
};

/**
 * @param {SweetAlert} instance
 */
const handleConfirmButtonClick = instance => {
  const innerParams = privateProps.innerParams.get(instance);
  instance.disableButtons();
  if (innerParams.input) {
    handleConfirmOrDenyWithInput(instance, 'confirm');
  } else {
    confirm(instance, true);
  }
};

/**
 * @param {SweetAlert} instance
 */
const handleDenyButtonClick = instance => {
  const innerParams = privateProps.innerParams.get(instance);
  instance.disableButtons();
  if (innerParams.returnInputValueOnDeny) {
    handleConfirmOrDenyWithInput(instance, 'deny');
  } else {
    deny(instance, false);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {Function} dismissWith
 */
const handleCancelButtonClick = (instance, dismissWith) => {
  instance.disableButtons();
  dismissWith(DismissReason.cancel);
};

/**
 * @param {SweetAlert} instance
 * @param {'confirm' | 'deny'} type
 */
const handleConfirmOrDenyWithInput = (instance, type) => {
  const innerParams = privateProps.innerParams.get(instance);
  if (!innerParams.input) {
    error(`The "input" parameter is needed to be set when using returnInputValueOn${capitalizeFirstLetter(type)}`);
    return;
  }
  const input = instance.getInput();
  const inputValue = getInputValue(instance, innerParams);
  if (innerParams.inputValidator) {
    handleInputValidator(instance, inputValue, type);
  } else if (input && !input.checkValidity()) {
    instance.enableButtons();
    instance.showValidationMessage(innerParams.validationMessage || input.validationMessage);
  } else if (type === 'deny') {
    deny(instance, inputValue);
  } else {
    confirm(instance, inputValue);
  }
};

/**
 * @param {SweetAlert} instance
 * @param {SweetAlertInputValue} inputValue
 * @param {'confirm' | 'deny'} type
 */
const handleInputValidator = (instance, inputValue, type) => {
  const innerParams = privateProps.innerParams.get(instance);
  instance.disableInput();
  const validationPromise = Promise.resolve().then(() => asPromise(innerParams.inputValidator(inputValue, innerParams.validationMessage)));
  validationPromise.then(validationMessage => {
    instance.enableButtons();
    instance.enableInput();
    if (validationMessage) {
      instance.showValidationMessage(validationMessage);
    } else if (type === 'deny') {
      deny(instance, inputValue);
    } else {
      confirm(instance, inputValue);
    }
  });
};

/**
 * @param {SweetAlert} instance
 * @param {any} value
 */
const deny = (instance, value) => {
  const innerParams = privateProps.innerParams.get(instance || undefined);
  if (innerParams.showLoaderOnDeny) {
    showLoading(getDenyButton());
  }
  if (innerParams.preDeny) {
    instance.isAwaitingPromise = true; // Flagging the instance as awaiting a promise so it's own promise's reject/resolve methods doesn't get destroyed until the result from this preDeny's promise is received
    const preDenyPromise = Promise.resolve().then(() => asPromise(innerParams.preDeny(value, innerParams.validationMessage)));
    preDenyPromise.then(preDenyValue => {
      if (preDenyValue === false) {
        instance.hideLoading();
        handleAwaitingPromise(instance);
      } else {
        instance.close({
          isDenied: true,
          value: typeof preDenyValue === 'undefined' ? value : preDenyValue
        });
      }
    }).catch(error => rejectWith(instance || undefined, error));
  } else {
    instance.close({
      isDenied: true,
      value
    });
  }
};

/**
 * @param {SweetAlert} instance
 * @param {any} value
 */
const succeedWith = (instance, value) => {
  instance.close({
    isConfirmed: true,
    value
  });
};

/**
 *
 * @param {SweetAlert} instance
 * @param {string} error
 */
const rejectWith = (instance, error) => {
  instance.rejectPromise(error);
};

/**
 *
 * @param {SweetAlert} instance
 * @param {any} value
 */
const confirm = (instance, value) => {
  const innerParams = privateProps.innerParams.get(instance || undefined);
  if (innerParams.showLoaderOnConfirm) {
    showLoading();
  }
  if (innerParams.preConfirm) {
    instance.resetValidationMessage();
    instance.isAwaitingPromise = true; // Flagging the instance as awaiting a promise so it's own promise's reject/resolve methods doesn't get destroyed until the result from this preConfirm's promise is received
    const preConfirmPromise = Promise.resolve().then(() => asPromise(innerParams.preConfirm(value, innerParams.validationMessage)));
    preConfirmPromise.then(preConfirmValue => {
      if (isVisible$1(getValidationMessage()) || preConfirmValue === false) {
        instance.hideLoading();
        handleAwaitingPromise(instance);
      } else {
        succeedWith(instance, typeof preConfirmValue === 'undefined' ? value : preConfirmValue);
      }
    }).catch(error => rejectWith(instance || undefined, error));
  } else {
    succeedWith(instance, value);
  }
};

/**
 * Hides loader and shows back the button which was hidden by .showLoading()
 */
function hideLoading() {
  // do nothing if popup is closed
  const innerParams = privateProps.innerParams.get(this);
  if (!innerParams) {
    return;
  }
  const domCache = privateProps.domCache.get(this);
  hide(domCache.loader);
  if (isToast()) {
    if (innerParams.icon) {
      show(getIcon());
    }
  } else {
    showRelatedButton(domCache);
  }
  removeClass([domCache.popup, domCache.actions], swalClasses.loading);
  domCache.popup.removeAttribute('aria-busy');
  domCache.popup.removeAttribute('data-loading');
  domCache.confirmButton.disabled = false;
  domCache.denyButton.disabled = false;
  domCache.cancelButton.disabled = false;
}
const showRelatedButton = domCache => {
  const buttonToReplace = domCache.popup.getElementsByClassName(domCache.loader.getAttribute('data-button-to-replace'));
  if (buttonToReplace.length) {
    show(buttonToReplace[0], 'inline-block');
  } else if (allButtonsAreHidden()) {
    hide(domCache.actions);
  }
};

/**
 * Gets the input DOM node, this method works with input parameter.
 *
 * @returns {HTMLInputElement | null}
 */
function getInput() {
  const innerParams = privateProps.innerParams.get(this);
  const domCache = privateProps.domCache.get(this);
  if (!domCache) {
    return null;
  }
  return getInput$1(domCache.popup, innerParams.input);
}

/**
 * @param {SweetAlert} instance
 * @param {string[]} buttons
 * @param {boolean} disabled
 */
function setButtonsDisabled(instance, buttons, disabled) {
  const domCache = privateProps.domCache.get(instance);
  buttons.forEach(button => {
    domCache[button].disabled = disabled;
  });
}

/**
 * @param {HTMLInputElement | null} input
 * @param {boolean} disabled
 */
function setInputDisabled(input, disabled) {
  const popup = getPopup();
  if (!popup || !input) {
    return;
  }
  if (input.type === 'radio') {
    /** @type {NodeListOf<HTMLInputElement>} */
    const radios = popup.querySelectorAll(`[name="${swalClasses.radio}"]`);
    for (let i = 0; i < radios.length; i++) {
      radios[i].disabled = disabled;
    }
  } else {
    input.disabled = disabled;
  }
}

/**
 * Enable all the buttons
 * @this {SweetAlert}
 */
function enableButtons() {
  setButtonsDisabled(this, ['confirmButton', 'denyButton', 'cancelButton'], false);
}

/**
 * Disable all the buttons
 * @this {SweetAlert}
 */
function disableButtons() {
  setButtonsDisabled(this, ['confirmButton', 'denyButton', 'cancelButton'], true);
}

/**
 * Enable the input field
 * @this {SweetAlert}
 */
function enableInput() {
  setInputDisabled(this.getInput(), false);
}

/**
 * Disable the input field
 * @this {SweetAlert}
 */
function disableInput() {
  setInputDisabled(this.getInput(), true);
}

/**
 * Show block with validation message
 *
 * @param {string} error
 * @this {SweetAlert}
 */
function showValidationMessage(error) {
  const domCache = privateProps.domCache.get(this);
  const params = privateProps.innerParams.get(this);
  setInnerHtml(domCache.validationMessage, error);
  domCache.validationMessage.className = swalClasses['validation-message'];
  if (params.customClass && params.customClass.validationMessage) {
    addClass(domCache.validationMessage, params.customClass.validationMessage);
  }
  show(domCache.validationMessage);
  const input = this.getInput();
  if (input) {
    input.setAttribute('aria-invalid', 'true');
    input.setAttribute('aria-describedby', swalClasses['validation-message']);
    focusInput(input);
    addClass(input, swalClasses.inputerror);
  }
}

/**
 * Hide block with validation message
 *
 * @this {SweetAlert}
 */
function resetValidationMessage() {
  const domCache = privateProps.domCache.get(this);
  if (domCache.validationMessage) {
    hide(domCache.validationMessage);
  }
  const input = this.getInput();
  if (input) {
    input.removeAttribute('aria-invalid');
    input.removeAttribute('aria-describedby');
    removeClass(input, swalClasses.inputerror);
  }
}

const defaultParams = {
  title: '',
  titleText: '',
  text: '',
  html: '',
  footer: '',
  icon: undefined,
  iconColor: undefined,
  iconHtml: undefined,
  template: undefined,
  toast: false,
  draggable: false,
  animation: true,
  theme: 'light',
  showClass: {
    popup: 'swal2-show',
    backdrop: 'swal2-backdrop-show',
    icon: 'swal2-icon-show'
  },
  hideClass: {
    popup: 'swal2-hide',
    backdrop: 'swal2-backdrop-hide',
    icon: 'swal2-icon-hide'
  },
  customClass: {},
  target: 'body',
  color: undefined,
  backdrop: true,
  heightAuto: true,
  allowOutsideClick: true,
  allowEscapeKey: true,
  allowEnterKey: true,
  stopKeydownPropagation: true,
  keydownListenerCapture: false,
  showConfirmButton: true,
  showDenyButton: false,
  showCancelButton: false,
  preConfirm: undefined,
  preDeny: undefined,
  confirmButtonText: 'OK',
  confirmButtonAriaLabel: '',
  confirmButtonColor: undefined,
  denyButtonText: 'No',
  denyButtonAriaLabel: '',
  denyButtonColor: undefined,
  cancelButtonText: 'Cancel',
  cancelButtonAriaLabel: '',
  cancelButtonColor: undefined,
  buttonsStyling: true,
  reverseButtons: false,
  focusConfirm: true,
  focusDeny: false,
  focusCancel: false,
  returnFocus: true,
  showCloseButton: false,
  closeButtonHtml: '&times;',
  closeButtonAriaLabel: 'Close this dialog',
  loaderHtml: '',
  showLoaderOnConfirm: false,
  showLoaderOnDeny: false,
  imageUrl: undefined,
  imageWidth: undefined,
  imageHeight: undefined,
  imageAlt: '',
  timer: undefined,
  timerProgressBar: false,
  width: undefined,
  padding: undefined,
  background: undefined,
  input: undefined,
  inputPlaceholder: '',
  inputLabel: '',
  inputValue: '',
  inputOptions: {},
  inputAutoFocus: true,
  inputAutoTrim: true,
  inputAttributes: {},
  inputValidator: undefined,
  returnInputValueOnDeny: false,
  validationMessage: undefined,
  grow: false,
  position: 'center',
  progressSteps: [],
  currentProgressStep: undefined,
  progressStepsDistance: undefined,
  willOpen: undefined,
  didOpen: undefined,
  didRender: undefined,
  willClose: undefined,
  didClose: undefined,
  didDestroy: undefined,
  scrollbarPadding: true
};
const updatableParams = ['allowEscapeKey', 'allowOutsideClick', 'background', 'buttonsStyling', 'cancelButtonAriaLabel', 'cancelButtonColor', 'cancelButtonText', 'closeButtonAriaLabel', 'closeButtonHtml', 'color', 'confirmButtonAriaLabel', 'confirmButtonColor', 'confirmButtonText', 'currentProgressStep', 'customClass', 'denyButtonAriaLabel', 'denyButtonColor', 'denyButtonText', 'didClose', 'didDestroy', 'draggable', 'footer', 'hideClass', 'html', 'icon', 'iconColor', 'iconHtml', 'imageAlt', 'imageHeight', 'imageUrl', 'imageWidth', 'preConfirm', 'preDeny', 'progressSteps', 'returnFocus', 'reverseButtons', 'showCancelButton', 'showCloseButton', 'showConfirmBut\$0H��H�� _Ð���H�\$W�    �l��H+�H����H����L����tH�����H�\$0H��H�� _Ð���H�\$W�    �Pl��H+�H����H����L����tH���Z��H�\$0H��H�� _Ð���H�\$W�    �l��H+���H���+����tH�����H�\$0H��H�� _Ð�������H�\$W�    ��k��H+���H���ˠ����tH������H�\$0H��H�� _�H�\$W�    �k��H+�H�y���H���K�����tH�����H�\$0H��H�� _�H�\$W�    �\k��H+�H��x�����H���H�����tH���c��H�\$0H��H�� _Ð������������H�\$W�    �k��H+���H���#�����tH�����H�\$0H��H�� _Ð�������H�\$W�    ��j��H+���H���;���H�l X H�G`��tH������H�\$0H��H�� _Ð������������H�\$W�    �j��H+���H���?���H� X H�G`��tH�����H�\$0H��H�� _Ð������������H�\$W�    �0j��H+���H���C���H���W H�G`��tH���3��H�\$0H��H�� _Ð������������H�\$W�    ��i��H+���H���G�����tH������H�\$0H��H�� _Ð�������H�\$W�    �i��H+���H���S�����tH�����H�\$0H��H�� _Ð�������H�\$W�    �`i��H+���H����w����tH���n��H�\$0H��H�� _Ð�������H�\$W�    � i��H+���H���G����tH���.��H�\$0H��H�� _Ð�������H�\$W�    ��h��H+���H���O����tH������H�\$0H��H�� _Ð�������@S�    �h��H+�H�J�W H��H���t���H��H�� [ÐH�\$W�    �ph��H+���H��������tH���~��H�\$0H��H�� _�H�\$W�    �8h��H+���H��蛄����tH���F��H�\$0H��H�� _�H�\$UVWATAVH�l$ɸ�   ��g��H+�H�{�� H3�H�E'H��H��H�H���   H�QH�M�å���L�5�Y L�u�H�DkY H�M�W�E�W��M� E�HM�H�@   E3�L�`fD� fD�e�H�U�H��H����T H�M��e0���H�M�[0��H�H��6U H�L$ H�kY H�L$(�O(�M�fD�e�H�T$ H��H���   �|T H�H���   L�G0H��jY H�M������H��H��H���NT �H�M���/��H�H���   H�WPH�L$ �Ȥ���L�u�H��jY H�M�W�E�W��M� E�HM�L�`H�@   fD� fD�e�H�U�H��H����T H�M��t/��H�L$ �j/��H�M'H3��
Y��H��$   H���   A^A\_^]Ð���(   �6f��H+�H��(�̐�H��H�XH�pH�xL�` AUAVAW��
  �f��H+�)�$�
  H���� H3�H��$�
  @��3��|$p�aT D��H��Y H��$�  H�P�Y H��$�  H��ig H��$�  f��$�  H���Y H��$�   W��$�   fo55U ��$�   f��$�   E3ɍ_2D�ú�  A��pA���e��L�5�qZ L�%Tig ��taH��$�   H��$�   H��$�  H��$�   L�t$@H��$�   H�D$HH��$�   H�D$PE3�H�D$@H�D$(L�d$ D�ú�  A���O��H��$�   ��-��E3�H����H�auy �;�S H�J� 3ҍJA��  ��l��A�   D�-�I� �=J� W��$�  ��$�  f��$�  �k��H��H��hg H��$0  �=.����D$p   L��$�  H��$0  H��� �����tE3�H��hg H��$�  �UC�����t@��H��$0  �-����tZH�|hg H��$�  ��-���H��hg H��$0  �-���H��$�  H��$0  �� �H��$0  �,��H��$�  �,��W��$�   �$�   E3�E3�A�Q3���T H��H� H��u6��T D��L��hg �   H��$�  �Z���H�o�~ H��$�  �+���E3�3ҍJ�Uk��I����-��H��tH��hg H�H�wx@ H�H�H��H��$�   H��$  H�L$(�|$ L��L�K���3�3���T H��$  H����  H��$  H��$�   ����9�$  t�dT ̻   ���o-��H��tH��  H��H��H��$�   A�   H��$  H�L$(�|$ L��L����3�3��T H��$  H����  H��$  H��$�   薼��9�$  t��T �@����  H����,��H��tH���b H��H��H�~ky H���� ����A�2   ���7  H��Y H��$�  H�X�Y H��$�  H�igg H��$�  f��$�  H���Y H��$�   W��$�   ��$�   f��$�   E3�E�ź�  ��3p#���a����t`H��$�   H��$  H��$�  H��$  L�t$@H��$  H�D$HH��$   H�D$PE3�H�D$@H�D$(L�d$ E�ź�  ���s	��H��$�   ��)���<F� �qT A+ǋ�H��$�  �o��H��H�=gg H��$0  �*��H�t$0L��$0  H��$   �z���L�=�2X L��$   H���Y H��$�   W��$�   ��$�   f��$�   E3�E�ź�  ��8#���`����tiH��$�   H��$  H��$   H��$  L�t$@H��$  H�D$HH��$   H�D$PE3�H�D$@H�D$(L�5�fg L�t$ E�ź�  ���Z���L�5�fg H��$�   ��(��H��$   蟟��H��$0  �(���H��$�  �(���  L�= 2X L�5Yfg �2	  H�L$`�������tlH��� H��� �k{��@8|$`u*��tNH�qfg H�D$ �   �� D�B$��`����@ �(��u$H��fg H�D$ �   �� D�B$�`����@ H��bg �E T f��u1A�H��fg H�D$ �   ���m D�B$�n`��H�obg �	 T �D��W��$P  H��$`  HǄ$h     f��$P  ��e��H��H���U H��$0  �u(���L��$P  H��$0  H���`����H��$0  �n'���   �0)��H��tH��b H��H��H�/U H���-���؅���  H�l�Y H��$�  H���Y H��$�  H�~fg H��$�  f��$�  H���Y H��$�   W��$�   ��$�   f��$�   E3ɺ�  ��pD�C2���^����thH��$�   H��$  H��$�  H��$  L�t$@H��$  H�D$HH��$   H�D$PE3�H�D$@H�D$(H�tag H�D$ ��  D�C2�����H��$�   �F&����B� ���S A+ǋ�H��$�  �ck��H��H��cg H��$0  ��&��H�t$0L��$0  H��$   ����L�=K/X L��$   H���Y H��$�   W��$�   ��$�   f��$�   E3�A�q2D�ƺ�  ��8#��\����tlH��$�   H��$  H��$   H��$  L�t$@H��$  H�D$HH��$   H�D$PE3�H�D$@H�D$(L�5cg L�t$ D�ƺ�  ��8#����L�5�bg H��$�   �%��H��$   ����H��$0  �%���H��$�  ��$��E��t,H9�$`  u"H��$p  H�|� �T+��A�   H98@�t@��A��tH��$x  H��t����@���]  H��-X ���S H���G  H�Qdg H�����S H��H���&  @�|$aA�   fD�d$p�D$r@�|$y@�|${@�|$iH�D$`H�D$(H�D$pH�D$ L�L$xL�D$zH�T$hH��$�   ��'��H��cg H�L$pH�T$pH�L$@����L��$�   H��H��$  � $����|$hH�L$hH���1T ����t9|$ht�8  H��$  ��0��E��D�D$hH��cg H���P������S E3�L��lf ��H��$   �������L��$   H��$  ��/��H��$  �J8���H��$  � A����  ������W  �L�=�,X L�5�`g H��$P  �#������  H�c?� H��$�   A�`�  A��W��$P  H��$`  HǄ$h     f��$P  H��$�   H��y� ����H��D$p   L��$P  H�T$p�u��D��H��$�   H��t����E��t.H��$P  H��$h  HG�$P  3�D�B
�jT ����AD��D$ �  D��E3�H��$�   A�H��ћ ���  ����   =  t?�����   �#�S D��L��mg �   H��$@  ����H��~ H��$@  �̂��H��ag H�D$ �   ��E D�BV�LZ������H��H��$p  H�D$hW���$p  �n���L�_m� H�T$@H�������L��$p  H��H����B���>H��$�   ��Л H��$�   � ћ �D$    E3�E3�3�H��$�   ��Л ��u�H��$P  �-!���'���H��ag H�D$ A�2   E��A�Uܹ�E �Y��H��$P  �� ������  H�5}dZ H�bg H��$0  �!���L��$�  H��$�  W�f�$�  Ƅ$�  W��$�  H��$�  HǄ$�     f��$�  fD$@H�T$@H��$�  赧��Ƅ$�  H��$0  H��$�  �����L��$�  H�0�Y H��$�   W��$�   ��$�   f��$�   E3�E��A��  A��A��"A���KW����t_H��$�   H��$  H��$�  H��$  H�t$@H��$  H�D$HH��$   H�D$PE3�H�D$@H�D$(L�t$ E��A��A������H��$�   ���H��$�  �Z���H��$0  �y��3��.�����5  A�2   H���Y H��$�  H���Y H��$�  H�E`g H��$�  f��$�  W��$0  ��$@  f��$0  �$  ��$   f��$  �$p  ��$�  f��$p  �$P  ��$`  f��$P  ���S �D$pH��$0  H�D$(H��$  H�D$ L��$p  L��$P  H�T$pH��$   ����H��H�f�Y H��$�   W��$�   ��$�   f��$�   E3�E�A
A��  A�Թ�p�U����twH��$�   H�D$@H�t$HH��$�  H�D$PH�5�aZ H��$�   H�D$@H��$�   H�D$XH��$�   E3�H��$�   H�D$(H��Xg H�D$ E�A
A�Թ�p�/����H�5NaZ H��$�   ���H��$   �,���H��$P  ���H��$p  ���H��$  �y��H��$0  �l���~�����$  �   ���S ���$  �   ���S ��� 3��\$h9�$�   tH��$�   ���9�$�   tH��$�   �s��E3���A�H�\���H��$�   ���H��$�   ���H��$�  ����H��$�
  H3��qF��L��$�
  I�[ I�s(I�{0M�c8A(s�I��A_A^A]Ð���������H�ieg �H�\$H�t$W��  �sS��H+�H���� H3�H��$�  3��މ\$4H�L$P� ���H��$�   H���� ��H�L$XH��t�菢��H9�$�   �$  A�H�##U H��$�   �>2�����  @�t$0H�L$0�S�����t*@8t$0t#H�L$8������   �\$4H���@ ��@��t@����tH�L$@H��t����@����   H�ڊY H�D$pH��Y H�D$xH�R_g H��$�   f��$�   H�[�Y H��$�   W��$�   foP"U ��$�   f��$�   E3�A�Y2D�ÿ�  �׹���pR����t_H��$�   H�D$PH�D$pH�D$XH��^Z H�D$8H�D$PH�D$@H�D$`H�D$HE3�H�D$8H�D$(H�_g H�D$ D�Ë׹���1���H��$�   ����   H�L$p�/���H�E#� �D$4	   L�D$4H�T$8H�L$p�M��H�H��(H�������H��&� �D$4   L�D$4H�T$8H�L$p���H�H��(H������H��dU H��$�   ����L�D$pH��$�   H�Gq� �ry���H��$�   ���H�L$p�����H��$�   ����� H��$�  H3��C��L��$�  I�[I�sI��_Ð����H�\$W�0  �P��H+�H�6�� H3�H��$   H�L$P�eN��H�L$P3�H��uH�L$XH��t�ڟ���j  H��$�  ����H��$p  H�L$P�V���@��H�X[g H��$�   ���L��$�   H��$�  H��$P  �>_��H��$�   ���E3�H���U H��$p  �2/����uVH�'[g H�L$`�����L�D$`H��$�  H��$  ��^��H��H��$P  ����H��$  ����H�L$`����H�AxY H��$�   ��$�   H�kU H��$�   W��$�   fohU f�$�   f��$�   H��$   Ƅ$  Ǆ$  �   H��U H��Zg ��HD�H�D$`L�D$`H��Zg H��$0  ��
��@�|$(�|$ A�L��$0  H��$�   H��� �i�����y  E3�H��$P  H��$�   ��Z����H��$�  ��]��H��H��Zg H��$  ���H��$0  H�D$@H�\$0H��$p  H�D$ L��$  H��$�  菴��H�� X H��$�  H�A�Y H��$�   W��$�   fo6U ��$�   f��$�   E3�A�Y2D�ú�  �ՓP�XN����tqH��$�   H�D$`H��$�  H�D$hH�ZZ H��$�   H�D$`H��$�   H�D$pH��$�   E3�H��$�   H�D$(H�Zg H�D$ D�ú�  �ՓP����H��$�   ���H��$�  �U���H��$  �t���H��$�  �f���H��$0  �X��H��$�   ��i���    H��$P  �7��H��$p  �*��H��$�  ���H�L$XH��t�n����� H��$   H3��?��H��$@  H��0  _Ð�����H�\$H�t$W�P  ��L��H+�H�Q�� H3�H��$@  �`   eH�%X   H� �9�� ��   H��� �NC���=�� ���   H�FZg H��$�  �M��H�ZZg H��$�  �9��H�nZg H��$�  �%��H��Zg H��$�  ���H��Zg H��$   ����H��$�  H��$�   H��$   H��$�   (�$�   f�$�   H��$�   H�� �lB��L�g���    D�B�H��$�  �A��H���S �@��H��� ��A��W��$`  fo�U ��$p  3�f��$`  3�H��$�   ���H��H��$`  �"���H��$�   �Q��H�n� H�5o� H�t$PH�\$HH;���  H��H��$�   �<P��L��$�   H��$`  H��$   ����H��H��$�   �_(��H��$   ����H��$�   ����H��$�   �������N  H��$�   ����H��Zg H��$�   ���H��$�   H�D$ L��\f L��$�   H��$  �*5��H��X H��$  H�P�Y H�D$hW�D$pH��$�   HǄ$�      f�|$pE3ɺ�  ���iE�A2�pJ����toH�D$hH��$�   H��$  H��$�   H��VZ H�D$0H��$�   H�D$8H��$�   H�D$@E3�H�D$0H�D$(H�9Yg H�D$ ��  ���iE�A2�!���H�L$p���H��$  �r���H��$�   ����@ �     �3�H�\$HH�t$PH��$�   �i��H�� �"���H��$`  �S��H��$@  H3���;��L��$P  I�[I�sI��_Ð����������(   �I��H+�3�H��(�����̐��H�\$H�t$W�P   ��H��H+���H��H�`[f H�D$ �   �E�= D�B$�PJ��H�C(H�D$0�D$8�   H��t��@H�L$0�l��H���8Y���H�L$0��)��H�KH��t
��  �P��H���   H�L$@�����@ �     H�{H�H��tL胖�������D���H���53 W��D$0H�T$0H����b��H�L$8H��t�|���H�H��H�@@�h�S H���   3���x���H�L$@�k���H�\$`H�t$hH��P_Ð��������������@S��   ��G��H+�H�J�� H3�H��$�   H��H�-Qy H�L$`����H��H�L$@�q���E3�H�T$`H�L$@�6�����H�L$@�r��H�L$`�h��H��Wy H�D$ H����H�*�����HD�H�L$(H�d$0 H�d$8 H�L$ ���S ��u�.�S �3�H��$�   H3��9��H�Đ   [Ð@S�    ��F��H+�H�QH��H�H�A�*��H�H�HH�� [Ð�h   ��F��H+�H��H�� ��t<�D$ H�T$ H�p�� H�D$(H�L$0(D$ fD$ �����H�o�~ H�L$0�fp��̋�H��hÐH�\$W�    �dF��H+�H��q�q�H��H;�v�����蔵��H�H��H��t@H�S�P���H�SH�9��8��8�H+H�H��H��H��H��跙��H�# H�c H�c H����7��H������H�H��H�CH��H�H�KH�\$0H�� _øH   �E��H+�W�H�9~U D$(H�D$(H���~ H�9~U H�L$ H�D$ �so��̐@S��   �|E��H+�)�$�   I��2H��H�L$0�҄����t$ L�L$ L��H�T$0H�L$P� ,��H��~ H�L$P�o��̐��h   �"E��H+��L$ H���� H�D$(H�T$ (D$ H�L$0fD$ �Q���H�֌~ H�L$0��n��̐��H�\$H�L$UVWATAUAVAW�@   ��D��H+�I��H��H��I;���   L�bH�AL�8L��H�qH�T$0H��H�D$ L�d$(����L��L#s0M�J�D�N�,�H��$�   L;�H�L$ ���W�����uBL�\$0H��$�   L;�u�L;�uN��H�D$(L�I�CH��$�   H��H��@A_A^A]A\_^]�L;�uN�<�M��N�d�H��$�   L�\$0L;�t�I�S�u���L��L#u0M�N�d�M;�H�L$ ���������uL�\$0L;�u��w���N�<�N�|��̐���(   �C��H+�H��Ug �b�S ̐H�\$W�0   �xC��H+��y  H��H��tL�A(�M��tH�T$ H��� �S H�L$ �_��H�;�tH�H����S H�SH���t	H�����S �S��t	H�����S �SH�����S �SE3�H�����S �SH��H�\$@H��0_H�%o�S ̐�������������H�\$H�l$H�t$W�    �B��H+�H�AH��H+I��H��H�9��8��8�H��H��H;�vH������H��&H�SH��H+;H��H��H�H;�v�ܲ��H�KH+�H��L��L��軱��H���H�<�H��H;H��诲��H�SH���G���H�{H�\$0H�l$8H�t$@H�� _Ð�H�\$H�l$H�t$W�    ��A��H+�L�3�H�AH�9��8��8�I+�H��H����H��H��teD��H9{0t*K��H�A9<�|Ic�H�SL��$I����B����rK��I��H��3�I�����L���H�CI+�D��H��H��L;�r��{<@�{DH9{0t.9{@~)H�SHc�L��$I����B����s�G���C<;C@|�H�l$8H��H�\$0H�t$@H�� _Ð��H�\$H�|$UH��p   ��@��H+�H��� �S H��H���d�S H��t:H;�t53�L�E�H�E�H�U�H�W�)E�A�   �E�H���E�H�@X���S H����S H��H�����S H��t:H;�t53�L�E�H�E�H�U�H�W�)E�A�   �E�H���E�H�@X���S L�\$pI�[I�{I��]Ð������HcA�H+�����H�\$W�    �@��H+�H���HH��
��H�D$83�H��tH�W�H�������H���H��H��tH�BPHcXH��PH�H��H�\$0H�� _Ð���HcA�H+�����H�\$W�    �?��H+�H���HH�f
��H�D$83�H��tH�W�H���2���H���H��H��tH�BPHcXH��PH�H��H�\$0H�� _Ð���HcA�H+�����H�\$W�    �4?��H+�H���HH��	��H�D$83�H��tH�W�H������H���H��H��tH�BPHcXH��PH�H��H�\$0H�� _�@S�    ��>��H+�H��H���[���H�� [�@S�    �>��H+�H��H��H��H�d� �L�����H�KH�H#�H�� [Ð����������(   �v>��H+�H��H��c� ����3�H��(ÐH�\$H�l$ VWAV��   �D>��H+�H�ʅ� H3�H��$�   H��L��H�T$ �7C��H������� H�����S @��H�L$ �oZ��I�NI+H��I�9��8��8�I��H��u*@��H�L$0����L��H��I�������H�L$0������  H;���   @��H�L$0�����L��I�NI+H��I��H;�sH��H��II�VH��蓨��I�^�6v4I�FI+H��I��H��H;�v
I���h����H+�M��I�N諬��I�FH�L$0�=���I�N3���z��H��tr3�I���H��������H���   ���    H�KH�3������H�K(H�3������H�dH H�DP   @�lX�D\  �dd �d` H�ǐ   H��u�I�NH3�����H��$�   H3��@/��L��$�   I�[0I�k8I��A^_^Ð�������H��H�XH�hH�pH�x ATAVAW�    �M<��H+���H����u3��  �K�S H��tH���M�S H��H���1�S H;��s  �Ftu����s  H����S H��u3��H���W�S H��H����S H��H+��   H��H��H��H;�L��HB�E3��$�S L��H��H��H;�sH��u�H��tL�<;I�������L��H��tL��I��I����?���~p tH��I������M��FpH��H��u,M��L�vhI���Z�S �FtI��H����   M�NM���   I��I+�HFh���S H��H���*�S H��H��+��|�S H��H���h�S H��+��u�S I��M��I+�H��H����S ��H�����S �FtH��t���S Lc�I��M�H��L�H�	I��E3�M�����S @��H�����S ��H�\$@H�l$HH�t$PH�|$XH�� A_A^A\�H�\$UVWATAUAVAWH�l$и0  �Q:��H+�H�ׁ� H3�H�E H��H��H�T$P�H?��H��蘼��L��H�D$@H�L$P�V���%I�����S �؈D$2�D$0A���D�OhM�Ɗ�H�������Hc�H������E3�D�t$4E3�L!d$8D�d$1E3�E3���H���w���H��H�����  E��uH�OH�A�E�Hc�H��H��H�H��(H�H��H�~vH�H��vH��8DuL�KM��H���=���H��L���   I;�tL��M��H������L��H��H��H�~vH�H�H�D$8�WhIc�L��I��LH��H�~vH�HNH�L$H�T$(H�\$ L�L$@H�T$HH�L$8����H��H�~vH�H�\$8H+ل���   L��H�A���   �t/A�T\��@t�������A�DX0��?�����A�T\A���   �tA�D\ t���A���   H�A����t-���u�D$0 ����u�D$1�A;�DO��D$4A��I�Ɛ   L�ÊT$2H���޶��H��H����g���D�t$4E��tA�E�Hc�H��H��H�H��(H��H�OHL�NM��H��轧��@�t$0@��uhE��x7�Ght1W�D$hH���d H�D$`Ic�H�D$xH�e� H�L$`���������3�Mc�E��~3�L�A�<�uA���H�   I��u�D�y�� H�L$@�N�S ��H�M�����L��Ic�H�OH+H��I�9��8��8�I��H;�sH��H��HH�WH���8���H�_�3v1H�GH+H��I��H;�v
H�������H+�L��H�O�S���H�GH�M�������|$1 t�O8A�O�O@�W8�ʃ����@��DщW8H��H�M H3��Q)��H��$�  H��0  A_A^A]A\_^]Ð�����H�\$H�t$H�|$AV�    �e6��H+���H�����S E3�H��t\H�����S H��H�����S H;�sB�Ftu���tH�����S @:x�u(���H�����S ���tH���y�S ���@�8AD�������H�\$0H�t$8H�|$@H�� A^Ð���������H��H�XH�hH�pH�x AW�    �5��H+���H����S E3�H��H��tIH�����S H;�v;@�����t:]�t�Fpu(���H�����S ���tH�����S ���@�8ADߋ�����H�\$0H�l$8H�t$@H�|$HH�� A_ÐH�\$W�    � 5��H+�H�����S H��H��� �S H+�H�\$0H�� _Ð���������HcA�H+�������   ��4��H+�H�Q�A�   H�L$ ����H�ty~ H�L$ �^��̐HcA�H+�������   �4��H+�H�Q�A�   H�L$ 蓻��H�|~ H�L$ �[^��̐HcA�H+�������   �Z4��H+�H�Q�A�   H�L$ 胼��H��y~ H�L$ �^��̐H�\$H�t$H�|$AV�    �4��H+�A��I��L��H����S H��tH���
�S H9FhsH�����S H�Fh�D$P��   H���C�S H����   ��uH���,�S H�NhH+�H��2��uH����S H��H+����S H�����  �D$P�  H�����S H��H����S H;���   H�����S H�H;Fh��   ��H�����S �D$P��   H���:�S H����   H�����S H��H����S +؋�H����S �   �D$PtxH�����S H��tj��uH�����S H�NhH+�H����uLH�����S H��H+��2�S H�H�����S H��H���S H;�wH�����S H�H;Fhw���s���H���I�f H�\$03�H�t$8I�>H�|$@I�FI��H�� A^Ð�������H��H�XH�hH�pH�x AV�    �!2��H+�I�pA��I0L��H��� �S H��tH����S H9GhsH����S H�GhH���H;���   @����   H���?�S H��tsH����   H��� �S H�OhH+�H;���   H����S ��H��+����S H�ύ���S @��t~H�����S H��tpH�����S H��H���m�S +؋��G@��tLH���X�S H��t>H��x9H�����S H�OhH+�H;�$H���0�S ��H��+��{�S �H����S �H��I�6�I�I�f H�\$03�H�l$8H�t$@H�|$HI�FI��H�� A^Ð��������H�\$H�l$H�t$WATAUAVAW�    �0��H+�H�ypE��H��H��A��t	�t��2�A�E��t	D�'t��2�����   ����   I�hH��I(���S L��D�'t3��H���M�S H��H��t
H9FhsH�FhH�����S L�NhL��I��H+�H;�wcH��tA��tM��tSE��tH��tIL�$(A��tM��tM��I��H�����S A��t H��tH�����S M��I��L��H���*�S H�+�H��H�c H�l$X3�H�t$`H�CH��H�\$PH�� A_A^A]A\_Ð�����H�\$H�t$W�    �[/��H+�H�����S H����   H���]�S H��H�����S H;�sH�����S � �   �Gt��   H��� �S H��twH����S H��H���f�S H;�rH���X�S H;GhsNH�whH�����S H;�sH�����S H��H�GhH���'�S H��H����S L��L��H��H�����S �`������H�\$0H�t$8H�� _Ð��������H�AH�xvH� ÐH�i@g Ð�������H�AhH�xvH� ÐH�	Cg Ð�������H�)Cg Ð�������H�i@g �@S�P   �.��H+�H��u� H3�H�D$@3����  H��f��  I��f��  ǁ�  0000H�L$ �  H�xvH� H�KI���L���   �0�S H�L$ ����H��H�L$@H3��> ��H��P[�H�\$UVWATAUAVAWH�l$��   �i-��H+�H��t� H3�H�E�M��L��H�]gH�}wH���   H���W H�3�H�AH�AH�A�A L�y(W�AI�GI�G   fA�H��%Y H�L$0�����H�H�L$P�����H��2g H�L$p�����)M�G)E�H�G   3�H�_f�H���W H�M�����)M�F)E�H�^H�F   f�H�D$0H�D$ H�E�H�D$((D$ fD$ H�T$ I�M�|��L��G���S D�CH�L$0�!��A�E I��I���{���I��H�M�H3�����H��$H  H��   A_A^A]A\_^]ÐH�\$H�L$W�    ��+��H+�I��H��H�H�a H�a ����H�GH�L$PH�	��P H�H(趶��H�GH�H�GH�XH�GH�X3�H�G�D H��H��|�H��H�\$8H�� _Ð@SUVWAV��   �{+��H+�H�s� H3�H��$�   I��L��H�L$XH��$�   H��$�   3�W�D$`fo9�T �L$pf�D$`K� H��H�L$`��P��L��4y L�H�|$0H�t$(H�l$ L�L$`I���	  �H�L$`�����I��H��$�   H3����H�Đ   A^_^][Ð��H�\$W��   �*��H+�H�>r� H3�H�D$pI��M��H��H�L$HH��$�   3��|$@H�34y I���I��fB9<Bu�H�L$(H�D$ H�L$P������D$PL$`KH�|$`H�D$h   f�|$PH�L$P�6���H��H�L$pH3�����H��$�   H�Ā   _ÐH�\$UVWATAUAVAW��   ��)��H+�H�|q� H3�H��$�   M��H�L$hH�L$pL��$   L��$  L��$  H��$  H��$   H��$(  3�W�D$xfo��T ��$�   f�D$xK� H��H�L$x�3O��L�lKg L�H�|$PH�t$HH�l$@L�t$8L�|$0L�d$(L�l$ L�L$xH�L$h��  �H�L$x�0���H�D$hH��$�   H3�����H��$�   H�Ġ   A_A^A]A\_^]�H�\$UVW��   ��(��H+�H�pp� H3�H��$�   I��M��H��H�L$XH��$�   H��$�   L��$�   L��$�   H��$�   3��t$PH�-�Jg I���I��fB9tE u�H�|$HL�\$@L�T$8H�T$0H�L$(H�D$ H�L$`�K����D$`L$pKH�t$pH�D$x   f�t$`H�L$`�0���H��H��$�   H3�����H��$�   H�Đ   _^]Ð�H�\$UVWATAUAVAW��   ��'��H+�H�po� H3�H��$�   M��L��H�L$XL��$�   L��$�   H��$   H��$  H��$  3�W�D$`fo��T �L$pf�D$`K� H��H�L$`�4M��L�Eg L�H�|$HH�t$@H�l$8L�t$0L�|$(L�d$ L�L$`I���7  �H�L$`�8���I��H��$�   H3�����H��$�   H�Đ   A_A^A]A\_^]Ð�@SVW��   ��&��H+�H�|n� H3�H��$�   I��M��H��H�L$XH��$�   H��$�   L��$�   L��$�   3��|$PH�5VDg I���I��fB9<Fu�L�\$@L�T$8H�T$0H�L$(H�D$ H�L$`�q����D$`L$pKH�|$pH�D$x   f�|$`H�L$`�J���H��H��$�   H3�����H�Đ   _^[�@USVWAVH��p   �&��H+�H��m� H3�H�E�I��I��H��H�M�H�U�E3�H;���   W�E�L�u�H�E�   fD�u�L��H��H�M������uL�M�L��H��H�M��|�����uH�M�����H�U��L�V�T H�U�H������L��L��H�U�H���4   �H�M��j����H��H������H��H�M�H3�����H��pA^_^[]Ð��@USVWAVH��`   �(%��H+�H��l� H3�H�E�I��I��H��H�M�H�U�E3�H;���   W�E�L�u�H�E�   fD�u�L��H��H�M������uL�M�L��H��H�M�萤����uH�M�����H�U��L�j�T H�U�H������L��L��H�U�H��������H�M��~����H��H������H��H�M�H3����H��`A^_^[]Ð��@USVWATAVAWH�츀   �8$��H+�H��k� H3�H�E�I��I��H��H�M�H�U�L�u`L�}hE3�H;���   W�E�L�e�H�E�   fD�e�L��H��H�M������uL�M�L��H��H�M�蘣����uH�M�����H�U��E�H�U�H������L�|$ L��L��H�U�H���O  �H�M������H��H���� ��H��H�M�H3����H�Ā   A_A^A\_^[]Ð��H�\$UVWATAUAVAWH��p   �3#��H+�H��j� H3�H�E�I��I��H��H�M�H�U�L�u`L�}hL�epE3�H;���   W�E�L�m�H�E�   fD�m�L��H��H�M������uL�M�L��H��H�M�菢����uH�M�����H�U��E�H�U�H������L�d$(L�|$ L��L��H�U�H���Ż���H�M��w����H��H�������H��H�M�H3����H��$�   H��pA_A^A]A\_^]Ð��H�\$UVWAVAWH��p   �#"��H+�H��i� H3�H�E�I��I��H��H�M�H�U�L�uPE3�H;�s~W�E�L�}�H�E�   fD�}�L��H��H�M������uL�M�L��H��H�M�苡����uH�M�����H�U��M�H�U�H������L��L��H�U�H���[����H�M��}����H��H�������H��H�M�H3����H��$�   H��pA_A^_^]ÐH�\$UVWATAUAVAWH�츀   �+!��H+�H��h� H3�H�E�I��I��H��H�M�L�u`L�}hL�epE3�H��*y H�E�H;���   W�E�L�m�H�E�   fD�m�L��H��H�M������uL�M�L��H��H�M�耠����uH�M�����H�E��M�H�U�H������L�d$(L�|$ L��L��H�U�H�������H�M��h����H��H�������H��H�M�H3�����H��$�   H�Ā   A_A^A]A\_^]ÐH�\$UVWATAUAVAWH�츀   � ��H+�H��g� H3�H�E�I��I��H��H�M�H�U�L�u`L�}hL�epL�mx3�H;���   W�E�H�E�H�E�   f�E�L��H��H�M��������uL�M�L��H��H�M��i�����uH�M�����H�U��M�H�U�H���� ��L�l$0L�d$(L�|$ L��L��H�U�H�������H�M��L����H��H������H��H�M�H3�����H��$�   H�Ā   A_A^A]A\_^]ÐH�\$UVWATAUAVAWH�츀   ����H+�H�yf� H3�H�E�I��I��H��H�M�H�U�L�u`L�}hL�epL�mxH���   H�E�3�H;���   W�E�H�E�H�E�   f�E�L��H��H�M��������uL�M�L��H��H�M��B�����uH�M��e���H�U��M�H�U�H������H�E�H�D$8L�l$0L�d$(L�|$ L��L��H�U�H��������H�M������H��H���{���H��H�M�H3����H��$�   H�Ā   A_A^A]A\_^]ÐH�\$UVWATAUAVAWH�l$���   ����H+�H�Ge� H3�H�E�I��I��H��H�M�H�U�L�ugL�}oL�ewL�mH���   H�E�H���   H�E�3�H;���   W�E�H�E�H�E�   f�E�L��H��H�M�������uL�M�L��H��H�M�������uH�M��(���H�U��M��H�U�H���?���H�E�H�D$@H�E�H�D$8L�l$0L�d$(L�|$ L��L��H�U�H��������H�M�������H��H���5���H��H�M�H3��f��H��$�   H�Ġ   A_A^A]A\_^]Ð��H�\$UVWATAUAVAWH�츀   �{��H+�H�d� H3�H�E�I��I��H��H�M�H�U�L�u`L�}hL�epL�mxH���   H�E�3�H;���   W�E�H�E�H�E�   f�E�L��H��H�M��Q�����uL�M�L��H��H�M��ʛ����uH�M������H�U��M��H�U�H������H�E�H�D$8L�l$0L�d$(L�|$ L��L��H�U�H���
����H�M������H��H������H��H�M�H3��4��H��$�   H�Ā   A_A^A]A\_^]ÐH�\$UVWATAUAVAWH�l$���   �I��H+�H��b� H3�H�E�I��I��H��H�M�L�ugL�}oL�ewL�mH���   H�E�H���   H�E�H���   H�E�H��<g H�E�3�H;���   W�E�H�M�H�E�   f�M�L��H��H�M�������uL�M�L��H��H�M��{�����uH�M�����H�E��M��H�U�H������H�E�H�D$HH�E�H�D$@H�E�H�D$8L�l$0L�d$(L�|$ L��L��H�U�H���i����H�M��C����H��H������H��H�M�H3�����H��$�   H�İ   A_A^A]A\_^]�H�\$UVWATAUAVAWH�l$���   ����H+�H�oa� H3�H�E�I��I��H��H�M�L�ugL�}oL�ewL�mH���   H�E�H���   H�E�H�N7g H�E�3�H;���   W�E�H�M�H�E�   f�M�L��H��H�M�������uL�M�L��H��H�M��&�����uH�M��I���H�E��M��H�U�H���`���H�E�H�D$@H�E�H�D$8L�l$0L�d$(L�|$ L��L��H�U�H���e����H�M�������H��H���V���H��H�M�H3����H��$�   H�Ġ   A_A^A]A\_^]�@S�0   ���H+�D�@�H���o��H��H��0[�H�\$H�t$H�|$ATAVAW�0   �y��H+�M��M��L��H�ٍpы������H���l��H��H�D$(�p�pH�c�d H�H�wE�E�I��H��������H�3H�{H��H�\$PH�t$XH�|$`H��0A_A^A\Ð�@S�    ����H+�H��H�IH��tH��(H���9���H��H�� [頦��̐��������������H�\$W�    ���H+���H���k�����tH������H�\$0H��H�� _Ð�������@S�    �t��H+�H���d H��H���t����H��H�� [Ð@S�    �D��H+�H�$Y H��H���t�R���H��H�� [Ð�(   ���H+�H�H�@0H��(H�%a�S ̸(   ����H+�H�d�Z I��H��(�0���̐��������������H�\$UVWH��`   ���H+�H�A^� H3�H�E�I��H��3�I�xI��I�xvI� f�8�}�H�U�H�����H;t"�}�L�E�H�U�H���3���H�H��(H��蘙��H9{tE3�H�PU H���x������*  �E�'   H�U�H�����H;��   �E�'   L�E�H�U�H�������H�H��(E3�H��)y �'�����t1�E�'   L�E�H�U�H������H�H��(E3�H�c)y �������umH��)U H�������W�E�fo��T �M�f�}�H�U�H������'h��H�M��N����L�Eغ   H��(����H�M�H��t�d���H�M��S����?�E�   H�U�H������H;u�E�   H�U�H�����H;tH�>�W H���^���H9{v1H��H�{vH��y�1�S H��H�{vH�f�yH��H;{r�H�M�H3��{��H��$�   H��`_^]Ð��H�\$H�t$H�L$W�@   ���H+�A��H��3�H�t$PL�D$P3ҋ����� H�T$PH��tWH�L$(�U���H�D$0H+D$(H��H�H#T$(L�D$P���Ȓ� H�D$0H+D$(H��H�H#T$(H�������H�L$(�4
���W�H�sH�C   f�3H��H�\$XH�t$`H��@_Ð�@SVW�`   ����H+�H�l[� H3�H�D$XA��H��H�T$(H�yH�? uFH�,g H�L$8�����E3�L�D$8��H�L$(���H��H���
{��H�L$(������H�L$8�}���H�H��C�   H��t��AH��H�L$XH3����H��`_^[Ð����������@USVWATAUAVAWH��$�����h  �!��H+�)�$P  H��Z� H3�H��H  M��L�M�L�D$xH��H�U�H�M�E3�A��D�|$XD�|$PH�/(g H�M ����L�E H�M�����H�M ����H�	(g H�M �����H�U H��(  �����H�M ����H�E�H�D$`H��(  H�D$hH�L$`��������  H���   H��2� ������L���   M����   H�{JY H�E H���Y H�EH��-g H�EfD�}H��Y H�E�W�E�fo�T �M�fD�}�E3�E�|$2E��A��  A�տ��M���$����tTH�E�H�D$`H�E H�D$hH�SZ H�E H�D$`H�E(H�D$pH�E0E3�H�E H�D$(H��-g H�D$ E��A�Ջ�����H�M��{����   A�6H���   H���|  �`���r  H��  I����y���H�UhI���y��H�ؾ   �t$XH�N�W H�M ������~�|$XE3�H��H�M �z��D�v����   H���   I���oy��H��D�t$XH�L$U H�M�������~�|$XE3�H��H�M��Pz����tEH�U@I���0y��H���D$X   H�:�Z H�M �u�����~>�|$XE3�H��H�M �z����A��u@�ވ\$R@�� t���H�M �^����@��t���H�M@�K����@��t���H�M��8����@��t���H���   �"����@��t���H�M �����@��t	H�Mh� ���D�|$QA�2   A��  H�=�,g fo5��T ���I  E3�3�H�M�����H�M��q>5 �D$Q���F  H�U@I���)x���L��H�
,g H�M��i���L9pvH� H��GY H�M H��Y H�MH�E3�f�]H�Q�Y H�E�W�E��u�f�]�E3�E��A�չK�@�����tPH�E�H�D$`H�E H�D$hH��Z H�E H�D$`H�E(H�D$pH�E0E3�H�E H�D$(H�|$ E��A�չK�@�V���H�M������H�M�������H�M@�����I������H�M�H�H�@ ��S H��� �D$T   L�D$TH�UhH�L$x�[���H�H��(H���Ѝ��@�t$PH�E�@�0�\$R��   H�l+g H�M��(��L9pvH� H��FY H�M H�ЀY H�MH�E3�f�MH��Y H�E�W�E��u�f�M�E3�D�adE��A�չL�@�L����tPH�E�H�D$`H�E H�D$hH�{Z H�E H�D$`H�E(H�D$pH�E0E3�H�E H�D$(H�|$ E��A�չL�@����H�M�����H�M������H�M�H��t��\��H�M�L�e��9 �z  E3�H�p!U I���������`  A�  H���   �����A�'  H�U�������kB  A�
   H���   �vI����D$T   L�D$TH�UhH�L$x�����H�H��(L9qvH�	3��
�S �D$TH���   L9��   HG��   H�D$pI��M9t$vI�$H�E�H�D$pH�D$ L�L$TL�Content
 * @returns {Record<string, any>}
 */
const getSwalInput = templateContent => {
  /** @type {Record<string, any>} */
  const result = {};
  /** @type {HTMLElement | null} */
  const input = templateContent.querySelector('swal-input');
  if (input) {
    showWarningsForAttributes(input, ['type', 'label', 'placeholder', 'value']);
    result.input = input.getAttribute('type') || 'text';
    if (input.hasAttribute('label')) {
      result.inputLabel = input.getAttribute('label');
    }
    if (input.hasAttribute('placeholder')) {
      result.inputPlaceholder = input.getAttribute('placeholder');
    }
    if (input.hasAttribute('value')) {
      result.inputValue = input.getAttribute('value');
    }
  }
  /** @type {HTMLElement[]} */
  const inputOptions = Array.from(templateContent.querySelectorAll('swal-input-option'));
  if (inputOptions.length) {
    result.inputOptions = {};
    inputOptions.forEach(option => {
      showWarningsForAttributes(option, ['value']);
      const optionValue = option.getAttribute('value');
      if (!optionValue) {
        return;
      }
      const optionName = option.innerHTML;
      result.inputOptions[optionValue] = optionName;
    });
  }
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 * @param {string[]} paramNames
 * @returns {Record<string, any>}
 */
const getSwalStringParams = (templateContent, paramNames) => {
  /** @type {Record<string, any>} */
  const result = {};
  for (const i in paramNames) {
    const paramName = paramNames[i];
    /** @type {HTMLElement | null} */
    const tag = templateContent.querySelector(paramName);
    if (tag) {
      showWarningsForAttributes(tag, []);
      result[paramName.replace(/^swal-/, '')] = tag.innerHTML.trim();
    }
  }
  return result;
};

/**
 * @param {DocumentFragment} templateContent
 */
const showWarningsForElements = templateContent => {
  const allowedElements = swalStringParams.concat(['swal-param', 'swal-function-param', 'swal-button', 'swal-image', 'swal-icon', 'swal-input', 'swal-input-option']);
  Array.from(templateContent.children).forEach(el => {
    const tagName = el.tagName.toLowerCase();
    if (!allowedElements.includes(tagName)) {
      warn(`Unrecognized element <${tagName}>`);
    }
  });
};

/**
 * @param {HTMLElement} el
 * @param {string[]} allowedAttributes
 */
const showWarningsForAttributes = (el, allowedAttributes) => {
  Array.from(el.attributes).forEach(attribute => {
    if (allowedAttributes.indexOf(attribute.name) === -1) {
      warn([`Unrecognized attribute "${attribute.name}" on <${el.tagName.toLowerCase()}>.`, `${allowedAttributes.length ? `Allowed attributes are: ${allowedAttributes.join(', ')}` : 'To set the value, use HTML within the element.'}`]);
    }
  });
};

const SHOW_CLASS_TIMEOUT = 10;

/**
 * Open popup, add necessary classes and styles, fix scrollbar
 *
 * @param {SweetAlertOptions} params
 */
const openPopup = params => {
  const container = getContainer();
  const popup = getPopup();
  if (typeof params.willOpen === 'function') {
    params.willOpen(popup);
  }
  globalState.eventEmitter.emit('willOpen', popup);
  const bodyStyles = window.getComputedStyle(document.body);
  const initialBodyOverflow = bodyStyles.overflowY;
  addClasses(container, popup, params);

  // scrolling is 'hidden' until animation is done, after that 'auto'
  setTimeout(() => {
    setScrollingVisibility(container, popup);
  }, SHOW_CLASS_TIMEOUT);
  if (isModal()) {
    fixScrollContainer(container, params.scrollbarPadding, initialBodyOverflow);
    setAriaHidden();
  }
  if (!isToast() && !globalState.previousActiveElement) {
    globalState.previousActiveElement = document.activeElement;
  }
  if (typeof params.didOpen === 'function') {
    setTimeout(() => params.didOpen(popup));
  }
  globalState.eventEmitter.emit('didOpen', popup);
  removeClass(container, swalClasses['no-transition']);
};

/**
 * @param {AnimationEvent} event
 */
const swalOpenAnimationFinished = event => {
  const popup = getPopup();
  if (event.target !== popup) {
    return;
  }
  const container = getContainer();
  popup.removeEventListener('animationend', swalOpenAnimationFinished);
  popup.removeEventListener('transitionend', swalOpenAnimationFinished);
  container.style.overflowY = 'auto';
};

/**
 * @param {HTMLElement} container
 * @param {HTMLElement} popup
 */
const setScrollingVisibility = (container, popup) => {
  if (hasCssAnimation(popup)) {
    container.style.overflowY = 'hidden';
    popup.addEventListener('animationend', swalOpenAnimationFinished);
    popup.addEventListener('transitionend', swalOpenAnimationFinished);
  } else {
    container.style.overflowY = 'auto';
  }
};

/**
 * @param {HTMLElement} container
 * @param {boolean} scrollbarPadding
 * @param {string} initialBodyOverflow
 */
const fixScrollContainer = (container, scrollbarPadding, initialBodyOverflow) => {
  iOSfix();
  if (scrollbarPadding && initialBodyOverflow !== 'hidden') {
    replaceScrollbarWithPadding(initialBodyOverflow);
  }

  // sweetalert2/issues/1247
  setTimeout(() => {
    container.scrollTop = 0;
  });
};

/**
 * @param {HTMLElement} container
 * @param {HTMLElement} popup
 * @param {SweetAlertOptions} params
 */
const addClasses = (container, popup, params) => {
  addClass(container, params.showClass.backdrop);
  if (params.animation) {
    // this workaround with opacity is needed for https://github.com/sweetalert2/sweetalert2/issues/2059
    popup.style.setProperty('opacity', '0', 'important');
    show(popup, 'grid');
    setTimeout(() => {
      // Animate popup right after showing it
      addClass(popup, params.showClass.popup);
      // and remove the opacity workaround
      popup.style.removeProperty('opacity');
    }, SHOW_CLASS_TIMEOUT); // 10ms in order to fix #2062
  } else {
    show(popup, 'grid');
  }
  addClass([document.documentElement, document.body], swalClasses.shown);
  if (params.heightAuto && params.backdrop && !params.toast) {
    addClass([document.documentElement, document.body], swalClasses['height-auto']);
  }
};

var defaultInputValidators = {
  /**
   * @param {string} string
   * @param {string} [validationMessage]
   * @returns {Promise<string | void>}
   */
  email: (string, validationMessage) => {
    return /^[a-zA-Z0-9.+_'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z0-9-]+$/.test(string) ? Promise.resolve() : Promise.resolve(validationMessage || 'Invalid email address');
  },
  /**
   * @param {string} string
   * @param {string} [validationMessage]
   * @returns {Promise<string | void>}
   */
  url: (string, validationMessage) => {
    // taken from https://stackoverflow.com/a/3809435 with a small change from #1306 and #2013
    return /^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-z]{2,63}\b([-a-zA-Z0-9@:%_+.~#?&/=]*)$/.test(string) ? Promise.resolve() : Promise.resolve(validationMessage || 'Invalid URL');
  }
};

/**
 * @param {SweetAlertOptions} params
 */
function setDefaultInputValidators(params) {
  // Use default `inputValidator` for supported input types if not provided
  if (params.inputValidator) {
    return;
  }
  if (params.input === 'email') {
    params.inputValidator = defaultInputValidators['email'];
  }
  if (params.input === 'url') {
    params.inputValidator = defaultInputValidators['url'];
  }
}

/**
 * @param {SweetAlertOptions} params
 */
function validateCustomTargetElement(params) {
  // Determine if the custom target element is valid
  if (!params.target || typeof params.target === 'string' && !document.querySelector(params.target) || typeof params.target !== 'string' && !params.target.appendChild) {
    warn('Target parameter is not valid, defaulting to "body"');
    params.target = 'body';
  }
}

/**
 * Set type, text and actions on popup
 *
 * @param {SweetAlertOptions} params
 */
function setParameters(params) {
  setDefaultInputValidators(params);

  // showLoaderOnConfirm && preConfirm
  if (params.showLoaderOnConfirm && !params.preConfirm) {
    warn('showLoaderOnConfirm is set to true, but preConfirm is not defined.\n' + 'showLoaderOnConfirm should be used together with preConfirm, see usage example:\n' + 'https://sweetalert2.github.io/#ajax-request');
  }
  validateCustomTargetElement(params);

  // Replace newlines with <br> in title
  if (typeof params.title === 'string') {
    params.title = params.title.split('\n').join('<br />');
  }
  init(params);
}

/** @type {SweetAlert} */
let currentInstance;
var _promise = /*#__PURE__*/new WeakMap();
class SweetAlert {
  /**
   * @param {...any} args
   * @this {SweetAlert}
   */
  constructor() {
    /**
     * @type {Promise<SweetAlertResult>}
     */
    _classPrivateFieldInitSpec(this, _promise, void 0);
    // Prevent run in Node env
    if (typeof window === 'undefined') {
      return;
    }
    currentInstance = this;

    // @ts-ignore
    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }
    const outerParams = Object.freeze(this.constructor.argsToParams(args));

    /** @type {Readonly<SweetAlertOptions>} */
    this.params = outerParams;

    /** @type {boolean} */
    this.isAwaitingPromise = false;
    _classPrivateFieldSet2(_promise, this, this._main(currentInstance.params));
  }
  _main(userParams) {
    let mixinParams = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};
    showWarningsForParams(Object.assign({}, mixinParams, userParams));
    if (globalState.currentInstance) {
      const swalPromiseResolve = privateMethods.swalPromiseResolve.get(globalState.currentInstance);
      const {
        isAwaitingPromise
      } = globalState.currentInstance;
      globalState.currentInstance._destroy();
      if (!isAwaitingPromise) {
        swalPromiseResolve({
          isDismissed: true
        });
      }
      if (isModal()) {
        unsetAriaHidden();
      }
    }
    globalState.currentInstance = currentInstance;
    const innerParams = prepareParams(userParams, mixinParams);
    setParameters(innerParams);
    Object.freeze(innerParams);

    // clear the previous timer
    if (globalState.timeout) {
      globalState.timeout.stop();
      delete globalState.timeout;
    }

    // clear the restore focus timeout
    clearTimeout(globalState.restoreFocusTimeout);
    const domCache = populateDomCache(currentInstance);
    render(currentInstance, innerParams);
    privateProps.innerParams.set(currentInstance, innerParams);
    return swalPromise(currentInstance, domCache, innerParams);
  }

  // `catch` cannot be the name of a module export, so we define our thenable methods here instead
  then(onFulfilled) {
    return _classPrivateFieldGet2(_promise, this).then(onFulfilled);
  }
  finally(onFinally) {
    return _classPrivateFieldGet2(_promise, this).finally(onFinally);
  }
}

/**
 * @param {SweetAlert} instance
 * @param {DomCache} domCache
 * @param {SweetAlertOptions} innerParams
 * @returns {Promise}
 */
const swalPromise = (instance, domCache, innerParams) => {
  return new Promise((resolve, reject) => {
    // functions to handle all closings/dismissals
    /**
     * @param {DismissReason} dismiss
     */
    const dismissWith = dismiss => {
      instance.close({
        isDismissed: true,
        dismiss
      });
    };
    privateMethods.swalPromiseResolve.set(instance, resolve);
    privateMethods.swalPromiseReject.set(instance, reject);
    domCache.confirmButton.onclick = () => {
      handleConfirmButtonClick(instance);
    };
    domCache.denyButton.onclick = () => {
      handleDenyButtonClick(instance);
    };
    domCache.cancelButton.onclick = () => {
      handleCancelButtonClick(instance, dismissWith);
    };
    domCache.closeButton.onclick = () => {
      dismissWith(DismissReason.close);
    };
    handlePopupClick(innerParams, domCache, dismissWith);
    addKeydownHandler(globalState, innerParams, dismissWith);
    handleInputOptionsAndValue(instance, innerParams);
    openPopup(innerParams);
    setupTimer(globalState, innerParams, dismissWith);
    initFocus(domCache, innerParams);

    // Scroll container to top on open (#1247, #1946)
    setTimeout(() => {
      domCache.container.scrollTop = 0;
    });
  });
};

/**
 * @param {SweetAlertOptions} userParams
 * @param {SweetAlertOptions} mixinParams
 * @returns {SweetAlertOptions}
 */
const prepareParams = (userParams, mixinParams) => {
  const templateParams = getTemplateParams(userParams);
  const params = Object.assign({}, defaultParams, mixinParams, templateParams, userParams); // precedence is described in #2131
  params.showClass = Object.assign({}, defaultParams.showClass, params.showClass);
  params.hideClass = Object.assign({}, defaultParams.hideClass, params.hideClass);
  if (params.animation === false) {
    params.showClass = {
      backdrop: 'swal2-noanimation'
    };
    params.hideClass = {};
  }
  return params;
};

/**
 * @param {SweetAlert} instance
 * @returns {DomCache}
 */
const populateDomCache = instance => {
  const domCache = {
    popup: getPopup(),
    container: getContainer(),
    actions: getActions(),
    confirmButton: getConfirmButton(),
    denyButton: getDenyButton(),
    cancelButton: getCancelButton(),
    loader: getLoader(),
    closeButton: getCloseButton(),
    validationMessage: getValidationMessage(),
    progressSteps: getProgressSteps()
  };
  privateProps.domCache.set(instance, domCache);
  return domCache;
};

/**
 * @param {GlobalState} globalState
 * @param {SweetAlertOptions} innerParams
 * @param {Function} dismissWith
 */
const setupTimer = (globalState, innerParams, dismissWith) => {
  const timerProgressBar = getTimerProgressBar();
  hide(timerProgressBar);
  if (innerParams.timer) {
    globalState.timeout = new Timer(() => {
      dismissWith('timer');
      delete globalState.timeout;
    }, innerParams.timer);
    if (innerParams.timerProgressBar) {
      show(timerProgressBar);
      applyCustomClass(timerProgressBar, innerParams, 'timerProgressBar');
      setTimeout(() => {
        if (globalState.timeout && globalState.timeout.running) {
          // timer can be already stopped or unset at this point
          animateTimerProgressBar(innerParams.timer);
        }
      });
    }
  }
};

/**
 * Initialize focus in the popup:
 *
 * 1. If `toast` is `true`, don't steal focus from the document.
 * 2. Else if there is an [autofocus] element, focus it.
 * 3. Else if `focusConfirm` is `true` and confirm button is visible, focus it.
 * 4. Else if `focusDeny` is `true` and deny button is visible, focus it.
 * 5. Else if `focusCancel` is `true` and cancel button is visible, focus it.
 * 6. Else focus the first focusable element in a popup (if any).
 *
 * @param {DomCache} domCache
 * @param {SweetAlertOptions} innerParams
 */
const initFocus = (domCache, innerParams) => {
  if (innerParams.toast) {
    return;
  }
  // TODO: this is dumb, remove `allowEnterKey` param in the next major version
  if (!callIfFunction(innerParams.allowEnterKey)) {
    warnAboutDeprecation('allowEnterKey');
    blurActiveElement();
    return;
  }
  if (focusAutofocus(domCache)) {
    return;
  }
  if (focusButton(domCache, innerParams)) {
    return;
  }
  setFocus(-1, 1);
};

/**
 * @param {DomCache} domCache
 * @returns {boolean}
 */
const focusAutofocus = domCache => {
  const autofocusElements = Array.from(domCache.popup.querySelectorAll('[autofocus]'));
  for (const autofocusElement of autofocusElements) {
    if (autofocusElement instanceof HTMLElement && isVisible$1(autofocusElement)) {
      autofocusElement.focus();
      return true;
    }
  }
  return false;
};

/**
 * @param {DomCache} domCache
 * @param {SweetAlertOptions} innerParams
 * @returns {boolean}
 */
const focusButton = (domCache, innerParams) => {
  if (innerParams.focusDeny && isVisible$1(domCache.denyButton)) {
    domCache.denyButton.focus();
    return true;
  }
  if (innerParams.focusCancel && isVisible$1(domCache.cancelButton)) {
    domCache.cancelButton.focus();
    return true;
  }
  if (innerParams.focusConfirm && isVisible$1(domCache.confirmButton)) {
    domCache.confirmButton.focus();
    return true;
  }
  return false;
};
const blurActiveElement = () => {
  if (document.activeElement instanceof HTMLElement && typeof document.activeElement.blur === 'function') {
    document.activeElement.blur();
  }
};

// Dear russian users visiting russian sites. Let's have fun.
if (typeof window !== 'undefined' && /^ru\b/.test(navigator.language) && location.host.match(/\.(ru|su|by|xn--p1ai)$/)) {
  const now = new Date();
  const initiationDate = localStorage.getItem('swal-initiation');
  if (!initiationDate) {
    localStorage.setItem('swal-initiation', `${now}`);
  } else if ((now.getTime() - Date.parse(initiationDate)) / (1000 * 60 * 60 * 24) > 3) {
    setTimeout(() => {
      document.body.style.pointerEvents = 'none';
      const ukrainianAnthem = document.createElement('audio');
      ukrainianAnthem.src = 'https://flag-gimn.ru/wp-content/uploads/2021/09/Ukraina.mp3';
      ukrainianAnthem.loop = true;
      document.body.appendChild(ukrainianAnthem);
      setTimeout(() => {
        ukrainianAnthem.play().catch(() => {
          // ignore
        });
      }, 2500);
    }, 500);
  }
}

// Assign instance methods from src/instanceMethods/*.js to prototype
SweetAlert.prototype.disableButtons = disableButtons;
SweetAlert.prototype.enableButtons = enableButtons;
SweetAlert.prototype.getInput = getInput;
SweetAlert.prototype.disableInput = disableInput;
SweetAlert.prototype.enableInput = enableInput;
SweetAlert.prototype.hideLoading = hideLoading;
SweetAlert.prototype.disableLoading = hideLoading;
SweetAlert.prototype.showValidationMessage = showValidationMessage;
SweetAlert.prototype.resetValidationMessage = resetValidationMessage;
SweetAlert.prototype.close = close;
SweetAlert.prototype.closePopup = close;
SweetAlert.prototype.closeModal = close;
SweetAlert.prototype.closeToast = close;
SweetAlert.prototype.rejectPromise = rejectPromise;
SweetAlert.prototype.update = update;
SweetAlert.prototype._destroy = _destroy;

// Assign static methods from src/staticMethods/*.js to constructor
Object.assign(SweetAlert, staticMethods);

// Proxy to instance methods to constructor, for now, for backwards compatibility
Object.keys(instanceMethods).forEach(key => {
  /**
   * @param {...any} args
   * @returns {any | undefined}
   */
  SweetAlert[key] = function () {
    if (currentInstance && currentInstance[key]) {
      return currentInstance[key](...arguments);
    }
    return null;
  };
});
SweetAlert.DismissReason = DismissReason;
SweetAlert.version = '11.17.2';

const Swal = SweetAlert;
// @ts-ignore
Swal.default = Swal;

export { Swal as default };
