using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _jumpsRemaining;
    public PlayerJumpState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
        _jumpsRemaining = Data.maxJumps;
    }

    public override void EnterState()
    {
        base.EnterState();

        Player.AudioManager.Play("Jump");

        Player.Jump(Data.jumpVel);
        Player.Ih.ConsumeJumpButton(); // ensure jump btn is used
        AbilityFinished = true; // "delegating" back up to AbilityState so no need to handle ChangeState here
        Player.AirborneState.SetMidJumping(); // Variadic jumping helper
        --_jumpsRemaining; // Multi Jump support
    }

    public bool StillCanJump()
    {
        return 0 < _jumpsRemaining;
    }

    public void ResetJumpsRemaining() => _jumpsRemaining = Data.maxJumps;
    public void StripJumpAbility() => _jumpsRemaining = 0;
    public void DecreaseJumpsRemaining() => --_jumpsRemaining;
}
