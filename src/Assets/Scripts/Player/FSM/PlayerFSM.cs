using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  This reference the desired state create from Player
 */
public class PlayerFSM
{
    // Reference to current state
    public PlayerState CurrState { get; private set; }

    // Initialize state
    public void Init(PlayerState firstState)
    {
        CurrState = firstState;
        CurrState.EnterState();
    }

    // Change state
    public void ChangeState(PlayerState newState)
    {
        CurrState.ExitState();
        CurrState = newState;
        CurrState.EnterState();
    }
}
