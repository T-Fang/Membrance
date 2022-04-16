using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "newDamagingWeaponData", menuName = "Data/Weapon Data/Damaging Weapon")]
public class DamagingWeaponData : WeaponData
{
    [SerializeField] public WeaponAttStuffs[] _attStuffs;
    
    // Make public set to swap attack stats..
    public WeaponAttStuffs[] AttackStuffs { get => _attStuffs; set => _attStuffs = value; }
    
    // Awake() for SO isn't same as normal script, OnEnable() is the one to go
    private void OnEnable()
    {
        // From WeaponData
        numberOfAttacks = _attStuffs.Length;
        movSPD = new float[numberOfAttacks];

        for (int i = 0; i < numberOfAttacks; ++i)
        {
            movSPD[i] = _attStuffs[i].movementSPD;
            movSPDY = _attStuffs[i].movementSPDY; // there is only one MOVSPDY
        }
    }
}
