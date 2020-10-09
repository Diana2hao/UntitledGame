using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;
using UnityEngine.AI;


//keep track of all fully grown trees, the birds on them, and all traps
public class BirdsControl : MonoBehaviour
{
    List<GrownTree> GrownTrees;
    List<GrownTree> OccupiedTrees;
    public GameObject birdPrefab;
    public NavMeshAgent birdNavAgent;
    GridController gridCon;
    List<TrapController> traps;

    float timer = 0f;
    public float newDeployTime;
    Random rnd = new Random();
    float trapAttrackRadius = 4.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        GrownTrees = new List<GrownTree>();
        OccupiedTrees = new List<GrownTree>();
        traps = new List<TrapController>();

        gridCon = GameObject.Find("Grid").GetComponent<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        //deploying birds
        if (GrownTrees.Count > 0)
        {
            timer += Time.deltaTime;
            if (timer > newDeployTime)
            {
                //deploy a new bird to a random grown tree
                int index = rnd.Next(GrownTrees.Count);
                GrownTree gt = GrownTrees[index];
                DeployBird(gt);

                timer -= newDeployTime;
            }
        }

        //send birds to traps

    }

    public void AddGrownTree(TreeControl tree)
    {
        //create and add to list
        GrownTree gt = new GrownTree(tree);
        GrownTrees.Add(gt);

        //deploy new bird and reset timer
        DeployBird(gt);
        timer = 0f;
    }

    public void RemoveGrownTree(TreeControl tc)
    {
        //find the tree and remove form either list
        GrownTree gt = GrownTrees.Concat(OccupiedTrees).ToList().Find(t => t.tree == tc.gameObject);

        //make each bird on that tree fly away
        GameObject[] tempBirds = new GameObject[gt.birds.Count];
        gt.birds.CopyTo(tempBirds);
        foreach (GameObject bird in tempBirds)
        {
            gt.RemoveBird(bird);
            bird.GetComponent<BirdAI>().FlyAway();
        }

        //remove the tree
        GrownTrees.Remove(gt);
        OccupiedTrees.Remove(gt);
    }

    public void RemoveABird(GameObject bird)
    {
        BirdAI ba = bird.GetComponent<BirdAI>();

        //find the tree this bird is on
        GrownTree gt = GrownTrees.Concat(OccupiedTrees).ToList().Find(t=>t.tree==ba.TargetTree.tree);

        gt.RemoveBird(bird);

        if (OccupiedTrees.Contains(gt))
        {
            OccupiedTrees.Remove(gt);
            GrownTrees.Add(gt);
        }
    }

    

    void DeployBird(GrownTree tree)
    {
        //find a spot that is not occupied
        int idx = -1;
        for(int i = 0; i < tree.occupied.Length; i++)
        {
            if (!tree.occupied[i])
            {
                idx = i;
            }
        }

        //if all occupied, return
        if(idx == -1)
        {
            return;
        }

        //instantiate a bird
        GameObject bird = Instantiate(birdPrefab);

        //set this bird's target position to the rest spot position, target tree to this tree
        BirdAI ba = bird.GetComponent<BirdAI>();
        ba.TargetRestPosition = tree.tree.transform.position + tree.restPositions[idx];
        ba.RestRotation = tree.restRotations[idx];
        ba.TargetTree = tree;
        ba.RestPosIdx = idx;

        //increase this tree's birdcount and add this bird to this tree
        tree.birdCount++;
        tree.birds.Add(bird);
        tree.occupied[idx] = true;

        //if this tree's birdcount reaches max, move this tree from grown list to occupied list
        if(tree.birdCount >= tree.maxBird)
        {
            GrownTrees.Remove(tree);
            OccupiedTrees.Add(tree);
        }
    }

    //Poacher Interactions--------------------------------------------------------------------------
    public bool FindTargetTree()
    {
        List<GrownTree> fullTrees = OccupiedTrees.ToList().FindAll(t => t.birdOnTree == t.maxBird);
        return fullTrees.Count > 0;
    }

    public bool FindTargetTreeForPoacher(out Vector3 trapPosition, out Vector3 poacherPosition, out Vector3 hidePosition)
    {
        List<GrownTree> fullTrees = OccupiedTrees.ToList().FindAll(t => t.birdOnTree == t.maxBird);

        //find a random tree that is not targeted by a poacher yet
        foreach (GrownTree gt in fullTrees.OrderBy(a => rnd.Next()).ToList())
        {
            //check if there is empty grids around the tree
            if (gridCon.FindTrapPosition(gt.tree, gt.scale, out trapPosition, out poacherPosition, out hidePosition))
            {
                return true;
            }
        }

        trapPosition = poacherPosition = hidePosition = Vector3.zero;
        return false;
    }

    public void AddTrap(TrapController trap)
    {
        traps.Add(trap);
        SendBirdsToTrap(trap);
    }

    public void RemoveTrap(TrapController trap)
    {
        traps.Remove(trap);
    }

    private void SendBirdsToTrap(TrapController trap)
    {
        HashSet<GameObject> trees = gridCon.FindTreeNextToTrap(trap);
        foreach(GameObject go in trees)
        {
            GrownTree gt = GrownTrees.Concat(OccupiedTrees).ToList().Find(t => t.tree == go);
            foreach(GameObject bird in gt.birds)
            {
                Vector3 randDest = FindRandomDestination(trap);
                bird.GetComponent<BirdAI>().AddTrap(trap, randDest);
            }
        }
    }

    private Vector3 FindRandomDestination(TrapController trap)
    {
        Vector3 target = Vector3.zero;
        
        NavMeshQueryFilter filterBird = new NavMeshQueryFilter();
        filterBird.areaMask = birdNavAgent.areaMask;
        filterBird.agentTypeID = birdNavAgent.agentTypeID;

        float radius = UnityEngine.Random.Range(0.8f, 1.5f);
        float angle = UnityEngine.Random.Range(0f, 360f);

        Vector3 randomPosition = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
        randomPosition += trap.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 0.2f, filterBird))
        {
            target = hit.position;
        }
        else
        {
            Debug.Log("random dest failed");
        }

        return target;
    }
}