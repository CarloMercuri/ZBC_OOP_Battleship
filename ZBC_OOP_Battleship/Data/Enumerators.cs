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

    public enum HitResult
    {
        Successful,
        Unsuccessful,
        Invalid
    }

    public enum PlayerIdentifier
    {
        PlayerOne,
        PlayerTwo
    }

    public enum MovementDirection
    {
        Left,
        Up,
        Right,
        Down
    }

    public enum EnemyBoardSlotStatus
    {
        SuccessfulHit,
        FailedHit,
        NotTried
    }

    public enum GameState
    {
        PlayerOneSettingUp,
        PlayerTwoSettingUp,
        WaitingToStart,
        PlayerOneTurn,
        PlayerTwoTurn,
        GameEnd,
        WaitingToChangeTurn,
        WaitingToEndTurn
    }
}
