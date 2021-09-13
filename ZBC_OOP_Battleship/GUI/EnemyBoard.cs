using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBC_OOP_Battleship
{
    public class EnemyBoard : PlayingBoard
    {
       
        private EnemyBoardSlotStatus[,] boardSlots;

        private PlayerIdentifier playerOwner;

        private Pen circlePen;
        private Pen crossPen;
        private int circleRadius = 15;


        public override void CreatePanel(Control parentControl, bool playerBoard, PlayerIdentifier source, int locX = 0, int locY = 0)
        {
            crossPen = new Pen(Color.Red, 2);
            circlePen = new Pen(Color.Blue, 3);
            playerOwner = source;
            boardSlots = new EnemyBoardSlotStatus[10, 10];

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    boardSlots[x, y] = EnemyBoardSlotStatus.NotTried;
                }
            }

            base.CreatePanel(parentControl, playerBoard, source, locX, locY);
        }

        /// <summary>
        /// If the slot has not been hit before
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool IsSlotValidTarget(Point cell)
        {
            if(boardSlots[cell.X, cell.Y] == EnemyBoardSlotStatus.NotTried)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Registers the result of a hit
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="isHit"></param>
        public void UpdateHitResult(Point cell, HitResult isHit)
        {
            if (isHit == HitResult.Successful)
            {
                boardSlots[cell.X, cell.Y] = EnemyBoardSlotStatus.SuccessfulHit;
            }
            else
            {
                boardSlots[cell.X, cell.Y] = EnemyBoardSlotStatus.FailedHit;
            }

            UpdateBoard();
        }

        /// <summary>
        /// Forces the board to update
        /// </summary>
        public override void UpdateBoard()
        {
            BattlePanel.Invalidate();
        }

        /// <summary>
        /// The main paint event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void BattlePanelPaint(object sender, PaintEventArgs e)
        {
            // Running the base paint event first, so whatever we draw here comes on top
            base.BattlePanelPaint(sender, e);

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if(boardSlots[x, y] == EnemyBoardSlotStatus.FailedHit)          // Draw a circle
                    {
                        e.Graphics.DrawEllipse(circlePen,
                                               Constants.CellSize * x + (Constants.CellSize / 2) - circleRadius,
                                               Constants.CellSize * y + (Constants.CellSize / 2) - circleRadius,
                                               circleRadius + circleRadius, circleRadius + circleRadius);
                    }
                    else if(boardSlots[x, y] == EnemyBoardSlotStatus.SuccessfulHit) // Draw a cross
                    {
                        // The line from top left to bottom right
                        e.Graphics.DrawLine(crossPen,
                                            Constants.CellSize * x + 6,                             // x1
                                            Constants.CellSize * y + 6,                             // y1
                                            Constants.CellSize * x + Constants.CellSize - 6,        // x2
                                            Constants.CellSize * y + Constants.CellSize - 6);       // y2

                        // The line from the top right to the bottom left
                        e.Graphics.DrawLine(crossPen,
                                           Constants.CellSize * x + Constants.CellSize - 6,         // x1
                                           Constants.CellSize * y + 6,                              // y1
                                           Constants.CellSize * x + 6,                              // x2
                                           Constants.CellSize * y + Constants.CellSize - 6);        // y2
                    }
                }
            }
        }
    }
}
