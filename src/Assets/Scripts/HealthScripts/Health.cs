using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, KnockbackableObject
{
    [SerializeField] public float startingHealth;
    public float currentHealth { get; private set; } // Value can get by other scripts but can only be set by this script 
    public bool isDead;

    private bool _isBeingKnocked;
    private float _knockbackStart;
    private Player _playerRef;
    private float damageToTake;
    private bool PlayerInIFrame;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Start()
    {
        _playerRef = GetComponentInParent<Player>();
    }

    public void Update()
    {
        PlayerInIFrame = _playerRef.InIFrame;
    }

    public void TakeDamage(float _damage)
    {
        if (PlayerInIFrame)
        {
            Debug.Log("Leh leh leh iframe");
            return;
        }

        FindObjectOfType<AudioManager>().Play("PlayerHurt");
        // Clamps the given value between min and max values
        //currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            // Player hurts
        }
        else
        {
            if (!isDead)
            {
                // Player dies
                isDead = true;
            }

        }
        //Debug.Log("Player -1 Health");
    }

    public void AddHealth(float _value)
    {
        // Clamps the given value between min and max values
        FindObjectOfType<AudioManager>().Play("Heal");
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Knockback(Vector2 angle, float magnitude, int direciton)
    {
        _playerRef.SetVelAtAngle(magnitude,angle,direciton);
        _playerRef.InIFrame = true;
        StartCoroutine(_playerRef.InFrame());

    }

    private IEnumerator InFrame()
    {
        Physics2D.IgnoreLayerCollision(3,7,true);

        _playerRef.Flickerer.Flash();
        _playerRef.SpriteRenderer.color = new Color(1.0f,1.0f,1.0f,0.5f);
        yield return new WaitForSeconds(_playerRef.data.iFrame);
        
        Physics2D.IgnoreLayerCollision(3,7,false);
        _playerRef.InIFrame = false;
        _playerRef.SpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0);
    }


}
