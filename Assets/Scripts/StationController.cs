using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StationBlendShape
{
    HOLDVACUUM,
    WATERTANKFILL,
    LIDOPEN
}

public class StationController : MonoBehaviour, IInteractable
{
    public SkinnedMeshRenderer smRenderer;
    public float fillAmountPerSecond;
    public GameObject fertilizerPrefab;
    public int fertilizerBagCountPerFullTank;
    public GameObject progressBar;

    GameObject vacPositionHolder;
    GameObject fertPositionHolder;

    int playerNum = 0;
    VacuumController vacuum;
    bool hasVacuum = false;
    bool vacuumReady = false;
    bool hasFertilizerOut = false;
    int fertBagCount = 0;
    FertilizerBagController fertBag;

    float holdPercent = 0f;
    float waterFillPercent = 0f;
    float batchAmount;

    float poopPercent = 0f;
    float poopLeftPercent = 0f;

    IEnumerator runningUnfillCoroutine = null;
    private Queue<IEnumerator> unfillCoroutineQueue = new Queue<IEnumerator>();

    IEnumerator runningPopCoroutine = null;
    private Queue<IEnumerator> popCoroutineQueue = new Queue<IEnumerator>();

    float maxPercent = 100f;
    float minPercent = 0f;

    bool isProcessing = false;
    float xOffset = 0f;

    FertilizerProgressBar fProgressBar;

    WaitForSeconds FixedWait = new WaitForSeconds(1f / 100f);
    WaitForSeconds FillWait;

    public float MaxPercent { get => maxPercent; set => maxPercent = value; }
    public bool HasVacuum { get => hasVacuum; }
    public float WaterFillPercent { get => waterFillPercent; }
    public float BatchAmount { get => batchAmount; }
    public float PoopPercent { get => poopPercent; }

