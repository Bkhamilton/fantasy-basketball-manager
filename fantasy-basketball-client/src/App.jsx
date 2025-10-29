import { useState, useEffect } from 'react';
import './App.css';
import RosterManager from './components/RosterManager';
import Recommendations from './components/Recommendations';
import Schedule from './components/Schedule';

const API_BASE_URL = 'http://localhost:5000/api';

function App() {
  const [players, setPlayers] = useState([]);
  const [recommendations, setRecommendations] = useState([]);
  const [schedule, setSchedule] = useState([]);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState('roster');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [playersRes, recsRes, scheduleRes] = await Promise.all([
        fetch(`${API_BASE_URL}/players`),
        fetch(`${API_BASE_URL}/analysis/start-sit-recommendations`),
        fetch(`${API_BASE_URL}/schedule/today`)
      ]);

      const playersData = await playersRes.json();
      const recsData = await recsRes.json();
      const scheduleData = await scheduleRes.json();

      setPlayers(playersData);
      setRecommendations(recsData);
      setSchedule(scheduleData);
    } catch (error) {
      console.error('Error fetching data:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleMovePlayer = async (playerId, moveToStarting) => {
    try {
      const endpoint = moveToStarting 
        ? `${API_BASE_URL}/roster/move-to-starting/${playerId}`
        : `${API_BASE_URL}/roster/move-to-bench/${playerId}`;
      
      await fetch(endpoint, { method: 'POST' });
      
      // Update local state
      setPlayers(players.map(p => 
        p.id === playerId ? { ...p, isStarting: moveToStarting } : p
      ));
    } catch (error) {
      console.error('Error moving player:', error);
    }
  };

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  return (
    <div className="app">
      <header className="app-header">
        <h1>🏀 Fantasy Basketball Manager</h1>
        <p className="subtitle">Optimize your lineup in minutes</p>
      </header>

      <nav className="tab-nav">
        <button 
          className={`tab-button ${activeTab === 'roster' ? 'active' : ''}`}
          onClick={() => setActiveTab('roster')}
        >
          My Roster
        </button>
        <button 
          className={`tab-button ${activeTab === 'recommendations' ? 'active' : ''}`}
          onClick={() => setActiveTab('recommendations')}
        >
          Recommendations
        </button>
        <button 
          className={`tab-button ${activeTab === 'schedule' ? 'active' : ''}`}
          onClick={() => setActiveTab('schedule')}
        >
          Today's Games
        </button>
      </nav>

      <main className="app-main">
        {activeTab === 'roster' && (
          <RosterManager players={players} onMovePlayer={handleMovePlayer} />
        )}
        {activeTab === 'recommendations' && (
          <Recommendations recommendations={recommendations} />
        )}
        {activeTab === 'schedule' && (
          <Schedule games={schedule} />
        )}
      </main>
    </div>
  );
}

export default App;
