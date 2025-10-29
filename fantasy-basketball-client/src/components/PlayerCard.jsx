import './PlayerCard.css';

const PlayerCard = ({ player }) => {
  const getInjuryClass = (status) => {
    switch (status?.toLowerCase()) {
      case 'healthy':
        return 'injury-healthy';
      case 'questionable':
        return 'injury-questionable';
      case 'doubtful':
        return 'injury-doubtful';
      case 'out':
        return 'injury-out';
      default:
        return '';
    }
  };

  return (
    <div className="player-card">
      <div className="player-header">
        <h3>{player.name}</h3>
        <span className="player-team">{player.team} - {player.position}</span>
      </div>

      {player.injury && (
        <div className={`injury-status ${getInjuryClass(player.injury.status)}`}>
          {player.injury.status}
          {player.injury.description && ` - ${player.injury.description}`}
        </div>
      )}

      {player.gameToday && player.gameToday.hasGame ? (
        <div className="game-info">
          <div className="game-detail">
            <strong>Game Today:</strong> vs {player.gameToday.opponent}
          </div>
          <div className="game-detail">
            <strong>Time:</strong> {player.gameToday.time}
          </div>
          <div className="game-detail">
            <strong>Location:</strong> {player.gameToday.isHomeGame ? 'Home' : 'Away'}
          </div>
        </div>
      ) : (
        <div className="no-game">No game today</div>
      )}

      {player.stats && (
        <div className="stats-grid">
          <div className="stat-item">
            <span className="stat-label">PTS</span>
            <span className="stat-value">{player.stats.points.toFixed(1)}</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">REB</span>
            <span className="stat-value">{player.stats.rebounds.toFixed(1)}</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">AST</span>
            <span className="stat-value">{player.stats.assists.toFixed(1)}</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">FG%</span>
            <span className="stat-value">{player.stats.fieldGoalPercentage.toFixed(1)}</span>
          </div>
        </div>
      )}
    </div>
  );
};

export default PlayerCard;
