const url = "https://localhost:5001"

async function login(username, password, rememberMe) {
    await fetch(url + "/api/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ username: username, password: password, rememberMe: rememberMe})
    })
        .then(response => {
            if (response.ok) {
                response.json().then(data => {
                    localStorage.setItem("token", data.token);
                    window.location.href = '/';
                })
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
    body.html("Error logging in.");
    toast.show();
}

$(document).ready(() => {
    $('#loginForm').on('submit', (event) => {
        event.preventDefault();
        const username = $("#Username").val();
        const password = $("#Password").val();
        const rememberMe = $('#RememberMe').is(':checked');
        login(username, password, rememberMe);
    });
})