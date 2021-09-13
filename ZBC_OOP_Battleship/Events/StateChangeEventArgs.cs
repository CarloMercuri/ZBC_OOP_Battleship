using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Battleship
{
    public class StateChangeEventArgs : EventArgs
    {
        private GameState gameState;

        /// <summary>
        /// The new game state
        /// </summary>
        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public StateChangeEventArgs(GameState gameState)
        {
            this.gameState = gameState;
        }

    }
}
