using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum VacuumBlendShape
{
    EXTENDED,
    OPEN,
    FILLED
}

public class VacuumController : MonoBehaviour, IInteractable
{
    public SkinnedMeshRenderer smRenderer;
    public float fillAmountPerSecond;

    public BoxCollider headCollider;
    public Vector3 BaseBoxcolliderCenter;
    public Vector3 ExtendedBoxcolliderCenter;

    public Vector3 holdPositionOffset;

    public AudioSource vacuumAirSound;
    public AudioSource vacuumPoopSound;
    public AudioSource vacuumFullBeepSound;

    float extendPercent = 0f;
    float openPercent = 0f;
    float fillPercent = 0f;

    float maxPercent = 100f;
    float minPercent = 0f;

    WaitForSeconds FixedWait = new WaitForSeconds(1f / 100f);
    WaitForSeconds FillWait;

    int playerNum = 0;
    bool isEmpty = true;
    bool isVacuuming = false;
    bool isOnPoop = false;
    bool isFilling = false;
    PoopSplatter currPoop;

    IEnumerator runningUnfillCoroutine = null;
    private Queue<IEnumerator> unfillCoroutineQueue = new Queue<IEnumerator>();

    public bool IsOnPoop { get => isOnPoop; set => isOnPoop = value; }
    public float FillPercent { get => fillPercent; }
    public bool IsVacuuming { get => isVacuuming; }

    // Start is called before the first frame update
    void Start()
    {
        FillWait = new WaitForSeconds(1f / fillAmountPerSecond);
    }

    // Update is called once per frame
    void Update()
    {
        if (isVacuuming && IsOnPoop && fillPercent < maxPercent)
        {
            if (isEmpty)
            {
                AddFirstPoop();
            }
            if (!isFilling)
            {
                StartCoroutine("Fill");
            }
        }

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Extend();
        //    OpenBottom();
        //    AddFirstPoop();
        //    StartCoroutine("Fill");
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Extend(true);
        //    OpenBottom(true);
        //}
    }

    public void VacuumSwitch()
    {
        isVacuuming = !isVacuuming;
        //Debug.Log(isVacuuming ? "switch on" : "switch off");
        if (isVacuuming)
        {
            if (fillPercent >= maxPercent)
            {
                vacuumFullBeepSound.Play();
                isVacuuming = false;
            }
            else
            {
                vacuumAirSound.Play();
            }
        }
        else
        {
            vacuumAirSound.Stop();
            vacuumPoopSound.Stop();
        }
        
    }

    public void SwitchPoop(GameObject poop)
    {
        if(currPoop != null)
        {
            currPoop.StopVacuum();
        }
        currPoop = poop.GetComponent<PoopSplatter>();
        if (isVacuuming && IsOnPoop && fillPercent < maxPercent)
        {
            currPoop.StartVacuum();
        }
    }

    public void Extend(bool isInverse = false)
    {
        StartCoroutine(isInverse ? "Retrieving" : "Extending");
    }
    
    public void OpenBottom(bool isInverse = false)
    {
        StartCoroutine(isInverse ? "Closing" : "Opening");
    }

    public void UnfillVacuum(float amount)
    {
        if(runningUnfillCoroutine == null)
        {
            runningUnfillCoroutine = Unfill(amount);
            StartCoroutine(runningUnfillCoroutine);
        }
        else
        {
            unfillCoroutineQueue.Enqueue(Unfill(amount));
        }
    }

    public void glow()
    {
        playerNum += 1;
        smRenderer.materials[1].SetFloat("_Emission", 1);
        smRenderer.materials[3].SetFloat("_Emission", 1);
        smRenderer.materials[4].SetFloat("_Emission", 1);
    }

