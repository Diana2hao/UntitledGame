using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAI : MonoBehaviour
{
    public float fleeDistance;

    GameObject currTarget;
    
    Animator anim;
    float minDist;
    TreeListController tl;

    GameObject[] allPlayers;
    List<GameObject> fleeFromPlayers;

    public GameObject CurrTarget { get => currTarget; set => currTarget = value; }
    public List<GameObject> FleeFromPlayers { get => fleeFromPlayers; set => fleeFromPlayers = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();

        tl = GameObject.FindObjectOfType<TreeListController>();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        fleeFromPlayers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetInteger("State") == (int)Transition.IDLE && tl.treeList.Count!=0)
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

            //set state to walktotarget
            anim.SetInteger("State", (int)Transition.WALKTOTARGET);

            

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
                //set state to flee
                if(anim.GetInteger("State") != (int)Transition.FLEE) { anim.SetInteger("State", (int)Transition.FLEE); }  //does setting state again calls onenter repeatedly?
                
                fleeFromPlayers.Add(player);
            }
        }
    }
    
}
