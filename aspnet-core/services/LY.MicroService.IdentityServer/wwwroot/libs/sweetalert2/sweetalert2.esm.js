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
// readerâ€™s list of elements (headings, form controls, landmarks, etc.) in the document.

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
const updatableParams = ['allowEscapeKey', 'allowOutsideClick', 'background', 'buttonsStyling', 'cancelButtonAriaLabel', 'cancelButtonColor', 'cancelButtonText', 'closeButtonAriaLabel', 'closeButtonHtml', 'color', 'confirmButtonAriaLabel', 'confirmButtonColor', 'confirmButtonText', 'currentProgressStep', 'customClass', 'denyButtonAriaLabel', 'denyButtonColor', 'denyButtonText', 'didClose', 'didDestroy', 'draggable', 'footer', 'hideClass', 'html', 'icon', 'iconColor', 'iconHtml', 'imageAlt', 'imageHeight', 'imageUrl', 'imageWidth', 'preConfirm', 'preDeny', 'progressSteps', 'returnFocus', 'reverseButtons', 'showCancelButton', 'showCloseButton', 'showConfirmBut\$0H‹ÇHƒÄ _ÃH‰\$W¸    èlÿÿH+àH‹ù‹ÚHƒÁèÛLÜÿöÃtH‹ÏèšÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    èPlÿÿH+àH‹ù‹ÚHƒÁèÃLÜÿöÃtH‹ÏèZÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    èlÿÿH+à‹ÚH‹ùè+ğÿöÃtH‹ÏèÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    èĞkÿÿH+à‹ÚH‹ùèË ùÿöÃtH‹ÏèŞÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è˜kÿÿH+àHyà‹ÚH‹ÏèKıÿÿöÃtH‹Ïè¢ÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è\kÿÿH+àH¹xÿÿÿ‹ÚH‹ÏèH„àÿöÃtH‹ÏècÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    èkÿÿH+à‹ÚH‹ùè#…àÿöÃtH‹ÏèÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    èĞjÿÿH+à‹ÚH‹ùè;ùÿÿHl X H‰G`öÃtH‹ÏèÓÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è€jÿÿH+à‹ÚH‹ùè?ùÿÿH X H‰G`öÃtH‹ÏèƒÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è0jÿÿH+à‹ÚH‹ùèCùÿÿHÌÿW H‰G`öÃtH‹Ïè3ÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    èàiÿÿH+à‹ÚH‹ùèGùÿÿöÃtH‹ÏèîÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è iÿÿH+à‹ÚH‹ùèSùÿÿöÃtH‹Ïè®ÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è`iÿÿH+à‹ÚH‹ùèëwæÿöÃtH‹ÏènÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è iÿÿH+à‹ÚH‹ùè¿GÛÿöÃtH‹Ïè.ÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    èàhÿÿH+à‹ÚH‹ùèOşÿöÃtH‹ÏèîÕÿH‹\$0H‹ÇHƒÄ _Ã@S¸    è¤hÿÿH+àHJşW H‹ÙH‰öÂtè²ÕÿH‹ÃHƒÄ [ÃH‰\$W¸    èphÿÿH+à‹ÚH‹ùè³øÿÿöÃtH‹Ïè~ÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$W¸    è8hÿÿH+à‹ÚH‹ùè›„ÙÿöÃtH‹ÏèFÕÿH‹\$0H‹ÇHƒÄ _ÃH‰\$UVWATAVHl$É¸à   èõgÿÿH+àH‹{¯Œ H3ÄH‰E'H‹òH‹ùH‹H‹˜¨   HQHMèÃ¥ïÿL5¶Y L‰uÏHDkY H‰M×WÀEßWÉóMï EßHMïHÇ@   E3äL‰`fD‰ fD‰eÿHUÏH‹ÎH‹ÃÿÊT HMßèe0ÖÿHMè[0ÖÿH‹Hı6U H‰L$ HkY H‰L$(‹O(‰M‡fD‰e‹HT$ H‹ÎH‹€¸   ÿ|T H‹H‹˜¨   LG0HïjY HM—èşóÿH‹ĞH‹ÎH‹ÃÿNT HM§èè/ÖÿH‹H‹˜¨   HWPHL$ èÈ¤ïÿL‰u—HÀjY H‰MŸWÀE§WÉóM· E§HM·L‰`HÇ@   fD‰ fD‰eÇHU—H‹ÎH‹ÃÿÙT HM§èt/ÖÿHL$ èj/ÖÿH‹M'H3Ìè
YÿÿH‹œ$   HÄà   A^A\_^]Ã¸(   è6fÿÿH+à±HƒÄ(ëÌH‹ÄH‰XH‰pH‰xL‰` AUAVAW¸ğ
  èfÿÿH+à)´$à
  H‹ƒ­Œ H3ÄH‰„$Ğ
  @Šñ3ÿ‰|$pÿaT D‹øHY H‰„$  HPØY H‰„$˜  H‘ig H‰„$   f‰¼$¨  HŠâY H‰„$à   WÀ„$è   fo55U ó´$ø   f‰¼$è   E3É_2D‹Ãº°  A½ÅpA‹ÍèeÜÿL5ãqZ L%Tig „ÀtaH„$à   H‰„$    H„$  H‰„$¨   L‰t$@H„$    H‰D$HH„$°   H‰D$PE3ÉHD$@H‰D$(L‰d$ D‹Ãº°  A‹ÍèOÔÿHŒ$è   èÖ-ÖÿE3ÀHˆğÿHauy ÿ;üS H‰J‘ 3ÒJA¸¸  èÄlıÿA½   D‰-÷I‘ ‰=J‘ WÀ„$à  ó´$ğ  f‰¼$à  è˜kÖÿH‹ØH¾hg HŒ$0  è=.ÖÿÇD$p   L„$à  H”$0  H‹Ëè —Öÿ„ÀtE3ÀH–hg HŒ$à  èUCİÿ…À³t@ŠßHŒ$0  è-Öÿ„ÛtZH|hg HŒ$  èÓ-ÖÿH×hg HŒ$0  è¾-ÖÿH”$  HŒ$0  è´ğ HŒ$0  èº,ÖÿHŒ$  è­,ÖÿWÀ„$    „$°   E3ÉE3ÀAQ3ÉÿÄT H‰ÕH‘ H…Àu6ÿ‚T D‹ÀL˜hg º   HŒ$°  èZÈÿÿHo¦~ HŒ$°  è+ÿÿE3À3ÒJèUkıÿI‹Íèı-ÖÿH…ÀtH™hg H‰Hwx@ H‰HëH‹ÇH‰„$€   HŒ$  H‰L$(‰|$ L‹ÈLKÎÿÿ3Ò3Éÿ‘T H‰„$  H…À„€  H”$  HŒ$    è½ëÿ9¼$  tÿdT Ì»   ‹Ëèo-ÖÿH…ÀtHÓ  H‰ëH‹ÇH‰„$€   A½   HŒ$  H‰L$(‰|$ L‹ÈLÎÿÿ3Ò3ÉÿT H‰„$  H…À„ä  H”$  HŒ$°   è–¼ëÿ9¼$  tÿÛT Ì@„ö„…  H‹Ëèá,ÖÿH…ÀtHµäb H‰ëH‹ÇH~ky H‹ÈèŞ ıÿ‹ØA½2   …À…7  HšY H‰„$  HXÔY H‰„$˜  Higg H‰„$   f‰¼$¨  H’ŞY H‰„$à   WÀ„$è   ó´$ø   f‰¼$è   E3ÉE‹Åº°  ¾Ë3p#‹Îè³aÜÿ„Àt`H„$à   H‰„$  H„$  H‰„$  L‰t$@H„$  H‰D$HH„$   H‰D$PE3ÉHD$@H‰D$(L‰d$ E‹Åº°  ‹Îès	ÔÿHŒ$è   èú)ÖÿÆ<F‘ ÿqT A+Ç‹ĞHŒ$  èoÕÿH‹ğH=gg HŒ$0  è¤*ÖÿH‰t$0L„$0  HŒ$   èzÉÿÿL=ÿ2X L‰¼$   H€İY H‰„$à   WÀ„$è   ó´$ø   f‰¼$è   E3ÉE‹Åº°  ¾¸8#‹Îè¡`Üÿ„ÀtiH„$à   H‰„$  H„$   H‰„$  L‰t$@H„$  H‰D$HH„$   H‰D$PE3ÉHD$@H‰D$(L5¾fg L‰t$ E‹Åº°  ‹ÎèZÔÿëL5¡fg HŒ$è   èØ(ÖÿHŒ$   èŸŸôÿHŒ$0  è¾(ÖÿHŒ$  è°(Öÿé•  L= 2X L5Yfg é2	  HL$`èîîÚÿ„ÀtlH–’ H¸“ èk{Ûÿ@8|$`u*„ÀtNHqfg H‰D$ º   ¹äŒ DB$èÑ`Üÿè‘@ ë(„Àu$H·fg H‰D$ º   ¹äŒ DB$è§`Üÿè‡@ H£bg ÿE T f…Àu1A´Hîfg H‰D$ º   ¹Úåm DB$èn`ÜÿHobg ÿ	 T ëDŠçWÀ„$P  H‰¼$`  HÇ„$h     f‰¼$P  èĞeÖÿH‹ØH¦İU HŒ$0  èu(ÖÿL„$P  H”$0  H‹Ëè`‘ÖÿHŒ$0  èn'Öÿ¹   è0)ÖÿH…ÀtHáb H‰ëH‹ÇH/U H‹Èè-ıÿ‹Ø…À…ş  Hl–Y H‰„$  H­ĞY H‰„$˜  H~fg H‰„$   f‰¼$¨  HçÚY H‰„$à   WÀ„$è   ó´$ø   f‰¼$è   E3Éº°  ¾ÇpDC2‹Îè^Üÿ„ÀthH„$à   H‰„$  H„$  H‰„$  L‰t$@H„$  H‰D$HH„$   H‰D$PE3ÉHD$@H‰D$(Htag H‰D$ º°  DC2‹Îè¿ÔÿHŒ$è   èF&ÖÿÆˆB‘ ÿ½ÿS A+Ç‹ĞHŒ$  èckÕÿH‹ğH‰cg HŒ$0  èğ&ÖÿH‰t$0L„$0  HŒ$   è‚ÄÿÿL=K/X L‰¼$   HÌÙY H‰„$à   WÀ„$è   ó´$ø   f‰¼$è   E3ÉAq2D‹Æº°  ¹¸8#èë\Üÿ„ÀtlH„$à   H‰„$  H„$   H‰„$  L‰t$@H„$  H‰D$HH„$   H‰D$PE3ÉHD$@H‰D$(L5cg L‰t$ D‹Æº°  ¹¸8#è¡ÔÿëL5èbg HŒ$è   è%ÖÿHŒ$   èæ›ôÿHŒ$0  è%ÖÿHŒ$  è÷$ÖÿE„ät,H9¼$`  u"H”$p  H|’ èT+ßÿA½   H98@¶t@Š÷AöÅtH‹Œ$x  H…Étè«Öÿ@„ö„]  H‡-X ÿÙşS H…À„G  HQdg H‹Èÿ˜şS H‹ğH…À„&  @ˆ|$aA¼   fD‰d$pÆD$r@ˆ|$y@ˆ|${@ˆ|$iHD$`H‰D$(HD$pH‰D$ LL$xLD$zHT$hHŒ$€   èã'òÿHğcg H‰L$pHT$pHL$@è¼âûÿL„$€   H‹ĞHŒ$  è $Üÿ‰|$hHL$hH‹Æÿ1T ‹ğ…Àt9|$htè8  HŒ$  èÏ0ÜÿE‹ÌD‹D$hH”cg H‹ÈèPïïÿÿ¦ıS E3ÉLülf ‹ĞHŒ$   èáÿ…ö•ÂL„$   HŒ$  èß/ÜÿHŒ$  èJ8ÜÿHŒ$  è AÜÿëè¹  èø÷ùÿèW  ëL=,X L5ç`g HŒ$P  è#Öÿ…Û…ª  H‹c?‘ H‰„$€   A½`ê  A‹õWÀ„$P  H‰¼$`  HÇ„$h     f‰¼$P  H”$ˆ   H”y’ èÊÓÿH‹ÇD$p   L„$P  HT$pè¾uÖÿDŠàH‹Œ$   H…Étèñ¨ÖÿE„ät.HŒ$P  Hƒ¼$h  HGŒ$P  3ÒDB
ÿjT ‹ğ…ÀADõÇD$ ÿ  D‹ÎE3ÀH”$€   AHÿÊÑ› …À„  ƒø„Ñ   =  t?ƒøÿ…ã   ÿ#üS D‹ÀL¡mg º   HŒ$@  èİşÿHœ~ HŒ$@  èÌ‚ÿÿHÜag H‰D$ º   ¹ÅE DBVèLZÜÿèûïÿH‹ğH„$p  H‰D$hWÀó„$p  ènûïÿL_m HT$@H‹ÈèşïÿL„$p  H‹ĞH‹ÎèòBßÿë>HŒ$à   ÿşĞ› HŒ$à   ÿ Ñ› ÇD$    E3ÉE3À3ÒHŒ$à   ÿÚĞ› …ÀuÂHŒ$P  è-!Öÿé'şÿÿHag H‰D$ A½2   E‹ÅAUÜ¹ÆE è‰YÜÿHŒ$P  èø Öÿ…Û…Š  H5}dZ Hbg HŒ$0  èµ!ÖÿL‰¼$  H‰¼$˜  WÀf„$   Æ„$°  WÉŒ$¸  H‰¼$È  HÇ„$Ğ     f‰¼$¸  fD$@HT$@HŒ$˜  èµ§éÿÆ„$°  H”$0  HŒ$¸  èÚÕÿL‰¼$  H0ÔY H‰„$à   WÀ„$è   ó´$ø   f‰¼$è   E3ÉE‹ÅA¼°  A‹ÔA¿¸"A‹ÏèKWÜÿ„Àt_H„$à   H‰„$  H„$  H‰„$  H‰t$@H„$  H‰D$HH„$   H‰D$PE3ÉHD$@H‰D$(L‰t$ E‹ÅA‹ÔA‹ÏèÿÓÿHŒ$è   è“ÖÿHŒ$  èZ–ôÿHŒ$0  èyÖÿ3Éè.½ıÿé5  A½2   H›Y H‰„$  HÜÈY H‰„$˜  HE`g H‰„$   f‰¼$¨  WÀ„$0  ó´$@  f‰¼$0  „$  ó´$   f‰¼$  „$p  ó´$€  f‰¼$p  „$P  ó´$`  f‰¼$P  ÿĞøS ‰D$pH„$0  H‰D$(H„$  H‰D$ LŒ$p  L„$P  HT$pHŒ$   è˜ÁùÿH‹ğHfÒY H‰„$à   WÀ„$è   ó´$ø   f‰¼$è   E3ÉEA
A¼®  A‹Ô¹Èpè„UÜÿ„ÀtwH„$à   H‰D$@H‰t$HH„$  H‰D$PH5¦aZ H‰´$ˆ   HD$@H‰„$   HD$XH‰„$˜   E3ÉH„$ˆ   H‰D$(HåXg H‰D$ EA
A‹Ô¹Èpè/ıÓÿëH5NaZ HŒ$è   è­ÖÿHŒ$   è,œùÿHŒ$P  è“ÖÿHŒ$p  è†ÖÿHŒ$  èyÖÿHŒ$0  èlÖÿé~üÿÿ‰¼$  ¹   ÿ±ûS ‰¼$  ¹   ÿûS ëë 3ÿ‹\$h9¼$¨   tHŒ$    è‰ôÿ9¼$¸   tHŒ$°   èsôÿE3À‹ÓAHè\ıÿHŒ$°   è§ôÿHŒ$    èšôÿHŒ$à  èÕÖÿH‹Œ$Ğ
  H3ÌèqFÿÿLœ$ğ
  I‹[ I‹s(I‹{0M‹c8A(sğI‹ãA_A^A]ÃHieg ÃH‰\$H‰t$W¸   èsSÿÿH+àH‹ùšŒ H3ÄH‰„$  3ö‹Ş‰\$4HL$Pè ×ÔÿH”$Ø   H‹èË úÿH‹L$XH…Étè¢ÖÿH9´$è   „$  A°H##U HŒ$Ø   è>2İÿ…À„  @ˆt$0HL$0èSâÚÿ„Àt*@8t$0t#HL$8è®ÖÔÿ»   ‰\$4H‹èœË@ „À@Šût@ŠşöÃtH‹L$@H…Étè¢Öÿ@„ÿ„ò   HÚŠY H‰D$pHÅY H‰D$xHR_g H‰„$€   f‰´$ˆ   H[ÏY H‰„$   WÀ„$˜   foP"U óŒ$¨   f‰´$˜   E3ÉAY2D‹Ã¿°  ‹×¹‰ÓèpRÜÿ„Àt_H„$   H‰D$PHD$pH‰D$XHš^Z H‰D$8HD$PH‰D$@HD$`H‰D$HE3ÉHD$8H‰D$(H_g H‰D$ D‹Ã‹×¹‰Óè1úÓÿHŒ$˜   è¸Öÿé¶   HL$pè/ğÿH‹E# ÇD$4	   LD$4HT$8HL$pèMëÿH‹HƒÁ(H‹ÓèÂĞÕÿH‹›& ÇD$4   LD$4HT$8HL$pèëÿH‹HƒÁ(H‹ÓèĞÕÿH©dU HŒ$¸   èÖÿLD$pH”$¸   HGq’ èryüÿHŒ$¸   èÖÿHL$pèÊğÿHŒ$Ø   èğÖÿë H‹Œ$  H3ÌèŠCÿÿLœ$   I‹[I‹sI‹ã_ÃH‰\$W¸0  è°PÿÿH+àH‹6˜Œ H3ÄH‰„$   HL$PèeNÕÿH‹L$P3ÿH…ÉuH‹L$XH…ÉtèÚŸÖÿéj  H”$  èÀúÿH”$p  H‹L$PèV¡÷ÿ@ŠßHX[g HŒ$¨   èÖÿL„$¨   H”$  HŒ$P  è>_ÛÿHŒ$¨   èÖÿE3ÀH§·U HŒ$p  è2/İÿ…ÀuVH'[g HL$`èÉÖÿLD$`H”$  HŒ$  èê^ÛÿH‹ĞHŒ$P  è‚ÒÕÿHŒ$  è±ÖÿHL$`è¦Öÿ³HAxY H‰„$Ğ   ‰¼$Ø   HkU H‰„$Ğ   WÀ„$à   fohU fŒ$ğ   f‰¼$à   H‰¼$   Æ„$  Ç„$  …   HıU HZg „ÛHDÁH‰D$`LD$`H—Zg HŒ$0  èÊ
Ûÿ@ˆ|$(‰|$ A±L„$0  H”$Ğ   H§“ èªiÕÿ„À„y  E3ÀH”$P  HŒ$Ğ   èêZßÿ¶ÓHŒ$°  èÚ]ÕÿH‹ØHØZg HŒ$  èÖÿH„$0  H‰D$@H‰\$0H„$p  H‰D$ L„$  HŒ$Ğ  è´ÿÿHÀ X H‰„$Ğ  HAËY H‰„$€   WÀ„$ˆ   fo6U óŒ$˜   f‰¼$ˆ   E3ÉAY2D‹Ãº°  ¹Õ“PèXNÜÿ„ÀtqH„$€   H‰D$`H„$Ğ  H‰D$hHZZ H‰„$¨   HD$`H‰„$°   HD$pH‰„$¸   E3ÉH„$¨   H‰D$(HZg H‰D$ D‹Ãº°  ¹Õ“PèöÓÿHŒ$ˆ   èÖÿHŒ$Ğ  èUôÿHŒ$  ètÖÿHŒ$°  èfÖÿHŒ$0  èXÖÿHŒ$Ğ   èÏiÕÿ€    HŒ$P  è7ÖÿHŒ$p  è*ÖÿHŒ$  èÖÿH‹L$XH…ÉtènœÖÿë H‹Œ$   H3Ìè§?ÿÿH‹œ$@  HÄ0  _ÃH‰\$H‰t$W¸P  èËLÿÿH+àH‹Q”Œ H3ÄH‰„$@  ¹`   eH‹%X   H‹ ‹9¼“ ÷   H¯“ èNCÿÿƒ=£“ ÿ…Ş   HFZg HŒ$€  èMÖÿHZZg HŒ$   è9ÖÿHnZg HŒ$À  è%ÖÿHŠZg HŒ$à  èÖÿH¦Zg HŒ$   èıÖÿH„$€  H‰„$   H„$   H‰„$˜   („$   f„$   H”$   H“ èlBÕÿLgñÿº    DBåHŒ$€  èAÿÿH ªS è£@ÿÿHÄ“ è÷AÿÿWÀ„$`  foŒU óŒ$p  3ÿf‰¼$`  3ÒHŒ$ğ   èûÿH‹ĞHŒ$`  è"ÎÕÿHŒ$ğ   èQÖÿH‹n“ H‹5o“ H‰t$PH‰\$HH;Ş„Ğ  H‹ÓHŒ$Ğ   è<PôÿL„$Ğ   H”$`  HŒ$   è§ğåÿH‹ĞHŒ$°   è_(ÖÿHŒ$   èæÖÿHŒ$Ğ   èÙÖÿHŒ$°   è¤ôåÿ„À„N  HŒ$°   èàôÿH„Zg HŒ$ğ   èƒÖÿH„$°   H‰D$ L“\f L„$ğ   HŒ$  è*5åÿHÏX H‰„$  HPÇY H‰D$hWÀD$pH‰¼$€   HÇ„$ˆ      f‰|$pE3Éº°  ¹¢“iEA2èpJÜÿ„ÀtoHD$hH‰„$   H„$  H‰„$˜   H”VZ H‰D$0H„$   H‰D$8H„$    H‰D$@E3ÉHD$0H‰D$(H9Yg H‰D$ º°  ¹¢“iEA2è!òÓÿHL$pè«ÖÿHŒ$  èr‰ôÿHŒ$ğ   è‘Öÿ@ „     ë3ÿH‹\$HH‹t$PHŒ$°   èiÖÿHƒÃ é"şÿÿHŒ$`  èSÖÿH‹Œ$@  H3Ìèï;ÿÿLœ$P  I‹[I‹sI‹ã_Ã¸(   èIÿÿH+à3ÉHƒÄ(éàâÿÿÌH‰\$H‰t$W¸P   èïHÿÿH+à‹òH‹ÙH`[f H‰D$ º   ¹EÒ= DB$èPJÜÿH‹C(H‰D$0ÇD$8“   H…Àtğÿ@HL$0èlßÿH‹Èè8YäÿHL$0èá)ßÿH‹KH…Ét
ºˆ  èPüÿH“¨   HL$@è™êÕÿ@ „     H{H‹H…ÉtLèƒ–ùÿƒÈÿ…öDğ‹ÖH‹èé53 WÀóD$0HT$0H‹ÏèçbÖÿH‹L$8H…Étè|—ÖÿH‹H‹ËH‹@@ÿhüS H‹˜   3ÒèâxäÿHL$@èkíÕÿH‹\$`H‹t$hHƒÄP_Ã@S¸   èÄGÿÿH+àH‹JŒ H3ÄH‰„$€   H‹ÚH-Qy HL$`èÖÿH‹ÓHL$@èqÖÿE3ÀHT$`HL$@è6ËÕÿŠØHL$@èrÖÿHL$`èhÖÿHıWy H‰D$ HáÿÿH*şÿÿ„ÛHDÈH‰L$(Hƒd$0 Hƒd$8 HL$ ÿ ŞS …Àuÿ.êS ë3ÀH‹Œ$€   H3Ìèº9ÿÿHÄ   [Ã@S¸    èôFÿÿH+àH‹QH‹ÙH‹H‰Aè*ÛÿH‹HÿHHƒÄ [Ã¸h   èÆFÿÿH+àH‹ÁHÁè …Àt<‰D$ HT$ HpşŒ H‰D$(HL$0(D$ fD$ èêÓÿÿHo~ HL$0èfpÿÿÌ‹ÁHƒÄhÃH‰\$W¸    èdFÿÿH+àH¸ÇqÇqÇH‹ÙH;Ğvè®øÓÿÌè”µøÿH‹H‹øH…Ét@H‹SèP±ÿÿH‹SH¸9ã8ã8H+H‹HÁúH¯ĞHÒHÁâè·™ÕÿHƒ# Hƒc Hƒc H‹Ïèñ7ÙÿH‹ÈèšÕÿH‰HÿH‰CHÁáHÈH‰KH‹\$0HƒÄ _Ã¸H   è¾EÿÿH+àWÀH9~U D$(H‰D$(H‹~ H9~U HL$ H‰D$ èsoÿÿÌ@S¸ğ   è|EÿÿH+à)´$à   I‹Ø2H‹ÑHL$0èÒ„Úÿót$ LL$ L‹ÃHT$0HL$Pè ,üÿH~ HL$PèoÿÿÌ¸h   è"EÿÿH+à‰L$ HÌüŒ H‰D$(HT$ (D$ HL$0fD$ èQÒÿÿHÖŒ~ HL$0èÍnÿÿÌH‰\$H‰L$UVWATAUAVAW¸@   èÁDÿÿH+àI‹øH‹êH‹ÙI;Ğ„€   L‹bHAL‹8L‹ÚH‹qH‰T$0HƒÂH‰D$ L‰d$(è°ÕÿL‹ğL#s0MöJ‹DöN‹,öH‰„$ˆ   L;ØHL$ ”ÃèWıÿÿ„ÛuBL‹\$0H‹„$ˆ   L;ßuÚL;íuN‰öH‹D$(L‰I‰CH‹œ$   H‹ÇHƒÄ@A_A^A]A\_^]ÃL;íuN‰<öM‹çN‰döH‹¬$€   L‹\$0L;ßt¶ISèu¯ÕÿL‹ğL#u0MöN‹döM;ÜHL$ ”ÃèÆüÿÿ„ÛuL‹\$0L;ßuâéwÿÿÿN‰<öN‰|öë°Ì¸(   èšCÿÿH+àHˆUg ÿbñS ÌH‰\$W¸0   èxCÿÿH+à€y  H‹úH‹ÙtLA(ëM…ÀtHT$ H‹Ïÿ êS HL$ è¶_ÙÿHƒ;ÿtH‹H‹ÏÿéS H‹SHƒúÿt	H‹ÏÿÉéS ŠS„Òt	H‹Ïÿ±éS ‹SH‹ÏÿéS ‹SE3ÀH‹ÏÿÆèS ‹SH‹ÏH‹\$@HƒÄ0_Hÿ%oéS ÌH‰\$H‰l$H‰t$W¸    è¶BÿÿH+àH‹AH‹ÙH+I‹èHÁøH¹9ã8ã8H¯ÁH‹òH;ĞvH‹ËèüÿÿH‹ë&H‹SH‹úH+;HÁÿH¯ùH‹H;÷vèÜ²ÿÿH‹KH+÷H‹ÖL‹ÅL‹Ëè»±ÿÿH‹øëH<öHÁçH;H‹×è¯²ÿÿH‹SH‹ÏèG­ÿÿH‰{H‹\$0H‹l$8H‹t$@HƒÄ _ÃH‰\$H‰l$H‰t$W¸    èòAÿÿH+àL‹3ÿH‹AH½9ã8ã8I+ÂH‹ÙHÁø‹÷H¯ÅH…ÀteD‹ÏH9{0t*KÉHÀA9<Â|IcÂH‹SL‹À$IÁè¶ÈB‹‚£ÈrKÉIƒÂHÁá3ÒIÊèşÔÿL‹ÿÆH‹CI+ÂD‹ÎHÁøH¯ÅL;Èr‰{<@ˆ{DH9{0t.9{@~)H‹SHcÇL‹À$IÁè¶ÈB‹‚£ÈsG‹ø‰C<;C@|×H‹l$8H‹ÃH‹\$0H‹t$@HƒÄ _ÃH‰\$H‰|$UH‹ì¸p   èô@ÿÿH+àH‹Ùÿ çS H‹ËH‹øÿdçS H…ÿt:H;øt53ÀLEÀH‰EğHUàH‹WÀ)EÀA¹   òEğH‹ËòEĞH‹@XÿıôS H‹ËÿçS H‹ËH‹øÿøæS H…ÿt:H;øt53ÀLEÀH‰EğHUàH‹WÀ)EÀA¹   òEğH‹ËòEĞH‹@Xÿ©ôS L\$pI‹[I‹{I‹ã]ÃHcAüH+ÈëH‰\$W¸    è@ÿÿH+àH‹ùHHèÖ
ÖÿH‰D$83ÛH…ÀtHW H‹ÈèÊÃÿÿH‹ĞëH‹ÓH…ÒtH‹BPHcXHƒÃPHÚH‹ÃH‹\$0HƒÄ _ÃHcAüH+ÈëH‰\$W¸    è¤?ÿÿH+àH‹ùHHèf
ÖÿH‰D$83ÛH…ÀtHW H‹Èè2ÅÿÿH‹ĞëH‹ÓH…ÒtH‹BPHcXHƒÃPHÚH‹ÃH‹\$0HƒÄ _ÃHcAüH+ÈëH‰\$W¸    è4?ÿÿH+àH‹ùHHèö	ÖÿH‰D$83ÛH…ÀtHW H‹Èè¢ÆÿÿH‹ĞëH‹ÓH…ÒtH‹BPHcXHƒÃPHÚH‹ÃH‹\$0HƒÄ _Ã@S¸    èØ>ÿÿH+àH‹ÙHƒÁè…[ÙÿÆHƒÄ [Ã@S¸    è´>ÿÿH+àH‹ÂH‹ÙH‹ÈHd èL‹ÛÿöØHKHÀH#ÁHƒÄ [Ã¸(   èv>ÿÿH+àH‹ÊHÙc è‹Ûÿ3ÀHƒÄ(ÃH‰\$H‰l$ VWAV¸Ğ   èD>ÿÿH+àH‹Ê…Œ H3ÄH‰„$À   H‹òL‹ñHT$ è7CÙÿH‹Èè‡Àÿÿ² H‹ÈÿüäS @ŠèHL$ èoZÙÿI‹NI+HÁùI¹9ã8ã8I¯ÉH…Éu*@ŠÕHL$0èÉÿÿL‹ÀH‹ÖI‹ÎèöúÿÿHL$0èÌÍÿÿé  H;ñ†   @ŠÕHL$0èÑÈÿÿL‹ÀI‹NI+HÁùI¯ÉH;ñsHöHÁãII‹VH‹Ëè“¨ÿÿI‰^ë6v4I‹FI+HÁøI¯ÁH‹ÖH;ğv
I‹Îèh«ÿÿëH+ÑM‹ÎI‹Nè«¬ÿÿI‰FHL$0è=ÍÿÿIN3ÒèÒzÕÿH…ötr3ÿI‹ƒÿH¸ÿÿÿÿÿÿÿH‰„€   ƒ¤ˆ    HKHÏ3ÒèüùÔÿHK(HÏ3ÒèîùÔÿHƒdH HÇDP   @ˆlXÇD\  ƒdd ƒd` HÇ   HƒîuINH3Òè°ùÔÿH‹Œ$À   H3Ìè@/ÿÿLœ$Ğ   I‹[0I‹k8I‹ãA^_^ÃH‹ÄH‰XH‰hH‰pH‰x ATAVAW¸    èM<ÿÿH+à‹êH‹ñƒúÿu3Àéª  ÿKâS H…ÀtH‹ÎÿMãS H‹ÎH‹Øÿ1âS H;Ã‚s  öFtuƒÈÿés  H‹ÎÿâS H…Àu3ÿëH‹ÎÿWâS H‹ÎH‹ØÿãS H‹øH+û¸   H‹ßHÑëH‹ÎH;ØL‹ÿHBØE3öÿ$âS L‹àH‹ËH÷ÑH;ÏsHÑëuğH…ÛtL<;I‹ÏèÀÕÿL‹ğH…ÿtL‹ÇI‹ÔI‹ÎèÚ?ÿÿ€~p tH‹×I‹ÌèÕÿMşÆFpH‹ÎH…ÿu,M‹ÇL‰vhI‹ÖÿZâS öFtI‹ÖH‹Î„‘   MNM‹Æé‹   I‹ÆI+ÄHFhÿ¦áS H‹ÎH‹Øÿ*áS H‹øH‹Î+ûÿ|áS H‹ÎH‹ØÿháS H‹Î+ØÿuáS I‹ÖM‹ÇI+ÔH‹ÎHĞÿèáS ‹×H‹ÎÿİàS öFtH‹ÎtÿÖàS LcÃI‹ÖMÆH‹ÎLHë	I‹ÖE3ÀM‹ÎÿµáS @ŠÕH‹ÎÿàS ‹ÅH‹\$@H‹l$HH‹t$PH‹|$XHƒÄ A_A^A\ÃH‰\$UVWATAUAVAWHl$Ğ¸0  èQ:ÿÿH+àH‹×Œ H3ÄH‰E H‹òH‹ùHT$PèH?ÙÿH‹Èè˜¼ÿÿL‹ğH‰D$@HL$Pè†VÙÿ²%I‹ÎÿûàS ŠØˆD$2ÆD$0AƒÏÿDŠOhM‹ÆŠĞH‹ÎèÿºÿÿHcĞH‹Ïè„ûÿÿE3öD‰t$4E3äL!d$8Dˆd$1E3íE3ÀŠÓH‹Îèw¸àÿH‹ØHƒøÿ„¾  E…íuHOHëAEÿHcÈHÉHÁâH‹HƒÁ(HÊH‹ÖHƒ~vH‹H‹ÆvH‹Š8DuLKM‹ÄH‹Öè=©ÿÿHƒÃL‹ãé   I;ÜtL‹ËM‹ÄH‹Öè©ÿÿL‹ãHÿÃH‹ÆHƒ~vH‹HÃH‰D$8ŠWhIcÅLÀIÁàLH‹ÎHƒ~vH‹HNH‰L$HˆT$(H‰\$ L‹L$@HT$HHL$8èãªÿÿH‹ÎHƒ~vH‹H‹\$8H+Ù„À„‘   L‹ãH‹A‹„ˆ   ¨t/A‹T\öÂ@tƒàşëƒàıAÆDX0â?ÿÿÿºêA‰T\A‰„ˆ   ¨tAöD\ tƒàıA‰„ˆ   H‹A‹ƒùıt-ƒùÿuÆD$0 ëƒùşuÆD$1ëA;ÏDOùÿD$4AÿÅIÆ   L‹ÃŠT$2H‹ÎèŞ¶àÿH‹ØHƒøÿ…gşÿÿD‹t$4E…ítAEÿHcÈHÉHÁâH‹HƒÁ(HÊëHOHL‹NM‹ÄH‹Öè½§ÿÿ@Št$0@„öuhE…ÿx7öGht1WÀD$hHñûd H‰D$`IcÇH‰D$xHƒe€ HL$`è¶ÿÿë3ÉMcÆE…ö~3ÀL‹Aƒ<ÿuA‰ÿÁH   IƒèuäDyÿ² H‹L$@ÿNŞS ŠĞHMèƒÂÿÿL‹ÀIcÖH‹OH+HÁùI¹9ã8ã8I¯ÉH;ÑsHÒHÁãHH‹WH‹Ëè8¢ÿÿH‰_ë3v1H‹GH+HÁøI¯ÁH;Ğv
H‹Ïè¥ÿÿëH+ÑL‹ÏH‹OèS¦ÿÿH‰GHMèæÆÿÿ€|$1 tƒO8AO‰O@‹W8‹ÊƒáşƒÊ@„öDÑ‰W8H‹ÇH‹M H3ÌèQ)ÿÿH‹œ$€  HÄ0  A_A^A]A\_^]ÃH‰\$H‰t$H‰|$AV¸    èe6ÿÿH+à‹úH‹ñÿÏÜS E3öH…Àt\H‹Îÿ¶ÜS H‹ÎH‹Øÿ²ÜS H;ØsBöFtuƒÿÿtH‹Îÿ™ÜS @:xÿu(ƒÊÿH‹ÎÿÜS ƒÿÿtH‹ÎÿyÜS ƒÿÿ@ˆ8ADş‹ÇëƒÈÿH‹\$0H‹t$8H‹|$@HƒÄ A^ÃH‹ÄH‰XH‰hH‰pH‰x AW¸    è±5ÿÿH+à‹ÚH‹ñÿÜS E3ÿH‹èH…ÀtIH‹ÎÿÿÛS H;èv;@Šûƒûÿt:]ÿtöFpu(ƒÊÿH‹ÎÿëÛS ƒûÿtH‹ÎÿÕÛS ƒûÿ@ˆ8ADß‹ÃëƒÈÿH‹\$0H‹l$8H‹t$@H‹|$HHƒÄ A_ÃH‰\$W¸    è 5ÿÿH+àH‹ùÿœÛS H‹ÏH‹Øÿ ÛS H+ÃH‹\$0HƒÄ _ÃHcAüH+Èë¸˜   èÚ4ÿÿH+àHQ A¸   HL$ èó¹ÿÿHty~ HL$ è›^ÿÿÌHcAüH+Èë¸˜   èš4ÿÿH+àHQ A¸   HL$ è“»ÿÿH|~ HL$ è[^ÿÿÌHcAüH+Èë¸˜   èZ4ÿÿH+àHQ A¸   HL$ èƒ¼ÿÿHÌy~ HL$ è^ÿÿÌH‰\$H‰t$H‰|$AV¸    è4ÿÿH+àA‹ÙI‹øL‹òH‹ñÿÚS H…ÀtH‹Îÿ
ÚS H9FhsH‹ÎÿûÙS H‰FhöD$P„â   H‹ÎÿCÚS H…À„Ğ   ƒûuH‹Îÿ,ÚS H‹NhH+ÈHùë2…ÛuH‹ÎÿÚS H‹ÎH+øÿÿÙS Høëƒû…  öD$P…  H‹ÎÿåÙS H‹ÎHÿĞÙS H;Ã‡ä   H‹ÎÿÆÙS HÇH;Fh‡Î   ‹×H‹Îÿ¶ÙS öD$P„¼   H‹Îÿ:ÙS H…À„ª   H‹ÎÿˆÙS H‹ÎH‹ØÿÙS +Ø‹ÓH‹ÎÿÙS éƒ   öD$PtxH‹ÎÿúØS H…ÀtjƒûuH‹ÎÿçØS H‹NhH+ÈHùë…ÛuLH‹ÎÿÎØS H‹ÎH+øÿ2ÙS HøH‹Îÿ¶ØS H‹ÎHÿÙS H;ÃwH‹Îÿ›ØS HÇH;Fhw‹×ésÿÿÿHƒÏÿIƒf H‹\$03ÀH‹t$8I‰>H‹|$@I‰FI‹ÆHƒÄ A^ÃH‹ÄH‰XH‰hH‰pH‰x AV¸    è!2ÿÿH+àI‹pA‹éI0L‹òH‹ùÿ ØS H…ÀtH‹ÏÿØS H9GhsH‹ÏÿØS H‰GhHƒËÿH;ó„å   @öÅ„   H‹Ïÿ?ØS H…ÀtsH…öˆ¼   H‹Ïÿ ØS H‹OhH+ÈH;ñ£   H‹ÏÿØS ‹ŞH‹Ï+Øÿú×S H‹Ïÿş×S @öÅt~H‹Ïÿ‡×S H…ÀtpH‹ÏÿÙ×S H‹ÏH‹Øÿm×S +Ø‹ÓëG@öÅtLH‹ÏÿX×S H…Àt>H…öx9H‹Ïÿ×S H‹OhH+ÈH;ñ$H‹Ïÿ0×S ‹ŞH‹Ï+Øÿ{×S H‹Ïÿ×S ëH‹óI‰6ëI‰Iƒf H‹\$03ÀH‹l$8H‹t$@H‹|$HI‰FI‹ÆHƒÄ A^ÃH‰\$H‰l$H‰t$WATAUAVAW¸    è0ÿÿH+àHypE‹ñH‹ÚH‹ñAöÁt	öt±ë2ÉA´E„ôt	D„'t°ë2À„É…½   „À…µ   I‹hH‹ÎI(ÿÂÖS L‹èD„'t3ÿëH‹ÎÿMÖS H‹øH…Àt
H9FhsH‰FhH‹ÎÿŠÖS L‹NhL‹øI‹ÉH+ÈH;éwcH…ítAöÆtM…ítSE„ôtH…ÿtIL$(AöÆtM…ítM‹ÄI‹×H‹ÎÿèÖS AöÆt H…ÿtH‹ÎÿäÖS M‹ÄI‹×L‹ÈH‹Îÿ*×S H‰+ëHƒÿHƒc H‹l$X3ÀH‹t$`H‰CH‹ÃH‹\$PHƒÄ A_A^A]A\_ÃH‰\$H‰t$W¸    è[/ÿÿH+àH‹ùÿÇÕS H…À„º   H‹Ïÿ]ÖS H‹ÏH‹Øÿ©ÕS H;ÃsH‹Ïÿ›ÕS ¶ é’   öGt„…   H‹Ïÿ ÕS H…ÀtwH‹ÏÿÕS H‹ÏH‹ØÿfÕS H;ÃrH‹ÏÿXÕS H;GhsNH‹whH‹ÏÿåÔS H;ğsH‹Ïÿ×ÔS H‹ğH‰GhH‹Ïÿ'ÕS H‹ÏH‹ØÿÕS L‹ÎL‹ÃH‹ĞH‹Ïÿ©ÕS é`ÿÿÿƒÈÿH‹\$0H‹t$8HƒÄ _ÃHAHƒxvH‹ ÃHi@g ÃHAhHƒxvH‹ ÃH	Cg ÃH)Cg ÃHi@g Ã@S¸P   è.ÿÿH+àH‹’uŒ H3ÄH‰D$@3À‰‰  H‹Ùf‰  I‹Ğf‰  Çˆ  0000HL$ è  HƒxvH‹ HKIƒÉÿL‹Àº   ÿ0ŞS HL$ è¢öÕÿH‹ÃH‹L$@H3Ìè> ÿÿHƒÄP[ÃH‰\$UVWATAUAVAWHl$ù¸   èi-ÿÿH+àH‹ïtŒ H3ÄH‰E÷M‹àL‹éH‹]gH‹}wH‹µ‡   HœÿW H‰3ÀH‰AH‰AH‰AÆA Ly(WÀAI‰GIÇG   fA‰HÕ%Y HL$0èßöÕÿH‹HL$PèÒöÕÿHç2g HL$pèÁöÕÿ)M—G)E§HÇG   3ÛH‰_f‰HşšW HM·è‘öÕÿ)M×F)EçH‰^HÇF   f‰HD$0H‰D$ HE÷H‰D$((D$ fD$ HT$ IMè«|éÿLœGñÿS DCHL$0è—!ÿÿAÆE I‹ÔI‹Ïè{¯ÕÿI‹ÅH‹M÷H3ÌèÜÿÿH‹œ$H  HÄ   A_A^A]A\_^]ÃH‰\$H‰L$W¸    èû+ÿÿH+àI‹ØH‹ùH‰Hƒa Hƒa èä—ÕÿH‰GH‹L$PH‹	‹‰P HH(è¶¶ÔÿH‹GH‰H‹GH‰XH‹GH‰X3ÉH‹GÆD HÿÁHƒù|îH‹ÇH‹\$8HƒÄ _Ã@SUVWAV¸   è{+ÿÿH+àH‹sŒ H3ÄH‰„$€   I‹éL‹ñH‰L$XH‹´$à   H‹¼$è   3ÀWÀD$`fo9ûT óL$pf‰D$`K H‹ÓHL$`èİPÕÿLÆ4y LÃH‰|$0H‰t$(H‰l$ LL$`I‹Îè¯	  HL$`èğóÕÿI‹ÆH‹Œ$€   H3Ìè‰ÿÿHÄ   A^_^][ÃH‰\$W¸€   è¸*ÿÿH+àH‹>rŒ H3ÄH‰D$pI‹ÁM‹ÈH‹ÙH‰L$HH‹Œ$°   3ÿ‰|$@H34y IƒÈÿIÿÀfB9<BuöH‰L$(H‰D$ HL$PèÕşÿÿD$PL$`KH‰|$`HÇD$h   f‰|$PHL$Pè6óÕÿH‹ÃH‹L$pH3ÌèÒÿÿH‹œ$˜   HÄ€   _ÃH‰\$UVWATAUAVAW¸    èö)ÿÿH+àH‹|qŒ H3ÄH‰„$˜   M‹éH‰L$hH‰L$pL‹¤$   L‹¼$  L‹´$  H‹¬$  H‹´$   H‹¼$(  3ÀWÀD$xfo’ùT óŒ$ˆ   f‰D$xK H‹ÓHL$xè3OÕÿLlKg LÃH‰|$PH‰t$HH‰l$@L‰t$8L‰|$0L‰d$(L‰l$ LL$xH‹L$hèÏ  HL$xè0òÕÿH‹D$hH‹Œ$˜   H3ÌèÇÿÿH‹œ$è   HÄ    A_A^A]A\_^]ÃH‰\$UVW¸   èê(ÿÿH+àH‹ppŒ H3ÄH‰„$€   I‹ÁM‹ÈH‹ÙH‰L$XH‹Œ$Ğ   H‹”$Ø   L‹”$à   L‹œ$è   H‹¼$ğ   3ö‰t$PH-’Jg IƒÈÿIÿÀfB9tE uõH‰|$HL‰\$@L‰T$8H‰T$0H‰L$(H‰D$ HL$`èKşÿÿD$`L$pKH‰t$pHÇD$x   f‰t$`HL$`è0ñÕÿH‹ÃH‹Œ$€   H3ÌèÉÿÿH‹œ$¸   HÄ   _^]ÃH‰\$UVWATAUAVAW¸   èê'ÿÿH+àH‹poŒ H3ÄH‰„$€   M‹áL‹éH‰L$XL‹¼$ğ   L‹´$ø   H‹¬$   H‹´$  H‹¼$  3ÀWÀD$`fo÷T óL$pf‰D$`K H‹ÓHL$`è4MÕÿLEg LÃH‰|$HH‰t$@H‰l$8L‰t$0L‰|$(L‰d$ LL$`I‹Íè7  HL$`è8ğÕÿI‹ÅH‹Œ$€   H3ÌèÑÿÿH‹œ$Ø   HÄ   A_A^A]A\_^]Ã@SVW¸   èö&ÿÿH+àH‹|nŒ H3ÄH‰„$€   I‹ÁM‹ÈH‹ÙH‰L$XH‹Œ$Ğ   H‹”$Ø   L‹”$à   L‹œ$è   3ÿ‰|$PH5VDg IƒÈÿIÿÀfB9<FuöL‰\$@L‰T$8H‰T$0H‰L$(H‰D$ HL$`èqşÿÿD$`L$pKH‰|$pHÇD$x   f‰|$`HL$`èJïÕÿH‹ÃH‹Œ$€   H3ÌèãÿÿHÄ   _^[Ã@USVWAVH‹ì¸p   è&ÿÿH+àH‹šmŒ H3ÄH‰EğI‹ùI‹ğH‹ÙH‰MÀH‰UÀE3öH;Öƒ‚   WÀEĞL‰uàHÇEè   fD‰uĞL‹ÇH‹ÖHMÀèÖÿ„ÀuLMĞL‹ÇH‹ÖHMÀè|¥Õÿ„ÀuHMĞèŸîÕÿH‹UÀë¦LVöT HUĞH‹ÏèÒÖÿL‹ÏL‹ÆH‹UÀH‹Ëè4   HMĞèjîÕÿëH‹×H‹ËèÉÖÿH‹ÃH‹MğH3ÌèúÿÿHƒÄpA^_^[]Ã@USVWAVH‹ì¸`   è(%ÿÿH+àH‹®lŒ H3ÄH‰EğI‹ùI‹ğH‹ÙH‰MÀH‰UÀE3öH;Öƒ‚   WÀEĞL‰uàHÇEè   fD‰uĞL‹ÇH‹ÖHMÀèÖÿ„ÀuLMĞL‹ÇH‹ÖHMÀè¤Õÿ„ÀuHMĞè³íÕÿH‹UÀë¦LjõT HUĞH‹ÏèæÖÿL‹ÏL‹ÆH‹UÀH‹ËèÌöÕÿHMĞè~íÕÿëH‹×H‹ËèİÖÿH‹ÃH‹MğH3ÌèÿÿHƒÄ`A^_^[]Ã@USVWATAVAWH‹ì¸€   è8$ÿÿH+àH‹¾kŒ H3ÄH‰EğI‹ùI‹ğH‹ÙH‰MÀH‰UÀL‹u`L‹}hE3äH;Öƒƒ   WÀEĞL‰eàHÇEè   fD‰eĞL‹ÇH‹ÖHMÀèÖÿ„ÀuLMĞL‹ÇH‹ÖHMÀè˜£Õÿ„ÀuHMĞè»ìÕÿH‹UÀë¦E‹HUĞH‹Ïèæ€ÚÿL‰|$ L‹ÏL‹ÆH‹UÀH‹ËèO  HMĞè…ìÕÿëH‹×H‹Ëèä ÖÿH‹ÃH‹MğH3ÌèÿÿHÄ€   A_A^A\_^[]ÃH‰\$UVWATAUAVAWH‹ì¸p   è3#ÿÿH+àH‹¹jŒ H3ÄH‰EğI‹ùI‹ğH‹ÙH‰MÀH‰UÀL‹u`L‹}hL‹epE3íH;Öƒˆ   WÀEĞL‰màHÇEè   fD‰mĞL‹ÇH‹ÖHMÀèÖÿ„ÀuLMĞL‹ÇH‹ÖHMÀè¢Õÿ„ÀuHMĞè²ëÕÿH‹UÀë¦E‹HUĞH‹ÏèİÚÿL‰d$(L‰|$ L‹ÏL‹ÆH‹UÀH‹ËèÅ»İÿHMĞèwëÕÿëH‹×H‹ËèÖÿÕÿH‹ÃH‹MğH3ÌèÿÿH‹œ$À   HƒÄpA_A^A]A\_^]ÃH‰\$UVWAVAWH‹ì¸p   è#"ÿÿH+àH‹©iŒ H3ÄH‰EğI‹ùI‹ğH‹ÙH‰MÀH‰UÀL‹uPE3ÿH;Ös~WÀEĞL‰}àHÇEè   fD‰}ĞL‹ÇH‹ÖHMÀèÖÿ„ÀuLMĞL‹ÇH‹ÖHMÀè‹¡Õÿ„ÀuHMĞè®êÕÿH‹UÀëªM‹HUĞH‹ÏèåÖÿL‹ÏL‹ÆH‹UÀH‹Ëè[ûÿÿHMĞè}êÕÿëH‹×H‹ËèÜşÕÿH‹ÃH‹MğH3ÌèÿÿH‹œ$°   HƒÄpA_A^_^]ÃH‰\$UVWATAUAVAWH‹ì¸€   è+!ÿÿH+àH‹±hŒ H3ÄH‰EøI‹ùI‹ğH‹ÙH‰MÀL‹u`L‹}hL‹epE3íH§*y H‰EÀH;Æƒˆ   WÀEØL‰mèHÇEğ   fD‰mØL‹ÇH‹ÖHMÀèÖÿ„ÀuLMØL‹ÇH‹ÖHMÀè€ Õÿ„ÀuHMØè£éÕÿH‹EÀë¦M‹HUØH‹ÏèÚÖÿL‰d$(L‰|$ L‹ÏL‹ÆH‹UÀH‹ËèüÿÿHMØèhéÕÿëH‹×H‹ËèÇıÕÿH‹ÃH‹MøH3ÌèøÿÿH‹œ$È   HÄ€   A_A^A]A\_^]ÃH‰\$UVWATAUAVAWH‹ì¸€   è ÿÿH+àH‹•gŒ H3ÄH‰EğI‹ùI‹ğH‹ÙH‰MÀH‰UÀL‹u`L‹}hL‹epL‹mx3ÀH;ÖƒŒ   WÀEĞH‰EàHÇEè   f‰EĞL‹ÇH‹ÖHMÀèğÿÕÿ„ÀuLMĞL‹ÇH‹ÖHMÀèiŸÕÿ„ÀuHMĞèŒèÕÿH‹UÀë¥M‹HUĞH‹ÏèÃ ÖÿL‰l$0L‰d$(L‰|$ L‹ÏL‹ÆH‹UÀH‹ËèüÿÿHMĞèLèÕÿëH‹×H‹Ëè«üÕÿH‹ÃH‹MğH3ÌèÜÿÿH‹œ$Ğ   HÄ€   A_A^A]A\_^]ÃH‰\$UVWATAUAVAWH‹ì¸€   èóÿÿH+àH‹yfŒ H3ÄH‰EøI‹ùI‹ğH‹ÙH‰MÈH‰UÀL‹u`L‹}hL‹epL‹mxH‹…€   H‰EÈ3ÀH;Öƒ•   WÀEØH‰EèHÇEğ   f‰EØL‹ÇH‹ÖHMÀèÉşÕÿ„ÀuLMØL‹ÇH‹ÖHMÀèBÕÿ„ÀuHMØèeçÕÿH‹UÀë¥M‹HUØH‹ÏèœÿÕÿH‹EÈH‰D$8L‰l$0L‰d$(L‰|$ L‹ÏL‹ÆH‹UÀH‹ËèöıÿÿHMØèçÕÿëH‹×H‹Ëè{ûÕÿH‹ÃH‹MøH3Ìè¬ÿÿH‹œ$Ğ   HÄ€   A_A^A]A\_^]ÃH‰\$UVWATAUAVAWHl$ù¸    èÁÿÿH+àH‹GeŒ H3ÄH‰E÷I‹ùI‹ğH‹ÙH‰M¿H‰U·L‹ugL‹}oL‹ewL‹mH‹…‡   H‰E¿H‹…   H‰EÏ3ÀH;Öƒ   WÀE×H‰EçHÇEï   f‰E×L‹ÇH‹ÖHM·èŒıÕÿ„ÀuLM×L‹ÇH‹ÖHM·èÕÿ„ÀuHM×è(æÕÿH‹U·ë¥M‹ÆHU×H‹Ïè?şÕÿH‹EÏH‰D$@H‹E¿H‰D$8L‰l$0L‰d$(L‰|$ L‹ÏL‹ÆH‹U·H‹ËèÌıÿÿHM×èÖåÕÿëH‹×H‹Ëè5úÕÿH‹ÃH‹M÷H3ÌèfÿÿH‹œ$ğ   HÄ    A_A^A]A\_^]ÃH‰\$UVWATAUAVAWH‹ì¸€   è{ÿÿH+àH‹dŒ H3ÄH‰EøI‹ùI‹ğH‹ÙH‰MÈH‰UÀL‹u`L‹}hL‹epL‹mxH‹…€   H‰EÈ3ÀH;Öƒ•   WÀEØH‰EèHÇEğ   f‰EØL‹ÇH‹ÖHMÀèQüÕÿ„ÀuLMØL‹ÇH‹ÖHMÀèÊ›Õÿ„ÀuHMØèíäÕÿH‹UÀë¥M‹ÆHUØH‹ÏèıÕÿH‹EÈH‰D$8L‰l$0L‰d$(L‰|$ L‹ÏL‹ÆH‹UÀH‹Ëè
²İÿHMØè¤äÕÿëH‹×H‹ËèùÕÿH‹ÃH‹MøH3Ìè4ÿÿH‹œ$Ğ   HÄ€   A_A^A]A\_^]ÃH‰\$UVWATAUAVAWHl$ù¸°   èIÿÿH+àH‹ÏbŒ H3ÄH‰E÷I‹ùI‹ğH‹ÙH‰M·L‹ugL‹}oL‹ewL‹mH‹…‡   H‰E·H‹…   H‰EÏH‹…—   H‰EÇHó<g H‰E§3ÉH;Æƒ§   WÀE×H‰MçHÇEï   f‰M×L‹ÇH‹ÖHM§èûÕÿ„ÀuLM×L‹ÇH‹ÖHM§è{šÕÿ„ÀuHM×èãÕÿH‹E§ë¥M‹ÆHU×H‹ÏèµûÕÿH‹EÇH‰D$HH‹EÏH‰D$@H‹E·H‰D$8L‰l$0L‰d$(L‰|$ L‹ÏL‹ÆH‹U§H‹ËèiüÿÿHM×èCãÕÿëH‹×H‹Ëè¢÷ÕÿH‹ÃH‹M÷H3ÌèÓÿÿH‹œ$ø   HÄ°   A_A^A]A\_^]ÃH‰\$UVWATAUAVAWHl$ù¸    èéÿÿH+àH‹oaŒ H3ÄH‰EÿI‹ùI‹ğH‹ÙH‰MÇL‹ugL‹}oL‹ewL‹mH‹…‡   H‰EÇH‹…   H‰E×HN7g H‰E·3ÉH;Æƒ   WÀEßH‰MïHÇE÷   f‰MßL‹ÇH‹ÖHM·è­ùÕÿ„ÀuLMßL‹ÇH‹ÖHM·è&™Õÿ„ÀuHMßèIâÕÿH‹E·ë¥M‹ÆHUßH‹Ïè`úÕÿH‹E×H‰D$@H‹EÇH‰D$8L‰l$0L‰d$(L‰|$ L‹ÏL‹ÆH‹U·H‹ËèeüÿÿHMßè÷áÕÿëH‹×H‹ËèVöÕÿH‹ÃH‹MÿH3Ìè‡ÿÿH‹œ$è   HÄ    A_A^A]A\_^]Ã@S¸0   è°ÿÿH+àD@øH‹Ùè•oäÿH‹ÃHƒÄ0[ÃH‰\$H‰t$H‰|$ATAVAW¸0   èyÿÿH+àM‹ñM‹øL‹âH‹ÙpÑ‹ÎèÈìÕÿH‹ÈèˆlÕÿH‹øH‰D$(‰p‰pHcİd H‰HwEŠE‹I‹ÔH‹ÎèûéŞÿH‰3H‰{H‹ÃH‹\$PH‹t$XH‹|$`HƒÄ0A_A^A\Ã@S¸    èøÿÿH+àH‹ÙH‹IH…ÉtHƒÁ(H‹Ñè9×ïÿH‹ËHƒÄ [é ¦ÖÿÌH‰\$W¸    è°ÿÿH+à‹ÚH‹ùèkÙïÿöÃtH‹Ïè¾ÉÔÿH‹\$0H‹ÇHƒÄ _Ã@S¸    ètÿÿH+àHŠÜd H‹ÙH‰öÂtè‚ÉÔÿH‹ÃHƒÄ [Ã@S¸    èDÿÿH+àH$Y H‹ÙH‰öÂtèRÉÔÿH‹ÃHƒÄ [Ã¸(   èÿÿH+àH‹H‹@0HƒÄ(Hÿ%aËS Ì¸(   èöÿÿH+àHdéZ I‹ÈHƒÄ(é0–ÕÿÌH‰\$UVWH‹ì¸`   è»ÿÿH+àH‹A^Œ H3ÄH‰EøI‹ØH‹ò3ÿI‰xI‹ÀIƒxvI‹ f‰8‰}ÀHUÀH‹ÎèôÿH;t"‰}ÀLEÀHUÈH‹Îè3áêÿH‹HƒÂ(H‹Ëè˜™ÕÿH9{tE3ÀHPU H‹ËèxõÜÿ…À…*  ÇEÀ'   HUÀH‹Îè­ôÿH;„Ï   ÇEÀ'   LEÀHUÈH‹ÎèÑàêÿH‹HƒÁ(E3ÀH´)y è'õÜÿ…Àt1ÇEÀ'   LEÀHUÈH‹Îè àêÿH‹HƒÁ(E3ÀHc)y èöôÜÿ…ÀumHÃ)U H‹Ëèû”ÕÿWÀEØfoœåT óMèf‰}ØHUØHØüÑÿè'häÿHMÈèN™ÔÿLEØº   H‹è(ÓØÿH‹MĞH…Étè½dÖÿHMØèSŞÕÿë?ÇEÀ   HUÀH‹ÎèÂôÿH;uÇEÀ   HUÀH‹ÎèªôÿH;tH>W H‹Ëè^”ÕÿH9{v1H‹ËHƒ{vH‹·yÿ1ÅS H‹ËHƒ{vH‹f‰yHÿÇH;{rÏH‹MøH3Ìè{ÿÿH‹œ$€   HƒÄ`_^]ÃH‰\$H‰t$H‰L$W¸@   èÿÿH+àA‹øH‹Ú3öH‰t$PLD$P3Ò‹Ïÿÿ’› H‹T$PH…ÒtWHL$(è«UôÿH‹D$0H+D$(H÷ØHÒH#T$(LD$P‹ÏÿÈ’› H‹D$0H+D$(H÷ØHÒH#T$(H‹ËèŞÕÿHL$(è4
ßÿëWÀH‰sHÇC   f‰3H‹ÃH‹\$XH‹t$`HƒÄ@_Ã@SVW¸`   èæÿÿH+àH‹l[Œ H3ÄH‰D$XA‹ğH‹ÚH‰T$(HyHƒ? uFH,g HL$8è’İÕÿE3ÉLD$8‹ÖHL$(è…ÜÿH‹ĞH‹Ïè
{ÔÿHL$(èÔôŞÿHL$8è}ÜÕÿH‹H‰ÇC“   H…ÉtğÿAH‹ÃH‹L$XH3ÌèÿÿHƒÄ`_^[Ã@USVWATAUAVAWH¬$˜şÿÿ¸h  è!ÿÿH+à)´$P  H‹ŸZŒ H3ÄH‰…H  M‹ñL‰MˆL‰D$xH‹ÚH‰U˜H‰ME3ÿA‹ÿD‰|$XDˆ|$PH/(g HM è²ÜÕÿLE HM°è…ŞîÿHM è¼ÛÕÿH	(g HM èŒÜÕÿHU H(  èğÕÿHM è‘ÛÕÿHE°H‰D$`H…(  H‰D$hHL$`èæüÿ„À„µ  H•¨   Hˆ2’ è×áŞÿL‹¥¨   M…ä…í   H{JY H‰E HÀ„Y H‰EHõ-g H‰EfD‰}HY H‰EØWÀEàfoâT óMğfD‰}àE3ÉE|$2E‹ÇA½°  A‹Õ¿¢”M‹Ïè$Üÿ„ÀtTHEØH‰D$`HE H‰D$hHSZ H‰E HD$`H‰E(HD$pH‰E0E3ÉHE H‰D$(H’-g H‰D$ E‹ÇA‹Õ‹Ïèğ¹ÓÿHMàè{ÚÕÿ¾   Aˆ6H‹°   H…É„|  è¾`Öÿér  H•  I‹ÌèÊyÔÿHUhI‹Ìè½yÔÿH‹Ø¾   ‰t$XHNŠW HM èÛÕÿ~‰|$XE3ÀH‹ÓHM èšzÖÿDv…À„‡   H•ˆ   I‹ÌèoyÔÿH‹ØD‰t$XHL$U HM¸è·ÚÕÿ~‰|$XE3ÀH‹ÓHM¸èPzÖÿ…ÀtEHU@I‹Ìè0yÔÿH‹ØÇD$X   H:âZ HM èuÚÕÿ~>‰|$XE3ÀH‹ÓHM èzÖÿ…ÀAŠßu@ŠŞˆ\$R@öÇ tƒçßHM è^ÙÕÿ@öÇtƒçïHM@èKÙÕÿ@öÇtƒç÷HM¸è8ÙÕÿ@öÇtƒçûHˆ   è"ÙÕÿ@öÇtƒçıHM èÙÕÿ@„şt	HMhè ÙÕÿDˆ|$QA¿2   A½°  H=¬,g fo5ÜßT „Û„I  E3À3ÒHM èâ¹şÿH‹M èq>5 ˆD$Q„À„F  HU@I‹Ìè)xÔÿL‹ÀH
,g HM¸èi´ŞÿL9pvH‹ HÁGY H‰M H‚Y H‰MH‰E3Ûf‰]HQŒY H‰EØWÀEàóuğf‰]àE3ÉE‹ÇA‹Õ¹K‘@è†Üÿ„ÀtPHEØH‰D$`HE H‰D$hHµZ H‰E HD$`H‰E(HD$pH‰E0E3ÉHE H‰D$(H‰|$ E‹ÇA‹Õ¹K‘@èV·ÓÿHMàèá×ÕÿHM¸èØ×ÕÿHM@èÎ×ÕÿI‹Ìè¢÷ÿÿH‹MH‹H‹@ ÿÃS H‹’ã ÇD$T   LD$THUhH‹L$xè[ÙêÿH‹HƒÁ(H‹ÓèĞÕÿ@ˆt$PH‹Eˆ@ˆ0Š\$RéÒ   Hl+g HM¸è»(üÿL9pvH‹ H‹FY H‰M HĞ€Y H‰MH‰E3Éf‰MH‹Y H‰EØWÀEàóuğf‰MàE3ÉDadE‹ÄA‹Õ¹L‘@èLÜÿ„ÀtPHEØH‰D$`HE H‰D$hH{Z H‰E HD$`H‰E(HD$pH‰E0E3ÉHE H‰D$(H‰|$ E‹ÄA‹Õ¹L‘@è¶ÓÿHMàè§ÖÕÿHM¸èÖÕÿH‹M¨H…Étèï\ÖÿH‹MˆL‹e˜€9 …z  E3ÀHp!U I‹Ìè ìÜÿ…À…`  A¸  H•è   è–øÿÿA¸'  HU¸è†øÿÿºkB  A¸
   Hˆ   èvIÜÿÇD$T   LD$THUhH‹L$xèÖ×êÿH‹HƒÁ(L9qvH‹	3Òÿ
®S ‰D$TH…ˆ   L9µ    HG…ˆ   H‰D$pI‹ÄM9t$vI‹$H‰EHD$pH‰D$ LL$TLContent
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
