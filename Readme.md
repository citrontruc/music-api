# Music Api

This is a **Work in progress**

## Table of content

- [Music Api](#music-api)
  - [Table of content](#table-of-content)
  - [Description](#description)
  - [Files \& Folders](#files--folders)
  - [Run the api](#run-the-api)
  - [TODO](#todo)

## Description

A simple API in dotnet to store albums and let you consult these albums in a database. We have no delete / put or patch methods for now.

It stores data in sqllite and uses scalar for documentation. Linting is handled with csharpier.

This is meant as an exercise in order to get comfortable with api versioning, asynchronous calls, api endpoints...

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

- Try to migrate doc to scalar.
- Separate endpoint registering from endpoint logic.
- Add Global filters to remove search elements marked for deletion.
- Interations between artists and albums (Add artists if does no exist).
- Make artists and albums case insensitive.

- Add tests.
- Add cache of data
