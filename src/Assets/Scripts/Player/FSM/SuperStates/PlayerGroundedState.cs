using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int XInput;
    private bool _jumpInputted;
    private bool _inCoyote;
    private bool _grounded;
    private bool _kissingWall;
    private bool _grabInputted;
    private bool _dashInputted;
    private bool _succInputted;
    private bool _changeBackInputted;
    public PlayerGroundedState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }


    public override void EnterState()
    {
        base.EnterState();
        Player.JumpState.ResetJumpsRemaining();
        Player.DashState.ResetDashStamina();
        Player.UpSlashAttackState.ResetInAirAttackRemaining();
    }


    public override void ExitState()
    {
        base.ExitState();
    }


    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Player.playerHealth.isDead)
        {
            StateMachine.ChangeState(Player.DeathState);
        }

        XInput = Player.Ih.NormalizedInputX;
        _jumpInputted = Player.Ih.JumpInputted;
        _grabInputted = Player.Ih.GrabInputted;
        _dashInputted = Player.Ih.DashInputted;
        _succInputted = Player.Ih.SuccBtnInputted;
        _changeBackInputted = Player.Ih.ChangeBackInputted;

        if (Player.Ih.AttackInputted[(int)AttackInputs.slash])
        {
            StateMachine.ChangeState(Player.SlashAttackState);
        }
        else if (Player.Ih.AttackInputted[(int)AttackInputs.opGun] && Player.DoMagicState.CanMagic() && Player.CurrentSanity > 0)
        {
            //Player.ShootFireball();
            StateMachine.ChangeState(Player.DoMagicState);
        }else if (_succInputted)
        {
            StateMachine.ChangeState(Player.SuccAbilityState);
        }else if (_changeBackInputted)
        {
            StateMachine.ChangeState(Player.BackToDefaultState);
        }
        else if (_jumpInputted && Player.JumpState.StillCanJump())
        {
            //Debug.Log("GroundedState: " + Player.Ih.JumpInputted);
            StateMachine.ChangeState(Player.JumpState);

        }
        else if (!_grounded)
        {
            Player.AirborneState.StartCoyote();
            StateMachine.ChangeState(Player.AirborneState);

        }
        else if (_kissingWall && _grabInputted) StateMachine.ChangeState(Player.WGrabState);
        else if (_dashInputted && Player.DashState.PlayerCanDash()) StateMachine.ChangeState(Player.DashState);

    }

    public override void HandlePhysics()
    {
        base.HandlePhysics();
        _grounded = Player.IsOnTheGround();
        _kissingWall = Player.IsKissingWall();
    }
}
