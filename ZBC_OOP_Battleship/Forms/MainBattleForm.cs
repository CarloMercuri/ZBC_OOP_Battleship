using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship.Forms
{
    public partial class MainBattleForm : Form
    {
        private PlayerBoard PlayerOneBoard;
        private EnemyBoard PlayerOneEnemyBoard;

        private PlayerBoard PlayerTwoBoard;
        private EnemyBoard PlayerTwoEnemyBoard;

        private BattleControl _control;

        private GameState localGameState;
        private GameState mainGameState;
        private Label overHeadLabel;
        //private Button btn_SetupNext;
        private Label label_Instruction;

        private Label label_Interstitial;

        private bool turnActionTaken;

        private List<Battleship> playerOneShips;
        private List<Battleship> playerTwoShips;


        public MainBattleForm()
        {
            InitializeComponent();


            Panel overHeadPanel = new Panel();
            overHeadPanel.Location = new Point(0, 0);
            overHeadPanel.Size = new Size(this.Width, 50);
            this.Controls.Add(overHeadPanel);


            label_Interstitial = new Label();
            label_Interstitial.Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);
            label_Interstitial.ForeColor = Color.Black;
            label_Interstitial.AutoSize = false;
            label_Interstitial.TextAlign = ContentAlignment.MiddleCenter;
            label_Interstitial.BackColor = Color.Bisque;
            label_Interstitial.Dock = DockStyle.Fill;
            label_Interstitial.Size = new Size(this.Width, 20);
            label_Interstitial.Location = new Point(this.Width / 2, 2);
            label_Interstitial.Visible = false;
            this.Controls.Add(label_Interstitial);


            overHeadLabel = new Label();
            overHeadLabel.Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);
            overHeadLabel.ForeColor = Color.Black;
            overHeadLabel.AutoSize = false;
            overHeadLabel.TextAlign = ContentAlignment.MiddleCenter;
            overHeadLabel.Dock = DockStyle.Fill;
            overHeadLabel.Size = new Size(this.Width, 20);
            overHeadLabel.Location = new Point(this.Width / 2, 2);
            overHeadLabel.Visible = false;
            overHeadPanel.Controls.Add(overHeadLabel);

            //
            // label_Instruction
            //

            label_Instruction = new Label();
            label_Instruction.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
            label_Instruction.Location = new Point(900, 250);
            label_Instruction.AutoSize = true;
            this.Controls.Add(label_Instruction);
            label_Instruction.Visible = false;

            // btn_setupNext
            //btn_SetupNext = new Button();
            //btn_SetupNext.Size = new Size(100, 60);
            //btn_SetupNext.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
            //btn_SetupNext.Text = "Done";
            //btn_SetupNext.Location = new Point(1000, 300);
            //btn_SetupNext.Click += btn_SetupNextClick;
            //this.Controls.Add(btn_SetupNext);


            // Boards initialization
            //PlayerOneBoard = new BoardDisplay();
            //PlayerOneBoard.CreatePanel(this, true, 80, 50);
            //PlayerOneBoard.AddClickEvent(PlayerCellClicked);
            //PlayerOneBoard.AddMouseMoveEvent(PlayerBoardMouseMove);
            //PlayerOneBoard.Hide();

            PlayerOneBoard = new PlayerBoard();
            PlayerOneBoard.CreatePanel(this, true, PlayerInputSource.PlayerOne, 80, 50);
            PlayerOneBoard.AddMouseMoveEvent(PlayerBoardMouseMove);
            PlayerOneBoard.Hide();

            PlayerOneEnemyBoard = new EnemyBoard();
            PlayerOneEnemyBoard.CreatePanel(this, false, PlayerInputSource.PlayerOne);
            PlayerOneEnemyBoard.Hide();
            PlayerOneEnemyBoard.AddClickEvent(EnemyCellClicked, PlayerInputSource.PlayerOne);

            PlayerTwoBoard = new PlayerBoard();
            PlayerTwoBoard.CreatePanel(this, true, PlayerInputSource.PlayerTwo, 700, 50);
            PlayerTwoBoard.Hide();

            PlayerTwoEnemyBoard = new EnemyBoard();
            PlayerTwoEnemyBoard.CreatePanel(this, false, PlayerInputSource.PlayerTwo);
            PlayerTwoEnemyBoard.Hide();
            PlayerTwoEnemyBoard.AddClickEvent(EnemyCellClicked, PlayerInputSource.PlayerTwo);

            localGameState = GameState.PlayerOneSettingUp;

            // initiate control
            _control = new BattleControl();

            // TEMP
            playerOneShips = new List<Battleship>();
            playerOneShips.Add(new Battleship(new Point(2, 4), 5, ShipDirection.East));
            playerOneShips.Add(new Battleship(new Point(1, 5), 2, ShipDirection.North));
            playerOneShips.Add(new Battleship(new Point(8, 7), 4, ShipDirection.North));

            playerTwoShips = new List<Battleship>();
            playerTwoShips.Add(new Battleship(new Point(2, 4), 5, ShipDirection.East));
            playerTwoShips.Add(new Battleship(new Point(1, 5), 2, ShipDirection.North));
            playerTwoShips.Add(new Battleship(new Point(8, 7), 4, ShipDirection.North));


            _control.StateChange += StateChangeEvent;
            // ALWAYS LAST
            label_Interstitial.BringToFront();
            StartSetUp();

        }

        private void EnemyCellClicked(Point cell, PlayerInputSource source)
        {
            HitResult hitResult = _control.RegisterHitInput(cell, source);

            if(hitResult == HitResult.Invalid)
            {
                return;
            }

            if(source == PlayerInputSource.PlayerOne)
            {
                PlayerOneEnemyBoard.UpdateHitResult(cell, hitResult);
            }
            else
            {
                PlayerTwoEnemyBoard.UpdateHitResult(cell, hitResult);
            }


        }

        private void StateChangeEvent(object sender, StateChangeEventArgs args)
        {
            Console.WriteLine($"State change to: {args.GameState}");
            // Change it regardless
            mainGameState = args.GameState;

            switch (args.GameState)
            {
                case GameState.PlayerOneTurn:
                    localGameState = GameState.WaitingToEndTurn;
                    TurnEnd();
                    break;

                case GameState.PlayerTwoTurn:
                    localGameState = GameState.WaitingToEndTurn;
                    TurnEnd();
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                Console.WriteLine($"Enter pressed. Gamestate: {localGameState}");
                switch (localGameState)
                {
                    case GameState.WaitingToStart:
                        label_Instruction.Visible = false;
                        InitiateGame();
                        break;

                    case GameState.PlayerOneSettingUp:
                        PlayerOneBoard.Hide();
                        PlayerTwoBoard.Show();
                        overHeadLabel.Text = "Player 2: Set up your board";
                        label_Instruction.Text = "Press ENTER to start match!";
                        localGameState = GameState.PlayerTwoSettingUp;
                        break;

                    case GameState.PlayerTwoSettingUp:
                        localGameState = GameState.WaitingToStart;
                        PlayerOneBoard.Hide();
                        PlayerTwoBoard.Hide();
                        label_Interstitial.Visible = true;
                        label_Interstitial.Text = "Player 1: Press ENTER to start (Player 2 look away!)";
                        break;

                    case GameState.WaitingToEndTurn:

                        label_Interstitial.Visible = true;
                        if(mainGameState == GameState.PlayerOneTurn)
                        {
                            label_Interstitial.Text = "Player ONE, Press ENTER to start your turn!";
                        }
                        else if (mainGameState == GameState.PlayerTwoTurn)
                        {
                            label_Interstitial.Text = "Player TWO, Press ENTER to start your turn!";
                        }

                        localGameState = GameState.WaitingToChangeTurn;
                        break;

                    case GameState.WaitingToChangeTurn:

                        label_Interstitial.Visible = false;

                        if (mainGameState == GameState.PlayerOneTurn)
                        {
                            PlayerOneEnemyBoard.IsActive = true;
                            PlayerTwoEnemyBoard.IsActive = false;
                            PlayerTwoBoard.Hide();
                            PlayerTwoEnemyBoard.Hide();
                            PlayerOneBoard.Show();
                            PlayerOneBoard.UpdateBoard(_control.GetPlayerOneShips());
                            PlayerOneEnemyBoard.Show();
                            localGameState = GameState.PlayerOneTurn;
                        }
                        else if (mainGameState == GameState.PlayerTwoTurn)
                        {
                            PlayerTwoEnemyBoard.IsActive = true;
                            PlayerOneEnemyBoard.IsActive = false;
                            PlayerOneBoard.Hide();
                            PlayerOneEnemyBoard.Hide();
                            PlayerTwoBoard.Show();
                            PlayerOneBoard.UpdateBoard(_control.GetPlayerTwoShips());
                            PlayerTwoEnemyBoard.Show();
                            localGameState = GameState.PlayerTwoTurn;
                        }
                        break;

                    



                    default:
                        break;
                }

            }

            label1.Text = localGameState.ToString();

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void StartSetUp()
        {
            PlayerOneBoard.CenterHorizontalLocation(this.Width, 100);
            PlayerOneBoard.Show();

            PlayerTwoBoard.CenterHorizontalLocation(this.Width, 100);
            PlayerTwoBoard.Hide();

            overHeadLabel.Visible = true;
            overHeadLabel.Text = "Player 1: Set up your board.";
            label_Instruction.Visible = true;
            label_Instruction.Text = "Press ENTER to go to Player 2 setup.";
            localGameState = GameState.PlayerOneSettingUp;
        }

        private void PlayerCellClicked(Point cell)
        {
            Console.WriteLine(cell);
        }

        private void PlayerBoardMouseMove(Point cell)
        {
            Console.WriteLine(cell);
        }

        private Point GetCellFromCoords(int x, int y)
        {
            int cellX = x / Constants.CellSize;
            int cellY = y / Constants.CellSize;

            return new Point(cellX, cellY);
        }

        private Point GetTopLeftCellCoords(int cellX, int cellY)
        {
            return new Point(Constants.CellSize * cellX, Constants.CellSize * cellY);
        }

        private Panel DrawPlayerBoard(int locX, int locY)
        {
            // The header panel
            Panel mainPanel = new Panel();
            mainPanel.Size = new Size(Constants.HeaderPanelSize, Constants.HeaderPanelSize);
            mainPanel.Location = new Point(locX, locY);
            mainPanel.BorderStyle = BorderStyle.FixedSingle;

            mainPanel.Paint += HeaderPanelPaint;


            //
            // Checkboard panel
            //

            Panel battlePanel = new Panel();
            battlePanel.Size = new Size(Constants.BattlePanelSize, Constants.BattlePanelSize);
            battlePanel.Location = new Point(Constants.CellSize, Constants.CellSize);
            battlePanel.BorderStyle = BorderStyle.FixedSingle;

            battlePanel.Paint += BattlePanelPaint;
            //battlePanel.MouseMove += checkerPanel;

            mainPanel.Controls.Add(battlePanel);

            this.Controls.Add(mainPanel);

            return mainPanel;
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

       

        private void TurnEnd()
        {
            overHeadLabel.Text = "Press ENTER to end turn.";
        }


        private void ShowInterplayScreen()
        {
            PlayerOneBoard.Hide();
            PlayerOneEnemyBoard.Hide();

            PlayerTwoBoard.Hide();
            PlayerTwoEnemyBoard.Hide();

            label_Interstitial.Visible = true;
            label_Interstitial.Text = "Press ENTER to start PLAYER 2 turn.";
        }

        private void InitiateGame()
        {
            localGameState =  _control.StartMatch(playerOneShips, playerTwoShips);

            label_Interstitial.Visible = false;

            // Position the boards
            PlayerOneBoard.CenterVerticalLocation(this.Height, 100);
            PlayerOneEnemyBoard.CenterVerticalLocation(this.Height, 700);
            PlayerTwoBoard.CenterVerticalLocation(this.Height, 100);
            PlayerTwoEnemyBoard.CenterVerticalLocation(this.Height, 700);
            PlayerOneBoard.Hide();
            PlayerOneEnemyBoard.Hide();
            PlayerTwoBoard.Hide();
            PlayerTwoEnemyBoard.Hide();

            // Show and update the proper one
            if (localGameState == GameState.PlayerOneTurn)
            {
                //PlayerOneBoard.UpdatePlayerShips(_control.GetPlayerOneShips());
                PlayerOneBoard.UpdateBoard(_control.GetPlayerOneShips());
                PlayerOneBoard.Show();
                PlayerOneEnemyBoard.IsActive = true;

                PlayerOneEnemyBoard.Show();
            }
            else
            {
                PlayerTwoBoard.UpdateBoard(_control.GetPlayerTwoShips());
                PlayerTwoBoard.Show();
                PlayerTwoEnemyBoard.IsActive = true;
                PlayerTwoEnemyBoard.Show();
            }


            localGameState = GameState.PlayerOneTurn;

        }

        private void btn_SetupNextClick(object sender, EventArgs e)
        {
            
        }

    }
}
