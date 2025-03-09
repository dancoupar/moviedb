# Running the Solution

## Docker Desktop

Clone the repo, switch to the root and run `docker-compose up --build`

Once the images have been built and the containers spun up, the UI should be listening on `localhost:5002` and the API on `localhost:5001`.

## Visual Studio

Clone the repo and open `moviedb.sln`. Select the "DefaultProfile" launch profile and hit **Start**. The UI should start up on `localhost:5192` and the API on `localhost:5223`.

Note: If for whatever reason you need to use different ports, a code change will be needed in `Program.cs` within the `MovieDb.Api` project to allow CORS.

# Remarks

If I had unlimited time and/or if this was a real life production system, I'd approach things a bit differently.

- I would not use SQLite, which is quite limiting. A more production oriented RDBMS like SQL Server would be more appropriate and would have better support for 'contains' text searches that utilise indexes. In addition, I'd look to use something like Elasticsearch, as this sort of thing is its bread and butter.

- I'd not use ASP.NET Core MVC for the front end and would instead take a more modern approach by creating a SPA using a JavaScript framework such as ReactJS.

- I'd potentially think about re-architecting into a clean or onion architecture, allowing for better separation of concerns by introducing application, domain and infra layers, then applying dependency injection. However, in terms of current functionality, I'm not convinced it justifies this complexity and I'm not a big fan of over engineering things unless it's warranted.

- Along similar lines, I'd potentially use the repository pattern.

- I'd potentially refactor so that caching is a separate concern by applying the decorator pattern.

- The data could be normalised further e.g. by having an `Genres` table and changing `MovieGenre` to have a FK to that, rather than duplicate genre descriptions. The same applies to actors.

- The actor names are not accurate in terms of who starred in which movie.
