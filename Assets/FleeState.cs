using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeState : StateMachineBehaviour
{
    public float safeDistance;
    public float fleeDistance;

    NavMeshAgent navAgent;
    FarmerAI fAI;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }

        if (fAI == null)
        {
            fAI = animator.GetComponent<FarmerAI>();
        }
        
        FindFleeDestination(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!navAgent.hasPath)
        {
            //check if should stop fleeing
            if (ShouldStopFlee(animator))
            {
                animator.SetInteger("State", (int)Transition.IDLE);
            }
            else
            {
                FindFleeDestination(animator);
            }
        }
        
    }

    bool ShouldStopFlee(Animator anim)
    {
        HashSet<GameObject> fleePlayerDelete = new HashSet<GameObject>();
        //record each player that is outside safe distance
        foreach(GameObject player in fAI.FleeFromPlayers)
        {
            if (Vector3.Distance(player.transform.position, fAI.transform.position) >= safeDistance)
            {
                fleePlayerDelete.Add(player);
            }
        }

        //delete safe players form flee from players
        fAI.FleeFromPlayers.ExceptWith(fleePlayerDelete);

        //if no player is inside safe distance, stop fleeing, enter idle state
        if(fAI.FleeFromPlayers.Count == 0)
        {
            return true;
        }

        return false;
    }


    void FindFleeDestination(Animator anim)
    {
        Vector3 center = CenterOfMass();
        Vector3 intendedPosition = (anim.transform.position - center).normalized * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(intendedPosition, out hit, 20f, 1))
        {
            Vector3 target = hit.position;
            navAgent.SetDestination(hit.position);
        }
        else
        {

        }
    }

    Vector3 CenterOfMass()
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        int n = 0;

        foreach(GameObject player in fAI.FleeFromPlayers)
        {
            n += 1;
            x += player.transform.position.x;
            y += player.transform.position.y;
            z += player.transform.position.z;
        }

        return new Vector3(x/n, y/n, z/n);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //after fleeing done, reset target
        fAI.HasTarget = false;
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
