using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This guy passes whats inside the sord collision box to the weapon script
 */
public class WeaponHitboxToWeapon : MonoBehaviour
{
    private DamagingWeapon _weaponRef;

    private void Awake()
    {
        _weaponRef = GetComponentInParent<DamagingWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _weaponRef.AddDetectedHittable(collision);    
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        _weaponRef.RemoveDetectedHittable(collision);
    }
}
