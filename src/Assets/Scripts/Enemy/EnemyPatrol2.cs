using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol2 : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Patrol")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Moving")]
    [SerializeField] private float speed;
    private Vector3 initDirection;
    private bool movingLeft;

    [Header("Idle")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    public Transform child;
    public int FacingDir { get; private set; }

    private void Start()
    {
        // Get the initial direction the enemy is facing
        initDirection = enemy.localScale;
        FacingDir = (int)initDirection.x;
    }
    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }
    private void Update()
    {
        child = this.gameObject.transform.GetChild(0);
        if (child.GetComponent<RangedEnemyHittable>().canMove)
        {
            if (movingLeft)
            {
                if (enemy.position.x >= leftEdge.position.x)
                {
                    // Keep moving in the left direction
                    MoveInDirection(-1);
                    FacingDir = -1;
                }
                else
                {
                    // Change direction
                    ChangeDirection();
                    FacingDir *= -1;
                }
            }
            else
            {
                if (enemy.position.x <= rightEdge.position.x)
                {
                    // Keep moving in the right direction
                    MoveInDirection(1);
                    FacingDir = 1;
                }
                else
                {
                    // Change direction
                    ChangeDirection();
                    FacingDir *= -1;
                }
            }
        }
    }

    private void ChangeDirection()
    {
        anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;

        // Let the enemy idle for a while before changing direction
        if (idleTimer > idleDuration)
        {
            // Indicate that the enemy should change direction
            movingLeft = !movingLeft;
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        anim.SetBool("moving", true);

        // Face in the correct direction
        enemy.localScale = new Vector3(Mathf.Abs(initDirection.x) * _direction, initDirection.y, initDirection.z);

        // Move in the correct direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
