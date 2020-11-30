using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventSystemController : MonoBehaviour
{
    public Button ResumeButton;
    public EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem.SetSelectedGameObject(ResumeButton.gameObject);
    }
    
}
