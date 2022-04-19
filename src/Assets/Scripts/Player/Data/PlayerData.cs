using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This guy declare all variables that a player has
 * The state wants these values to work
 * Just put here for organizational
 *
 * Scriptable Obj allows creation of assets
 */

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Mobile State")] 
    public float walkingVel = 5.0f;
    public float accel = 1.2f;
    public float deccel = 0.5f;

    [Header("Jump State")] 
    public float originalJumpVel = 16.0f;
    public float jumpVel = 16.0f;
    public int maxJumps = 2;
    public float jumpForce = 10.0f; // To be deprecated

    [Header("Airborne State")] 
    public float coyoteTolerance = 0.269f;
    public float vertsDamper = 0.4f;

    [Header("Watchdogs")] 
    public float beepBeepTolerance = 0.18f;
    public LayerMask groundLabel;
    public float wallBeepTolerance = 0.34f;

    [Header("Wall States")] 
    public float wallClimbVel = 3.0f;

    public float wallJumpvel = 14.0f;
    public float wallJumpTime = 0.2f; // Prevent moving back to wall right after jump
    public Vector2 wallJumpAngle = new Vector2(1,2);

    [Header("Dash State")] 
    public float dashCD = 0.1f;

    public float dashTime = 0.25f;
    public float originalDashVel = 20.69f;
    public float dashVel = 20.69f;
    public float dragForce = 10.0f; // Some air resist 
    public float dashEndDamper = 0.5f;
    public float afterImageDist = 0.169f;

    [Header("Attack Stuffs")]
    public int MaxSanity = 100;

    // For audio manager
    public float MagicCd;
    public string MagicId;
    public int MagicCost;

    public float iFrame = 3f;
    public int numOfFlickers = 100;
    public float flickerDelay = 0.01f;
    public float flickerAlpha = 0.5f;
    
    public void OnEnable()
    {
        MaxSanity = 100; 
        walkingVel = 5.0f;
        dragForce = 10.0f; // Some air resist 
        dashVel = originalDashVel;
        jumpVel = originalJumpVel;
        
    }


}
