using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Battleship
{
    public enum ShipDirection
    {
        North,
        East
    }

    public enum MovementDirection
    {
        Left,
        Up,
        Right,
        Down
    }

    public enum CurrentTurn
    {
        PlayerOne,
        Playertwo
    }

    public enum GameState
    {
        PlayerOneSettingUp,
        PlayerTwoSettingUp,
        WaitingToStart,
        PlayerOneTurn,
        PlayerTwoTurn,
        GameEnd
    }
}
