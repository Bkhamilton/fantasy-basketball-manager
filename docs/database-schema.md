# Fantasy Basketball Manager - Database Schema

## Overview

This document describes the SQL Server database schema for the Fantasy Basketball Manager application. The schema is designed to support data migration from in-memory storage to a persistent SQL Server database.

## Database Tables

### Players Table

Stores information about NBA players in the fantasy roster.

**Columns:**
- `Id` (int, Primary Key, Identity) - Unique identifier for each player
- `Name` (nvarchar(100), Required) - Player's full name
- `Team` (nvarchar(10), Required) - NBA team abbreviation (e.g., LAL, GSW)
- `Position` (nvarchar(10), Required) - Player position (PG, SG, SF, PF, C)
- `IsStarting` (bit, Required) - Whether player is in starting lineup

**Player Statistics (Owned Entity):**
- `Stats_Points` (decimal(5,2)) - Average points per game
- `Stats_Rebounds` (decimal(5,2)) - Average rebounds per game
- `Stats_Assists` (decimal(5,2)) - Average assists per game
- `Stats_Steals` (decimal(5,2)) - Average steals per game
- `Stats_Blocks` (decimal(5,2)) - Average blocks per game
- `Stats_FieldGoalPercentage` (decimal(5,2)) - Field goal percentage
- `Stats_ThreePointPercentage` (decimal(5,2)) - Three-point percentage

**Injury Information (Owned Entity):**
- `Injury_Status` (nvarchar(50)) - Status: Healthy, Questionable, Doubtful, Out
- `Injury_Description` (nvarchar(500)) - Optional injury description

**Game Today (Owned Entity):**
- `GameToday_HasGame` (bit) - Whether player has a game today
- `GameToday_Opponent` (nvarchar(10)) - Opponent team abbreviation
- `GameToday_Time` (nvarchar(20)) - Game time
- `GameToday_IsHomeGame` (bit) - Whether it's a home game

### NbaGames Table

Stores NBA game schedule information.

**Columns:**
- `Id` (int, Primary Key, Identity) - Unique identifier for each game
- `HomeTeam` (nvarchar(10), Required) - Home team abbreviation
- `AwayTeam` (nvarchar(10), Required) - Away team abbreviation
- `GameTime` (datetime2, Required) - Scheduled game time
- `Status` (nvarchar(50), Required) - Game status: Scheduled, Live, Final

## Entity Relationships

The schema uses owned entity types for complex objects:
- **PlayerStats** - Owned by Player
- **InjuryStatus** - Owned by Player
- **GameToday** - Owned by Player

This design keeps related data together in a single table while maintaining logical separation in the code.

## Connection String

The default connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FantasyBasketballDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Connection String Options

For different SQL Server configurations, you can modify the connection string:

**Local SQL Server:**
```
Server=localhost;Database=FantasyBasketballDb;Trusted_Connection=true;
```

**SQL Server with username/password:**
```
Server=localhost;Database=FantasyBasketballDb;User Id=your_username;Password=your_password;
```

**Azure SQL Database:**
```
Server=tcp:your-server.database.windows.net,1433;Database=FantasyBasketballDb;User Id=your_username;Password=your_password;Encrypt=true;
```

## Database Migrations

### Creating the Database

The Entity Framework Core migrations system is used to create and update the database schema.

**Initial Migration (Already Created):**
```bash
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
```

**Apply Migration to Database:**
```bash
dotnet ef database update
```

This will create the database and all tables based on the migration scripts.

### Viewing Migration Status
```bash
dotnet ef migrations list
```

### Removing Last Migration (if needed):**
```bash
dotnet ef migrations remove
```

### Creating New Migrations

When you make changes to the models:
```bash
dotnet ef migrations add DescriptiveNameOfChange
dotnet ef database update
```

## Seed Data

The `DbInitializer` class in `Data/DbInitializer.cs` provides seed data for initial development and testing.

### Included Players (10 total)

