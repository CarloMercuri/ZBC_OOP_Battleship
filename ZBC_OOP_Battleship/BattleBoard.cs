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

        private GridSlot[,] mainGrid;

        public GridSlot[,] MainGrid
        {
            get { return mainGrid; }
            set { mainGrid = value; }
        }

        private List<Battleship> battleships;

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

            foreach (Battleship ship in ships)
            {
                foreach (ShipSection section in ship.Sections)
                {
                    mainGrid[section.SectionCell.X, section.SectionCell.Y].Occupied = true;
                    mainGrid[section.SectionCell.X, section.SectionCell.Y].ShipID = ship.ID;
                }
            }
        }

        public bool IsShipHit(Point cell)
        {
            if (CellContainsShip(cell))
            {
                RegisterHit(cell);
                return true;
            }

            return false;
        }

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

        public GridSlot GetGridSlot(int x, int y)
        {
            return mainGrid[x, y];
        }

        public bool CellContainsShip(Point cell)
        {
           if(mainGrid[cell.X, cell.Y].ShipID != -1)
            {
                return true;
            }

            return false;
        }

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
