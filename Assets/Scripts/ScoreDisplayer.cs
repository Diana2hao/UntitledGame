using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreDisplayer : MonoBehaviour
{
    public List<TextMeshProUGUI> scoreTexts;
    public List<float> fullScores;
    public TextMeshProUGUI percentageText;

    public GameObject aButton;
    public GameObject spaceButton;

    List<int> scores;
    int countFinished = 0;
    bool percentDisplayed = false;
    string nextSceneName;

    public List<int> Scores { get => scores; set => scores = value; }
    
    // Start is called before the first frame update
    void Start()
    {
        scores = PlayerData.lastGameScores;
        for(int i = 0; i < scores.Count; i++)
        {
            StartCoroutine(CountUpToTarget(scores[i], scoreTexts[i], 1f));
        }
        nextSceneName = this.gameObject.name.Split('_')[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!percentDisplayed && countFinished == scores.Count)
        {
            CalculatePercentage();
            percentDisplayed = true;
            (PlayerData.mainPlayerControlScheme == "KeyboardAll" ? spaceButton : aButton).SetActive(true);
        }

        if (percentDisplayed)
        {
            if ((PlayerData.mainPlayerControlScheme == "KeyboardAll" && Keyboard.current.spaceKey.wasPressedThisFrame)
                 || (PlayerData.mainPlayerControlScheme == "Gamepad" && Gamepad.current.buttonSouth.wasPressedThisFrame))
            {
                PlayerData.backFromScore = true;
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    IEnumerator CountUpToTarget(int targetScore, TextMeshProUGUI displayText, float duration)
    {
        float currentDisplayScore = 0f;
        while (currentDisplayScore < targetScore)
        {
            currentDisplayScore += targetScore / (duration / Time.deltaTime); // or whatever to get the speed you like
            currentDisplayScore = Mathf.Clamp(currentDisplayScore, 0f, targetScore);
            displayText.text = Mathf.FloorToInt(currentDisplayScore) + "";
            yield return null;
        }
        countFinished++;
        
    }

    private void CalculatePercentage()
    {
        float percent = 0;

        for (int i = 0; i < scores.Count; i++)
        {
            percent += scores[i] / fullScores[i];
        }

        percent = Mathf.FloorToInt(Mathf.Clamp(percent / scores.Count * 100, 0, 100));

        PlayerData.UpdateLevelPercentage(this.gameObject.name, (int)percent);

        percentageText.text = percent + "% Recovered";
    }
}
