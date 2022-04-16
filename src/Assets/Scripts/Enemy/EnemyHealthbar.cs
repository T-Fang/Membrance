using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    public Slider Slider;
    public Color Low;
    public Color High;
    //public Transform FollowThisGuy;
    //public Vector3 Offset = new Vector3(0.0f, 1.0f, 0.0f);
    public Gradient gradient;

    void Start()
    {
        //FollowThisGuy = GetComponentInParent<MeleeEnemy>().transform;
    }

    void Update()
    {
        // Move slider with object
        //Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
        //Slider.transform.position = FollowThisGuy.position + Offset;

    }

    public void SetMaxHealth(float health)
    {
        Slider.value = health;
        Slider.maxValue= health;
        gradient.Evaluate(1f);
        Slider.fillRect.GetComponentInChildren<Image>().color = gradient.Evaluate(1f);
        //Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }

    public void SetHealth(float health, float maxHealth)
    {
        //Slider.gameObject.SetActive(true);
        Slider.value = health;
        Slider.maxValue = maxHealth;
        Slider.fillRect.GetComponentInChildren<Image>().color = gradient.Evaluate(Slider.normalizedValue);
        //Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
        //Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }
}
