using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CutTreeState : StateMachineBehaviour
{
    FarmerAI fAI;
    TreeControl currTarget;

    float timer;
    float cutTime = 1f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fAI == null)
        {
            fAI = animator.GetComponent<FarmerAI>();
        }

        currTarget = fAI.CurrTarget.GetComponent<TreeControl>();
        timer = 0f;

        currTarget.StartCutting();
        currTarget.CurrentHealth -= 1;  //subtract 1 health immediately after cutting started to avoid delay related situations
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        //every cuttime(2 sec), tree loses some health
        if (timer > cutTime)
        {
            timer -= cutTime;
            currTarget.CurrentHealth -= currTarget.maxHealth/10;
            currTarget.healthBar.SetFillAmount(((float)currTarget.CurrentHealth) / currTarget.maxHealth);

            //if health goes below zero and tree already at smallest model, destroy tree
            if (currTarget.CurrentHealth <= 0 && currTarget.CurTree ==0)
            {
                //destroy tree
                fAI.DestroyCurrentTarget();

                //after tree destroyed, goes back to idle state
                animator.SetInteger("State", (int)FarmerTransition.IDLE);

            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currTarget.StopCutting();
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
