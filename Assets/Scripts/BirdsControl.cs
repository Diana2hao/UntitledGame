using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;

public class BirdsControl : MonoBehaviour
{
    List<GrownTree> GrownTrees;
    List<GrownTree> OccupiedTrees;
    public GameObject birdPrefab;

    float timer = 0f;
    public float newDeployTime;
    Random rnd = new Random();

    struct GrownTree
    {
        public GameObject tree;
        public List<Vector3> restPositions;
        public List<GameObject> birds;
        public bool[] occupied;
        public int birdCount;
        public int maxBird;

        //struct RestSpot
        //{
        //    public bool isOccupied;
        //    public GameObject bird;
        //    public Vector3 position;
        //}

        public GrownTree(TreeControl tc)
        {
            tree = tc.gameObject;
            restPositions = tc.RestSpots;
            birds = new List<GameObject>();
            occupied = new bool[restPositions.Count];
            birdCount = 0;
            maxBird = restPositions.Count;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GrownTrees = new List<GrownTree>();
        OccupiedTrees = new List<GrownTree>();
    }

    // Update is called once per frame
    void Update()
    {
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
        //remove form either list
        FindAndRemove(tc.gameObject, GrownTrees);
        FindAndRemove(tc.gameObject, OccupiedTrees);
    }

    void FindAndRemove(GameObject tree, List<GrownTree> gtlist)
    {
        //find the tree
        int idx = -1;
        for(int i = 0; i < gtlist.Count; i++)
        {
            if(gtlist[i].tree == tree)
            {
                idx = i;
            }
        }

        //if found
        if(idx != -1)
        {
            GrownTree gt = gtlist[idx];

            //make each bird on that tree fly away
            GameObject[] tempBirds = new GameObject[gt.birds.Count];
            gt.birds.CopyTo(tempBirds);
            foreach (GameObject bird in tempBirds)
            {
                RemoveBirdFrom(bird, gt);
            }

            //remove the tree
            gtlist.RemoveAt(idx);
        }
        
    }

    public void RemoveABird(GameObject bird)
    {
        BirdAI ba = bird.GetComponent<BirdAI>();

        //find the tree this bird is on
        GrownTree gt = GrownTrees.Concat(OccupiedTrees).ToList().Find(t=>t.tree==ba.TargetTree);

        RemoveBirdFrom(bird, gt);
    }

    void RemoveBirdFrom(GameObject bird, GrownTree gt)
    {
        BirdAI ba = bird.GetComponent<BirdAI>();

        //find the spot this bird occupies and set it to unoccupied
        //int idx = gt.restPositions.IndexOf(ba.TargetRestPosition-gt.tree.transform.position);
        //Debug.Log(idx);
        gt.occupied[ba.RestPosIdx] = false;

        //remove this bird from this tree's bird list, and decrease the count
        gt.birds.Remove(bird);
        gt.birdCount -= 1;

        //TODO: make bird fly away (animation and movement)
        ba.FlyAway();

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
        ba.TargetTree = tree.tree;
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
}
