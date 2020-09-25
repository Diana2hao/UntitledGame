using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : StateMachineBehaviour
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
        //if (!birdAI.IsDeploying)
        //{
        //    float step = birdAI.speed * Time.deltaTime; // calculate distance to move
        //    birdAI.transform.position = Vector3.MoveTowards(birdAI.transform.position, birdAI.CurrTarget, step);
        //    Vector3 dir = birdAI.CurrTarget - birdAI.transform.position;
        //    birdAI.transform.rotation = Quaternion.Slerp(birdAI.transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), 0.3f);
        //}
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
