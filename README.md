# HotelsWebAPI

This is a .NET Core 8 Web API designed as part of a technical interview. After many hours of research, I decided to stick to good old KISS principle. For database, I used Entity Framework and Code-First approach. I implemented NetTopologySuite to be able to work with geolocations. Other than that, I used MediatR and Fluent Validation.

## Assignment

The assignment was to develop a JSON REST web service for hotel search. The service had to have two API interfaces. One was CRUD interface for hotels and the other was search interface. In the search interface, the list of hotels is ordered by price and proximity to user's location.

## Migrations

To create a new migration use the following console command

### Creating migration

> [!NOTE]
> Position yourself in the solution folder

```console
Add-Migration MIGRATION_NAME -OutputDir DAL/Migrations
```

> [!NOTE]
> Update Db fom the console or just run the application

```console
Update-Database
```

## Removing last migration

> [!CAUTION]
> This should be done ONLY IF migration was not applied to the Db

```console
Remove-Migration
```
