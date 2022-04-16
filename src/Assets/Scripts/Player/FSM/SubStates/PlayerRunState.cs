using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundedState
{

    
    public PlayerRunState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
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
        if(XInput == 0 && !ExittingToAnotherState) StateMachine.ChangeState(Player.IdleState);
    }

    public override void UpdatePhysics()
    {
        // Physics magic for movement acceleration and decceleration
        // Migrating away from AddForce, not used..
        /*
        base.UpdatePhysics();
        var targetSpeed = XInput * Player.data.walkingVel; // 0 if stoppping, max if keep on pressing
        var maxCurrDiff = targetSpeed - Player.CurrVel.x; // difference until max speed
        var a = (0.001f < Mathf.Abs(targetSpeed)) ? Player.data.accel : Player.data.deccel;
        float velocity = Mathf.Pow(Mathf.Abs( maxCurrDiff) * a, 1.0f) * Mathf.Sign( maxCurrDiff);
        Player.BodyRef.AddForce(velocity * Vector2.right);
    */
        Player.FlipIfNeeded(XInput);
        // TODO: Find out how to do acceleration with MovePosition
        Player.SetVelX(Data.walkingVel * XInput);
    }
}
