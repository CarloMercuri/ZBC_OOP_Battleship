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

        public int ID
        {
            get { return publicID; }
            set { publicID = value; }
        }

        private bool isDestroyed;

        public bool IsDestroyed
        {
            get { return isDestroyed; }
            set { isDestroyed = value; }
        }


        private List<ShipSection> sections;

        public List<ShipSection> Sections
        {
            get { return sections; }
            set { sections = value; }
        }

        private ShipDirection direction;

        public ShipDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private int lenght;

        public int Lenght
        {
            get { return lenght; }
            set { lenght = value; }
        }

        private Point startCell;

        public Point StartCell
        {
            get { return startCell; }
            set { startCell = value; }
        }


        private Panel mainPanel;

        public Panel MainPanel
        {
            get { return mainPanel; }
            set { mainPanel = value; }
        }

        public Battleship(Point startCell, int lenght, ShipDirection direction = ShipDirection.East)
        {
            this.startCell = startCell;
            this.lenght = lenght;
            this.direction = direction;
            id++;
            publicID = id;

            sections = new List<ShipSection>();

            if(direction == ShipDirection.East)
            {
                for (int i = 0; i < lenght; i++)
                {
                    sections.Add(new ShipSection(new Point(StartCell.X + i, StartCell.Y)));
                }
            } 
            else
            {
                for (int i = 0; i < lenght; i++)
                {
                    sections.Add(new ShipSection(new Point(StartCell.X, StartCell.Y - i)));
                }
            }
        }

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
