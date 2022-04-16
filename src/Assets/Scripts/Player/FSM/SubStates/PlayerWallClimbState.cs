using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerKissingWallState 
{
    public PlayerWallClimbState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (ExittingToAnotherState) return;
        Player.SetVelY(Data.wallClimbVel);
        if (YInput != 1) StateMachine.ChangeState(Player.WGrabState);
    }
}
