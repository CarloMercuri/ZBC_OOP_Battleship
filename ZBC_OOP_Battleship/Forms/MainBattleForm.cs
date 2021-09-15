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
        // The boards
        private PlayerBoard PlayerOneBoard;
        private EnemyBoard PlayerOneEnemyBoard;

        private PlayerBoard PlayerTwoBoard;
        private EnemyBoard PlayerTwoEnemyBoard;

        // References
        private BattleControl _control;

        // Game states
        private GameState localGameState;
        private GameState mainGameState;

        // Text
        private Label overHeadLabel;
        private Label label_Instruction;
        private Label label_Interstitial;

        // If the current turn already saw action
        private bool turnPlayed;

        // Creation mode
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

            //
            // Playing boards
            //

            PlayerOneBoard = new PlayerBoard();
            PlayerOneBoard.CreatePanel(this, true, PlayerIdentifier.PlayerOne, 80, 50);
            PlayerOneBoard.Hide();

            PlayerOneEnemyBoard = new EnemyBoard();
            PlayerOneEnemyBoard.CreatePanel(this, false, PlayerIdentifier.PlayerOne);
            PlayerOneEnemyBoard.Hide();
            PlayerOneEnemyBoard.AddClickEvent(EnemyCellClicked, PlayerIdentifier.PlayerOne);

            PlayerTwoBoard = new PlayerBoard();
            PlayerTwoBoard.CreatePanel(this, true, PlayerIdentifier.PlayerTwo, 700, 50);
            PlayerTwoBoard.Hide();

            PlayerTwoEnemyBoard = new EnemyBoard();
            PlayerTwoEnemyBoard.CreatePanel(this, false, PlayerIdentifier.PlayerTwo);
            PlayerTwoEnemyBoard.Hide();
            PlayerTwoEnemyBoard.AddClickEvent(EnemyCellClicked, PlayerIdentifier.PlayerTwo);

            localGameState = GameState.PlayerOneSettingUp;

            // initiate control
            _control = new BattleControl();

            // events
            _control.MatchEnd += MatchEndEvent;

            // TEMP
            playerOneShips = new List<Battleship>();
            playerOneShips.Add(new Battleship(new Point(2, 4), 5, ShipDirection.East));
            playerOneShips.Add(new Battleship(new Point(1, 5), 2, ShipDirection.North));
            playerOneShips.Add(new Battleship(new Point(5, 9), 4, ShipDirection.North));
            playerOneShips.Add(new Battleship(new Point(7, 4), 2, ShipDirection.East));
            playerOneShips.Add(new Battleship(new Point(9, 9), 4, ShipDirection.North));

            playerTwoShips = new List<Battleship>();
            playerTwoShips.Add(new Battleship(new Point(3, 2), 2, ShipDirection.East));
            playerTwoShips.Add(new Battleship(new Point(1, 2), 5, ShipDirection.East));
            playerTwoShips.Add(new Battleship(new Point(5, 2), 2, ShipDirection.North));
            playerTwoShips.Add(new Battleship(new Point(8, 8), 3, ShipDirection.North));
            playerTwoShips.Add(new Battleship(new Point(9, 5), 4, ShipDirection.North));



            _control.StateChange += StateChangeEvent;
            // ALWAYS LAST
            label_Interstitial.BringToFront();
            StartSetUp();

        }

        /// <summary>
        /// What happens at the end of a match
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MatchEndEvent(object sender, MatchEndEventArgs e)
        {
            localGameState = GameState.GameEnd;

            overHeadLabel.ForeColor = Color.Red;

            if(e.WinningPlayer == PlayerIdentifier.PlayerOne)
            {
                overHeadLabel.Text = "Player ONE wins! Congratulations!";
            }
            else
            {
                overHeadLabel.Text = "Player TWO wins! Congratulations!";
            }

        }

        /// <summary>
        /// Called when you click on the enemy board in a cell, when allowed to do so
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="source"></param>
        private void EnemyCellClicked(Point cell, PlayerIdentifier source)
        {
            // Avoid double events
            if (turnPlayed)
            {
                return;
            }

            // Or if the game ended already
            if(localGameState == GameState.GameEnd)
            {
                return;
            }

            // If the slot was already attempted
            if(source == PlayerIdentifier.PlayerOne && !PlayerOneEnemyBoard.IsSlotValidTarget(cell))
            {
                return;
            }

            // If the slot was already attempted
            if (source == PlayerIdentifier.PlayerTwo && !PlayerTwoEnemyBoard.IsSlotValidTarget(cell))
            {
                return;
            }

            // Register the turn as played
            turnPlayed = true;

            // Get the result of the hit
            HitResult hitResult = _control.RegisterHitInput(cell, source);

            if(hitResult == HitResult.Invalid)
            {
                return;
            }

            if(source == PlayerIdentifier.PlayerOne)
            {
                PlayerOneEnemyBoard.UpdateHitResult(cell, hitResult);
            }
            else
            {
                PlayerTwoEnemyBoard.UpdateHitResult(cell, hitResult);
            }


        }

        /// <summary>
        /// Called when the game state changes in the Logic class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StateChangeEvent(object sender, StateChangeEventArgs args)
        {
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

                case GameState.GameEnd:
                    break;

            }
        }

        /// <summary>
        /// Registers any key press, regardless of what control is in focus
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                // Do something specific depending on the current game state
                switch (localGameState)
                {
                    case GameState.GameEnd:
                        break;

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
                        // Show interscreen
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
                        turnPlayed = false;
                        label_Interstitial.Visible = false;

                        if (mainGameState == GameState.PlayerOneTurn)
                        {
                            overHeadLabel.Text = "Player ONE: Make your move.";
                            PlayerOneEnemyBoard.IsActive = true;
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
                            PlayerOneBoard.Hide();
                            PlayerOneEnemyBoard.Hide();
                            PlayerTwoBoard.Show();
                            PlayerTwoBoard.UpdateBoard(_control.GetPlayerTwoShips());
                            PlayerTwoEnemyBoard.Show();
                            localGameState = GameState.PlayerTwoTurn;
                            overHeadLabel.Text = "Player TWO: Make your move.";
                        }
                        break;
                        
                    default:
                        break;
                }

            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Lodas the start setup
        /// </summary>
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
    
        /// <summary>
        /// Changes settings for a turn end
        /// </summary>
        private void TurnEnd()
        {
            overHeadLabel.Text = "Press ENTER to end turn.";
        }

        /// <summary>
        /// Loads settings for the beginning of a game
        /// </summary>
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
            overHeadLabel.Text = "Player ONE: Make your move.";

        }
    }
}
