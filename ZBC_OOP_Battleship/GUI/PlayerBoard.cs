using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship
{
    public class PlayerBoard : PlayingBoard
    {
               
        public override void UpdateBoard(List<Battleship> ships)
        {
            activeShips.Clear();

            foreach (Battleship ship in ships)
            {
                ShipDisplay shipDisplay = new ShipDisplay(ship, BattlePanel);

                if (ship.Direction == ShipDirection.North)
                {
                    shipDisplay.SetLocation(GetTopLeftCellCoords(ship.StartCell.X, ship.StartCell.Y - (ship.Lenght - 1)));
                }
                else
                {
                    shipDisplay.SetLocation(GetTopLeftCellCoords(ship.StartCell.X, ship.StartCell.Y));
                }

                activeShips.Add(shipDisplay);
            }
        }


    }
}
