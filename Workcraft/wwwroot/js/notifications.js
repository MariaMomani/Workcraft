document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        const tab = btn.dataset.tab;
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
        document.querySelectorAll('.tab-panel').forEach(p => p.classList.remove('active'));
        btn.classList.add('active');
        document.getElementById('panel-' + tab).classList.add('active');
    });
});

function getToken() {
    return document.getElementById('af-token').value;
}

function fadeRemove(el, callback) {
    el.style.transition = 'opacity 0.3s, transform 0.3s';
    el.style.opacity = '0';
    el.style.transform = 'translateX(20px)';
    setTimeout(() => { el.remove(); if (callback) callback(); }, 300);
}

function dismissOne(id) {
    fetch('/User/DismissNotification', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: '__RequestVerificationToken=' + encodeURIComponent(getToken()) + '&id=' + id
    })
        .then(r => r.json())
        .then(data => {
            if (data.success) {
                const el = document.getElementById('notif-' + id);
                if (el) fadeRemove(el);
            }
        });
}

const markAllBtn = document.getElementById('markAllBtn');
if (markAllBtn) {
    markAllBtn.addEventListener('click', () => {
        fetch('/User/MarkAllRead', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: '__RequestVerificationToken=' + encodeURIComponent(getToken())
        })
            .then(r => r.json())
            .then(data => {
                if (data.success) {
                    document.querySelectorAll('.notif-unread').forEach(el => {
                        el.classList.remove('notif-unread');
                    });
                    document.querySelectorAll('.notif-dot').forEach(d => d.remove());
                    markAllBtn.remove();
                }
            });
    });
}