import { DragDropContext, Droppable, Draggable } from '@hello-pangea/dnd';
import PlayerCard from './PlayerCard';
import './RosterManager.css';

const RosterManager = ({ players, onMovePlayer }) => {
  const startingPlayers = players.filter(p => p.isStarting);
  const benchPlayers = players.filter(p => !p.isStarting);

  const handleDragEnd = (result) => {
    if (!result.destination) return;

    const { source, destination, draggableId } = result;
    
    // If dropped in the same position, do nothing
    if (source.droppableId === destination.droppableId && 
        source.index === destination.index) {
      return;
    }

    const playerId = parseInt(draggableId);
    const moveToStarting = destination.droppableId === 'starting';
    
    onMovePlayer(playerId, moveToStarting);
  };

  const renderPlayerList = (playerList, droppableId, title) => (
    <div className="roster-section">
      <h2>{title}</h2>
      <Droppable droppableId={droppableId}>
        {(provided, snapshot) => (
          <div
            ref={provided.innerRef}
            {...provided.droppableProps}
            className={`player-list ${snapshot.isDraggingOver ? 'dragging-over' : ''}`}
          >
            {playerList.length === 0 ? (
              <div className="empty-roster">
                Drop players here
              </div>
            ) : (
              playerList.map((player, index) => (
                <Draggable
                  key={player.id}
                  draggableId={player.id.toString()}
                  index={index}
                >
                  {(provided, snapshot) => (
                    <div
                      ref={provided.innerRef}
                      {...provided.draggableProps}
                      {...provided.dragHandleProps}
                      className={snapshot.isDragging ? 'dragging' : ''}
                    >
                      <PlayerCard player={player} />
                    </div>
                  )}
                </Draggable>
              ))
            )}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    </div>
  );

  return (
    <DragDropContext onDragEnd={handleDragEnd}>
      <div className="roster-manager">
        {renderPlayerList(startingPlayers, 'starting', '🏀 STARTING LINEUP')}
        {renderPlayerList(benchPlayers, 'bench', '💺 BENCH')}
      </div>
    </DragDropContext>
  );
};

export default RosterManager;
