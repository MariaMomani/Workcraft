function changeStatus(status) {

    fetch("/User/UpdateStatus", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            status: status
        })
    })
        .then(res => res.json())
        .then(data => {

            if (data.success) {

                document.querySelectorAll(".status-btn").forEach(b => {
                    b.classList.remove("active");
                });

                event.target.classList.add("active");

            }
        });

}