using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayTutorialManagerR1L0 : MonoBehaviour
{
    public GameObject plantBoxArrow;
    public GameObject bucketArrow;
    public GameObject pondArrow;
    public GameObject treeArrow;
    public GameObject vacArrow;
    public GameObject poopArrow;
    public GameObject fertStationArrow;
    public GameObject FertAddWater;
    public GameObject FertAddPoop;
    public GameObject fertBagArrow;

    public GameObject arrowPrefab;

    List<PlayerGameplayTutR1L0> allPlayerTuts = new List<PlayerGameplayTutR1L0>();
    List<PlayerGameplayTutR1L0> allPlayerWaitForPoop = new List<PlayerGameplayTutR1L0>();
    GameObject[] allTrees;
    GameObject[] allPoops;
    TreeControl tutTree;
    bool fertTutSkipped;

    // Start is called before the first frame update
    void Start()
    {
        plantBoxArrow.SetActive(true);
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            allPlayerTuts.Add(player.GetComponent<PlayerGameplayTutR1L0>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //allTrees = GameObject.FindGameObjectsWithTag("Plant");
        //foreach(GameObject tree in allTrees)
        //{
        //    if (tree.GetComponent<TreeControl>().IsGrowing)
        //    {
        //        SetupTreeArrow(tree.transform);

        //    }
        //}
        if (allPlayerWaitForPoop.Count > 0)
        {
            allPoops = GameObject.FindGameObjectsWithTag("Poop");
            if (allPoops.Length > 0)
            {
                SetupPoopArrow(allPoops[0].transform);
                PlayerGameplayTutR1L0 p = allPlayerWaitForPoop[0];
                p.StartCollectPoop();
                allPlayerWaitForPoop.Remove(p);
            }
        }

        if (!fertTutSkipped && tutTree != null && tutTree.CurTree >= 1)
        {
            foreach(PlayerGameplayTutR1L0 ptut in allPlayerTuts)
            {
                ptut.SkipFertTut();
            }

            fertTutSkipped = true;
        }
        
    }

    public void SetupTreeArrow(Transform tree)
    {
        treeArrow = Instantiate(arrowPrefab, tree.position + new Vector3(0,2.7f,0), Quaternion.identity);
        Destroy(treeArrow, 15f);
        if (tutTree == null)
        {
            tutTree = tree.GetComponent<TreeControl>();
        }
    }

    public void DeactivateTreeArrow()
    {
        try
        {
            treeArrow.SetActive(false);
        }
        catch (MissingReferenceException)
        {

        }
    }

    public void LookForPoop(PlayerGameplayTutR1L0 player)
    {
        allPlayerWaitForPoop.Add(player);

    }

    void SetupPoopArrow(Transform poop)
    {
        if (poopArrow == null)
        {
            poopArrow = Instantiate(arrowPrefab, poop.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
        else
        {
            poopArrow.SetActive(true);
        }
    }
}
