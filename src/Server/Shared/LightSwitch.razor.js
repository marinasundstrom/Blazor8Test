export function init() {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
        const settings = JSON.parse(localStorage.getItem('lightSwitch'));
        if (settings != null) {
            return;
        }

        if (event.matches) {
            darkMode();
        }
        else {
            lightMode();
        }
    });
}

export function getMode() {
    return JSON.parse(localStorage.getItem('lightSwitch'));
}

export function darkMode() {
    document.body.setAttribute("data-bs-theme", "dark");
}

export function lightMode() {
    document.body.setAttribute("data-bs-theme", "light");
}

export function getSystemDefaultTheme() {
    const darkThemeMq = window.matchMedia('(prefers-color-scheme: dark)');
    if (darkThemeMq.matches) {
        return 'dark';
    }
    return 'light';
}

export function setDarkMode() {
    darkMode();
    localStorage.setItem('lightSwitch', JSON.stringify('dark'));
}

export function setLightMode() {
    lightMode();
    localStorage.setItem('lightSwitch', JSON.stringify('light'));
}

export function setAutoMode() {
    let settings = getSystemDefaultTheme();

    if (settings == "light") {
        lightMode();
    } else {
        darkMode();
    }

    localStorage.setItem('lightSwitch', JSON.stringify(null));
}
