using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BirdTransition
{
    FLY,
    IDLEONTREE,
    WALK,
    EAT,
    CAPTURED
}

public class FlyState : StateMachineBehaviour
{
    BirdAI birdAI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (birdAI == null)
        {
            birdAI = animator.GetComponent<BirdAI>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!birdAI.IsDeploying)
        {
            if (Vector3.Distance(birdAI.transform.position, birdAI.CurrTarget) > 0.001f)
            {
                float step = birdAI.speed * 0.7f * Time.deltaTime; // calculate distance to move
                birdAI.transform.position = Vector3.MoveTowards(birdAI.transform.position, birdAI.CurrTarget, step);
                Vector3 dir = birdAI.CurrTarget - birdAI.transform.position;
                birdAI.transform.rotation = Quaternion.Slerp(birdAI.transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), 0.1f);
            }
            else
            {
                if (birdAI.IsAttractedToTrap)
                {
                    animator.SetInteger("State", (int)BirdTransition.WALK);
                }

                if (birdAI.IsFlyingAway)
                {
                    //destroy
                    Destroy(birdAI.gameObject);
                    //TODO: reduce player points

                }
                
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
