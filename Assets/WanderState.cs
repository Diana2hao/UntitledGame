using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : StateMachineBehaviour
{
    NavMeshAgent navAgent;
    public float minWanderDistance = 5;
    public float maxWanderDistance = 15;
    Vector3 target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }
        
        //set a random destination to wander to
        SetRandomDestination(navAgent);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if reached destination, go back to idle state
        if (!navAgent.hasPath)
        {
            animator.SetInteger("State", (int)Transition.IDLE);
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

    void SetRandomDestination(NavMeshAgent _agent)
    {
        float radius = Random.Range(minWanderDistance, maxWanderDistance);
        Vector3 randomPosition = Random.insideUnitSphere * radius;
        randomPosition += _agent.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            target = hit.position;
            _agent.SetDestination(hit.position);
        }
        else
        {
            Debug.Log("random dest failed");
        }
    }
}
