using UnityEngine;

public class EnemyProjectile : EnemyDamage // Will damage the player every time
{

    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;

    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);    

    }
    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.unscaledDeltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Execute logic from parent script (enemy damage)
        base.OnTriggerEnter2D(collision);

        // Deactivate when hits any object
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
