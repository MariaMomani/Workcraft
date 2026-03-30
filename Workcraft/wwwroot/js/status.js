function changeStatus(status) {

    const badge = document.getElementById("currentStatus");
    badge.innerText = status;

    badge.className = "status-badge";

    if (status === "Available") badge.style.backgroundColor = "#28a745";
    else if (status === "Busy") badge.style.backgroundColor = "#dc3545";
    else if (status === "Break") badge.style.backgroundColor = "#ffc107";
    else if (status === "Offline") badge.style.backgroundColor = "#6c757d";

    fetch('/User/UpdateStatus', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ status: status })
    })
        .then(res => res.json())
        .then(data => {
            console.log("Status updated");
        });
}