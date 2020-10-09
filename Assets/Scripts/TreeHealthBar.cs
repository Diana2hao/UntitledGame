using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeHealthBar : MonoBehaviour
{
    public Image fillBarImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFillAmount(float amount)
    {
        fillBarImage.fillAmount = amount;
    }

    public float GetFillAmount()
    {
        return fillBarImage.fillAmount;
    }
}
