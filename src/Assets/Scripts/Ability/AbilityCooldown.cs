using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    [Header("Ability")]
    public Image abilityImageCooldown;
    public new string name;
    public float cooldownTimeLen;
    bool isCooldown = false;

    [SerializeField] bool isLearned = false;

    private void Start()
    {
        abilityImageCooldown.fillAmount = 0;
    }

    private void Update()
    {
        if (isLearned)
        {
            Ability();
        }
    }

    void Ability()
    {
        if (isCooldown)
        {
            abilityImageCooldown.fillAmount -= 1 / cooldownTimeLen * Time.deltaTime;

            if (abilityImageCooldown.fillAmount <= 0)
            {
                abilityImageCooldown.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

}
