using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamagingWeapon : Weapon
{
    private List<HittableObject> _detectedHittableObjects = new List<HittableObject>();
    //private List<KnockbackableObject> _detectedKnockableObjects = new List<KnockbackableObject>();
    
    // Publicise to swap...
    protected DamagingWeaponData MdamagingWeaponData;

    protected override void Awake()
    {
        base.Awake();
        if (_weaponData.GetType() == typeof(DamagingWeaponData)) 
            MdamagingWeaponData = (DamagingWeaponData) _weaponData; //down-casting
        //else Debug.LogError("Weapon data might be wrong!");
    }

    public DamagingWeaponData ThisDamageWeaponData()
    {
        return MdamagingWeaponData;
    }

    public void SetDamagingWeaponData(DamagingWeaponData data)
    {
        MdamagingWeaponData = data;
    }

    public override void TriggerActionAnimation()
    {
        base.TriggerActionAnimation();
        DoMeleeAttackIfNeeded(); // Might be bad performance wise, supposed to be inside TriggerAction
    }

    public void AddDetectedHittable(Collider2D collision)
    {
        HittableObject hittable = collision.GetComponent<HittableObject>();
        if (hittable != null)
        {
            _detectedHittableObjects.Add(hittable);     
            //DoMeleeAttackIfNeeded(); // Might be bad performance wise, supposed to be inside TriggerAction
        }
        /*
        KnockbackableObject knockbackable = collision.GetComponent<KnockbackableObject>();
        if (knockbackable != null)
        {
            _detectedHittableObjects.Add(knockbackable);
        }
        */
    }
    

    public void RemoveDetectedHittable(Collider2D collision)
    {
        HittableObject hittable = collision.GetComponent<HittableObject>();
        if (hittable != null)
        {
            _detectedHittableObjects.Remove(hittable);     
        }

        /*
        KnockbackableObject knockbackable = collision.GetComponent<KnockbackableObject>();
        if (knockbackable != null)
        {
            _detectedHittableObjects.Remove(knockbackable);
        }
        */

    }

    private void DoMeleeAttackIfNeeded()
    {
        //WeaponAttStuffs stats = MdamagingWeaponData.AttackStuffs[ComboCounter];
        // TODO: Dangerous logic, fix this 
        Debug.Log(Player.absorbedId+Player.TypeOfAttack);
        WeaponAttStuffs stats = Player.WeaponList.ListOfWeapons[Player.absorbedId+Player.TypeOfAttack].AttackStuffs[ComboCounter];
        foreach (HittableObject obj in _detectedHittableObjects.ToList())
        {
            obj.Damage(stats.damage);
        }

        /*
        foreach (KnockbackableObject obj in _detectedKnockbackableObjects.ToList())
        {
            obj.Knockback(stats.knockbackAngle, stats.knockbackMagnitude, Player.FacingDir);
        }
    */
    }

}
