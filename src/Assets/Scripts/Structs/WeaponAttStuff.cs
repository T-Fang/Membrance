using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttStuffs
{
    public string attackName;
    public float movementSPD;
    public float movementSPDY; // for jump attack only
    public float damage;
    
    // For knockbacking enemies later on
    /*
    public float knockbackMagnitude;
    public Vector2 knockbackAngle;
    */
    
}
