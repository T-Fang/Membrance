/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttack : StateMachineBehaviour
{
    private int damage;
    private Health playerHealth;
    private Animator anim;
    BossConfiguration boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossConfiguration>();
        damage = boss.MeleeAttack1();
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerHealth.TakeDamage(damage);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
*/