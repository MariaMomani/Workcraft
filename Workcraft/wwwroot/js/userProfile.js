function openEditProfileModal() {
    document.getElementById("editProfileModal").style.display = "flex";
}

function closeEditProfileModal() {
    document.getElementById("editProfileModal").style.display = "none";
}

//function openChangePasswordModal() {
//    document.getElementById("changePasswordModal").style.display = "flex";
//}

window.onclick = function (event) {
    const modal = document.getElementById("editProfileModal");
    if (event.target === modal) {
        modal.style.display = "none";
    }
}