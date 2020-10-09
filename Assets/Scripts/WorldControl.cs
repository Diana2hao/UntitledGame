using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControl : MonoBehaviour
{
    public GameObject poacherPrefab;
    public GameObject farmerPrefab;
    public RaftController raft;

    int birdIncrement = 0;
    int treeIncrement = 0;

    int birdsPerPoacher;
    int treesPerFarmer;

    // Start is called before the first frame update
    void Start()
    {
        birdsPerPoacher = 4;
        treesPerFarmer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(birdIncrement >= birdsPerPoacher)
        {
            //deploy a poacher to the next raft
            raft.AddPoacher();
            birdIncrement = 0;
            birdsPerPoacher = Random.Range(4, 9);
        }

        if(treeIncrement >= treesPerFarmer)
        {
            //deploy a farmer to the next raft
            raft.AddFarmer();
            treeIncrement = 0;
            treesPerFarmer = Random.Range(2, 5);
        }
    }

    public void AddBird()
    {
        birdIncrement += 1;
    }

    public void AddTree()
    {
        treeIncrement += 1;
    }
}
