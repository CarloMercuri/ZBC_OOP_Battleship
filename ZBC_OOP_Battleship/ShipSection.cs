using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Battleship
{
    public class ShipSection
    {
        private Point sectionCell;

        public Point SectionCell
        {
            get { return sectionCell; }
            set { sectionCell = value; }
        }

        private int sectionNumber;

        public int SectionNumber
        {
            get { return sectionNumber; }
            set { sectionNumber = value; }
        }


        private bool isHit;

        public bool IsHit
        {
            get { return isHit; }
            set { isHit = value; }
        }

        public ShipSection(Point sectionCell, int sectionNumber)
        {
            this.sectionNumber = sectionNumber;
            this.sectionCell = sectionCell;
            this.isHit = false;
        }


    }
}
