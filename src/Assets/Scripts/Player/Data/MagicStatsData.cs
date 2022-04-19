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

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Magic Data")]
public class MagicStatsData : ScriptableObject
{
 public float UniqueSpeed; // set this for different abilities, don't bother setting projectile speed
 public float ProjectileSpeed;
 public float MagicATT;

 void OnEnable()
 {
  ProjectileSpeed = UniqueSpeed;
 }


}
