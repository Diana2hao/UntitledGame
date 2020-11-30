using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayTutR1L1 : MonoBehaviour
{
    public GameObject GameplayCanvas;
    public bool isEnabled = false;

    PlayerController pCon;
    GameplayTutorialManagerR1L1 tutManager;
    GameObject tutCanvas;

    int curTut = -1;
    
    bool healFinished;
    bool revealFinished;

    bool healWaiting;
    bool revealWaiting;

    // Start is called before the first frame update
    void Start()
    {
        pCon = this.GetComponent<PlayerController>();
        tutManager = GameObject.Find("TutorialManager").GetComponent<GameplayTutorialManagerR1L1>();

        tutCanvas = Instantiate(GameplayCanvas);
        tutCanvas.GetComponent<FollowPlayer>().playerToFollow = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInteract()
    {
        if (!isEnabled)
        {
            return;
        }

        if (curTut == 0 && pCon.CurrentHandheldObject.CompareTag("FertilizerBag"))
        {
            tutManager.PickupFertBag();
        }
    }

    public void StartHeal()
    {
        if (curTut == -1)
        {
            curTut = 0;
            tutCanvas.transform.GetChild(curTut).gameObject.SetActive(true);
        }

        else
        {
            healWaiting = true;
        }
    }

    public void FinishHeal()
    {
        if (curTut == 0)
        {
            tutCanvas.transform.GetChild(curTut).gameObject.SetActive(false);
            curTut = -1;
        }

        healFinished = true;
        healWaiting = false;

        if (revealWaiting)
        {
            curTut = 1;
            tutCanvas.transform.GetChild(curTut).gameObject.SetActive(true);
            revealWaiting = false;
        }
    }

    public void StartReveal()
    {
        if (curTut == -1)
        {
            curTut = 1;
            tutCanvas.transform.GetChild(curTut).gameObject.SetActive(true);
        }

        else
        {
            revealWaiting = true;
        }
    }

    public void FinishReveal()
    {
        if (curTut == 1)
        {
            tutCanvas.transform.GetChild(curTut).gameObject.SetActive(false);
            curTut = -1;
        }

        revealFinished = true;
        revealWaiting = false;

        if (healWaiting)
        {
            curTut = 0;
            tutCanvas.transform.GetChild(curTut).gameObject.SetActive(true);
            healWaiting = false;
        }
    }
}
