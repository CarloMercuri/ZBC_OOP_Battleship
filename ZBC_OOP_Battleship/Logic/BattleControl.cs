using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Battleship
{
    public class BattleControl
    {
        // events
        public event EventHandler<StateChangeEventArgs> StateChange;
        public event EventHandler<MatchEndEventArgs> MatchEnd;

        private BattleBoard playerOneBoard;
        private BattleBoard playerTwoBoard;

        private bool turnHasPlayed;

        private GameState gameState;

        /// <summary>
        /// The current state of the game
        /// </summary>
        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; OnStateChange(value); }
        }

        /// <summary>
        /// Call the event for a state change
        /// </summary>
        /// <param name="state"></param>
        private void OnStateChange(GameState state)
        {
            StateChange?.Invoke(this, new StateChangeEventArgs(state));
        }


        public BattleControl()
        {
            turnHasPlayed = false;
        }

        /// <summary>
        /// Creates the player one board
        /// </summary>
        /// <param name="ships"></param>
        private void CreatePlayerOneBoard(List<Battleship> ships)
        {
            playerOneBoard = new BattleBoard(ships);

        }

        /// <summary>
        /// Creates the player two board
        /// </summary>
        /// <param name="ships"></param>
        private void CreatePlayerTwoBoard(List<Battleship> ships)
        {
            playerTwoBoard = new BattleBoard(ships);
        }

        /// <summary>
        /// Returns the ships owned by player one
        /// </summary>
        /// <returns></returns>
        public List<Battleship> GetPlayerOneShips()
        {
            return playerOneBoard.Battleships;
        }

        /// <summary>
        /// Returns the ships owned by player two
        /// </summary>
        /// <returns></returns>
        public List<Battleship> GetPlayerTwoShips()
        {
            return playerTwoBoard.Battleships;
        }

        /// <summary>
        /// Call this from the UI to attempt a hit
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public HitResult RegisterHitInput(Point cell, PlayerIdentifier source)
        {

            // avoid multiple inputs in the same turn
            if (turnHasPlayed)
            {
                return HitResult.Unsuccessful;
            }

            // Bad
            if(gameState == GameState.PlayerOneTurn && source == PlayerIdentifier.PlayerTwo)
            {
                OnStateChange(gameState);
                return HitResult.Unsuccessful;
            }

            // Bad
            if (gameState == GameState.PlayerTwoTurn && source == PlayerIdentifier.PlayerOne)
            {
                OnStateChange(gameState);
                return HitResult.Unsuccessful;
            }

            // Use the right board
            BattleBoard board;

            if (gameState == GameState.PlayerOneTurn)
            {
                board = playerTwoBoard;
            }
            else
            {
                board = playerOneBoard;
            }


            turnHasPlayed = true;

            // If it's hit
            if (board.IsShipHit(cell))
            {
                // If all the ships have been destroyed
                if (ShouldMatchEnd(board))
                {
                    EndMatch();
                    return HitResult.Successful;
                }

                // Hit
                ChangeTurn();
                return HitResult.Successful;
            }
            else
            {
                // No hit
                ChangeTurn();
                return HitResult.Unsuccessful;
            }
            


        }

        /// <summary>
        /// Sets up things for a match start
        /// </summary>
        /// <param name="playerOneShips"></param>
        /// <param name="playerTwoShips"></param>
        /// <returns></returns>
        public GameState StartMatch(List<Battleship> playerOneShips, List<Battleship> playerTwoShips)
        {
            CreatePlayerOneBoard(playerOneShips);
            CreatePlayerTwoBoard(playerTwoShips);
            gameState = GameState.PlayerOneTurn;
            return gameState;
        }

        /// <summary>
        /// Forces the end of a game
        /// </summary>
        private void EndMatch()
        {
            MatchEndEventArgs args = new MatchEndEventArgs();

            if (gameState == GameState.PlayerOneTurn)
            {
                args.WinningPlayer = PlayerIdentifier.PlayerOne;
            }
            else if (gameState == GameState.PlayerTwoTurn)
            {
                args.WinningPlayer = PlayerIdentifier.PlayerTwo;
            }

            MatchEnd?.Invoke(this, args);            
        }

        /// <summary>
        /// Checks if all the ships on that board are destroyed
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private bool ShouldMatchEnd(BattleBoard board)
        {
            foreach (Battleship ship in board.Battleships)
            {
                if (!ship.IsDestroyed)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Changes turn (and changing the gamestate sends an event)
        /// </summary>
        public void ChangeTurn()
        {
            if(GameState == GameState.PlayerOneTurn)
            {
                GameState = GameState.PlayerTwoTurn;
            }
            else
            {
                GameState = GameState.PlayerOneTurn;
            }

            turnHasPlayed = false;

            
        }
  
        private Point GetMovementVector(MovementDirection direction)
        {
            Point vector = new Point(0, 0);

            if(direction == MovementDirection.Down)
            {
                vector.Y = -1;
            }
            else if(direction == MovementDirection.Up)
            {
                vector.Y = 1;
            }
            else if(direction == MovementDirection.Left)
            {
                vector.X = -1;
            }
            else if(direction == MovementDirection.Right)
            {
                vector.X = 1;
            }

            return vector;
        }

    }
}
