using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship
{
    public class PlayingBoard
    {
        // GUI
        private Panel mainPanel { get; set; }
        private Panel battlePanel;

        // The source of an input
        private PlayerIdentifier playerSource;

        public PlayerIdentifier PlayerSource
        {
            get { return playerSource; }
            private set { playerSource = value; }
        }

        private bool isActive;

        /// <summary>
        /// If the board is actively updating
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }


        public Panel BattlePanel
        {
            get { return battlePanel; }
            set { battlePanel = value; }
        }

        public virtual void UpdateBoard()
        {

        }

        public virtual void UpdateBoard(List<Battleship> ships)
        {

        }

        /// <summary>
        /// Creates the visual aspect of the board
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="playerBoard"></param>
        /// <param name="source"></param>
        /// <param name="locX"></param>
        /// <param name="locY"></param>
        public virtual void CreatePanel(Control parentControl, bool playerBoard, PlayerIdentifier source, int locX = 0, int locY = 0)
        {
            playerSource = source;

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

            mainPanel.Controls.Add(battlePanel);

            parentControl.Controls.Add(mainPanel);

        }

        /// <summary>
        /// Returns the coordinates of the top left corner of a cell
        /// </summary>
        /// <param name="cellX"></param>
        /// <param name="cellY"></param>
        /// <returns></returns>
        public Point GetTopLeftCellCoords(int cellX, int cellY)
        {
            return new Point(Constants.CellSize * cellX, Constants.CellSize * cellY);
        }

        /// <summary>
        /// Hides the board
        /// </summary>
        public void Hide()
        {
            mainPanel.Visible = false;
        }

        /// <summary>
        /// Displays the board
        /// </summary>
        public void Show()
        {
            mainPanel.Visible = true;
        }
               
        /// <summary>
        /// Places the board at a specified Y level, keeping the X centered in the form
        /// </summary>
        /// <param name="formWidth"></param>
        /// <param name="y"></param>
        public void CenterHorizontalLocation(int formWidth, int y)
        {
            mainPanel.Location = new Point(formWidth / 2 - mainPanel.Width / 2, y);
        }

        /// <summary>
        /// Places the board at a specified X leve, keeping the Y centered in the form
        /// </summary>
        /// <param name="formHeight"></param>
        /// <param name="x"></param>
        public void CenterVerticalLocation(int formHeight, int x)
        {
            mainPanel.Location = new Point(x, formHeight / 2 - mainPanel.Height / 2);
        }

        /// <summary>
        /// Registers an event for a mouse click
        /// </summary>
        /// <param name="method"></param>
        /// <param name="source"></param>
        public void AddClickEvent(Action<Point, PlayerIdentifier> method, PlayerIdentifier source)
        {
            // TO-DO: Find out the better way
            battlePanel.Click += (sender, args) =>
            {
                MouseEventArgs margs = (MouseEventArgs)args;
                int cellX = margs.X / Constants.CellSize;
                int cellY = margs.Y / Constants.CellSize;

                Point cell = new Point(cellX, cellY);
                method(cell, source);
            };
        }

        /// <summary>
        /// Returns the cell coordinates from the mouse coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point GetCellFromCoords(int x, int y)
        {
            int cellX = x / Constants.CellSize;
            int cellY = y / Constants.CellSize;

            return new Point(cellX, cellY);
        }

        /// <summary>
        /// Sets the location of this board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetLocation(int x, int y)
        {
            mainPanel.Location = new Point(x, y);
        }

        /// <summary>
        /// Registers a mouse move event
        /// </summary>
        /// <param name="method"></param>
        public void AddMouseMoveEvent(Action<Point> method)
        {
            battlePanel.MouseMove += (sender, args) =>
            {

                MouseEventArgs margs = (MouseEventArgs)args;
                Point cell = GetCellFromCoords(margs.X, margs.Y);
                method(cell);
            };
        }

        /// <summary>
        /// Paint method for the header panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // Draw the letters
            for (int letters = 0; letters < 10; letters++)
            {
                e.Graphics.DrawString(((char)(65 + letters)).ToString(), textFont, textBrush, 48 + Constants.CellSize * letters, 6);
            }

            int numbersX = 10;

            // Draw the numbers
            for (int numbers = 1; numbers < 11; numbers++)
            {
                if (numbers >= 10) numbersX = 2;
                e.Graphics.DrawString(numbers.ToString(), textFont, textBrush, numbersX, 5 + Constants.CellSize * numbers);
            }
        }

        /// <summary>
        /// Paint event for the checkered panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void BattlePanelPaint(object sender, PaintEventArgs e)
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
