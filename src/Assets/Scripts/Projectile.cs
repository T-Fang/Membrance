using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private AudioSource enemyHitAudio;
    [SerializeField] private AudioSource fireballImpactAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (hit) return;
        float movementspeed = speed * Time.unscaledDeltaTime * direction;
        transform.Translate(movementspeed, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Checkpoint"))
        {
            return;
        }
        if (!collision.gameObject.name.Equals("Player"))
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("Explode");
        }
        if (collision.gameObject.name.Contains("Enemy"))
        {
            fireballImpactAudio.Play();
            enemyHitAudio.Play();
            hit = true;
            collision.gameObject.GetComponent<HittableObject>().Damage(69);
            boxCollider.enabled = false;
            anim.SetTrigger("Explode");
            //Destroy(collision.gameObject);
        }
    }
    public void SetDirection(float _direction)  //to deal with the direction
    {
        Debug.Log(_direction);
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        Debug.Log("hi");
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        direction = _direction;
        Debug.Log(_direction);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/
}
