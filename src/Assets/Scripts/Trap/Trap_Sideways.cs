using UnityEngine;

public class Trap_Sideways : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        // Set movement distance in the left and right directions
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                // Move towards left if left edge is not reached
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                // Move towards right if right edge is not reached
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.GetComponent<Player>().InIFrame)
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            //Player.SingoTone.MakeInvincible();
            collision.GetComponent<Player>().MakeInvincible();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.GetComponent<Player>().InIFrame)
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            //Player.SingoTone.MakeInvincible();
            collision.GetComponent<Player>().MakeInvincible();
        }
    }
}
