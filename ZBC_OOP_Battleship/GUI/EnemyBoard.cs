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

        private Pen circlePen;
        private Pen crossPen;
        private int circleRadius = 15;


        public override void CreatePanel(Control parentControl, bool playerBoard, PlayerIdentifier source, int locX = 0, int locY = 0)
        {
            crossPen = new Pen(Color.Red, 2);
            circlePen = new Pen(Color.Blue, 3);

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

        public override void UpdateBoard()
        {
            BattlePanel.Invalidate();
        }

        public override void BattlePanelPaint(object sender, PaintEventArgs e)
        {
            base.BattlePanelPaint(sender, e);

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if(boardSlots[x, y] == EnemyBoardSlotStatus.FailedHit)
                    {
                        e.Graphics.DrawEllipse(circlePen,
                                               Constants.CellSize * x + (Constants.CellSize / 2) - circleRadius,
                                               Constants.CellSize * y + (Constants.CellSize / 2) - circleRadius,
                                               circleRadius + circleRadius, circleRadius + circleRadius);
                    }
                    else if(boardSlots[x, y] == EnemyBoardSlotStatus.SuccessfulHit)
                    {
                        e.Graphics.DrawLine(crossPen,
                                            Constants.CellSize * x + 6,
                                            Constants.CellSize * y + 6,
                                            Constants.CellSize * x + Constants.CellSize - 6,
                                            Constants.CellSize * y + Constants.CellSize - 6);

                        e.Graphics.DrawLine(crossPen,
                                           Constants.CellSize * x + Constants.CellSize - 6,
                                           Constants.CellSize * y + 6,
                                           Constants.CellSize * x + 6,
                                           Constants.CellSize * y + Constants.CellSize - 6);
                    }
                }
            }
        }
    }
}
