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
        private Panel battlePanel;

        private Panel CreateShipPanel(Battleship ship)
        {
            Panel mainPanel = new Panel();

            if (ship.Direction == ShipDirection.East)
            {
                mainPanel.Width = Constants.CellSize * ship.Lenght;
                mainPanel.Height = Constants.CellSize;
            }
            else
            {
                mainPanel.Width = Constants.CellSize;
                mainPanel.Height = Constants.CellSize * ship.Lenght;
            }

            mainPanel.Paint += ShipPanelPaint;

            battlePanel.Controls.Add(mainPanel);
            return mainPanel;
        }

        private void ShipPanelPaint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            e.Graphics.FillRectangle(new SolidBrush(Constants.ShipInnerColor), new Rectangle(0, 0,
                                                                                             mainPanel.Width,
                                                                                             mainPanel.Height));

            int borderSize = 3;
            Pen borderPen = new Pen(new SolidBrush(Constants.ShipBorderColor), borderSize);

            for (int x = 0; x < panel.Width / 40; x++)
            {
                for (int y = 0; y < panel.Height / 40; y++)
                {
                    e.Graphics.DrawRectangle(borderPen, new Rectangle(Constants.CellSize * x, Constants.CellSize * y, Constants.CellSize, Constants.CellSize));
                }
            }
        }

    }
}
