Here is a `README.md` file that includes instructions for setting up the connection string in API projects, restoring the database, and handling migrations for both the `CallCredit` and `FinancialService` applications.


# Project Setup Instructions

This guide will help you set up the database and run migrations for both the CallCredit and FinancialService projects.

## Prerequisites

Ensure you have the following installed on your system:
- .NET SDK
- Entity Framework Core CLI
- Sql Server 

## Configuration

First, update the connection strings in the `appsettings.json` file of each API project (`CallCredit.API` and `FinancialService.API`). These connection strings should point to your SQL server instances where the databases will be created.

Example of a connection string:
json
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;"
}


## Database Migrations and Updates

Follow these steps to set up and update your databases:

### For CallCredit Project

1. **Remove Existing Migrations (if applicable)**:
   If the `Migrations` folder already exists, delete it to start fresh.

2. **Add Initial Migration**:
   Run the following command in your terminal:
   ```bash
   dotnet ef migrations add InitialMigration --project CallCredit.Data --startup-project CallCredit.API
   ```

3. **Update Database**:
   Apply the migration to your database:
   ```bash
   dotnet ef database update --project CallCredit.Data --startup-project CallCredit.API
   ```

### For FinancialService Project

1. **Remove Existing Migrations (if applicable)**:
   Similarly, if the `Migrations` folder exists, remove it before starting.

2. **Add Initial Migration**:
   Execute the following command:
   ```bash
   dotnet ef migrations add InitialMigration --project FinancialService.Data --startup-project FinancialService.API
   ```

3. **Update Database**:
   Apply the migration with this command:
   ```bash
   dotnet ef database update --project FinancialService.Data --startup-project FinancialService.API
   ```

## Final Notes

After running these commands, your databases should be set up and connected to your respective projects. If you encounter any issues, check the connection strings and ensure your SQL server is accessible.


