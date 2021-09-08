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
        private BattleBoard playerOneBoard;
        private BattleBoard playerTwoBoard;

        private bool turnHasPlayed;

        private CurrentTurn currentTurn;

        public CurrentTurn CurrentTurn
        {
            get { return currentTurn; }
            set { currentTurn = value; }
        }


        public BattleControl()
        {
            turnHasPlayed = false;
        }

        public void CreatePlayerOneBoard(List<Battleship> ships)
        {
            playerOneBoard = new BattleBoard(ships);

        }

        public void CreatePlayerTwoBoard(List<Battleship> ships)
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

        public bool HitResult(Point cell)
        {
            if (turnHasPlayed)
            {
                return false;
            }

            BattleBoard board;

            if (currentTurn == CurrentTurn.PlayerOne)
            {
                board = playerOneBoard;
            }
            else
            {
                board = playerTwoBoard;
            }


            turnHasPlayed = true;

            if (board.IsShipHit(cell))
            {
                if (ShouldMatchEnd(board))
                {
                    EndMatch();
                }

                return true;
            }

            return false;

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
            if(currentTurn == CurrentTurn.PlayerOne)
            {
                currentTurn = CurrentTurn.Playertwo;
            }
            else
            {
                currentTurn = CurrentTurn.PlayerOne;
            }

            turnHasPlayed = false;
        }

        public bool RequestShipMovement(Battleship ship, MovementDirection direction)
        {
            BattleBoard board;

            if (currentTurn == CurrentTurn.PlayerOne)
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
