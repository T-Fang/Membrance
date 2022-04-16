using UnityEngine;

[CreateAssetMenu]
public class SloMoAbility : Ability
{
    // Change the slow down factor in the public field of the Player
    public float slowdownFactor = 0.05f;
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);

        Debug.Log("Activate SloMo");

        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        parent.GetComponent<PlayerMovementTest>().BeginSloMo(Time.timeScale);
    }

    public override void BeginCooldown(GameObject parent)
    {
        base.BeginCooldown(parent);

        Debug.Log("Begin SloMo Cooldown");

        parent.GetComponent<PlayerMovementTest>().EndSloMo(Time.timeScale);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

    }
}
