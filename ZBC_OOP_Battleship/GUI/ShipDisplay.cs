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
        // The ship data
        private Battleship shipData;

        private Panel mainPanel;
        private Control parentControl;

        // Paint event
        private Pen shipBorderPen;
        private Pen crossPen;
        private int borderSize = 3;
        private int crossMargin = 6;


        public ShipDisplay(Battleship ship, Control parentControl)
        {
            shipData = ship;
            this.parentControl = parentControl;

            shipBorderPen = new Pen(new SolidBrush(Constants.ShipBorderColor), borderSize);

            crossPen = new Pen(Color.FromArgb(180, 180, 0, 0), 2);


            CreateShipPanel();
        }

        /// <summary>
        /// Sets the location of this ship
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetLocation(int x, int y)
        {
            mainPanel.Location = new Point(x, y);
        }

        /// <summary>
        /// Sets the location of this ship
        /// </summary>
        /// <param name="point"></param>
        public void SetLocation(Point point)
        {
            mainPanel.Location = point;
        }

        public void UpdateVisual()
        {
            mainPanel.Invalidate();
        }

        /// <summary>
        /// Creates the main panel
        /// </summary>
        /// <returns></returns>
        private Panel CreateShipPanel()
        {
            mainPanel = new Panel();

            // Make it horizontal or vertical depending on how it's created
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

        /// <summary>
        /// Main paint event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // Draw the crosses on the hit sections
            foreach (ShipSection section in shipData.Sections)
            {
                if (section.IsHit)
                {
                    if (shipData.Direction == ShipDirection.East)
                    {
                        // The line from top left to bottom right
                        e.Graphics.DrawLine(crossPen,                                       
                                            Constants.CellSize * section.SectionNumber + crossMargin,                         // x1
                                            crossMargin,                                                                      // y1
                                            Constants.CellSize * section.SectionNumber + Constants.CellSize - crossMargin,    // x2
                                            Constants.CellSize - crossMargin);                                                // y2

                        // The line from top right to bottom left
                        e.Graphics.DrawLine(crossPen,
                                           Constants.CellSize * section.SectionNumber + Constants.CellSize - crossMargin,     // x1
                                           crossMargin,                                                                       // y1
                                           Constants.CellSize * section.SectionNumber  + crossMargin,                         // x2
                                           Constants.CellSize - crossMargin);                                                 // y2
                    } 
                    else
                    {
                        // The line from top left to bottom right
                        e.Graphics.DrawLine(crossPen,
                                           crossMargin,                                                                                          // x1
                                           Constants.CellSize * shipData.Lenght - Constants.CellSize * section.SectionNumber - 34,               // y1
                                           Constants.CellSize - crossMargin,                                                                     // x2
                                           Constants.CellSize * shipData.Lenght - Constants.CellSize * section.SectionNumber - crossMargin );    // y2 

                        // The line from top right to bottom left
                        e.Graphics.DrawLine(crossPen,
                                           Constants.CellSize - crossMargin,                                                                   // x1
                                           Constants.CellSize * shipData.Lenght - Constants.CellSize * section.SectionNumber - 34,             // y1
                                           crossMargin,                                                                                        // x2
                                           Constants.CellSize * shipData.Lenght - Constants.CellSize * section.SectionNumber - crossMargin);   // y2
                    }

                  
                }
            }
        }

    }
}
