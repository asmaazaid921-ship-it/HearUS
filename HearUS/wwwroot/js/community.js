document.addEventListener("submit", async function (event) {
    const form = event.target.closest(".js-like-form");
    if (!form) {
        return;
    }

    event.preventDefault();

    const button = form.querySelector(".js-like-btn");
    const label = form.querySelector(".js-like-label");
    const count = form.querySelector(".js-like-count");

    button.disabled = true;

    try {
        const response = await fetch(form.action, {
            method: "POST",
            body: new FormData(form),
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        if (!response.ok) {
            throw new Error("request failed");
        }

        const data = await response.json();
        count.textContent = `(${data.count})`;
        label.textContent = data.liked ? "أعجبني" : "إعجاب";
        button.classList.toggle("btn-primary", data.liked);
        button.classList.toggle("btn-outline-primary", !data.liked);
    } catch {
        form.submit();
        return;
    } finally {
        button.disabled = false;
    }
});
