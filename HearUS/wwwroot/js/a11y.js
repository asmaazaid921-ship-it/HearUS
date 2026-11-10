(function () {
    "use strict";

    const root = document.documentElement;
    const storageKey = "hearus-a11y";
    const toggles = Array.from(document.querySelectorAll("[data-a11y-toggle]"));

    if (!toggles.length) {
        return;
    }

    let state = { largeText: false, highContrast: false };

    try {
        const saved = JSON.parse(localStorage.getItem(storageKey) || "{}");
        state = {
            largeText: saved.largeText === true,
            highContrast: saved.highContrast === true
        };
    } catch (error) {
        state = { largeText: false, highContrast: false };
    }

    function apply() {
        root.classList.toggle("a11y-large-text", state.largeText);
        root.classList.toggle("a11y-high-contrast", state.highContrast);

        toggles.forEach(function (button) {
            const key = button.dataset.a11yToggle;
            const active = key === "large-text" ? state.largeText : state.highContrast;
            button.setAttribute("aria-pressed", active ? "true" : "false");
            button.classList.toggle("active", active);
        });
    }

    function save() {
        try {
            localStorage.setItem(storageKey, JSON.stringify(state));
        } catch (error) {
            // التخزين المحلي قد يكون معطلاً في وضع التصفح الخاص.
        }
    }

    toggles.forEach(function (button) {
        button.addEventListener("click", function () {
            if (button.dataset.a11yToggle === "large-text") {
                state.largeText = !state.largeText;
            } else {
                state.highContrast = !state.highContrast;
            }

            save();
            apply();
        });
    });

    apply();
})();
