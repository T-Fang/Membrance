using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    protected Vector2 knockbackAngle = Vector2.one;
    protected float magnitude = 3f;
    protected RangedEnemyHittable parent;

    public void Start()
    {
        parent = GetComponentInParent<RangedEnemyHittable>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Stop null checking everywhere
        if (!collision.CompareTag("Player") || collision.gameObject.GetComponent<Player>() == null) return;
        if (collision.gameObject.GetComponent<Player>().InIFrame) return;
        collision.GetComponent<Health>().TakeDamage(damage);
        collision.GetComponent<Health>().Knockback(knockbackAngle,magnitude,-1);
    }
}
