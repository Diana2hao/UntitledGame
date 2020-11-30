using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAI : MonoBehaviour
{
    public float fleeDistance;
    public Vector2 entryPointRange;
    public float entryPointZ;

    bool enteredLevel = false;
    float entryPoint;

    GameObject currTarget;
    bool hasTarget;
    
    Animator anim;
    float minDist;
    TreeListController tl;
    GridController gridCon;

    GameObject[] allPlayers;
    HashSet<GameObject> fleeFromPlayers;

    GameObject LC;

    public GameObject CurrTarget { get => currTarget; set => currTarget = value; }
    public HashSet<GameObject> FleeFromPlayers { get => fleeFromPlayers; set => fleeFromPlayers = value; }
    public bool HasTarget { get => hasTarget; set => hasTarget = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();

        tl = GameObject.FindObjectOfType<TreeListController>();
        gridCon = GameObject.Find("Grid").GetComponent<GridController>();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        fleeFromPlayers = new HashSet<GameObject>();
        hasTarget = false;

        LC = GameObject.FindGameObjectWithTag("LevelControl");

        entryPoint = Random.Range(entryPointRange.x, entryPointRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!enteredLevel)
        {
            if (transform.position.x > entryPoint)
            {
                this.transform.parent = null;
                this.GetComponent<Animator>().SetBool("EnterLevel", true);
                this.transform.position = new Vector3(transform.position.x, transform.position.y, entryPointZ);
                this.transform.rotation = Quaternion.identity;
                enteredLevel = true;
            }
        }

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
                fleeFromPlayers.Add(player);

                //remember to uncheck "can transition to self" in editor
                anim.SetInteger("State", (int)FarmerTransition.FLEE);
            }
        }
    }

    public void DestroyCurrentTarget()
    {
        tl.treeList.Remove(currTarget);
        gridCon.RemoveGameObjectOfScale(currTarget, currTarget.GetComponent<TreeControl>().FinalSize);
        hasTarget = false;
        Destroy(currTarget.gameObject);
    }
    
}
