using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeWaterBar : MonoBehaviour
{
    public Image fillBarImage;
    public int waterPerGrowth;

    int waterCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool WaterOnce()
    {
        waterCount++;
        fillBarImage.fillAmount = 1f/waterPerGrowth * waterCount;
        if (waterCount == waterPerGrowth)
        {
            return true;
        }

        return false;
    }

    public void ResetBar()
    {
        waterCount = 0;
        fillBarImage.fillAmount = 0f;
    }
}
