using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship
{
    public class Battleship
    {
        private static int id;

        private int publicID;

        /// <summary>
        /// The unique ID of this ship
        /// </summary>
        public int ID
        {
            get { return publicID; }
            set { publicID = value; }
        }

        private bool isDestroyed;

        /// <summary>
        /// True if all the sections have been hit
        /// </summary>
        public bool IsDestroyed
        {
            get { return isDestroyed; }
            set { isDestroyed = value; }
        }


        private List<ShipSection> sections;

        /// <summary>
        /// The sections this ship is divided into
        /// </summary>
        public List<ShipSection> Sections
        {
            get { return sections; }
            set { sections = value; }
        }

        private ShipDirection direction;

        /// <summary>
        /// The direction the ship is facing
        /// </summary>
        public ShipDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private int lenght;

        /// <summary>
        /// The lenght in grid cells of this ship
        /// </summary>
        public int Lenght
        {
            get { return lenght; }
            set { lenght = value; }
        }

        private Point startCell;

        /// <summary>
        /// The origin cell of this ship
        /// </summary>
        public Point StartCell
        {
            get { return startCell; }
            set { startCell = value; }
        }

        public Battleship(Point startCell, int lenght, ShipDirection direction = ShipDirection.East)
        {
            this.startCell = startCell;
            this.lenght = lenght;
            this.direction = direction;

            // Assign a unique ID 
            id++;
            publicID = id;

            // Create the ship sections
            sections = new List<ShipSection>();

            // Left to right
            if(direction == ShipDirection.East)
            {
                for (int i = 0; i < lenght; i++)
                {
                    sections.Add(new ShipSection(new Point(StartCell.X + i, StartCell.Y), i));
                }
            } 
            else
            {
                // Down to up
                for (int i = 0; i < lenght; i++)
                {
                    sections.Add(new ShipSection(new Point(StartCell.X, StartCell.Y - i), i));
                }
            }

            // Make sure all the sections of a new ship are marked as not hit
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i].IsHit = false;
            }
        }

        /// <summary>
        /// Accepts a hit and changes data
        /// </summary>
        /// <param name="gridCell"></param>
        public void RegisterHit(Point gridCell)
        {
            // Set the hit
            for (int i = 0; i < sections.Count; i++)
            {
                if(gridCell == sections[i].SectionCell)
                {
                    sections[i].IsHit = true;
                }
            }

            // Check if the whole ship has been hit
            bool destroyed = true;

            foreach(ShipSection sect in sections)
            {
                if (!sect.IsHit)
                {
                    destroyed = false;
                }
            }

            if (destroyed)
            {
                isDestroyed = true;
            }
        }

        
       
    }
}
