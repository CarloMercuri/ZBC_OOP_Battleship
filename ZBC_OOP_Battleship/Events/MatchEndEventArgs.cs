using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Battleship
{
    public class MatchEndEventArgs : EventArgs
    {
        private PlayerIdentifier winningPlayer;

        /// <summary>
        /// The player that won the game
        /// </summary>
        public PlayerIdentifier WinningPlayer
        {
            get { return winningPlayer; }
            set { winningPlayer = value; }
        }

 
    }
}
