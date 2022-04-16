using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimToWeapon : MonoBehaviour
{
    private Weapon _weaponRef;

    private void Start()
    {
        _weaponRef = GetComponentInParent<Weapon>();
    }

    private void TriggerAnimationFinish() => _weaponRef.TriggerFinishAnimation();
    private void TriggerMovement() => _weaponRef.TriggerMovement();
    private void TriggerStopMovement() => _weaponRef.TriggerStopMovement();
    private void TriggerDisableFlip() => _weaponRef.TriggerDisableFlip();
    private void TriggerEnableFlip() => _weaponRef.TriggerEnableFlip();
    private void TriggerActionAnimation() => _weaponRef.TriggerActionAnimation();
}
