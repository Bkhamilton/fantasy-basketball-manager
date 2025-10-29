import './Recommendations.css';

const Recommendations = ({ recommendations }) => {
  return (
    <div className="recommendations">
      <h2>📊 Start/Sit Recommendations</h2>
      <div className="recommendations-list">
        {recommendations.map((rec) => (
          <div 
            key={rec.playerId} 
            className={`recommendation-card ${rec.recommendation.toLowerCase()}`}
          >
            <div className="rec-header">
              <h3>{rec.playerName}</h3>
              <span className={`rec-badge ${rec.recommendation.toLowerCase()}`}>
                {rec.recommendation}
              </span>
            </div>
            <div className="confidence-bar">
              <div 
                className="confidence-fill"
                style={{ width: `${rec.confidenceScore}%` }}
              />
              <span className="confidence-text">
                {rec.confidenceScore.toFixed(0)}% Confidence
              </span>
            </div>
            <div className="reasons">
              {rec.reasons.map((reason, index) => (
                <div key={index} className="reason-item">
                  • {reason}
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Recommendations;
