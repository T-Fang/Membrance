using UnityEngine;

public class BossAttackProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Health playerHealth;

    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);

    }
    private void Update()
    {
        float movementSpeed = speed * Time.unscaledDeltaTime;
        transform.Translate(-movementSpeed, 0, 0);

        lifetime += Time.unscaledDeltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            playerHealth.TakeDamage(1);
            gameObject.SetActive(false);
        }
    }



}

