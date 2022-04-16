using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState 
{
    public PlayerIdleState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
       base.EnterState();
       Player.SetVelX(0.0f);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (XInput != 0 && !ExittingToAnotherState) StateMachine.ChangeState(Player.RunState);
    }
}
