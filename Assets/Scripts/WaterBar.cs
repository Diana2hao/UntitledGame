using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    public Slider slider;

    public void SetWaterCount(int count)
    {
        slider.value = count;
    }
}
