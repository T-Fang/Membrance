using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder2 : MonoBehaviour
{
    public Ability ability;

    float cooldownTime;

    float activeTime;

    private Player _playerRef;

    public Image abilityImageCooldown;

    [SerializeField] bool isLearned = false;

    private void Start()
    {
        _playerRef = GetComponentInParent<Player>();
    }

    public bool IsLearned()
    {
        return isLearned;
    }

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState state = AbilityState.ready;

    public KeyCode key;
    public int AbilitiyId;

    private void Update()
    {
        if (isLearned)
        {
            switch (state)
            {
                case AbilityState.ready:
                    if (Input.GetKeyDown(key) || _playerRef.Ih.CurrentAbility == AbilitiyId)
                    {
                        abilityImageCooldown.fillAmount = 1;

                        ability.Activate(gameObject);
                        state = AbilityState.active;
                        activeTime = ability.activeTimeLen;
                        _playerRef.Ih.HackyConsumption();
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
                        abilityImageCooldown.fillAmount = cooldownTime / ability.cooldownTimeLen;
                    }
                    else
                    {
                        state = AbilityState.ready;
                    }
                    break;
            }
        }
    }
}
