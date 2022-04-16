using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public override void Activate(GameObject parent)
    {
       base.Activate(parent);
       // TODO: Find a way to reference player data instead of hard coding
       /*
       cooldownTimeLen = PlayerData.dashCD;
       activeTimeLen = PlayerData.dashTime;
       */
       
    }

    public override void BeginCooldown(GameObject parent)
    {
        base.BeginCooldown(parent);
        
    }
}
