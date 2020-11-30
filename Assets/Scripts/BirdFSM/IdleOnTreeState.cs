using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleOnTreeState : StateMachineBehaviour
{
    NavMeshAgent navAgent;
    BirdAI birdAI;

    float waitTime;
    float timer = 0f;

    float poopTime;
    float poopTimer = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (birdAI == null)
        {
            birdAI = animator.GetComponent<BirdAI>();
        }
        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }

        waitTime = Random.Range(3f, 6f);
        poopTime = Random.Range(5f, 10f);

        birdAI.TargetTree.birdOnTree += 1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if bird got scared away
        if (birdAI.IsFlyingAway)
        {
            animator.SetInteger("State", (int)BirdTransition.FLY);
        }
        else if (birdAI.IsAttractedToTrap)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                timer = 0f;
                birdAI.CurrTarget = birdAI.TrapDest;
                animator.SetInteger("State", (int)BirdTransition.FLY);
            }
        }
        else if (birdAI.CanPoop)
        {
            poopTimer += Time.deltaTime;
            if (poopTimer > poopTime)
            {
                birdAI.Poop();
                poopTimer = 0f;
                poopTime = Random.Range(15f, 20f);
            }
        }
    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        birdAI.TargetTree.birdOnTree -= 1;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
