using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int _wallJumpDir;
    private bool _inCoyote;
    public PlayerWallJumpState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        if (ExittingToAnotherState) return;
        Player.Ih.ConsumeJumpButton();
        Player.JumpState.ResetJumpsRemaining();
        Player.DashState.ResetDashStamina();
        Player.SetVelAtAngle(Data.wallJumpvel,Data.wallJumpAngle,_wallJumpDir);
        Player.FlipIfNeeded(_wallJumpDir); // Flip to direction of jump
        Player.JumpState.DecreaseJumpsRemaining();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        // TODO: Some animation stuff, when got more sprites

        if (StartTime + Data.wallJumpTime <= Time.unscaledTime) AbilityFinished = true;
    }

    public void SetWallJumpDir(bool isKissingWall)
    {
        if (isKissingWall) _wallJumpDir = -Player.FacingDir;
        else _wallJumpDir = Player.FacingDir;
    }

    public void SetWallJumpDir2(int dir)
    {
        _wallJumpDir = dir;
    }

}


