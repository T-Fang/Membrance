using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKissingWallState : PlayerState
{
    protected bool Grounded;
    protected bool KissingWall;
    protected bool GrabInputted;
    protected int XInput;
    protected int YInput;
    protected bool JumpInputted;
    public PlayerKissingWallState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
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
        if(Player.playerHealth.isDead) {
            StateMachine.ChangeState(Player.DeathState);
        }
        XInput = Player.Ih.NormalizedInputX;
        YInput = Player.Ih.NormalizedInputY;
        GrabInputted = Player.Ih.GrabInputted;
        JumpInputted = Player.Ih.JumpInputted;

        if (JumpInputted)
        {  
            Player.Ih.ConsumeJumpButton();
            Player.WJumpState.SetWallJumpDir(KissingWall);
            StateMachine.ChangeState(Player.WJumpState);
        }
        else if (Grounded && !GrabInputted)
        {
            StateMachine.ChangeState(Player.IdleState);
        }else if (!KissingWall || (XInput != Player.FacingDir && !GrabInputted))
        {
            StateMachine.ChangeState(Player.AirborneState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void HandlePhysics()
    {
        base.HandlePhysics();
        Grounded = Player.IsOnTheGround();
        KissingWall = Player.IsKissingWall();
    }
}
