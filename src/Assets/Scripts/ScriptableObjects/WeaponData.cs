using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon Data/Weapon")]
public class WeaponData : ScriptableObject
{
    public int numberOfAttacks; // within the combo
    
    // Making this public to swap data...
    public float[] movSPD { get; set; }
    public float movSPDY { get; set; }
}