    // Start is called before the first frame update
    void Start()
    {
        vacPositionHolder = this.transform.GetChild(0).gameObject;
        fertPositionHolder = this.transform.GetChild(1).gameObject;

        batchAmount = maxPercent / fertilizerBagCountPerFullTank;
        FillWait = new WaitForSeconds(1f / fillAmountPerSecond);

        fProgressBar = progressBar.GetComponent<FertilizerProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isProcessing = !isProcessing;
            StartCoroutine("ProcessingTextureAnimation");
        }
    }

    public void glow()
    {
        playerNum += 1;
        smRenderer.materials[1].EnableKeyword("_EMISSION");
    }

    public void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            smRenderer.materials[1].DisableKeyword("_EMISSION");
        }
    }

    public void OnPlayerInteract(GameObject player)
    {
        PlayerController pControl = player.GetComponent<PlayerController>();
        if (pControl.CurrentHandheldObject != null )
        {
            if (pControl.CurrentHandheldObject.CompareTag("Vacuum"))
            {
                // place vacuum on station
                HoldVacuum();
                GameObject vac = pControl.CurrentHandheldObject;
                pControl.OnDrop();
                
                vac.transform.position = vacPositionHolder.transform.position;
                vac.transform.rotation = this.transform.rotation;
                vac.transform.parent = vacPositionHolder.transform;

                vacuum = vac.GetComponent<VacuumController>();
                vacuum.PlaceOnStation();

                hasVacuum = true;

                // start processing poop
                poopPercent = vacuum.FillPercent;
                int processableCount = CalculateProcessableCount();
                if (processableCount > 0)
                {
                    //unfill vacuum 
                    vacuum.OpenBottom();
                    vacuum.UnfillVacuum(processableCount * batchAmount);

                    //unfill water tank
                    UnfillWaterTank(processableCount * batchAmount);

                    //play processing animation
                    isProcessing = true;
                    StartCoroutine("ProcessingTextureAnimation");

                    //start progress bar
                    //progressBar.SetActive(true);
                    fProgressBar.AddBags(processableCount);
                }
                else
                {
                    vacuumReady = true;

                    //TODO: warn player not enough poop/water
                    if (poopPercent < batchAmount)
                    {
                        // show a sign
                    }

                    if(waterFillPercent < batchAmount)
                    {
                        // show another sign
                    }
                }
            }
            else if (pControl.CurrentHandheldObject.CompareTag("Bucket"))
            {
                // if bucket has water, fill tank
                if (pControl.Water())
                {
                    //fill 1/4 tank
                    waterFillPercent += batchAmount;
                    if(waterFillPercent > maxPercent) { waterFillPercent = maxPercent; }
                    smRenderer.SetBlendShapeWeight((int)StationBlendShape.WATERTANKFILL, waterFillPercent);

                    //if enought poop left, process another bag
                    if (poopLeftPercent >= batchAmount)
                    {
                        vacuumReady = false;
                        vacuum.UnfillVacuum(batchAmount);
                        UnfillWaterTank(batchAmount);
                        if (!isProcessing)
                        {
                            isProcessing = true;
                            StartCoroutine("ProcessingTextureAnimation");
                        }
                        fProgressBar.AddBags(1);
                        poopLeftPercent -= batchAmount;
                    }
                }
            }
        }
        else
        {
            if (hasVacuum && vacuumReady)
            {
                if (!hasFertilizerOut)
                {
                    // player take vacuum
                    PlayerTakeAwayVacuum(player);
                }
                else
                {
                    float vacDistance = Vector3.Distance(player.transform.position, vacPositionHolder.transform.position);
                    float fertDistance = Vector3.Distance(player.transform.position, fertPositionHolder.transform.position);

                    // player closer to vac side, take vac
                    if (vacDistance < fertDistance)
                    {
                        // player take vacuum
                        PlayerTakeAwayVacuum(player);
                    }
                    // player closer to fert side, take fert
                    else
                    {
                        PlayerTakeAwayFertilizer(player);
                    }

                }
            }
            else
            {
                if (hasFertilizerOut)
                {
                    // player take fertilizer
                    PlayerTakeAwayFertilizer(player);
                }
            }
        }
    }

    public void AddABag()
    {
        if (!hasFertilizerOut && runningPopCoroutine == null)
        {
            runningPopCoroutine = PopOutAFertBag();
            StartCoroutine(runningPopCoroutine);
        }
        else
        {
            popCoroutineQueue.Enqueue(PopOutAFertBag());
        }
    }

    IEnumerator PopOutAFertBag()
    {
        float openPercent = 0f;
        while(openPercent < 100f)
        {
            yield return new WaitForSeconds(0.5f);
            openPercent += 50f;
            smRenderer.SetBlendShapeWeight((int)StationBlendShape.LIDOPEN, openPercent);
        }

        GameObject bag = Instantiate(fertilizerPrefab);
        fertBag = bag.GetComponent<FertilizerBagController>();
        fertBag.PopOutOfStation();
        bag.transform.position = fertPositionHolder.transform.position;
        bag.transform.rotation = this.transform.rotation;
        bag.transform.parent = fertPositionHolder.transform;

        while (openPercent > 0f)
        {
            yield return new WaitForSeconds(0.5f);
            openPercent -= 50f;
            smRenderer.SetBlendShapeWeight((int)StationBlendShape.LIDOPEN, openPercent);
        }

        runningPopCoroutine = null;
        hasFertilizerOut = true;
    }

    public void FinishProcessing()
    {
        vacuumReady = true;
        isProcessing = false;
    }

    private int CalculateProcessableCount()
    {
        int poopCount = Mathf.FloorToInt(poopPercent / batchAmount);
        int waterCount = Mathf.FloorToInt(waterFillPercent / batchAmount);
        int processableCount = Mathf.Min(poopCount, waterCount);
        poopLeftPercent = poopPercent - processableCount * batchAmount;

        return processableCount;
    }

    private void UnfillWaterTank(float amount)
    {
        if (runningUnfillCoroutine == null)
        {
            runningUnfillCoroutine = Unfill(amount);
            StartCoroutine(runningUnfillCoroutine);
        }
        else
        {
            unfillCoroutineQueue.Enqueue(Unfill(amount));
        }
    }

    private void HoldVacuum(bool isInverse = false)
    {
        StartCoroutine(isInverse ? "Releasing" : "Holding");
    }

    private void PlayerTakeAwayVacuum(GameObject player)
    {
        HoldVacuum(true);
        vacuum.OpenBottom(true);
        vacuum.OnPlayerInteract(player);
        hasVacuum = false;
        vacuumReady = false;

        //shouldn't need these, just in case
        poopLeftPercent = 0f;
        poopPercent = 0f;
    }

    private void PlayerTakeAwayFertilizer(GameObject player)
    {
        fertBag.OnPlayerInteract(player);

        hasFertilizerOut = false;
        if (popCoroutineQueue.Count > 0)
        {
            runningPopCoroutine = popCoroutineQueue.Dequeue();
            StartCoroutine(runningPopCoroutine);
        }
    }

    IEnumerator Holding()
    {
        yield return new WaitWhile(() => holdPercent > minPercent);
        while (holdPercent < maxPercent)
        {
            yield return FixedWait;
            holdPercent++;
            smRenderer.SetBlendShapeWeight((int)StationBlendShape.HOLDVACUUM, holdPercent);
        }
    }

    IEnumerator Releasing()
    {
        yield return new WaitWhile(() => holdPercent < maxPercent);
        while (holdPercent > minPercent)
        {
            yield return FixedWait;
            holdPercent--;
            smRenderer.SetBlendShapeWeight((int)StationBlendShape.HOLDVACUUM, holdPercent);
        }
    }

    IEnumerator Fill()
    {
        while (waterFillPercent < maxPercent)
        {
            yield return FillWait;
            waterFillPercent++;
            smRenderer.SetBlendShapeWeight((int)StationBlendShape.WATERTANKFILL, waterFillPercent);
        }
    }

    IEnumerator Unfill(float amount)
    {
        float targetPercent = waterFillPercent - amount;
        while (waterFillPercent > targetPercent)
        {
            yield return FillWait;
            waterFillPercent--;
            smRenderer.SetBlendShapeWeight((int)StationBlendShape.WATERTANKFILL, waterFillPercent);
        }

        //check if should run the next queued coroutine
        runningUnfillCoroutine = null;
        if (unfillCoroutineQueue.Count > 0)
        {
            runningUnfillCoroutine = unfillCoroutineQueue.Dequeue();
            StartCoroutine(runningUnfillCoroutine);
        }
    }

    

    IEnumerator ProcessingTextureAnimation()
    {
        while (isProcessing)
        {
            xOffset += 1f / 3f;
            smRenderer.materials[5].SetTextureOffset("_BaseMap", new Vector2(xOffset, 0));
            yield return new WaitForSeconds(0.3f);
        }

        xOffset = 0f;
        smRenderer.materials[5].SetTextureOffset("_BaseMap", new Vector2(xOffset, 0));
    }

    public void OnDrop()
    {
        
    }

    public bool OnThrow(float throwForce)
    {
        return false;
    }
}
