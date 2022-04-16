using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon _weaponRef;
    private float _velWhenAttack;
    private bool _shouldSetVel;
    private int _xInput;
    private bool _kitingFrame;
    private bool _grounded;
    private int _attackInAirRemaining;
    public PlayerAttackState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
        _attackInAirRemaining = 1;
    }

    public override void EnterState()
    {
        base.EnterState();

        // Play corresponding sound
        if (Player.Ih.AttackInputted[(int)AttackInputs.slash])
        {
            Player.AudioManager.Play("Slashing");
        }
        /*
        else if (Player.Ih.AttackInputted[(int)AttackInputs.opGun])
        {
            Player.AudioManager.Play("FireGun");
        }
        */

        _shouldSetVel = false;
        _weaponRef.EnterWeapon();
    }

    public override void ExitState()
    {
        base.ExitState();
        _weaponRef.ExitWeapon();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _xInput = Player.Ih.NormalizedInputX;

        /* Helps with kiting, let's say
         * Player is <- -> very quickly while attacking
         * We should flip attack only if the change in direction is pressed very late
         */
        if (_kitingFrame) Player.FlipIfNeeded(_xInput);
        if (_shouldSetVel)
        {
            if (_grounded) Player.SetVelX(_velWhenAttack * Player.FacingDir);
            else Player.SetVelX((Player.data.walkingVel + _velWhenAttack) * Player.FacingDir);
        }
    }

    public void SetCurrWeapon(Weapon weapon)
    {
        this._weaponRef = weapon;
        _weaponRef.InitWeapon(this,Player);
    }

    public void SetPlayerVel(float vel, float vely)
    {
        Player.SetVelX(vel * Player.FacingDir);
        Player.SetVelY(vely);

        _velWhenAttack = vel;
        _shouldSetVel = true;
    }

    public void SetIsInKitingFrame(bool val)
    {
        _kitingFrame = val;
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _grounded = Player.IsOnTheGround();
    }

    public bool CanAttackInAir()
    {
        return 0 < _attackInAirRemaining;
    }

    public void DecreaseAttackInAirRemaining() => --_attackInAirRemaining;
    public void ResetInAirAttackRemaining() => _attackInAirRemaining = 1;


    // ===================================================================
    // ANIMATION STUFF
    // ===================================================================
    public override void TriggerFinishAnimation()
    {
        base.TriggerFinishAnimation();
        AbilityFinished = true;
    }
}