    public void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            smRenderer.materials[1].SetFloat("_Emission", 0);
            smRenderer.materials[3].SetFloat("_Emission", 0);
            smRenderer.materials[4].SetFloat("_Emission", 0);
        }
    }

    public void OnPlayerInteract(GameObject player)
    {
        if (player.GetComponent<PlayerController>().Hold(this.gameObject, this.transform.GetChild(0).GetComponent<BoxCollider>(), holdPositionOffset, Quaternion.identity))
        {
            DeactivateRigidbody();
            Extend();
        }
    }

    public void OnDrop()
    {
        DeactivateRigidbody(true);
        Extend(true);
        vacuumAirSound.Stop();
        vacuumPoopSound.Stop();
    }

    public bool OnThrow(float throwForce)
    {
        return false;
    }

    public void PlaceOnStation()
    {
        DeactivateRigidbody();
        Extend(true);
        vacuumAirSound.Stop();
    }

    //ie hold or place
    private void DeactivateRigidbody(bool isInverse = false)
    {
        this.GetComponent<Rigidbody>().isKinematic = !isInverse;
        this.GetComponent<BoxCollider>().enabled = isInverse;
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = isInverse;
    }


    IEnumerator Extending()
    {
        yield return new WaitWhile(() => extendPercent > minPercent);
        while (extendPercent < maxPercent)
        {
            yield return FixedWait;
            extendPercent++;
            smRenderer.SetBlendShapeWeight((int)VacuumBlendShape.EXTENDED, extendPercent);
            headCollider.center = BaseBoxcolliderCenter + (extendPercent / maxPercent) * (ExtendedBoxcolliderCenter - BaseBoxcolliderCenter);
        }
    }

    IEnumerator Retrieving()
    {
        yield return new WaitWhile(() => extendPercent < maxPercent);
        while (extendPercent > minPercent)
        {
            yield return FixedWait;
            extendPercent--;
            smRenderer.SetBlendShapeWeight((int)VacuumBlendShape.EXTENDED, extendPercent);
            headCollider.center = BaseBoxcolliderCenter + (extendPercent / maxPercent) * (ExtendedBoxcolliderCenter - BaseBoxcolliderCenter);
        }
    }

    IEnumerator Opening()
    {
        yield return new WaitUntil(() => openPercent <= minPercent);
        while (openPercent < maxPercent)
        {
            yield return FixedWait;
            openPercent++;
            smRenderer.SetBlendShapeWeight((int)VacuumBlendShape.OPEN, openPercent);
        }
    }

    IEnumerator Closing()
    {
        yield return new WaitUntil(() => openPercent >= maxPercent);
        while (openPercent > minPercent)
        {
            yield return FixedWait;
            openPercent--;
            smRenderer.SetBlendShapeWeight((int)VacuumBlendShape.OPEN, openPercent);
        }
    }

    IEnumerator Fill()
    {
        isFilling = true;
        vacuumPoopSound.Play();
        currPoop.StartVacuum();
        
        while (fillPercent < maxPercent)
        {
            if (!isVacuuming || !IsOnPoop)
            {
                currPoop.StopVacuum();
                isFilling = false;
                vacuumPoopSound.Stop();
                yield break;
            }
            
            yield return FillWait;
            fillPercent++;
            smRenderer.SetBlendShapeWeight((int)VacuumBlendShape.FILLED, fillPercent);
        }

        fillPercent = maxPercent;
        currPoop.StopVacuum();
        isFilling = false;
        isVacuuming = false;
        
        vacuumFullBeepSound.Play();
        vacuumPoopSound.Stop();
        vacuumAirSound.Stop();
    }

    IEnumerator Unfill(float amount)
    {
        float targetPercent = fillPercent - amount;
        while (fillPercent > targetPercent)
        {
            yield return FillWait;
            fillPercent--;
            smRenderer.SetBlendShapeWeight((int)VacuumBlendShape.FILLED, fillPercent);
            if(fillPercent == 0f)
            {
                AddFirstPoop(true);
            }
        }

        runningUnfillCoroutine = null;
        if (unfillCoroutineQueue.Count > 0)
        {
            runningUnfillCoroutine = unfillCoroutineQueue.Dequeue();
            StartCoroutine(runningUnfillCoroutine);
        }
    }

    private void AddFirstPoop(bool inverse = false)
    {
        Color color = smRenderer.materials[5].GetColor("_BaseColor");
        color.a = inverse ? 0f : 1f;
        smRenderer.materials[5].SetColor("_BaseColor", color);
        isEmpty = inverse;
    }
}
