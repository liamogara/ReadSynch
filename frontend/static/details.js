const url = "https://localhost:5001/api/";

async function loadBookDetails(bookId) {
    await fetch(url + "books/" + bookId, {
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    })
        .then(response => {
            if (response.ok) {
                response.json()
                    .then(book => {
                        $("#bookImg").attr('src', book.coverImageUrl);
                        $("#bookTitle").html(book.title);
                        $("#bookAuthor").html("By " + book.author);
                        $("#bookStatus").html(`
                                <p>${book.status}</p>
                                <div class="btn-group">
                                    <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" aria-expanded="false">
                                        Edit
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li><button class="dropdown-item statusBtn" type="button">Not Read</button></li>
                                        <li><button class="dropdown-item statusBtn" type="button">Reading</button></li>
                                        <li><button class="dropdown-item statusBtn" type="button">Read</button></li>
                                    </ul>
                                </div>
                        `);
                        $("#bookRating").html(createRating(book.id, book.rating, bookId));
                        $("#bookDescription").html(book.description);
                        $("#bookPages").html(book.pageCount + " pages");

                        $("#bookStatus").find(".statusBtn").on("click", (event) => {
                            newStatus = event.target.textContent;
                            updateStatus(book.id, newStatus, bookId);
                        });
                    })
            } else {
                if (response.status == "401") {
                    window.location.href = "/login";
                }
                errorToast();
                console.log("Error searching book: " + response.statusText);
            }
        })
        .catch(error => {
            errorToast();
            console.log("Error searching book: " + error);
        })
}

async function updateStatus(id, newStatus, bookId) {
    await fetch(url + `books/${id}/status`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({ status: newStatus })
    })
        .then(response => {
            if (response.ok) {
                loadBookDetails(bookId);
                updateStatusToast();
            } else {
                if (response.status == "401") {
                    window.location.href = "/login";
                }
                errorToast();
                console.error("Error updating book:", response.statusText);
            }
        })
        .catch(error => {
            errorToast();
            console.error("Error updating book:", error);
        });
}

function createRating(id, rating, bookId) {
    const stars = $("<div>");
    for (let i = 0; i < 5; i++) {
        const star = $("<i style='cursor: pointer'>");
        if (i < rating) {
            star.addClass("bi bi-star-fill star");
        } else {
            star.addClass("bi bi-star star");
        }
        star.hover(
            function () {
                $(this).parent().children("i").each(function (index) {
                    $(this).toggleClass("bi-star-fill", index <= i);
                    $(this).toggleClass("bi-star", index > i);
                })
            },
            function () {
                $(this).parent().children("i").each(function (index) {
                    $(this).toggleClass("bi-star-fill", index < rating);
                    $(this).toggleClass("bi-star", index >= rating);
                })
            }
        );
        star.on("click", () => {
            updateRating(id, i + 1, bookId);
        })
        stars.append(star);
    }
    return stars;
}

async function updateRating(id, newRating, bookId) {
    await fetch(url + `books/${id}/rating`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({ rating: newRating })
    })
        .then(response => {
            if (response.ok) {
                loadBookDetails(bookId);
                updateRatingToast();
            } else {
                if (response.status == "401") {
                    window.location.href = "/login";
                }
                errorToast();
                console.error("Error updating book:", response.statusText);
            }
        })
        .catch(error => {
            errorToast();
            console.error("Error updating book:", error);
        });
}

function updateStatusToast() {
    const toast = bootstrap.Toast.getOrCreateInstance($('#toast'));
    const header = $('#toast').find(".toast-header strong");
    const body = $('#toast').find(".toast-body");
    header.removeClass("text-danger");
    header.addClass("text-success");
    header.html("Book Status Updated");
    body.html("Successfully updated book status.");
    toast.show();
}

function updateRatingToast() {
    const toast = bootstrap.Toast.getOrCreateInstance($('#toast'));
    const header = $('#toast').find(".toast-header strong");
    const body = $('#toast').find(".toast-body");
    header.removeClass("text-danger");
    header.addClass("text-success");
    header.html("Book Rating Updated");
    body.html("Successfully updated book rating.");
    toast.show();
}

function errorToast() {
    const toast = bootstrap.Toast.getOrCreateInstance($('#toast'));
    const header = $('#toast').find(".toast-header strong");
    const body = $('#toast').find(".toast-body");
    header.removeClass("text-success");
    header.addClass("text-danger");
    header.html("Error Occurred");
    body.html("An error occurred during the request.");
    toast.show();
}

function searchBookData(input) {
    localStorage.setItem("searchParam", input);
    window.location.href = "/browse";
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "/login";
}

$(document).ready(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const bookId = urlParams.get('book');
    const navInput = $("#navBookSearch");
    const navSearchButton = $("#navBookSearchButton");
    const logoutButton = $("#logoutBtn");

    loadBookDetails(bookId);

    navSearchButton.on("click", () => {
        searchBookData(navInput.val());
    })
    logoutButton.on("click", () => {
        logout();
    })

})