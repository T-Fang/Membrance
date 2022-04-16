using System.Collections;
using UnityEngine;

public class BossAttackTrigger2 : MonoBehaviour
{
    private float lifetime;
    private Health playerHealth;
    private bool isTriggered = false;
    private bool isDamage = false;

    public void ActivateTrigger()
    {
        lifetime = 0;
        if (!isTriggered)
        {
            gameObject.SetActive(true);
            StartCoroutine(Pause());
            isTriggered = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isDamage)
        {
            playerHealth = collision.GetComponent<Health>();
            isDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerHealth = null;
    }

    private void Update()
    {
        // Damage the player
        if (isTriggered && isDamage && playerHealth != null)
        {
            playerHealth.TakeDamage(2);
            isDamage = false;
        }
    }

    private void End()
    {
        gameObject.SetActive(false);
        isTriggered = false;
    }

    private IEnumerator Pause()
    {
        // Wait for delay
        yield return new WaitForSeconds(1.5f);
    }
}
