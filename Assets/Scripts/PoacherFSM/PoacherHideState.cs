﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherHideState : StateMachineBehaviour
{
    PoacherAI PAI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PAI == null)
        {
            PAI = animator.GetComponent<PoacherAI>();
        }

        PAI.Camouflage();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if enought birds entered trap, start capturing
        if (PAI.CanCapture)
        {
            PAI.CurrTargetDest = PAI.RetrieveTrapPosition;
            PAI.DeCamouflage();
            animator.SetInteger("State", (int)PoacherTransition.CAPTURE);
            PAI.CanCapture = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PAI.TrapCon.TriggerTrap();
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
