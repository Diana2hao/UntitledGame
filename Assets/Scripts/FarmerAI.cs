using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAI : MonoBehaviour
{
    public float fleeDistance;

    GameObject currTarget;
    bool hasTarget;
    
    Animator anim;
    float minDist;
    TreeListController tl;

    GameObject[] allPlayers;
    HashSet<GameObject> fleeFromPlayers;

    public GameObject CurrTarget { get => currTarget; set => currTarget = value; }
    public HashSet<GameObject> FleeFromPlayers { get => fleeFromPlayers; set => fleeFromPlayers = value; }
    public bool HasTarget { get => hasTarget; set => hasTarget = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();

        tl = GameObject.FindObjectOfType<TreeListController>();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        fleeFromPlayers = new HashSet<GameObject>();
        hasTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        //find a target (idle state will monitor current target and execute transition)
        if (!hasTarget && tl.treeList.Count!=0)
        {
            //find the nearest tree that is not targeted
            //TODO: check if tree is already being cut
            GameObject nearest = tl.treeList[0];
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

            currTarget = nearest;
            hasTarget = true;

            //set plant status to isTarget
            TreeControl tc = nearest.GetComponent<TreeControl>();
            tc.isTarget = true;
        }

        CheckIfShouldFlee();
        
    }

    private void CheckIfShouldFlee()
    {
        foreach(GameObject player in allPlayers)
        {
            if(Vector3.Distance(player.transform.position, this.transform.position) <= fleeDistance)
            {
                //remember to uncheck "can transition to self" in editor
                anim.SetInteger("State", (int)Transition.FLEE);
                
                fleeFromPlayers.Add(player);
            }
        }
    }

    public void DestroyCurrentTarget()
    {
        tl.treeList.Remove(currTarget);
        hasTarget = false;
        Destroy(currTarget.gameObject);
    }
    
}
