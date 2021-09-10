using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship
{
    public class BoardDisplay
    {
        private List<Point> crossLocations;
        private Panel battlePanel;
        private Panel mainPanel;
        private List<Panel> activeShips;


        public BoardDisplay()
        {
            crossLocations = new List<Point>();
            activeShips = new List<Panel>();
        }

        /// <summary>
        /// Creates a board visual, playerBoard true if it's the player's own board, false if it's the adversary's
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="playerBoard"></param>
        /// <param name="locX"></param>
        /// <param name="locY"></param>
        /// <returns></returns>
        public Panel CreatePanel(Control parentControl, bool playerBoard, int locX = 0, int locY = 0)
        {
            // The header panel
            mainPanel = new Panel();
            mainPanel.Size = new Size(Constants.HeaderPanelSize, Constants.HeaderPanelSize);
            mainPanel.Location = new Point(locX, locY);
            mainPanel.BorderStyle = BorderStyle.FixedSingle;

            mainPanel.Paint += HeaderPanelPaint;

            //
            // Checkboard panel
            //

            battlePanel = new Panel();
            battlePanel.Size = new Size(Constants.BattlePanelSize, Constants.BattlePanelSize);
            battlePanel.Location = new Point(Constants.CellSize, Constants.CellSize);
            battlePanel.BorderStyle = BorderStyle.FixedSingle;

            battlePanel.Paint += BattlePanelPaint;
            //battlePanel.MouseMove += checkerPanel;

            mainPanel.Controls.Add(battlePanel);

            parentControl.Controls.Add(mainPanel);

            return mainPanel;
        }

        public void UpdateShips(List<Battleship> ships)
        {
            activeShips.Clear();

            foreach(Battleship ship in ships)
            {
                //Panel panel_Ship = ship.CreatePanel(battlePanel);
                Panel panel_Ship = CreateShipPanel(ship);

                if(ship.Direction == ShipDirection.North)
                {
                    panel_Ship.Location = GetTopLeftCellCoords(ship.StartCell.X, ship.StartCell.Y - (ship.Lenght - 1));
                } 
                else
                {
                    panel_Ship.Location = GetTopLeftCellCoords(ship.StartCell.X, ship.StartCell.Y);
                }
               
                activeShips.Add(panel_Ship);
                battlePanel.Controls.Add(panel_Ship);
            }
        }

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

        private Point GetTopLeftCellCoords(int cellX, int cellY)
        {
            return new Point(Constants.CellSize * cellX, Constants.CellSize * cellY);
        }

        public void Hide()
        {
            mainPanel.Visible = false;
        }

        public void Show()
        {
            mainPanel.Visible = true;
        }

        public void CenterHorizontalLocation(int formWidth, int y)
        {
            mainPanel.Location = new Point(formWidth / 2 - mainPanel.Width / 2, y);
        }

        public void CenterVerticalLocation(int formHeight, int x)
        {
            mainPanel.Location = new Point(x, formHeight / 2 - mainPanel.Height / 2);
        }

        public void SetLocation(int x, int y)
        {
            mainPanel.Location = new Point(x, y);
        }

        public void AddMouseMoveEvent(Action<Point> method)
        {
            battlePanel.MouseMove += (sender, args) =>
            {
                MouseEventArgs margs = (MouseEventArgs)args;
                Point cell = GetCellFromCoords(margs.X, margs.Y);
                method(cell);
            };
        }

        public void AddClickEvent(Action<Point> method)
        {
            // TO-DO: Find out the better way
            battlePanel.Click += (sender, args) =>
            {
                MouseEventArgs margs = (MouseEventArgs)args;
                Point cell = GetCellFromCoords(margs.X, margs.Y);
                method(cell);
            };
        }

        private Point GetCellFromCoords(int x, int y)
        {
            int cellX = x / Constants.CellSize;
            int cellY = y / Constants.CellSize;

            return new Point(cellX, cellY);
        }

        public void SetCross(Point cell)
        {
            crossLocations.Add(cell);
            battlePanel.Invalidate();
        }

        private void HeaderPanelPaint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;

            // Draw grey background on headers
            SolidBrush headersBG = new SolidBrush(Color.FromArgb(150, 150, 150, 150));
            e.Graphics.FillRectangle(headersBG, new Rectangle(Constants.CellSize, 0, p.Width - Constants.CellSize, Constants.CellSize));
            e.Graphics.FillRectangle(headersBG, new Rectangle(0, Constants.CellSize, Constants.CellSize, p.Height - Constants.CellSize));

            // Text
            SolidBrush textBrush = new SolidBrush(Color.FromArgb(190, 100, 100, 100));
            Font textFont = new Font(FontFamily.GenericSansSerif, 18);

            for (int letters = 0; letters < 10; letters++)
            {
                e.Graphics.DrawString(((char)(65 + letters)).ToString(), textFont, textBrush, 48 + Constants.CellSize * letters, 6);
            }

            int numbersX = 10;

            for (int numbers = 1; numbers < 11; numbers++)
            {
                if (numbers >= 10) numbersX = 2;
                e.Graphics.DrawString(numbers.ToString(), textFont, textBrush, numbersX, 5 + Constants.CellSize * numbers);
            }
        }

        private void BattlePanelPaint(object sender, PaintEventArgs e)
        {
            int xPos = 0;
            int yPos = 0;

            Panel p = sender as Panel;

            // Sea background
            SolidBrush bgBrush = new SolidBrush(Color.FromArgb(100, 199, 229, 237));
            e.Graphics.FillRectangle(bgBrush, new Rectangle(0, 0, p.Width, p.Height));


            // Draw grid
            Pen gridPen = new Pen(new SolidBrush(Color.Gray));
            gridPen.Width = 2;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    e.Graphics.DrawLine(gridPen, new Point(xPos, yPos + Constants.CellSize * y),
                                             new Point(Constants.BattlePanelSize, yPos + Constants.CellSize * y));
                }

                e.Graphics.DrawLine(gridPen, new Point(xPos + Constants.CellSize * x, yPos),
                                             new Point(xPos + Constants.CellSize * x, Constants.BattlePanelSize));
            }

        }
    }
}