1. **LeBron James** - LAL, SF, Starting
2. **Stephen Curry** - GSW, PG, Starting
3. **Kevin Durant** - PHX, SF, Bench
4. **Giannis Antetokounmpo** - MIL, PF, Starting
5. **Joel Embiid** - PHI, C, Bench (Injured)
6. **Luka Doncic** - DAL, PG, Starting
7. **Damian Lillard** - MIL, PG, Bench
8. **Jayson Tatum** - BOS, SF, Bench
9. **Nikola Jokic** - DEN, C, Starting (NEW)
10. **Anthony Davis** - LAL, PF, Starting (NEW)

### Seeding Process

The database is automatically seeded when the application starts if no data exists:

```csharp
// In Program.cs or Startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FantasyBasketballContext>();
    DbInitializer.Initialize(context);
}
```

## Database Context

The `FantasyBasketballContext` class provides the Entity Framework Core DbContext:

```csharp
public class FantasyBasketballContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<NbaGame> NbaGames { get; set; }
}
```

## Usage Examples

### Querying Players
```csharp
// Get all players
var players = await context.Players.ToListAsync();

// Get starting players
var starters = await context.Players
    .Where(p => p.IsStarting)
    .ToListAsync();

// Get players with games today
var playingToday = await context.Players
    .Where(p => p.GameToday.HasGame)
    .ToListAsync();
```

### Updating Player Status
```csharp
var player = await context.Players.FindAsync(id);
if (player != null)
{
    player.IsStarting = true;
    await context.SaveChangesAsync();
}
```

### Adding New Players
```csharp
var newPlayer = new Player
{
    Name = "New Player",
    Team = "NYK",
    Position = "SG",
    IsStarting = false,
    Stats = new PlayerStats { Points = 20.0, ... },
    Injury = new InjuryStatus { Status = "Healthy" }
};
context.Players.Add(newPlayer);
await context.SaveChangesAsync();
```

## Next Steps for Phase 1

1. **Database Setup**: Ensure SQL Server or LocalDB is installed
2. **Apply Migrations**: Run `dotnet ef database update`
3. **Verify Connection**: Test database connectivity
4. **Update Services**: Migrate PlayerService to use DbContext instead of in-memory data
5. **Test Data Migration**: Verify all existing functionality works with database
6. **Performance**: Add indexes as needed for frequently queried columns

## Database Management Tools

### Recommended Tools:
- **SQL Server Management Studio (SSMS)** - Full-featured database management
- **Azure Data Studio** - Cross-platform database tool
- **Visual Studio Server Explorer** - Built into Visual Studio
- **dotnet ef** CLI - Command-line database operations

## Backup and Recovery

### Backing Up the Database
```bash
# Using SQL Server Management Studio
Right-click database → Tasks → Back Up...

# Or use T-SQL
BACKUP DATABASE FantasyBasketballDb 
TO DISK = 'C:\Backups\FantasyBasketballDb.bak'
```

### Restoring the Database
```bash
# Using SQL Server Management Studio
Right-click Databases → Restore Database...

# Or use T-SQL
RESTORE DATABASE FantasyBasketballDb 
FROM DISK = 'C:\Backups\FantasyBasketballDb.bak'
```

## Performance Considerations

### Recommended Indexes (Future Enhancement)
```sql
-- Index for team lookups
CREATE INDEX IX_Players_Team ON Players(Team);

-- Index for position lookups
CREATE INDEX IX_Players_Position ON Players(Position);

-- Index for starting lineup queries
CREATE INDEX IX_Players_IsStarting ON Players(IsStarting);

-- Index for game schedule queries
CREATE INDEX IX_NbaGames_GameTime ON NbaGames(GameTime);
```

## Security Notes

1. **Connection Strings**: In production, use environment variables or Azure Key Vault for connection strings
2. **User Permissions**: Grant only necessary database permissions to the application user
3. **SQL Injection**: Entity Framework Core provides parameterized queries by default
4. **Sensitive Data**: Consider encrypting sensitive player information if required

## Troubleshooting

### Common Issues

**Issue**: Migration fails with "database already exists"
**Solution**: Either drop the database or use `dotnet ef database update` to apply pending migrations

**Issue**: Connection string error
**Solution**: Verify SQL Server is running and connection string is correct in appsettings.json

**Issue**: LocalDB not found
**Solution**: Install SQL Server Express with LocalDB or use a full SQL Server instance

## References

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
