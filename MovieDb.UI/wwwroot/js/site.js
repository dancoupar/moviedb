// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.onload = function () {
    document.getElementById("searchButton").onclick = function () {
        performSearch(1);
	};
}

function performSearch(pageNumber) {
    const titleContains = document.getElementById("titleSearchInput").value;
    const maxResults = document.getElementById("maxResultsSelect").value;
    fetch(`https://localhost:7177/movies?titleContains=${titleContains}&maxNumberOfResults=${maxResults}&pageSize=20&pageNumber=${pageNumber}`)
        .then(response => response.json())
        .then(data => renderSearchResults(data))
        .catch(error => console.error('Error during search: ', error));
}

function renderSearchResults(data) {
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

    renderPagingLinks(data);
}

function renderPagingLinks(data) {
    const numberOfPages = Math.ceil(data.totalElements / data.pageSize);
    
    const pagingLinks = document.getElementById("pagingLinks");
    pagingLinks.innerHTML = "";

    for (let i = 1; i < numberOfPages + 1; i++) {
        let pageLink = document.createElement("a");
        pageLink.textContent = i.toString();
        pageLink.href = "#";
        pageLink.onclick = function () { performSearch(i); };
        pagingLinks.append(pageLink);
    }
}