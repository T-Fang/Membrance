using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor;
using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    
    // =============================================================
    // Inputs
    // =============================================================
    private int _xInput;
    private bool _jumpInputted;
    private bool _jumpBtnLifted;
    private bool _grabInputted;
    private bool _dashInputted;
    private bool _swordAttackInputted;
    
    private bool _grounded;
    private bool _inCoyote;
    private bool _inWallJumpCoyote; // helps with chance wall jumping
    private bool _midJumping;
    private bool _kissingWall;
    private bool _prevKissingWall; // helps with chance wall jumping
    private bool _backKissingWall;
    private bool _prevBackKissingWall; // helps with chance wall jumping
    private float _startWallJumpTime;
    public PlayerAirborneState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        _prevKissingWall = false;
        _prevBackKissingWall = false;
        _kissingWall = false;
        _backKissingWall = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Player.playerHealth.isDead)
        {
            StateMachine.ChangeState(Player.DeathState);
        }
        TrackCoyote();
        TrackWJumpCoyote();
        HandleVariadicJump();
        _xInput = Player.Ih.NormalizedInputX;
        _jumpInputted = Player.Ih.JumpInputted;
        _jumpBtnLifted = Player.Ih.JumpBtnLifted;
        _grabInputted = Player.Ih.GrabInputted;
        _dashInputted = Player.Ih.DashInputted;
        _swordAttackInputted = Player.Ih.AttackInputted[(int) AttackInputs.slash] || Player.Ih.AttackInputted[(int)AttackInputs.upSlash];
        
        if (Player.UpSlashAttackState.CanAttackInAir() && _swordAttackInputted)
        {
            if (Player.Ih.AttackInputted[(int) AttackInputs.upSlash])
            {
                // Combo keystrokes is surprisingly cumbersome for Unity lmao
                // Can't assign to 'C' also, owells
                Player.UpSlashAttackState.DecreaseAttackInAirRemaining();
                StateMachine.ChangeState(Player.UpSlashAttackState);
            }else if(Player.Ih.AttackInputted[(int)AttackInputs.slash]) {
                StateMachine.ChangeState(Player.SlashAttackState);
            }
        }
        if (Player.Ih.AttackInputted[(int) AttackInputs.opGun] && Player.DoMagicState.CanMagic() && Player.CurrentSanity > 0 )
        {
            /*
            Player.ShootFireball();
            Player.mentalDown(25);
            */
            StateMachine.ChangeState(Player.DoMagicState);
        }
        // Landed on flo
        else if(_grounded && Player.CurrVel.y <= 0.001f) StateMachine.ChangeState(Player.TdState);
        else if (_jumpInputted && (_kissingWall || _backKissingWall || _inWallJumpCoyote))
        {
            _inWallJumpCoyote = false;
            Player.WJumpState.SetWallJumpDir(_kissingWall);
            //Player.WJumpState.SetWallJumpDir2(_xInput);
            StateMachine.ChangeState(Player.WJumpState);
        }
        else if (_jumpInputted && Player.JumpState.StillCanJump())
        {
            StateMachine.ChangeState(Player.JumpState);
        }
        else if(_kissingWall && _grabInputted) StateMachine.ChangeState(Player.WGrabState);
        //else if (_kissingWall && _xInput == Player.FacingDir) //StateMachine.ChangeState(Player.WSlideState);
        else if (_dashInputted && Player.DashState.PlayerCanDash()) StateMachine.ChangeState(Player.DashState);
        else
        {
            Player.FlipIfNeeded(_xInput);
            // Airborne movement
            if(Player.Ih.NormalizedInputX != 0) Player.SetVelX(Data.walkingVel * _xInput);
            else Player.SetVelX(Player.CurrVel.x); // abit hacky, allow momentum
        }

    }

    public override void HandlePhysics()
    {
        base.HandlePhysics();
        _grounded = Player.IsOnTheGround();
        _prevKissingWall = _kissingWall;
        _prevBackKissingWall = _backKissingWall;
        _kissingWall = Player.IsKissingWall();
        _backKissingWall = Player.IsBackKissingWall();
        
        // if not touching wall this time but touches it last frame
        if(!_inWallJumpCoyote && !_kissingWall && !_backKissingWall && (_prevKissingWall || _prevBackKissingWall)) {
            StartWallJumpCoyote();        
        }
    }

    private void TrackCoyote()
    {
        // Basically saying: If inCoyote and Time exceeds start+tolerance, strip jump ability
        if (!_inCoyote || !(StartTime + Data.coyoteTolerance< Time.unscaledTime)) return;
        _inCoyote = false;
        Player.JumpState.StripJumpAbility();
    }

    private void TrackWJumpCoyote()
    {
        if (_inWallJumpCoyote || !(_startWallJumpTime + Data.coyoteTolerance< Time.unscaledTime)) return;
        _inWallJumpCoyote = false;
    }

    private void HandleVariadicJump()
    {
        if (!_midJumping) return;
        if(Player.Ih.JumpBtnLifted) {
            //Debug.Log("Damping");
            Player.Jump(Player.CurrVel.y * Data.vertsDamper); // Lifted early, jump short king
            _midJumping = false; // You do not want to damp forever
        }
        else if (Player.CurrVel.y <= 0.001f) _midJumping = false;  // You don't want it to fall slower on the way down
    }

    public void StartCoyote() => _inCoyote = true;

    public void StartWallJumpCoyote()
    {
        _inWallJumpCoyote = true;
        _startWallJumpTime = Time.unscaledTime;
    }

    public void SetMidJumping() => _midJumping = true;

}
