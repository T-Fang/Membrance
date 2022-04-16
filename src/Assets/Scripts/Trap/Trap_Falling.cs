using System.Collections;
using UnityEngine;

public class Trap_Falling : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] public float activationDelay;
    private SpriteRenderer spriteRenderer;
    private bool isTriggered;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isTriggered)
        {
            spriteRenderer.color = Color.magenta;
            float movementSpeed = speed * Time.deltaTime;
            transform.Translate(0, -movementSpeed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isTriggered = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<Player>().InIFrame)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            //Player.SingoTone.MakeInvincible();
            collision.gameObject.GetComponent<Player>().MakeInvincible();
        }
        // Deactivate when hits any object
        gameObject.SetActive(false);
    }
}
