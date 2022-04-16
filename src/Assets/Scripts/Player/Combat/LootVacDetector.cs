using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootVacDetector : MonoBehaviour
{
    //public List<SuckableAbility> DetectedAbilities = new List<SuckableAbility>();
    public List<AbilityLoot> DetectedAbilities = new List<AbilityLoot>();
    public GameObject TransitionSmokeParticle;

    public void AddDetectedAbility(Collider2D collision)
    {
        //SuckableAbility suckable = collision.GetComponent<SuckableAbility>();
        AbilityLoot suckable = collision.GetComponent<AbilityLoot>();
        if (suckable != null)
        {
            DetectedAbilities.Add(suckable);
        }
    }

    public void RemoveDetectedAbility(Collider2D collision)
    {
        
        //SuckableAbility suckable = collision.GetComponent<SuckableAbility>();
        AbilityLoot suckable = collision.GetComponent<AbilityLoot>();
        if (suckable != null)
        {
            DetectedAbilities.Remove(suckable);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AddDetectedAbility(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        RemoveDetectedAbility(other);
    }

}
