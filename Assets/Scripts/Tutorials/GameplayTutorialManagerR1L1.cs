using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayTutorialManagerR1L1 : MonoBehaviour
{
    public GameObject raft;
    public GameObject arrowPrefab;
    public GameObject fertBagPrefab;
    public GameObject waterIconPrefab;

    bool healFinished;
    bool damagedTreeFound;
    TreeControl tree;
    GameObject treeArrow;
    GameObject fertBag;
    GameObject fertBagArrow;

    bool revealFinished;
    bool revealStarted;
    bool poacherFound;
    PoacherAI firstPoacher;
    GameObject waterIcon;

    PlayerGameplayTutR1L1[] allPlayers;
    TreeControl[] allTrees;

    // Start is called before the first frame update
    void Start()
    {
        allPlayers = FindObjectsOfType<PlayerGameplayTutR1L1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!damagedTreeFound)
        {
            allTrees = FindObjectsOfType<TreeControl>();
            foreach(TreeControl t in allTrees)
            {
                if (t.IsPlanted && t.CurrentHealth < t.maxHealth)
                {
                    tree = t;
                    damagedTreeFound = true;
                    StartHealTut();
                    break;
                }
            }
        }

        if (!healFinished && damagedTreeFound)
        {
            if(tree.CurrentHealth == tree.maxHealth)
            {
                healFinished = true;
                FinishHealTut();
            }
        }

        if (!poacherFound)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Poacher");
            if (p != null)
            {
                firstPoacher = p.GetComponent<PoacherAI>();
                poacherFound = true;
            }
        }

        if (!revealStarted && poacherFound)
        {
            if (firstPoacher.IsHiding)
            {
                StartRevealTut();
                revealStarted = true;
            }
        }

        if (!revealFinished && revealStarted)
        {
            if (!firstPoacher.IsHiding)
            {
                FinishRevealTut();
                revealFinished = true;
            }
        }

        if (revealFinished && healFinished)
        {
            DisableAllPlayerTut();
            this.enabled = false;
        }
    }

    void StartHealTut()
    {
        treeArrow = Instantiate(arrowPrefab, tree.transform.position + new Vector3(0, 2.8f, 0), Quaternion.identity);
        fertBag = Instantiate(fertBagPrefab, tree.transform.position + new Vector3(-0.5f, 0, -0.5f), Quaternion.Euler(0, 90, 0));
        fertBagArrow = Instantiate(arrowPrefab, fertBag.transform.position + new Vector3(0, 0.46f, 0), Quaternion.identity);

        foreach(PlayerGameplayTutR1L1 p in allPlayers)
        {
            p.StartHeal();
        }
    }

    void FinishHealTut()
    {
        Destroy(treeArrow);

        foreach (PlayerGameplayTutR1L1 p in allPlayers)
        {
            p.FinishHeal();
        }
    }

    void StartRevealTut()
    {
        waterIcon = Instantiate(waterIconPrefab, firstPoacher.transform.position + new Vector3(0, 1.64f, 0), Quaternion.identity);

        foreach (PlayerGameplayTutR1L1 p in allPlayers)
        {
            p.StartReveal();
        }
    }

    void FinishRevealTut()
    {
        Destroy(waterIcon);

        foreach (PlayerGameplayTutR1L1 p in allPlayers)
        {
            p.FinishReveal();
        }
    }

    void DisableAllPlayerTut()
    {
        foreach (PlayerGameplayTutR1L1 p in allPlayers)
        {
            p.isEnabled = false;
        }
    }

    public void PickupFertBag()
    {
        if (fertBagArrow != null)
        {
            Destroy(fertBagArrow);
            fertBagArrow = null;
        }
    }
}
