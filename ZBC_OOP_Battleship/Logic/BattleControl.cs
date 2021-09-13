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

        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; OnStateChange(value); }
        }

        private void OnStateChange(GameState state)
        {
            StateChange?.Invoke(this, new StateChangeEventArgs(state));
        }


        public BattleControl()
        {
            turnHasPlayed = false;
        }

        private void CreatePlayerOneBoard(List<Battleship> ships)
        {
            playerOneBoard = new BattleBoard(ships);

        }

        private void CreatePlayerTwoBoard(List<Battleship> ships)
        {
            playerTwoBoard = new BattleBoard(ships);
        }

        public List<Battleship> GetPlayerOneShips()
        {
            return playerOneBoard.Battleships;
        }

        public List<Battleship> GetPlayerTwoShips()
        {
            return playerTwoBoard.Battleships;
        }

        public HitResult RegisterHitInput(Point cell, PlayerIdentifier source)
        {

            // avoid multiple inputs in the same turn
            if (turnHasPlayed)
            {
                return HitResult.Unsuccessful;
            }

            if(gameState == GameState.PlayerOneTurn && source == PlayerIdentifier.PlayerTwo)
            {
                OnStateChange(gameState);
                return HitResult.Unsuccessful;
            }

            if (gameState == GameState.PlayerTwoTurn && source == PlayerIdentifier.PlayerOne)
            {
                OnStateChange(gameState);
                return HitResult.Unsuccessful;
            }

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

            if (board.IsShipHit(cell))
            {
                if (ShouldMatchEnd(board))
                {
                    EndMatch();
                    return HitResult.Successful;
                }

                ChangeTurn();
                return HitResult.Successful;
            }
            else
            {
                ChangeTurn();
                return HitResult.Unsuccessful;
            }
            


        }

        public GameState StartMatch(List<Battleship> playerOneShips, List<Battleship> playerTwoShips)
        {
            CreatePlayerOneBoard(playerOneShips);
            CreatePlayerTwoBoard(playerTwoShips);
            gameState = GameState.PlayerOneTurn;
            return gameState;
        }

        private void EndMatch()
        {

        }

        private bool ShouldMatchEnd(BattleBoard board)
        {
            bool matchShouldEnd = true;

            foreach (Battleship ship in board.Battleships)
            {
                if (!ship.IsDestroyed)
                {
                    return false;
                }
            }

            return true;
        }

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

        public bool RequestShipMovement(Battleship ship, MovementDirection direction)
        {
            BattleBoard board;

            if (GameState == GameState.PlayerOneTurn)
            {
                board = playerOneBoard;
            }
            else
            {
                board = playerTwoBoard;
            }
            

            Point vector = GetMovementVector(direction);

            foreach(ShipSection section in ship.Sections)
            {
                Point newPoint = new Point(section.SectionCell.X + vector.X, section.SectionCell.Y + vector.Y);

                if(board.CellContainsDifferentShip(ship, newPoint))
                {
                    return false;
                }
            }

            // Actually do it
            for (int i = 0; i < ship.Sections.Count; i++)
            {
                ship.Sections[i].SectionCell = new Point(ship.Sections[i].SectionCell.X + vector.X,
                                                         ship.Sections[i].SectionCell.Y + vector.Y);
            }

            return true;
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

        private bool IsShipLocationValid(Point location, Battleship ship)
        {
            return false;
        }


    }
}
