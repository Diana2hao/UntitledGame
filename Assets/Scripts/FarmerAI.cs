using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float wanderRadius;
    public float wanderTimer;

    float timer;
    Animator anim;
    //GameObject[] plants;
    GameObject nearest;
    float minDist;
    TreeListController tl;


    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();

        //plants = GameObject.FindGameObjectsWithTag("Plant");

        tl = GameObject.FindObjectOfType<TreeListController>();

        timer = wanderTimer;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(tl.treeList.Count == 0)
        {
            //walk and idle randomly
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }

        }*/

        if(!anim.GetBool("isCutting") && tl.treeList.Count!=0)
        {
            //find the nearest tree that is not targeted
            nearest = tl.treeList[0];
            minDist = -1.0F;
            foreach(GameObject plant in tl.treeList)
            {
                float dist = Vector3.Distance(plant.transform.position, transform.position);
                if(minDist < 0 || dist < minDist)
                {
                    minDist = dist;
                    nearest = plant;
                }
            }

            //set walking to true
            anim.SetBool("isWalking", true);
            agent.SetDestination(nearest.transform.position);
            

            //direction = (transform.position - nearest.transform.position) / minDist;
            //direction = new Vector3(direction.x, 0, direction.z);

            //set plant status to isTarget
            TreeControl tc = nearest.GetComponent<TreeControl>();
            tc.isTarget = true;

        }
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collide");
        if (collision.collider.CompareTag("Plant"))
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            //start cutting
            anim.SetBool("isWalking", false);
            anim.SetBool("isCutting", true);

            //turn off nav, turn on rigidbody
            this.GetComponent<NavMeshAgent>().enabled = false;
            this.GetComponent<Rigidbody>().isKinematic = false;

            //set plant status to cutting

        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

}
