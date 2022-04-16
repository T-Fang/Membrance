using System;
using UnityEngine;

public class Trap_Spikes : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || collision.gameObject.GetComponent<Player>() == null) return;
        if (collision.gameObject.GetComponent<Player>().InIFrame) return;
        
        FindObjectOfType<AudioManager>().Play("Spike");
            
        // TODO: Avoid null checking, line always crashes for some reason tho
        // Workaround: Reset the Health script attached to Player in the inspector
        if (collision.GetComponent<Health>() != null)
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }

        collision.GetComponent<Player>().MakeInvincible();
        //throw new ArgumentException(nameof(Health) + " could not be found.");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || collision.gameObject.GetComponent<Player>() == null) return;
        if (collision.gameObject.GetComponent<Player>().InIFrame) return;
        
        FindObjectOfType<AudioManager>().Play("Spike");
            
        // TODO: Avoid null checking, line always crashes for some reason tho
        // Workaround: Reset the Health script attached to Player in the inspector
        if (collision.GetComponent<Health>() != null)
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }

        collision.GetComponent<Player>().MakeInvincible();
        //throw new ArgumentException(nameof(Health) + " could not be found.");
    }
}
