using UnityEngine;

[CreateAssetMenu]
public class SloMoAbilityPlayer2 : Ability
{
    // Change the slow down factor in the public field of the Player
    public float slowdownFactor = 0.05f;
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);

        Debug.Log("Activate SloMo");

        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        parent.GetComponent<Player>().BeginSloMo(Time.timeScale);
    }

    public override void BeginCooldown(GameObject parent)
    {
        base.BeginCooldown(parent);

        Debug.Log("Begin SloMo Cooldown");

        parent.GetComponent<Player>().EndSloMo(Time.timeScale);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

    }
}
