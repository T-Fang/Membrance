using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchDownState : PlayerGroundedState
{
    public PlayerTouchDownState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (ExittingToAnotherState) return;
        if(XInput != 0) StateMachine.ChangeState(Player.RunState);
        else if(Player.Ih.SuccBtnInputted) StateMachine.ChangeState(Player.SuccAbilityState);
        else StateMachine.ChangeState(Player.IdleState);
    }
}
