// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var currentPage;
var currentSort;
var sortDescending;

window.onload = function () {    
    document.getElementById("searchButton").onclick = function () {
        currentPage = 1;
        currentSort = "Title";
        sortDescending = false;
        performSearch();
    };
    document.getElementById("titleSortLink").onclick = function () {
        applySort("Title");
    };
    document.getElementById("releaseDateSortLink").onclick = function () {
        applySort("ReleaseDate");
    };
    document.addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            document.getElementById("searchButton").click();
        }
    });
    fetchGenres();
}

function performSearch() {
    const titleContains = document.getElementById("titleSearchInput").value;
    const actorContains = document.getElementById("actorSearchInput").value;
    const maxResults = document.getElementById("maxResultsSelect").value;
    let searchUrl = `${window.appSettings.apiUrl}/movies`
        + `?titleContains=${titleContains}`
        + `&actorContains=${actorContains}`
        + `&maxNumberOfResults=${maxResults}`
        + "&pageSize=20"
        + `&pageNumber=${currentPage}`
        + `&sortBy=${currentSort}`
        + `&sortDescending=${sortDescending}`;
    const genreBoxes = document.getElementById("genres").getElementsByTagName("input");
    for (let i = 0; i < genreBoxes.length; i++) {
        if (genreBoxes[i].checked) {
            searchUrl += `&genres=${genreBoxes[i].value}`;
        }
    }
    fetch(searchUrl)        
        .then(response => {
            return response.json().then(data => {
                if (response.ok) {
                    renderSearchResults(data);
                }
                else {
                    handleError(data);
                }
            });
        })
        .catch(error => { console.log("Error: " + error); });
}

function applySort(sortBy) {
    if (currentSort !== sortBy) {
        currentSort = sortBy;
        sortDescending = false;
    }
    else {
        sortDescending = !sortDescending;
    }
    performSearch();
}

function handleError(data) {    
    const errors = document.getElementById("errors");
    errors.innerHTML = "";
    for (let field in data.errors) {
        if (data.errors.hasOwnProperty(field)) {
            const errorMessages = data.errors[field];
            errorMessages.forEach(message => {
                errors.appendChild(document.createTextNode(message));
                errors.appendChild(document.createElement("br"));
            });
        }
    }
}

function renderSearchResults(data) {
    document.getElementById("errors").innerHTML = "";    

    if (data.content.length > 0) {
        currentPage = data.pageNumber;

        document.getElementById("searchTable").removeAttribute("hidden");
        const tableBody = document.getElementById("searchResults");
        tableBody.innerHTML = "";

        data.content.forEach(item => {
            const row = document.createElement("tr");

            const titleCell = document.createElement("td");
            titleCell.textContent = item.title;
            row.appendChild(titleCell);

            const releaseDateCell = document.createElement("td");
            releaseDateCell.textContent = item.releaseDate;
            row.appendChild(releaseDateCell);

            const genreCell = document.createElement("td");
            genreCell.textContent = item.genre;
            row.appendChild(genreCell);

            tableBody.appendChild(row);
        });
    }
    else {
        document.getElementById("searchTable").setAttribute("hidden", "hidden");
        const errors = document.getElementById("errors");
        errors.appendChild(document.createTextNode("No movies matching the search criteria were found."));
    }

    renderPagingLinks(data);
}

function renderPagingLinks(data) {
    const numberOfPages = Math.ceil(data.totalElements / data.pageSize);
    
    const pagingLinks = document.getElementById("pagingLinks");
    pagingLinks.innerHTML = "";

    for (let i = 1; i < numberOfPages + 1; i++) {
        let pageLink = document.createElement("a");
        pageLink.textContent = i;
        pageLink.href = "#";
        pageLink.onclick = function () { currentPage = i; performSearch(); };
        pagingLinks.appendChild(pageLink);
    }
}

function fetchGenres() {
    fetch(`${ window.appSettings.apiUrl }/genres`)
        .then(response => {
            return response.json().then(data => {
                if (response.ok) {
                    populateGenres(data);
                }
                else {
                    handleError(data);
                }
            });
        })
        .catch(error => { console.log("Error: " + error); });
}

function populateGenres(data) {        
    const genres = document.getElementById("genres");

    for (let i = 0; i < data.length; i++) {
        let checkBox = document.createElement("input");
        checkBox.setAttribute("id", data[i]);
        checkBox.setAttribute("type", "checkbox");
        checkBox.setAttribute("value", data[i]);
        genres.appendChild(checkBox);

        let label = document.createElement("label");
        label.setAttribute("for", data[i]);
        label.textContent = data[i];
        genres.appendChild(label);

        if (i === Math.round(data.length / 2)) {
            genres.appendChild(document.createElement("br"));
        }
    }
}