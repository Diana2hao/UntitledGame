using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayTutR1L0 : MonoBehaviour
{
    public GameObject GameplayCanvas;
    public bool isEnabled = false;

    PlayerController pCon;
    VacuumController vCon;
    StationController sCon;
    GameObject tutCanvas;
    GameplayTutorialManagerR1L0 tutManager;

    int tutStep = 0;

    GameObject latestTree;

    // Start is called before the first frame update
    void Start()
    {
        pCon = this.GetComponent<PlayerController>();
        tutCanvas = Instantiate(GameplayCanvas);
        tutCanvas.GetComponent<FollowPlayer>().playerToFollow = this.gameObject.transform;
        tutManager = GameObject.Find("TutorialManager").GetComponent<GameplayTutorialManagerR1L0>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tutStep == 10)
        {
            if (vCon.IsOnPoop)
            {
                tutManager.poopArrow.SetActive(false);
                tutStep++;
            }
        }

        if (tutStep == 11)
        {
            if (vCon.FillPercent > 25)
            {
                ToggleTutCanvas(4, 6);
                tutManager.fertStationArrow.SetActive(true);
                tutManager.FertAddPoop.SetActive(true);
                tutManager.FertAddWater.SetActive(true);
                tutStep++;
            }
        }
    }

    void OnInteract()
    {
        if (!isEnabled)
        {
            return;
        }

        if(pCon.CurrentHandheldObject != null && pCon.CurrentHandheldObject.CompareTag("Plant"))
        {
            latestTree = pCon.CurrentHandheldObject;
        }

        switch (tutStep)
        {
            case 0:
                if (pCon.CurrentHandheldObject.CompareTag("Plant"))
                {
                    ToggleTutCanvas(0, 1);
                    tutManager.plantBoxArrow.SetActive(false);
                    tutStep++;
                }
                break;

            case 1:
                if (pCon.CurrentHandheldObject == null)
                {
                    ToggleTutCanvas(1, 2);
                    tutManager.bucketArrow.SetActive(true);
                    tutStep++;
                }
                break;

            case 2:
                if (pCon.CurrentHandheldObject.CompareTag("Bucket"))
                {
                    //ToggleTutCanvas(2, 3);
                    tutManager.bucketArrow.SetActive(false);
                    tutManager.pondArrow.SetActive(true);
                    tutStep++;
                }
                break;

            case 3:
                if (pCon.CurrentHandheldObject.CompareTag("Bucket"))
                {
                    if (pCon.CurrentHandheldObject.GetComponent<BucketController>().IsFilled)
                    {
                        tutManager.pondArrow.SetActive(false);
                        tutManager.SetupTreeArrow(latestTree.transform);
                        tutStep++;
                    }
                }
                break;

            case 4:
                if (pCon.CurrentHandheldObject.CompareTag("Bucket"))
                {
                    if (!pCon.CurrentHandheldObject.GetComponent<BucketController>().IsFilled)
                    {
                        tutManager.DeactivateTreeArrow();
                    }

                    if (pCon.CurInteractObject != null && pCon.CurInteractObject.GetComponent<TreeControl>().IsGrowing)
                    {
                        ToggleTutCanvas(2, 3);
                        tutManager.SetupTreeArrow(pCon.CurInteractObject.transform);
                        tutManager.fertBagArrow.SetActive(true);
                        tutStep++;
                    }
                }
                break;

            case 5:
                if (pCon.CurrentHandheldObject.CompareTag("FertilizerBag"))
                {
                    tutManager.fertBagArrow.SetActive(false);
                    tutStep++;
                }
                break;

            case 6:
                if (pCon.CurInteractObject.GetComponent<TreeControl>().growBar.GetBagCount() > 0)
                {
                    tutManager.fertBagArrow.SetActive(false);
                    tutManager.DeactivateTreeArrow();
                    tutCanvas.transform.GetChild(3).gameObject.SetActive(false);
                    tutStep++;
                    tutManager.LookForPoop(this);
                }
                break;

            case 8:
                if (pCon.CurrentHandheldObject.CompareTag("Vacuum"))
                {
                    vCon = pCon.CurrentHandheldObject.GetComponent<VacuumController>();
                    tutManager.vacArrow.SetActive(false);
                    ToggleTutCanvas(4, 5);
                    tutStep++;
                }
                break;

            case 9:
                if (vCon.IsVacuuming)
                {
                    ToggleTutCanvas(5, 4);
                    tutStep++;
                }
                break;

            case 12:
                if (pCon.CurInteractObject.CompareTag("Station"))
                {
                    sCon = pCon.CurInteractObject.GetComponent<StationController>();
                    if (sCon.PoopPercent > sCon.BatchAmount)
                    {
                        tutManager.FertAddPoop.SetActive(false);
                        tutStep++;
                    }
                }
                break;

            case 13:
                if (pCon.CurInteractObject.CompareTag("Station"))
                {
                    if (sCon.WaterFillPercent > 0)
                    {
                        tutCanvas.transform.GetChild(6).gameObject.SetActive(false);
                        tutManager.fertStationArrow.SetActive(false);
                        tutManager.FertAddWater.SetActive(false);
                        tutStep++;
                    }
                }
                break;

            default:
                break;
        }
    }

    public void SkipFertTut()
    {
        if (tutStep == 5 || tutStep == 6)
        {
            tutManager.fertBagArrow.SetActive(false);
            tutManager.DeactivateTreeArrow();
            tutCanvas.transform.GetChild(3).gameObject.SetActive(false);
            tutStep = 7;
            tutManager.LookForPoop(this);
        }
    }
    
    public void StartCollectPoop()
    {
        if (tutStep == 7)
        {
            tutCanvas.transform.GetChild(4).gameObject.SetActive(true);
            tutManager.vacArrow.SetActive(true);
            tutStep++;
        }
    }

    private void ToggleTutCanvas(int deactivate, int activate)
    {
        tutCanvas.transform.GetChild(deactivate).gameObject.SetActive(false);
        tutCanvas.transform.GetChild(activate).gameObject.SetActive(true);
    }
}
