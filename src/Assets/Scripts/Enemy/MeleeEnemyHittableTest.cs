using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: Make enemies knocbackable
// TODO: Delegate stats to scriptable Objects
public class MeleeEnemyHittableTest : MonoBehaviour, HittableObject
{
    [Header("Attack")] [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] public Vector2 knockbackAngle;
    [SerializeField] public float knockbackMagnitude;

    [Header("Collider")] [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player")] [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    KnockbackableObject knockbackableObject;

    [SerializeField] private GameObject hitVFX;

    private Animator anim; //ok
    private Health playerHealth;
    [SerializeField] private Player _targetPlayer;
    private EnemyPatrol enemyPatrol;
    private bool PlayerInIFrame;

    [Header("Enemy")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    public EnemyHealthbar Healthbar;
    public SimpleFlash Flickerer;
    public bool canMove;

    private void Awake() 
    {
        canMove = true;
        anim = GetComponent<Animator>(); //ok
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Start()
    {
        health = maxHealth;
        Healthbar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        PlayerInIFrame = _targetPlayer.InIFrame;
        cooldownTimer += Time.deltaTime;

        // Only attack when player is in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                // Attack
                canMove = false;
                anim.SetBool("moving", false);
                anim.SetTrigger("attack");
            }
        }


        // Only patrol when player is not in sight and enemy is not doing other actions
        if (enemyPatrol != null) enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        // Set origin, size, angle and direction, distance and layer mask of the box collider which detects player
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        
        if (hit.collider != null && !PlayerInIFrame)
        {
            Debug.Log("Slash!");
            playerHealth = hit.transform.GetComponent<Health>();
            knockbackableObject = hit.collider.GetComponent<KnockbackableObject>();
        }


        // Return true when player hit collider
        return hit.collider != null;

    }

    private void DamagePlayer()
    {
        // Damage player if he is in the attack range
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
            knockbackableObject?.Knockback(knockbackAngle, knockbackMagnitude, gameObject.GetComponentInParent<EnemyPatrol>().FacingDir);
            anim.SetBool("moving", true);
        }
        canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void Damage(float damage1)
    {
        FindObjectOfType<AudioManager>().Play("Smash");
        //Debug.Log("-" + damage + " hp");
        health -= damage1;
        Healthbar.SetHealth(health,maxHealth);
        Instantiate(hitVFX, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        if (health <= 0.001f)
        {
            canMove = false;
            anim.SetBool("moving", false);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            anim.SetTrigger("die");
        }
        Flickerer.Flash();
    }

    private void MakeDead()
    {
        //Destroy(gameObject); //TODO: Find out how to destroy properly
        gameObject.SetActive(false);
    }

}