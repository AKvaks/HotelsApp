# HotelsWebAPI

This is a .NET Core Web API designed as part of a technical interview.

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
