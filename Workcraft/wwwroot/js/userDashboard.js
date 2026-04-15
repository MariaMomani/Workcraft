document.addEventListener("DOMContentLoaded", function () {
    const savedStatus = document.getElementById("savedStatus").value;
    changeStatus(savedStatus);
});

function dismissNotification(id) {
    const token = document.getElementById("af-token").value;

    fetch('/User/DismissNotification', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: '__RequestVerificationToken=' + encodeURIComponent(token) + '&id=' + id
    })
        .then(r => r.json())
        .then(data => {
            if (data.success) {
                const el = document.getElementById('notif-' + id);
                el.style.transition = 'opacity 0.3s, transform 0.3s';
                el.style.opacity = '0';
                el.style.transform = 'translateX(20px)';
                setTimeout(() => el.remove(), 300);
            }
        });
}