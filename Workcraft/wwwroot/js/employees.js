function openAddModal() {
    document.getElementById('addEmployeeModal').classList.add('open');
}
function closeAddModal() {
    document.getElementById('addEmployeeModal').classList.remove('open');
}

function openEditModal() {
    document.getElementById('editEmployeeModal').classList.add('open');
}
function closeEditModal() {
    document.getElementById('editEmployeeModal').classList.remove('open');
}

document.querySelectorAll('.edit-btn').forEach(button => {
    button.addEventListener('click', function () {
        document.getElementById('editId').value = this.getAttribute('data-id');
        document.getElementById('editFullName').value = this.getAttribute('data-name');
        document.getElementById('editEmail').value = this.getAttribute('data-email');
        document.getElementById('editPosition').value = this.getAttribute('data-position') || '';
        openEditModal();
    });
});

document.querySelectorAll('.modal-overlay').forEach(overlay => {
    overlay.addEventListener('click', function (e) {
        if (e.target === this) this.classList.remove('open');
    });
});

let currentFilter = 'all';

function setFilter(val, btn) {
    currentFilter = val;
    document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active'));
    btn.classList.add('active');
    filterCards();
}

function filterCards() {
    const query = document.getElementById('searchInput').value.toLowerCase().trim();
    document.querySelectorAll('.emp-card').forEach(card => {
        const nameMatch = card.dataset.name.includes(query);
        const statusMatch = currentFilter === 'all' || card.dataset.status === currentFilter;
        card.classList.toggle('card-hidden', !(nameMatch && statusMatch));
    });
}