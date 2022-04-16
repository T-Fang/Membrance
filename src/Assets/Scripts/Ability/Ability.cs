using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;

    public float cooldownTimeLen;

    public float activeTimeLen;

    // TODO: Find a way to do this
    //public PlayerData DataReference;

    public float AbilityLimit = 100f;

    public float Cost;

    public virtual void Activate(GameObject parent)
    {
    }

    public virtual void BeginCooldown(GameObject parent)
    {
    }

    public virtual void ConsumeMana()
    {
        AbilityLimit -= Cost;
    }

    public virtual void ExpireAbilityIfNeeded()
    {
        // TODO: Kill off ability somehow
    }

}
