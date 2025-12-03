# Music Api

**Work in progress**

## Description

A simple API in dotnet to store albums and let you consult these albums in a database. We have no delete / put or patch methods for now.

It stores data in sqllite and uses scalar for documentation. Linting is handled with csharpier.

## Files & Folders

- Database contains database context and code to access our database.
- Endpoints folder defines all the endpoinnts of the application.
- Models to define the objects we manipulate.
- Repositories to implement repository pattern.
- Utils for helpers.

## Run the api

Go to the folder where you cloned this repository and run

```bash
dotnet run .
```

In order to have a look at the documentation, got to the /scalar endpoint, you will find the swagger of the API.

## TODO

- Add Endpoints for groups / songs.
- Add tests.
