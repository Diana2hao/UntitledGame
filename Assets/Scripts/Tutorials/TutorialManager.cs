using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    //playerpref keys
    string keyboardTut = "keyboardTut";
    string gamepadTut = "gamepadTut";

    //public
    public GameObject departureReady;
    public GameObject departureNotReady;
    public GameObject departureWarning;
    public AudioSource departureSound;

    bool keyboardFinished;
    bool gamepadFinished;

    int readyPlayerCount = 0;

    List<GameObject> allTutCanvas = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        departureReady.SetActive(false);
        departureNotReady.SetActive(true);

        if (PlayerPrefs.HasKey(keyboardTut))
        {
            keyboardFinished = (PlayerPrefs.GetInt(keyboardTut) == 1);
        }
        else
        {
            keyboardFinished = false;
        }

        if (PlayerPrefs.HasKey(gamepadTut))
        {
            gamepadFinished = (PlayerPrefs.GetInt(gamepadTut) == 1);
        }
        else
        {
            gamepadFinished = false;
        }

        if(keyboardFinished && gamepadFinished)
        {
            departureReady.SetActive(true);
            departureNotReady.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanStart()
    {
        readyPlayerCount++;
        if(readyPlayerCount == PlayerInputManagerController.PlayerNum)
        {
            TurnOnDepartureWarning();
        }
    }

    public void TurnOnDepartureWarning()
    {
        departureReady.SetActive(true);
        departureNotReady.SetActive(false);
        departureWarning.SetActive(true);
        departureSound.Play();
    }

    public void AddTutCanvas(GameObject tut)
    {
        allTutCanvas.Add(tut);
    }

    public void DeleteAllTut()
    {
        foreach(GameObject tut in allTutCanvas)
        {
            Destroy(tut);
        }
    }
}
