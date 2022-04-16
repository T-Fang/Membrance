using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerKissingWallState
{
    private readonly float TOLERANCE = 0.0001f;
    private Vector2 _holdPosVec;
    public PlayerWallGrabState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _holdPosVec = Player.transform.position;
        HoldPos();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (ExittingToAnotherState) return;
        HoldPos();

        //if (Math.Abs(YInput - 0.0001f) > TOLERANCE) StateMachine.ChangeState(Player.WClimbState);
        if(0.0001f < YInput) StateMachine.ChangeState(Player.WClimbState);
        else if(!GrabInputted) StateMachine.ChangeState(Player.AirborneState);
    }

    private void HoldPos()
    {
        Player.transform.position = _holdPosVec;
        Player.SetVelX(0.0f);
        Player.SetVelY(0.0f);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void HandlePhysics()
    {
        base.HandlePhysics();
    }
}
