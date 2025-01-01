# CarManagementAPI

1.Applying Existing Migrations

    Ensure Database Connection:
        Check the connection string in your appsettings.json file to ensure it points to the correct database.

    Run the Update Command:
        Open Package Manager Console and run:

        Update-Database

        This command applies all pending migrations to your database.

If You Need to Redo Migrations:

    Delete the Existing Database (if required):
        You can manually delete the database from your SQL Server or use a command:

    Drop-Database

Remove Existing Migrations:

    If migrations need to be re-created, delete the Migrations folder or use:

    Remove-Migration

    Repeat until all migrations are removed.

Re-Create Migrations:

    Create a new migration:

    Add-Migration InitialCreate

Apply the New Migration:

    Apply the newly created migration:

Update-Database

2.To start the backend, follow these steps:

    Open Package Manager Console in your project.
    Run the following command:

dotnet run

This will start the backend server port 8080. Ensure that the correct port and settings are configured in your launchSettings.json file.



Note:

There is a minor issue after creating a car:

    The newly created car may not immediately appear with its associated garages in the frontend.
    However, if you refresh the frontend after creating the car, the car will display correctly with its assigned garages.
