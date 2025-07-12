const url = "https://localhost:5001/api/";

function formatBookResult(book) {
    const row = $("<div class='row justify-content-center'>");
    row.html(`
                <div class="card mb-3">
                    <div class="row g-0">
                        <div class="col-md-2 text-center">
                            <img src=${book.coverImageUrl} id="bookImg" class="img-fluid m-3" alt="...">
                            <button type="button" class="btn btn-primary mb-3">Add to My Books</button>
                        </div>
                        <div class="col-md-10">
                            <div class="card-body">
                                <h5 class="card-title" id="bookTitle">${book.title}</h5>
                                <h6 class="card-subtitle mb-2 text-body-secondary" id="bookAuthor">By ${book.author}</h6>
                                <p class="card-text">
                                    <small class="text-body-secondary" id="bookPages">${book.pageCount} pages</small>
                                </p>
                                <p class="card-text">
                                    <small class="text-body-secondary" id="bookGenre">${book.genre}</small>
                                </p>
                                <p class="card-text" id="bookDescription">${book.description}</p>
                            </div>
                        </div>
                    </div>
                </div>
    `);
    row.find("button").on("click", () => {
        addBook(book);
    })
    return row;
}

async function searchBookData(input) {
    await fetch(url + `booksearch?query=${input}`, {
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    })
        .then(response => {
            if (response.ok) {
                response.json().then(data => {
                    const bookResults = $("#bookResults");
                    bookResults.html(``);
                    data.forEach(book => {
                        const row = formatBookResult(book);
                        bookResults.append(row);
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

async function addBook(book) {
    await fetch(url + `books`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify(book)
    })
        .then(response => {
            if (response.ok) {
                addBookToast();
            } else {
                if (response.status == "401") {
                    window.location.href = "/login";
                }
                errorToast();
                console.error("Error adding book:", response.statusText);
            }
        })
        .catch(error => {
            errorToast();
            console.error("Error adding book:", error);
        });
}

function addBookToast() {
    const toast = bootstrap.Toast.getOrCreateInstance($('#toast'));
    const header = $('#toast').find(".toast-header strong");
    const body = $('#toast').find(".toast-body");
    header.removeClass("text-danger");
    header.addClass("text-success");
    header.html("Book Added");
    body.html("Successfully added to My Books.");
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

function logout() {
    localStorage.removeItem("token");
    window.location.href = "/login";
}

$(document).ready(() => {
    const input = $("#bookSearch");
    const searchButton = $("#bookSearchButton");
    const navInput = $("#navBookSearch");
    const navSearchButton = $("#navBookSearchButton");
    const logoutButton = $("#logoutBtn");

    searchButton.on("click", () => {
        searchBookData(input.val());
    })
    navSearchButton.on("click", () => {
        $("#bookSearch").val(navInput.val());
        searchBookData(navInput.val());
    })
    logoutButton.on("click", () => {
        logout();
    })

    const searchParam = localStorage.getItem("searchParam");
    if (searchParam) {
        $("#bookSearch").val(searchParam);
        searchBookData(searchParam);
        localStorage.removeItem("searchParam");
    }
})