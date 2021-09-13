using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship
{
    public class BattleBoard
    {
        /// <summary>
        /// The main data grid
        /// </summary>
        private GridSlot[,] mainGrid;

        /// <summary>
        /// The list of battleships on this board
        /// </summary>
        private List<Battleship> battleships;

        /// <summary>
        /// The list of battleships on this board
        /// </summary>
        public List<Battleship> Battleships
        {
            get { return battleships; }
            set { battleships = value; }
        }

        public BattleBoard(List<Battleship> ships)
        {
            // Initialize grid array
            mainGrid = new GridSlot[10, 10];

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    mainGrid[x, y] = new GridSlot();
                }
            }

            battleships = ships;

            // Go through the ships assigned to this board and mark the slots where a ship section exists
            foreach (Battleship ship in ships)
            {
                foreach (ShipSection section in ship.Sections)
                {
                    mainGrid[section.SectionCell.X, section.SectionCell.Y].Occupied = true;
                    mainGrid[section.SectionCell.X, section.SectionCell.Y].ShipID = ship.ID;
                }
            }
        }

        /// <summary>
        /// Returns true if the cell contains a ship section
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool IsShipHit(Point cell)
        {
            if (CellContainsShip(cell))
            {
                RegisterHit(cell);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the ship's data
        /// </summary>
        /// <param name="cell"></param>
        private void RegisterHit(Point cell)
        {
            foreach (Battleship ship in battleships)
            {
                if(ship.ID == mainGrid[cell.X, cell.Y].ShipID)
                {
                    ship.RegisterHit(cell);
                }
            }
        }

        /// <summary>
        /// Returns a GridSlot object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GridSlot GetGridSlot(int x, int y)
        {
            return mainGrid[x, y];
        }

        /// <summary>
        /// True if a cell contains a ship
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool CellContainsShip(Point cell)
        {
           if(mainGrid[cell.X, cell.Y].ShipID != -1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// True if the cell contains a ship that is NOT equal to the provided ship
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool CellContainsDifferentShip(Battleship ship, Point cell)
        {
            foreach (Battleship s in battleships)
            {
                foreach (ShipSection section in s.Sections)
                {
                    if (section.SectionCell == cell && s.ID != ship.ID)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


       
        

    }
}
