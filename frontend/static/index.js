function searchBookData(input) {
    localStorage.setItem("searchParam", input);
    window.location.href = "/browse";
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "/login";
}

$(document).ready(() => {
    const navInput = $("#navBookSearch");
    const navSearchButton = $("#navBookSearchButton");
    const logoutButton = $("#logoutBtn");

    navSearchButton.on("click", () => {
        searchBookData(navInput.val());
    })
    logoutButton.on("click", () => {
        logout();
    })
})