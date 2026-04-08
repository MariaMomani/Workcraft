function openAddModal() {
    document.getElementById('addEmployeeModal').style.display = 'flex';
}

function closeAddModal() {
    document.getElementById('addEmployeeModal').style.display = 'none';
}

document.querySelectorAll('.edit-btn').forEach(button => {
    button.addEventListener('click', function () {

        const id = this.getAttribute('data-id');
        const name = this.getAttribute('data-name');
        const email = this.getAttribute('data-email');
        const position = this.getAttribute('data-position');
        const status = this.getAttribute('data-status');

        document.getElementById('editId').value = id;
        document.getElementById('editFullName').value = name;
        document.getElementById('editEmail').value = email;

        document.getElementById('editPosition').value = position || "";

        document.getElementById('editStatus').value = status;

        document.getElementById('editEmployeeModal').style.display = 'block';
    });
});

function closeEditModal() {
    document.getElementById('editEmployeeModal').style.display = 'none';
}

window.onclick = function (event) {
    if (event.target.className === 'modal') {
        event.target.style.display = 'none';
    }
}