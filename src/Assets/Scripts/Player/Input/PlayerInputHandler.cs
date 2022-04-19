using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop; // refer to README

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawInput { get; private set; } 
    // public Vector2Int DashDirInput { get; private set; }  // Limit dashes to 0,45,90,... degree
    public int NormalizedInputX { get; private set; } 
    public int NormalizedInputY { get; private set; }
    public bool JumpInputted { get; private set; } // Is there a past tense for "input"? Looks so weird haha
    public bool JumpBtnLifted { get; private set; } // This guy helps with variadic jumps
    public bool DashInputted { get; private set; }
    public bool DashBtnLifted { get; private set; }
    public bool SlowMoBtnInputted { get; private set; }
    public bool SlowMoBtnLifted { get; private set; }
    public bool[] AttackInputted { get; private set; }
    public bool SuccBtnInputted { get; private set; }
    public bool ChangeBackInputted { get; private set; }

    public int CurrentAbility { get; private set; }

    private float _inputStart;
    private float _dashInputStart;
    private float _inputTolerance = 0.2f;

    public bool GrabInputted { get; private set; }

    private void Start()
    {
        var count = Enum.GetValues(typeof(AttackInputs)).Length;
        AttackInputted = new bool[count];
    }

    public void Update()
    {
       CheckJumpBtnHoldTime();
       CheckDashBtnHoldTime();
       //OldJumpHandler();
    }


    public void OnRunInput(InputAction.CallbackContext context)
    {
        RawInput = context.ReadValue<Vector2>();
        
        // right = inputx, up = inputy
        NormalizedInputX = (int) (RawInput * Vector2.right).normalized.x;
        NormalizedInputY = (int) (RawInput * Vector2.up).normalized.y;
        //NormalizedInputX = Mathf.RoundToInt(RawInput.x);
        //NormalizedInputX = Mathf.RoundToInt(RawInput.y);
    }

    /**
     * I thought I was cool for using the new input system, I wasn't
     * For some reason context.started keep registering ButtonDown more than once
     * Online says .performed should perform the registration only once, doesn't seem that way
     *
     * Below is a hack that i do not comprehend how it works, accidentally found out that
     * assigning using Time.unscaledTime somehow screw up the registrations. If i put it exclusively in
     * the if statement it works FOR NOW though
     */
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.started) _inputStart = Time.unscaledTime;
            JumpInputted = true;
            JumpBtnLifted = false;
        }

        else if (context.canceled)
        {
            JumpBtnLifted = true;
        }

    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started) GrabInputted = true;
        if (context.canceled) GrabInputted = false;
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.started) {_dashInputStart = Time.unscaledTime;}
            DashInputted = true;
            DashBtnLifted = false;
            CurrentAbility = (int) Abilities.dash;
        }

        else if (context.canceled)
        {
            DashBtnLifted = true;
            CurrentAbility = (int) Abilities.none;
        }
    }

    public void OnSlashAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputted[(int)AttackInputs.slash] = true;
            Player.SetCurrentTypeOfAttack((int)AttackInputs.slash);
        }

        if (context.canceled)
        {
            AttackInputted[(int)AttackInputs.slash] = false;
        }
    }

    public void OnGunAttackInput(InputAction.CallbackContext context)
    {
            if (context.started)
            {
                AttackInputted[(int)AttackInputs.opGun] = true;
                Player.SetCurrentTypeOfAttack((int)AttackInputs.opGun);
            }

        if (context.canceled)
        {
            AttackInputted[(int)AttackInputs.opGun] = false;
        }

    }

    public void OnUpAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputted[(int)AttackInputs.upSlash] = true;
            Player.SetCurrentTypeOfAttack((int)AttackInputs.upSlash);
        }

        if (context.canceled)
        {
            AttackInputted[(int)AttackInputs.upSlash] = false;
        }
    }

    public void OnSlowMoInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SlowMoBtnInputted = true;
            SlowMoBtnLifted = false;
            CurrentAbility = (int) Abilities.slomo;
        }

        if (context.canceled)
        {
            SlowMoBtnLifted = true;
            CurrentAbility = (int) Abilities.none;
        }
    }

    public void OnSuccInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SuccBtnInputted = true;
        }

        if (context.canceled)
        {
            SuccBtnInputted = false;
        }
    }

    public void OnChangeBackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ChangeBackInputted = true;
        }

        if (context.canceled)
        {
            ChangeBackInputted = false;
        }
    }



    // These are important to let Unity know we are not inputting forever
    public void ConsumeJumpButton() => JumpInputted = false;
    public void ConsumeDashButton() => DashInputted = false;
    public void ConsumeSlowMoButton() => SlowMoBtnInputted = false;
    public void ConsumeSuccButton() => SuccBtnInputted = false;
    public void ConsumeChangeBackButton() => ChangeBackInputted = false;

    public void HackyConsumption()
    {
        switch (CurrentAbility)
        {
            case (int)(Abilities.none): break;
            case (int)(Abilities.dash): ConsumeDashButton();
                break;
            case (int)(Abilities.slomo): ConsumeSlowMoButton();
                break;
        }
    }

    // JumpInputted = false when time is beyond tolerated value (doesn't hold value for too long)
    private void CheckJumpBtnHoldTime() => JumpInputted = !(_inputStart + _inputTolerance <= Time.unscaledTime);
    private void CheckDashBtnHoldTime() => DashInputted = !(_dashInputStart + _inputTolerance <= Time.unscaledTime);
    
}

public enum AttackInputs
{
    slash,
    opGun,
    upSlash
}

public enum Abilities
{
    none,
    slomo,
    dash
}
