using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FertilizerProgressBar : MonoBehaviour
{
    public Transform progressBar;
    public Transform textIndicator;
    public Transform fertilizerStation;

    Image progressBarImage;
    TextMeshProUGUI bagCountText;
    
    StationController station;
    float fillAmountPerSec;
    float totalFillAmount;
    float currentAmount = 0f;

    int processingBagCount = 0;

    WaitForSeconds FillWait;

    IEnumerator runningFillCoroutine = null;
    private Queue<IEnumerator> fillCoroutineQueue = new Queue<IEnumerator>();

    // Start is called before the first frame update
    void Start()
    {
        progressBarImage = progressBar.GetComponent<Image>();
        bagCountText = textIndicator.GetComponent<TextMeshProUGUI>();

        station = fertilizerStation.GetComponent<StationController>();
        totalFillAmount = (station.MaxPercent / station.fertilizerBagCountPerFullTank);
        fillAmountPerSec = station.fillAmountPerSecond;
        FillWait = new WaitForSeconds(1f / fillAmountPerSec);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBags(int count)
    {
        //increment bag count and set text
        processingBagCount += count;
        bagCountText.text = processingBagCount.ToString();

        for(int i = 0; i < count; i++)
        {
            //start coroutine if no running ones, otherwise queue this coroutine
            if (runningFillCoroutine == null)
            {
                runningFillCoroutine = Fill();
                StartCoroutine(runningFillCoroutine);
            }
            else
            {
                fillCoroutineQueue.Enqueue(Fill());
            }
        }
    }

    IEnumerator Fill()
    {
        //fill the bar
        while (currentAmount < totalFillAmount)
        {
            yield return FillWait;
            currentAmount++;
            progressBarImage.fillAmount = currentAmount / totalFillAmount;
        }

        //bag count -1 and set text, reset progress bar
        processingBagCount--;
        bagCountText.text = processingBagCount.ToString();
        currentAmount = 0f;
        progressBarImage.fillAmount = 0f;

        //tell station to drop a bag of fertilizer
        station.AddABag();

        //check if should run the next queued coroutine
        runningFillCoroutine = null;
        if (fillCoroutineQueue.Count > 0)
        {
            runningFillCoroutine = fillCoroutineQueue.Dequeue();
            StartCoroutine(runningFillCoroutine);
        }
        //if no bags left to process, notify station
        else
        {
            station.FinishProcessing();
        }
    }
}
