function init() {
    let settings = getMode();
    if (settings == null) {
        settings = getSystemDefaultTheme();
    }

    setTheme(settings)
}

function getMode() {
    return JSON.parse(localStorage.getItem('lightSwitch'));
}

function setTheme(theme) {
    document.body.setAttribute("data-bs-theme", theme);
}

function getSystemDefaultTheme() {
    const darkThemeMq = window.matchMedia('(prefers-color-scheme: dark)');
    if (darkThemeMq.matches) {
        return 'dark';
    }
    return 'light';
}

init();