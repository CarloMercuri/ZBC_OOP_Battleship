using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Battleship
{
    public class GridSlot
    {
        private bool occupied;

        /// <summary>
        /// Wether or not this slot is occupied
        /// </summary>
        public bool Occupied
        {
            get { return occupied; }
            set { occupied = value; }
        }

        private int shipID;

        /// <summary>
        /// The ID of the ship occupying this slot. -1 if empty
        /// </summary>
        public int ShipID
        {
            get { return shipID; }
            set { shipID = value; }
        }

        public GridSlot()
        {
            occupied = false;
            shipID = -1;
        }

    }
}
