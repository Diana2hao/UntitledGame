using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLevelStarter : MonoBehaviour
{
    public GameObject countdownCanvas;
    public TextMeshProUGUI timerText;

    public float timeRemaining = 300f;
    bool timerStarted = false;

    public GameLevelEnd endController;

    // Start is called before the first frame update
    void Start()
    {
        DisplayTime(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                DisplayTime(timeRemaining);

                //trigger end game events
                endController.EndLevel();
            }
        }
    }

    public void StartCountdown()
    {
        countdownCanvas.SetActive(true);
    }

    public void StartLevel()
    {
        countdownCanvas.SetActive(false);
        PlayerData.isPlayingCutscene = false;
        timerStarted = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
