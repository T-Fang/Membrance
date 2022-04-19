using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossConfiguration : MonoBehaviour, HittableObject
{
    [Header("Stats")]
    [SerializeField] public float health;
    [SerializeField] public float healthThreshold;
    [SerializeField] public float speed;
    [SerializeField] public float attackRange;
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private float attackCooldown;

    [Header("Attack")]
    [SerializeField] public int meleeAttack1;
    [SerializeField] public int meleeAttack2;
    [SerializeField] public int rangedAttack1;

    [Header("Firepoint")]
    [SerializeField] private Transform firepoint1;
    [SerializeField] private GameObject[] attackObject1;
    [SerializeField] private Transform firepoint2;
    [SerializeField] private GameObject[] attackObject2;
    [SerializeField] private Transform firepoint3;
    [SerializeField] private Transform firepoint4;

    [Header("Collider")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    private float initialHealth;
    public Transform player;
    private Health playerHealth;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Transform playerBody;
    private GameObject effect;
    private Renderer effectRenderer;
    private int damageToPlayer;
    public bool isTurn = false;
    private bool isInvulnerable = false;
    private bool isPlayerInRange = false;
    private bool isBoosted = false;
    private bool isStageTransited = false;
    private int rangedAttackType = 0;
    private float cooldownTimer = Mathf.Infinity;
    
    
    [SerializeField] private GameObject hitVFX;

    public void Awake()
    {
        playerBody = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialHealth = health;
        effect = this.gameObject.transform.GetChild(0).gameObject;
        effectRenderer = effect.GetComponent<Renderer>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        // Boss die when it has no more HP
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void TriggerEnding() => SceneManager.LoadScene("Ending");

    public void LookAtPlayer()
    {
        Vector3 changeDirection = transform.localScale;
        changeDirection.z *= -1.0f;

        // Turn the enemy to always face the direction of the player
        if (transform.position.x > player.position.x && isTurn)
        {
            transform.localScale = changeDirection;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            isTurn = false;
        } 
        else if (transform.position.x < player.position.x && !isTurn)
        {
            transform.localScale = changeDirection;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            isTurn = true;
        }  
    }

    public void Damage(float damage)
    {
        Instantiate(hitVFX, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        BossInjured(damage);
    }

    public void BossInjured(float takeDamage)
    {
        // Boss will not take damage when it is invulnerable
        if (isInvulnerable) return;

        animator.SetTrigger("injured");
        health -= takeDamage;

        // Boss enters stage 2 when HP below threshold
        if (health < healthThreshold)
        {
            if (!isStageTransited)
            {
                animator.Play("Boss_Stage_Transition");
                animator.SetBool("stage2", true);
                isStageTransited = true;
                isInvulnerable = true; // TODO: Never got set back to false, find the logic
            }

            // Boost the boss's stats when it enters stage 2
            if (!isBoosted)
            {
                spriteRenderer.material.color = Color.red;
                health = initialHealth / 2;
                speed *= 2;
                transform.localScale += new Vector3(10.0f, 10.0f, 0.0f);
                transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                effectRenderer.enabled = true;
                isBoosted = true;
            }

            if (health < 0.001f)
            {
                animator.SetTrigger("defeated");
            }
        }


    }

    public void BossDie()
    {  
       TriggerEnding(); 
        Destroy(gameObject);
    }

    public float BossSpeed()
    {
        return speed;
    }
    public float BossAttackRange()
    {
        return attackRange;
    }

    public void MeleeAttack1()
    {
        isInvulnerable = true;
        damageToPlayer = meleeAttack1;
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        
        // Deal damage when the player is still within attack range
        if (isPlayerInRange)
        {
            playerHealth.TakeDamage(damageToPlayer);
        }
        isInvulnerable = false;
    }

    public void MeleeAttack2()
    {
        isInvulnerable = true;
        damageToPlayer = meleeAttack2;
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();

        // Deal damage when the player is still within attack range
        if (isPlayerInRange)
        {
            playerHealth.TakeDamage(damageToPlayer);
        }
        isInvulnerable = false;
    }

    public void RangedAttack1()
    {
        if (cooldownTimer >= attackCooldown)
        {
            isInvulnerable = true;
            cooldownTimer = 0;
            // Attack
            attackObject1[FindAttackObject1()].transform.position = firepoint1.position;
            attackObject1[FindAttackObject1()].GetComponent<BossAttackProjectile>().ActivateProjectile();
            attackObject2[FindAttackObject2()].transform.position = firepoint2.position;
            attackObject2[FindAttackObject2()].GetComponent<BossAttackProjectile>().ActivateProjectile();
            isInvulnerable = false;
        }
    }

    public void RangedAttack2()
    {
        if (cooldownTimer >= attackCooldown)
        {
            isInvulnerable = true ;
            cooldownTimer = 0;
            // Attack
            this.gameObject.transform.GetChild(2).GetChild(0).transform.position = firepoint3.position;
            this.gameObject.transform.GetChild(2).GetChild(0).GetComponent<BossAttackTrigger>().ActivateTrigger();
            this.gameObject.transform.GetChild(2).GetChild(1).transform.position = firepoint4.position;
            this.gameObject.transform.GetChild(2).GetChild(1).GetComponent<BossAttackTrigger2>().ActivateTrigger();
            isInvulnerable = false;
        }
    }

    private int FindAttackObject1()
    {

        for (int i = 0; i < attackObject1.Length; i++)
        {
            if (!attackObject1[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private int FindAttackObject2()
    {

        for (int i = 0; i < attackObject2.Length; i++)
        {
            if (!attackObject2[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangedAttackRange * transform.localScale.x * colliderDistance,
                            new Vector3(boxCollider.bounds.size.x * rangedAttackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void TriggerStage2AnimFinish() => isInvulnerable = false;
}
