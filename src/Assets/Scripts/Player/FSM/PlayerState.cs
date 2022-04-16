using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Base State lives here, all other states inherits this guy
 *  Should not be on a game object
 */
public class PlayerState 
{
    // Reference to player, fsm, data
    // TODO: Come back to work on animation
    protected Player Player;
    protected PlayerFSM StateMachine;
    protected PlayerData Data;

    protected float StartTime;
    protected bool AnimationFinished;
    /**
     * This is important, when a childState calls base.UpdateLogic(),
     * we potentially triggers a ChangeState() in the parent's code.
     * This means that whatever code in the old childState below the base.UpdateLogic() call
     * shouldn't happen. This bool helps signalling that childState that we shouldn't
     * There probably exists a better design of an FSM out there, but this tutorial is
     * okay for now
     *
     */
    protected bool ExittingToAnotherState;

    // Animator
    private string _animBoolName;

    public PlayerState(Player player, PlayerFSM sm, PlayerData data, string animBoolName)
    {
        Player = player;
        StateMachine = sm;
        Data = data;
        _animBoolName = animBoolName;
        AnimationFinished = false;
    }

    public virtual void EnterState()
    {
        HandlePhysics();
        Player.Animator.SetBool(_animBoolName, true);
        StartTime = Time.unscaledTime;
        AnimationFinished = false;
        ExittingToAnotherState = false;
    }

    public virtual void ExitState()
    {
        Player.Animator.SetBool(_animBoolName, false);
        ExittingToAnotherState = true;
    }

    // Analogous to unity's Update()
    public virtual void UpdateLogic()
    {
    }

    // Analogous to unity's FixedUpdate()
    public virtual void UpdatePhysics()
    {
        HandlePhysics();
    }

    // Call from enter, physics_update
    public virtual void HandlePhysics()
    {
    }

    public virtual void TriggerAnimation()
    {
    }

    public virtual void TriggerFinishAnimation() => AnimationFinished = true;
}