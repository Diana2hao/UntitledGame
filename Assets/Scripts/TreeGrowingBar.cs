using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeGrowingBar : MonoBehaviour
{
    public Image fertBarImage;
    public Image growBarImage;
    public float initialGrowSpeed; //percent per second (20)
    public float speedMultiplierPerBag; //1.5
    public int totalFertilizerBagsAddable;
    public TreeControl tc;

    bool isGrowing;
    float growPercent = 0f;
    int bagCount = 0;

    WaitForSeconds FixedWait = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGrowing()
    {
        isGrowing = true;
        StartCoroutine("Grow");
    }

    public void StopGrowing()
    {
        isGrowing = false;
    }

    public void AddOneBagOfFert()
    {
        bagCount++;
        fertBarImage.fillAmount = 1f / totalFertilizerBagsAddable * bagCount;
    }

    public float GetBagCount()
    {
        return bagCount;
    }

    IEnumerator Grow()
    {
        while (isGrowing)
        {
            yield return FixedWait;
            growPercent += 0.5f * initialGrowSpeed * Mathf.Pow(speedMultiplierPerBag, bagCount);
            growBarImage.fillAmount = growPercent / 100f;
            if (growBarImage.fillAmount >= 1)
            {
                //tell tree to change model
                tc.GrowUpOneStage();
                isGrowing = false;
            }
        }
    }

    public void ResetBar()
    {
        growPercent = 0f;
        bagCount = 0;
        growBarImage.fillAmount = growPercent;
        fertBarImage.fillAmount = bagCount;
    }
}
