# Fantasy Basketball Manager

A .NET and React application that helps users manage their fantasy basketball lineups with intelligent start/sit recommendations.

## Features

- **Player Cards** - View player information including:
  - Today's games and matchups
  - Injury status
  - Season statistics (Points, Rebounds, Assists, FG%)
  
- **Drag & Drop Roster Management** - Easily move players between:
  - 🏀 Starting Lineup
  - 💺 Bench

- **Start/Sit Recommendations** - AI-powered analysis based on:
  - Game schedule (playing today vs not playing)
  - Injury status (Healthy, Questionable, Doubtful, Out)
  - Player performance statistics
  - Confidence scores and detailed reasoning

- **Daily NBA Schedule** - View all games scheduled for today

## Architecture

### Backend (.NET 9.0 API)

Located in `/FantasyBasketballApi`

**Key Endpoints:**

- `GET /api/players` - Get all players
- `GET /api/players/with-games-today` - Get players with games today
- `GET /api/players/{id}` - Get specific player
- `POST /api/roster/move-to-starting/{id}` - Move player to starting lineup
- `POST /api/roster/move-to-bench/{id}` - Move player to bench
- `GET /api/roster` - Get current roster (starting + bench)
- `GET /api/analysis/start-sit-recommendations` - Get AI recommendations
- `GET /api/schedule/today` - Get today's NBA schedule
- `POST /api/sportsdb/fetch-and-insert-players` - Fetch all NBA players from SportsDB and insert into database
- `GET /api/sportsdb/fetch-players/{teamName}` - Fetch players for a specific team from SportsDB

**Technology Stack:**
- ASP.NET Core Web API
- Dependency Injection
- RESTful API design
- CORS enabled for React frontend
- SportsDB API integration for player data

### Frontend (React + Vite)

Located in `/fantasy-basketball-client`

**Components:**
- `PlayerCard` - Displays player info, stats, injuries, and games
- `RosterManager` - Drag-and-drop interface for roster management
- `Recommendations` - Shows start/sit analysis with confidence scores
- `Schedule` - Displays today's NBA games

**Technology Stack:**
- React 18
- Vite (build tool)
- @hello-pangea/dnd (drag and drop)
- CSS3 (responsive design)

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- npm (comes with Node.js)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or LocalDB recommended for development)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Bkhamilton/fantasy-basketball-manager.git
   cd fantasy-basketball-manager
   ```

2. **Setup Backend**
   ```bash
   cd FantasyBasketballApi
   dotnet restore
   dotnet build
   ```

3. **Setup Database** (Optional for now - can use in-memory data)
   
   The project includes SQL Server database schema for Phase 1 development. The application currently uses in-memory data by default but includes full database infrastructure:
   ```bash
   # Install EF Core tools if not already installed
   dotnet tool install --global dotnet-ef
   
   # Create and seed the database
   dotnet ef database update
   ```
   
   For detailed database setup instructions, see [docs/database-setup-guide.md](docs/database-setup-guide.md)

4. **Configure SportsDB API (Optional - for importing real NBA player data)**
   
   To fetch real NBA player data from TheSportsDB:
   - Get a free API key from [TheSportsDB.com](https://www.thesportsdb.com/)
   - Set the environment variable:
     ```bash
     # Windows PowerShell
     $env:SPORTSDB_API_KEY="your_api_key_here"
     
     # Linux/macOS
     export SPORTSDB_API_KEY=your_api_key_here
     ```
   
   For detailed SportsDB integration instructions, see [docs/sportsdb-integration.md](docs/sportsdb-integration.md)

5. **Setup Frontend**
   ```bash
   cd ../fantasy-basketball-client
   npm install
   ```

### Running the Application

You need to run both the backend and frontend servers:

#### Terminal 1 - Backend API
```bash
cd FantasyBasketballApi
dotnet run
```
The API will start at `http://localhost:5000`

#### Terminal 2 - Frontend
```bash
cd fantasy-basketball-client
npm run dev
```
The React app will start at `http://localhost:5173` (or similar port shown in terminal)

### Usage

1. Open your browser to the frontend URL (typically `http://localhost:5173`)
2. View your roster in the "My Roster" tab
3. Drag and drop players between Starting Lineup and Bench
4. Check "Recommendations" tab for AI-powered start/sit advice
5. View "Today's Games" tab to see the NBA schedule

## Development

### Backend Development

The backend now supports SQL Server database persistence using Entity Framework Core. For detailed information:

- **Database Schema**: See [docs/database-schema.md](docs/database-schema.md) for complete schema documentation
- **Setup Guide**: See [docs/database-setup-guide.md](docs/database-setup-guide.md) for step-by-step setup instructions

The application currently uses in-memory data in `PlayerService.cs` for backward compatibility. Future development will migrate to use the database context for data persistence.

### Frontend Development

The frontend is built with React and uses the Fetch API to communicate with the backend.

To modify styling:
- Edit component-specific CSS files in `src/components/`
- Modify global styles in `src/App.css` and `src/index.css`

### API Testing

You can test the API endpoints using:
- Browser: Navigate to `http://localhost:5000/api/players`
- cURL: `curl http://localhost:5000/api/players`
- Postman or similar API testing tools

## Project Structure

```
fantasy-basketball-manager/
├── FantasyBasketballApi/          # .NET Backend
│   ├── Controllers/                # API endpoints
│   │   ├── PlayersController.cs
│   │   ├── RosterController.cs
│   │   ├── AnalysisController.cs
│   │   └── ScheduleController.cs
│   ├── Models/                     # Data models
│   │   ├── Player.cs
│   │   ├── StartSitRecommendation.cs
│   │   └── NbaGame.cs
│   ├── Services/                   # Business logic
│   │   ├── PlayerService.cs
│   │   ├── AnalysisService.cs
│   │   └── NbaScheduleService.cs
│   ├── Data/                       # Database context and migrations
│   │   ├── FantasyBasketballContext.cs
│   │   ├── DbInitializer.cs
│   │   └── Migrations/             # EF Core migrations
│   └── Program.cs                  # App configuration
├── fantasy-basketball-client/      # React Frontend
│   ├── src/
│   │   ├── components/             # React components
│   │   │   ├── PlayerCard.jsx
│   │   │   ├── RosterManager.jsx
│   │   │   ├── Recommendations.jsx
│   │   │   └── Schedule.jsx
│   │   ├── App.jsx                 # Main app component
│   │   └── main.jsx                # Entry point
│   └── package.json
├── docs/                           # Documentation
│   ├── plans.md                    # Development roadmap
│   ├── database-schema.md          # Database schema documentation
│   ├── database-setup-guide.md     # Database setup instructions
│   └── sportsdb-integration.md     # SportsDB API integration guide
└── README.md
```

## Future Enhancements

- User authentication and multiple roster support
- Real-time NBA data integration
- Historical player performance tracking
- Advanced analytics and projections
- Mobile app version
- Push notifications for player updates
- League integration (ESPN, Yahoo, etc.)

## License

This project is open source and available under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
