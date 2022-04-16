using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This guy is responsible for telling AttackState
 * how the weapon supposed to be controlled
 *
 * Also setting up the animation for weapons
 */
public class Weapon : MonoBehaviour
{
    protected Animator BodyAnimator;
    protected Animator WeaponAnimator;
    protected PlayerAttackState AttState;
    protected Player Player;

    [SerializeField]
    protected WeaponData _weaponData;
        
    protected int ComboCounter;
    

    protected virtual void Awake()
    {
        BodyAnimator = transform.Find("Base").GetComponent<Animator>();
        WeaponAnimator = transform.Find("Weapon").GetComponent<Animator>();
        
        gameObject.SetActive(false); // So weapon animator won't run constantly even though we not using
    }

    // "Enter" from AttackState
    public virtual void EnterWeapon()
    {
        Debug.Log("Current Num of attack: " + _weaponData.numberOfAttacks);
        ComboCounter %= _weaponData.numberOfAttacks;
        gameObject.SetActive(true);
        BodyAnimator.SetBool("attack",true);
        WeaponAnimator.SetBool("attack",true);
        BodyAnimator.SetInteger("comboCounter",ComboCounter);
        WeaponAnimator.SetInteger("comboCounter",ComboCounter);
    }

    public virtual void ExitWeapon()
    {
        BodyAnimator.SetBool("attack", false);
        WeaponAnimator.SetBool("attack", false);

        ComboCounter++;
        gameObject.SetActive(false);
    }
    
    

    // State will initialize the weapon, so it can ref the state
    public void InitWeapon(PlayerAttackState attState, Player player)
    {
        this.AttState = attState;
        this.Player = player;
    }
    
    // ===================================================================
    // ANIMATION STUFF
    // ===================================================================
    public virtual void TriggerFinishAnimation()
    {
        AttState.TriggerFinishAnimation();        
    }


    public virtual void TriggerMovement()
    {
       //AttState.SetPlayerVel(_weaponData.movSPD[ComboCounter], _weaponData.movSPDY); 
       DamagingWeaponData current = Player.WeaponList.ListOfWeapons[Player.absorbedId + Player.TypeOfAttack];
       AttState.SetPlayerVel(current.movSPD[ComboCounter], current.movSPDY); 
    }

    public virtual void TriggerStopMovement()
    {
        AttState.SetPlayerVel(0.0f,0.0f);
    }

    public virtual void TriggerEnableFlip()
    {
        AttState.SetIsInKitingFrame(true);
    }

    public virtual void TriggerDisableFlip()
    {
        AttState.SetIsInKitingFrame(false);
    }

    // Logic about damaging weapons live here
    public virtual void TriggerActionAnimation()
    {
    }
}
