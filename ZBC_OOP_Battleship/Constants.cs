using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Battleship
{
    public static class Constants
    {
        public static int CellSize { get; set; } = 40;
        public static Color ShipBorderColor { get; set; } = Color.FromArgb(180, 40, 40, 40);
        public static Color ShipInnerColor { get; set; } = Color.FromArgb(180, 130, 130, 130);

        public static int BattlePanelSize = 400;

        public static int HeaderPanelSize = 440;

    }
}
