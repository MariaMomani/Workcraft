<script>
    document.getElementById("positionSelect").addEventListener("change", function () {

        const position = this.value;
    const employeeDropdown = document.getElementById("employeeSelect");

    employeeDropdown.innerHTML = '<option value="">-- Select Employee --</option>';

    if (!position) return;

    fetch('/Admin/GetEmployeesByPosition?position=' + position)
            .then(response => response.json())
            .then(data => {

                if (data.length === 0) {
        employeeDropdown.innerHTML += '<option disabled>No available employees</option>';
    return;
                }

                data.forEach(emp => {
                    const option = document.createElement("option");
    option.value = emp.id;
    option.text = emp.name;
    employeeDropdown.appendChild(option);
                });
            })
            .catch(error => console.error("Error:", error));
    });
</script>