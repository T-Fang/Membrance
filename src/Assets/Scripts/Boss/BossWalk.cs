using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalk : StateMachineBehaviour
{
    public float speed;
    public float attackRange;
    private float randomNumber;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    Transform player;
    Rigidbody2D bossBody;
    BossConfiguration boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player location
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossBody = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BossConfiguration>();
        speed = boss.BossSpeed();
        attackRange = boss.BossAttackRange();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cooldownTimer += Time.unscaledDeltaTime;

        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, bossBody.position.y);
        // Set the target position and move the boss towards the player
        Vector2 newPosition = Vector2.MoveTowards(bossBody.position, target, speed * Time.fixedUnscaledDeltaTime);
        bossBody.MovePosition(newPosition);

        // Attack player when in range
        if (Mathf.Abs(player.position.x - bossBody.position.x) <= attackRange && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            // Generate random random number to determine which melee attack to use
            randomNumber = Random.Range(1, 12);

            // Use meleeAttack1 1/3 of the times, meleeAttack2 1/3 of the times, rangedAttack1 1/6 of the times, rangedAttack2 1/6 of the times
            if (randomNumber <= 4) animator.SetTrigger("meleeAttack1");
            else if (randomNumber <= 8) animator.SetTrigger("meleeAttack2");
            else if (randomNumber <= 10) animator.SetTrigger("rangedAttack1");
            else animator.SetTrigger("rangedAttack2");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (randomNumber <= 4) animator.ResetTrigger("meleeAttack1");
        else if (randomNumber <= 8) animator.ResetTrigger("meleeAttack2");
        else if (randomNumber <= 10) animator.ResetTrigger("rangedAttack1");
        else animator.ResetTrigger("rangedAttack2");
    }

}
