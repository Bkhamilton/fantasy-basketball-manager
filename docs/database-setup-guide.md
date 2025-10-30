# Database Setup Quick Start Guide

This guide will help you set up the SQL Server database for the Fantasy Basketball Manager application.

## Prerequisites

You need one of the following:
- **SQL Server Express with LocalDB** (recommended for development)
- **SQL Server Express**
- **SQL Server Developer Edition**
- **Azure SQL Database**

## Installation

### Option 1: SQL Server Express LocalDB (Recommended for Development)

LocalDB is a lightweight version of SQL Server that's perfect for development.

**Windows:**
1. Download from [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Choose "Download now" under Express edition
3. During installation, select "Custom" and ensure "LocalDB" is selected
4. Complete the installation

**Verify Installation:**
```bash
sqllocaldb info
```

### Option 2: SQL Server Express

Full SQL Server Express installation:
1. Download from [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Run the installer and follow the wizard
3. Choose "Basic" installation for defaults
4. Note the connection information provided at the end

### Option 3: SQL Server Developer Edition

Full-featured version for development:
1. Download from [SQL Server Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Run the installer
3. Follow the installation wizard

## Database Setup Steps

### Step 1: Verify SQL Server is Running

**For LocalDB:**
```bash
sqllocaldb start mssqllocaldb
sqllocaldb info mssqllocaldb
```

**For SQL Server Express/Developer:**
- Open SQL Server Configuration Manager
- Ensure SQL Server service is running

### Step 2: Navigate to Project Directory

```bash
cd FantasyBasketballApi
```

### Step 3: Update Connection String (if needed)

**Important**: All subsequent commands should be run from the `FantasyBasketballApi` directory.

Edit `appsettings.json` if you're not using LocalDB:

**For SQL Server Express (default instance):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FantasyBasketballDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**For SQL Server with credentials:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FantasyBasketballDb;User Id=your_user;Password=your_password;TrustServerCertificate=true;"
  }
}
```

### Step 4: Apply Database Migrations

This creates the database and all tables:

```bash
dotnet ef database update
```

Expected output:
```
Build started...
Build succeeded.
Applying migration '20251030000205_InitialCreate'.
Done.
```

### Step 5: Verify Database Creation

**Using SQL Server Management Studio (SSMS):**
1. Open SSMS
2. Connect to `(localdb)\mssqllocaldb` or your SQL Server instance
3. Expand "Databases"
4. You should see "FantasyBasketballDb"
5. Expand it to see the "Players" and "NbaGames" tables

**Using Command Line:**
```bash
sqlcmd -S (localdb)\mssqllocaldb -d FantasyBasketballDb -Q "SELECT COUNT(*) as TableCount FROM INFORMATION_SCHEMA.TABLES"
```

### Step 6: Seed Initial Data

The database will be automatically seeded with 10 players when you first run the application:

```bash
dotnet run
```

The application will:
1. Create the database if it doesn't exist
2. Run migrations
3. Seed initial data (10 players + sample games)
4. Start the API server

## Verifying Setup

### Check if Tables Exist

```bash
sqlcmd -S (localdb)\mssqllocaldb -d FantasyBasketballDb -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"
```

Expected output:
```
TABLE_NAME
-----------------
Players
NbaGames
__EFMigrationsHistory
```

### Check Player Count

```bash
sqlcmd -S (localdb)\mssqllocaldb -d FantasyBasketballDb -Q "SELECT COUNT(*) as PlayerCount FROM Players"
```

Expected output: 10 players

### View Sample Data

```bash
sqlcmd -S (localdb)\mssqllocaldb -d FantasyBasketballDb -Q "SELECT TOP 5 Id, Name, Team, Position FROM Players"
```

## Alternative: Create Database Using SQL Script

If you prefer to use the SQL script directly:

1. Open SQL Server Management Studio
2. Connect to your server
3. Open the file: `FantasyBasketballApi/Data/Migrations/schema.sql`
4. Execute the script (F5)
5. Verify the database and tables were created

## Troubleshooting

### Issue: "Login failed for user"
**Solution:** Check your connection string credentials or use Windows Authentication (Trusted_Connection=true)

### Issue: "Cannot open database"
**Solution:** Make sure SQL Server is running and the database name is correct

### Issue: "LocalDB not found"
**Solution:** 
```bash
# List LocalDB instances
sqllocaldb info

# Create new instance if needed
sqllocaldb create "mssqllocaldb"

# Start the instance
sqllocaldb start "mssqllocaldb"
```

### Issue: dotnet-ef command not found
**Solution:**
```bash
dotnet tool install --global dotnet-ef
```

### Issue: Migration already applied
**Solution:** 
```bash
# Check migration status
dotnet ef migrations list

# If you need to rollback
dotnet ef database update 0

# Then reapply
dotnet ef database update
```

### Issue: Connection timeout
**Solution:** 
- Verify SQL Server is running
- Check firewall settings
- Try adding `;Timeout=60;` to connection string

## Common Commands

### Database Operations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply all pending migrations
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Generate SQL script
dotnet ef migrations script

# Remove last migration (if not applied)
dotnet ef migrations remove

# Drop database (careful!)
dotnet ef database drop
```

### SQL Server Operations

```bash
# Start LocalDB
sqllocaldb start mssqllocaldb

# Stop LocalDB
sqllocaldb stop mssqllocaldb

# List all LocalDB instances
sqllocaldb info

# Delete LocalDB instance
sqllocaldb delete mssqllocaldb
```

## Next Steps

After setting up the database:

1. **Run the application**: `dotnet run`
2. **Test API endpoints**: Navigate to `http://localhost:5000/api/players`
3. **Verify data**: Check that players are returned from the database
4. **Modify data**: Use the API to update player statuses
5. **Check persistence**: Restart the app and verify data persists

## Database Management Tools

Recommended tools for working with the database:

1. **SQL Server Management Studio (SSMS)** - [Download](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
2. **Azure Data Studio** - [Download](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio)
3. **Visual Studio Server Explorer** - Built into Visual Studio
4. **DBeaver** - Cross-platform database tool

## Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [SQL Server Express Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [SQL Server Configuration](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-configuration-manager)

## Support

If you encounter issues not covered in this guide:
1. Check the main [database-schema.md](database-schema.md) documentation
2. Review Entity Framework Core logs in the application output
3. Check SQL Server error logs
4. Consult the project README
