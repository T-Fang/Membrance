using System.Collections;
using UnityEngine;

public class Trap_Fire : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Health playerHealth;
    private bool isTriggered;
    private bool isActive;
    private bool isDamage;
    private Player _playerRef;

    void Awake()
    {   
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerRef = collision.GetComponent<Player>();
            if (!isTriggered)
            {
                // Activate the firetrap
                StartCoroutine(ActivateFireTrap());
                isDamage = true;
            }

            playerHealth = collision.GetComponent<Health>();
      
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerHealth = null;
    }

    private void Update()
    {
        // Damage the player
        if (isActive && isDamage && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            _playerRef.MakeInvincible();
            isDamage = false;
        }
    }

    private IEnumerator ActivateFireTrap()
    {
        // Turn the sprite red to notify that the firetrap is triggered
        isTriggered = true;
        spriteRenderer.color = Color.red; 

        // Wait for delay, activate trap, turn on animaton, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        // Turn the sprite back to white
        spriteRenderer.color = Color.white; 
        isActive = true;
        animator.SetBool("activate", true);

        // Wait for delay, deactivate trap, reset all variables and animator
        yield return new WaitForSeconds(activationDelay);
        isActive = false;
        isTriggered = false;
        animator.SetBool("activate", false);
    }
}
