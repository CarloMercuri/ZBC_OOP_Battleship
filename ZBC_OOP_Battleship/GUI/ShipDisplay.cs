using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship
{
    public class ShipDisplay
    {
        private Battleship shipData;

        public Battleship ShipData
        {
            get { return shipData; }
            set { shipData = value; }
        }

        private Panel mainPanel;

        private Control parentControl;

        private Pen shipBorderPen;
        private Pen crossPen;
        private int borderSize = 3;


        public ShipDisplay(Battleship ship, Control parentControl)
        {
            shipData = ship;
            this.parentControl = parentControl;

            shipBorderPen = new Pen(new SolidBrush(Constants.ShipBorderColor), borderSize);

            crossPen = new Pen(Color.FromArgb(180, 180, 0, 0), 2);


            CreateShipPanel();
        }

        public void SetLocation(int x, int y)
        {
            mainPanel.Location = new Point(x, y);
        }

        public void SetLocation(Point point)
        {
            mainPanel.Location = point;
        }

        public void UpdateVisual()
        {
            mainPanel.Invalidate();
        }

        private Panel CreateShipPanel()
        {
            mainPanel = new Panel();

            if (shipData.Direction == ShipDirection.East)
            {
                mainPanel.Width = Constants.CellSize * shipData.Lenght;
                mainPanel.Height = Constants.CellSize;
            }
            else
            {
                mainPanel.Width = Constants.CellSize;
                mainPanel.Height = Constants.CellSize * shipData.Lenght;
            }

            mainPanel.Paint += ShipPanelPaint;

            parentControl.Controls.Add(mainPanel);
            return mainPanel;
        }

        private void ShipPanelPaint(object sender, PaintEventArgs e)
        {
            // Draw background color
            e.Graphics.FillRectangle(new SolidBrush(Constants.ShipInnerColor), new Rectangle(0, 0,
                                                                                             mainPanel.Width,
                                                                                             mainPanel.Height));
            // Draw border
            e.Graphics.DrawRectangle(shipBorderPen, new Rectangle(0,
                                                  0,
                                                  mainPanel.Width - 1,
                                                  mainPanel.Height - 1));

            foreach (ShipSection section in shipData.Sections)
            {
                if (section.IsHit)
                {
                    if(ShipData.Direction == ShipDirection.East)
                    {
                        e.Graphics.DrawLine(crossPen,
                                            Constants.CellSize * section.SectionNumber + 6,
                                            6,
                                            Constants.CellSize * section.SectionNumber + Constants.CellSize - 6,
                                            Constants.CellSize - 6);

                        e.Graphics.DrawLine(crossPen,
                                           Constants.CellSize * section.SectionNumber + Constants.CellSize - 6,
                                           6,
                                           Constants.CellSize * section.SectionNumber  + 6,
                                           Constants.CellSize - 6);
                    } 
                    else
                    {
                        e.Graphics.DrawLine(crossPen,
                                            6,                                                  // x1
                                            Constants.CellSize * section.SectionNumber + 6,     // y1
                                            Constants.CellSize - 6,                             // x2
                                            Constants.CellSize * section.SectionNumber  + Constants.CellSize - 6);    // y2

                        e.Graphics.DrawLine(crossPen,
                                           Constants.CellSize - 6,
                                           Constants.CellSize * section.SectionNumber + 6,
                                           6,
                                           Constants.CellSize * section.SectionNumber + Constants.CellSize - 6);
                    }

                  
                }
            }

            //for (int x = 0; x < panel.Width / 40; x++)
            //{
            //    for (int y = 0; y < panel.Height / 40; y++)
            //    {
            //        e.Graphics.DrawRectangle(borderPen, new Rectangle(Constants.CellSize * x, Constants.CellSize * y, Constants.CellSize, Constants.CellSize));
            //    }
            //}


        }

    }
}
