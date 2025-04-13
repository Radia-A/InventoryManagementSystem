document.addEventListener("DOMContentLoaded", () => {
    const modal = new bootstrap.Modal(document.getElementById("productModal"));
    const modalContent = document.getElementById("productModalBody");

    // Open modal for edit/delete
    document.querySelectorAll("[data-action='edit'], [data-action='delete']").forEach(button => {
        button.addEventListener("click", async () => {
            const url = button.getAttribute("data-url");
            const response = await fetch(url);
            const html = await response.text();
            modalContent.innerHTML = html;
            modal.show();
        });
    });

    // AJAX form submission
    document.addEventListener("submit", async (e) => {
        if (e.target.id === "editProductForm" || e.target.id === "deleteProductForm") {
            e.preventDefault();
            const form = e.target;
            const url = form.id === "editProductForm" ? "/Products/EditAjax" : "/Products/DeleteAjax";
            const formData = new FormData(form);

            const response = await fetch(url, {
                method: "POST",
                body: formData
            });

            if (response.ok) {
                modal.hide();
                location.reload(); // or reload via AJAX for full SPA feel
            }
        }
    });
});
