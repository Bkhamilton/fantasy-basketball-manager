# SportsDB Integration

This document explains how to use the SportsDB API integration to fetch and store NBA player data in the database.

## Prerequisites

1. Get a SportsDB API key from [TheSportsDB.com](https://www.thesportsdb.com/)
2. Set up the environment variable or configuration for the API key

## Configuration

### Option 1: Environment Variable (Recommended)

Set the `SPORTSDB_API_KEY` environment variable:

**Windows (PowerShell):**
```powershell
$env:SPORTSDB_API_KEY="your_api_key_here"
```

**Windows (Command Prompt):**
```cmd
set SPORTSDB_API_KEY=your_api_key_here
```

**Linux/macOS:**
```bash
export SPORTSDB_API_KEY=your_api_key_here
```

### Option 2: Configuration File

Update `appsettings.Development.json`:
```json
{
  "SPORTSDB_API_KEY": "your_api_key_here",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Note:** Never commit your actual API key to source control. The `appsettings.Development.json` file should not contain actual secrets in production.

## API Endpoints

### 1. Fetch and Insert All NBA Players

Fetches players from all 30 NBA teams and inserts them into the database.

**Endpoint:** `POST /api/sportsdb/fetch-and-insert-players`

**Example using curl:**
```bash
curl -X POST http://localhost:5000/api/sportsdb/fetch-and-insert-players
```

**Example using PowerShell:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/sportsdb/fetch-and-insert-players" -Method Post
```

**Response:**
```json
{
  "success": true,
  "message": "Successfully inserted 450 players",
  "players": [...]
}
```

### 2. Fetch Players for a Specific Team

Fetches players for a single NBA team (without inserting into database).

**Endpoint:** `GET /api/sportsdb/fetch-players/{teamName}`

**Team Name Format:** City_TeamName (e.g., `Los_Angeles_Lakers`, `Boston_Celtics`)

**Example:**
```bash
curl http://localhost:5000/api/sportsdb/fetch-players/Los_Angeles_Lakers
```

**Response:**
```json
{
  "success": true,
  "teamName": "Los_Angeles_Lakers",
  "count": 15,
  "players": [...]
}
```

## Supported NBA Teams

The following teams are supported (use these exact names in the API):

- Atlanta_Hawks
- Boston_Celtics
- Brooklyn_Nets
- Charlotte_Hornets
- Chicago_Bulls
- Cleveland_Cavaliers
- Dallas_Mavericks
- Denver_Nuggets
- Detroit_Pistons
- Golden_State_Warriors
- Houston_Rockets
- Indiana_Pacers
- Los_Angeles_Clippers
- Los_Angeles_Lakers
- Memphis_Grizzlies
- Miami_Heat
- Milwaukee_Bucks
- Minnesota_Timberwolves
- New_Orleans_Pelicans
- New_York_Knicks
- Oklahoma_City_Thunder
- Orlando_Magic
- Philadelphia_76ers
- Phoenix_Suns
- Portland_Trail_Blazers
- Sacramento_Kings
- San_Antonio_Spurs
- Toronto_Raptors
- Utah_Jazz
- Washington_Wizards

## Implementation Details

### Service: `SportsDbService`

The service handles:
- Fetching player data from SportsDB API
- Mapping SportsDB player data to internal Player model
- Checking for duplicate players before insertion
- Inserting new players into the database
- Team abbreviation mapping
- Position normalization

### Controller: `SportsDbController`

Exposes two endpoints:
1. `POST /api/sportsdb/fetch-and-insert-players` - Batch insert all NBA players
2. `GET /api/sportsdb/fetch-players/{teamName}` - Fetch players for a specific team

### Models: `SportsDbModels`

Contains:
- `SportsDbPlayerResponse` - Wrapper for API response
- `SportsDbPlayer` - Individual player data from SportsDB API

## Error Handling

The implementation includes comprehensive error handling:
- Missing API key detection
- HTTP request errors
- JSON deserialization errors
- Duplicate player detection
- Per-team error isolation (one team failure doesn't stop the whole process)

## Logging

The service logs:
- Start/completion of operations
- Each team being processed
- Players being added
- Warnings for teams with no players
- Errors with stack traces

Check the application logs for detailed information about the import process.

## Notes

- The service adds a 250ms delay between team requests to avoid overwhelming the API
- Duplicate players (same name and team) are automatically skipped
- Players are inserted with default stats (0 values) and "Healthy" injury status
- Position names are normalized to standard abbreviations (PG, SG, SF, PF, C)
