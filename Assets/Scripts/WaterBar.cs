using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(int maxValue)
    {
        slider.maxValue = maxValue;
    }

    public void SetCurrentValue(int count)
    {
        slider.value = count;
    }
}
