/* Scripts personalizados para o e-commerce */

// Função para inicializar tooltips do Bootstrap
function initTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Função para inicializar popovers do Bootstrap
function initPopovers() {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
}

// Função para validar formulário de pagamento
function validatePaymentForm() {
    const form = document.getElementById('payment-form');
    if (!form) return;

    form.addEventListener('submit', function (event) {
        if (!form.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
        }
        form.classList.add('was-validated');
    });
}

// Função para atualizar quantidade no carrinho
function updateCartQuantity() {
    const quantityInputs = document.querySelectorAll('.cart-quantity-input');
    if (!quantityInputs.length) return;

    quantityInputs.forEach(input => {
        input.addEventListener('change', function () {
            const form = this.closest('form');
            form.submit();
        });
    });
}

// Função para preview de imagem em formulários de upload
function imagePreview() {
    const imageInput = document.getElementById('product-image-input');
    const imagePreview = document.getElementById('product-image-preview');
    
    if (!imageInput || !imagePreview) return;
    
    imageInput.addEventListener('change', function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                imagePreview.src = e.target.result;
                imagePreview.style.display = 'block';
            }
            reader.readAsDataURL(file);
        }
    });
}

// Função para confirmar exclusão
function confirmDelete() {
    const deleteButtons = document.querySelectorAll('.confirm-delete');
    if (!deleteButtons.length) return;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            if (!confirm('Tem certeza que deseja excluir este item? Esta ação não pode ser desfeita.')) {
                event.preventDefault();
            }
        });
    });
}

// Inicializar funções quando o documento estiver pronto
document.addEventListener('DOMContentLoaded', function () {
    initTooltips();
    initPopovers();
    validatePaymentForm();
    updateCartQuantity();
    imagePreview();
    confirmDelete();
});
