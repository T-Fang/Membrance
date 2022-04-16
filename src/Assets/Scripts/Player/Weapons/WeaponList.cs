using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataList", menuName = "Data/WeaponList Data/WeaponList")]
public class WeaponList : ScriptableObject
{
    public DamagingWeaponData[] ListOfWeapons;
}
