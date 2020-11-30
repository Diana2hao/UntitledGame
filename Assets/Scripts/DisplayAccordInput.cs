using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAccordInput : MonoBehaviour
{
    public GameObject keyboardDisplay;
    public GameObject gamepadDisplay;

    // Start is called before the first frame update
    void Start()
    {
        if( PlayerData.mainPlayerControlScheme == "Gamepad")
        {
            gamepadDisplay.SetActive(true);
        }
        else
        {
            keyboardDisplay.SetActive(true);
        }
    }
    
}
