document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("filterForm");
    const resultsDiv = document.getElementById("productResults");

    form.addEventListener("submit", async e => {
        e.preventDefault();
        const formData = new FormData(form);
        const params = new URLSearchParams(formData).toString();

        const response = await fetch(`/Products/FilteredList?${params}`);
        const html = await response.text();
        resultsDiv.innerHTML = html;
    });
});
