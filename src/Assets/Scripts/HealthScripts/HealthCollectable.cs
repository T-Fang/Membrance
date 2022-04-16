using System;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{

    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only add health when the player is not full health
        // TODO: Stop null-check hax
        if (!collision.CompareTag("Player") || collision.gameObject.GetComponent<Health>() == null) return;
        if (!(Math.Abs(collision.GetComponent<Health>().currentHealth -
                       collision.GetComponent<Health>().startingHealth) > 0.001f)) return;
        
        collision.GetComponent<Health>().AddHealth(healthValue);
        gameObject.SetActive(false);
    }
}
