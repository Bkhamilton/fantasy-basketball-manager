import './Schedule.css';

const Schedule = ({ games }) => {
  const formatTime = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleTimeString('en-US', { 
      hour: 'numeric', 
      minute: '2-digit',
      hour12: true 
    });
  };

  return (
    <div className="schedule">
      <h2>📅 Today's NBA Schedule</h2>
      <div className="games-grid">
        {games.map((game) => (
          <div key={game.id} className="game-card">
            <div className="game-status">{game.status}</div>
            <div className="matchup">
              <div className="team away-team">
                <span className="team-label">AWAY</span>
                <span className="team-name">{game.awayTeam}</span>
              </div>
              <div className="vs">@</div>
              <div className="team home-team">
                <span className="team-label">HOME</span>
                <span className="team-name">{game.homeTeam}</span>
              </div>
            </div>
            <div className="game-time">{formatTime(game.gameTime)}</div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Schedule;
