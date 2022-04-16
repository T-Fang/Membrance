using UnityEngine;

public class RangedEnemyHittable : MonoBehaviour, HittableObject
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] attackObject;

    [Header("Collider")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    KnockbackableObject knockbackableObject;

    private Animator anim;
    private EnemyPatrol enemyPatrol;
    private Health PlayerHealth;

    [Header("Enemy")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    public EnemyHealthbar Healthbar;
    public SimpleFlash Flickerer;
    public bool canMove;

    [SerializeField] private GameObject hitVFX;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    private void Start()
    {
        health = maxHealth;
        Healthbar.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        canMove = true;
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
        // Only patrol when player is not in sight
        if (enemyPatrol != null) enemyPatrol.enabled = !PlayerInSight();
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        attackObject[FindAttackObject()].transform.position = firepoint.position;
        attackObject[FindAttackObject()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindAttackObject()
    {
        for (int i = 0; i < attackObject.Length; i++)
        {
            if (!attackObject[i].activeInHierarchy)
                return i;
        }

        return 0;
    }

    private bool PlayerInSight()
    {
        // Set origin, size, angle and direction, distance and layer mask of the box collider which detects player
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                                            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                                            0, Vector2.left, 0, playerLayer);

        // Return true when player hit collider
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void Damage(float damage1)
    {
        FindObjectOfType<AudioManager>().Play("Smash");
        //Debug.Log("-" + damage + " hp");
        health -= damage1;
        Healthbar.SetHealth(health,maxHealth);
        Instantiate(hitVFX, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        //anim.SetTrigger("hurt");
        if (health <= 0.0001f)
        {
            canMove = false;
            anim.SetBool("moving", false);
            anim.SetTrigger("die");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        Flickerer.Flash();
    }

    private void MakeDead()
    {
        //Destroy(gameObject); //TODO: Find out how to destroy properly
        gameObject.SetActive(false);
    }
}
