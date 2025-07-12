const url = "https://localhost:7157"

async function createUser(username, password, name) {
    await fetch(url + "/api/users", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ username: username, password: password, name: name })
    })
        .then(response => {
            if (response.ok) {
                window.location.href = '/login';
            } else {
                errorToast();
                console.log("Error creating user: " + response.statusText);
            }
        })
        .catch(error => {
            errorToast();
            console.log("Error creating user: " + error);
        })
}

function errorToast() {
    const toast = bootstrap.Toast.getOrCreateInstance($('#toast'));
    const header = $('#toast').find(".toast-header strong");
    const body = $('#toast').find(".toast-body");
    header.removeClass("text-success");
    header.addClass("text-danger");
    header.html("Error Occurred");
    body.html("Error creating user.");
    toast.show();
}

$(document).ready(() => {
    $('#createUserForm').on('submit', (event) => {
        event.preventDefault();
        const username = $("#Username").val();
        const password = $("#Password").val();
        const name = $("#Name").val();
        createUser(username, password, name);
    });
})