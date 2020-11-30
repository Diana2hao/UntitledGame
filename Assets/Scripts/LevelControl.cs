using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelControl : MonoBehaviour
{
    public GameObject poacherPrefab;
    public GameObject farmerPrefab;
    public RaftController raft;
    public TextMeshProUGUI birdScore;
    public TextMeshProUGUI treeScore;

    int birdIncrement = 0;
    int treeIncrement = 0;
    int totalBird = 0;
    int totalTree = 0;

    int birdsPerPoacher;
    int treesPerFarmer;
    bool poacherFull;

    public int TotalBird { get => totalBird; }
    public int TotalTree { get => totalTree; }

    // Start is called before the first frame update
    void Start()
    {
        birdsPerPoacher = 4;
        treesPerFarmer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(birdIncrement >= birdsPerPoacher && !poacherFull)
        {
            //deploy a poacher to the next raft
            raft.AddPoacher();
            birdIncrement = 0;
            poacherFull = true;
        }

        if(treeIncrement >= treesPerFarmer)
        {
            //deploy a farmer to the next raft
            raft.AddFarmer();
            treeIncrement = 0;
            treesPerFarmer = Random.Range(6, 8);
        }
    }

    public void AddBird()
    {
        birdIncrement += 1;
        totalBird++;
        birdScore.text = totalBird.ToString();
    }

    public void AddTree()
    {
        treeIncrement += 1;
        totalTree++;
        treeScore.text = totalTree.ToString();
    }

    public void MinusBird()
    {
        totalBird--;
        birdScore.text = totalBird.ToString();
    }

    public void MinusTree()
    {
        totalTree--;
        treeScore.text = totalTree.ToString();
    }
}
