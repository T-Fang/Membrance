using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;

    float cooldownTime;

    float activeTime;

    public Image abilityImageCooldown;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState state = AbilityState.ready;

    public KeyCode key;

    private void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    abilityImageCooldown.fillAmount = 1;
                    ability.Activate(gameObject);
                    state = AbilityState.active;
                    activeTime = ability.activeTimeLen;
                }
                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.unscaledDeltaTime;
                }
                else
                {
                    ability.BeginCooldown(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = ability.cooldownTimeLen;
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.unscaledDeltaTime;
                    abilityImageCooldown.fillAmount -= 1 / ability.cooldownTimeLen * Time.unscaledDeltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                }
                break;
        }
    }
}
