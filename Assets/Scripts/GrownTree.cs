using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles birds, poacher interactions
public class GrownTree
{
    public GameObject tree;
    public int scale;
    public List<Vector3> restPositions;
    public List<Quaternion> restRotations;
    public List<GameObject> birds;
    public bool[] occupied;
    public int birdCount;
    public int birdOnTree;
    public int maxBird;
    public bool isPoacherTarget;

    //struct RestSpot
    //{
    //    public bool isOccupied;
    //    public GameObject bird;
    //    public Vector3 position;
    //}

    public GrownTree(TreeControl tc)
    {
        tree = tc.gameObject;
        scale = tc.FinalSize;
        restPositions = tc.RestSpots;
        restRotations = tc.RestRotations;
        birds = new List<GameObject>();
        occupied = new bool[restPositions.Count];
        birdCount = 0;
        birdOnTree = 0;
        maxBird = restPositions.Count;
        isPoacherTarget = false;
    }

    public void RemoveBird(GameObject bird)
    {
        BirdAI ba = bird.GetComponent<BirdAI>();

        //find the spot this bird occupies and set it to unoccupied
        occupied[ba.RestPosIdx] = false;

        //remove this bird from this tree's bird list, and decrease the count
        birds.Remove(bird);
        birdCount -= 1;

        //TODO: make bird fly away (animation and movement)
        //ba.FlyAway();
    }
}
