using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool AbilityFinished;
    protected bool FromChangeBack = false;
    private bool _grounded;
    public PlayerAbilityState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        AbilityFinished = false;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(Player.playerHealth.isDead) {
            StateMachine.ChangeState(Player.DeathState);
        }
        if (!AbilityFinished){return;}
        // Doesn't really work on its own
        if(Player.IsAbsorbing) StateMachine.ChangeState(Player.AbsorbState);
        else if (FromChangeBack)
        {
            FromChangeBack = false;
            StateMachine.ChangeState(Player.AbsorbState);
        }
        else if(_grounded && Player.CurrVel.y < 0.0001f) {StateMachine.ChangeState(Player.IdleState);}
        else StateMachine.ChangeState(Player.AirborneState);
    }

    public override void HandlePhysics()
    {
        base.HandlePhysics();
        _grounded = Player.IsOnTheGround();
    }
}
