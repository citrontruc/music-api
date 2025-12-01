# Music Api

**Work in progress**

## Description

A simple API in dotnet to store albums and let you consult these albums. It stores data in sqllite and uses scalar for documentation.

## Files & Folders

- Endpoints folder defines all the endpoinnts of the application.
- Models to define the objects we manipulate.
- Repositories
- Utils for helpers

## Run the api

Go to the folder where you cloned this repository and run

```bash
dotnet run .
```

In order to have a look at the documentation, got to the /scalar endpoint, you will find the swagger of the API.

## TODO

- Add Endpoints for groups / songs.
- Add tests.
- Making things async would be cool.
