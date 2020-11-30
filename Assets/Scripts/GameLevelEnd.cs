using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelEnd : MonoBehaviour
{
    public LevelControl levelControl;
    public LevelSpecificSettings levelSpecificSettings;
    public GameObject endCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndLevel()
    {
        List<int> scores = new List<int>() { levelControl.TotalTree, levelControl.TotalBird };
        PlayerData.UpdateLastLevelScore(scores);
        PlayerData.lastFinishedLevel = SceneManager.GetActiveScene().name;
        Time.timeScale = 0;
        endCanvas.SetActive(true);
    }

    public void LoadScoreScene()
    {
        SceneManager.LoadScene("Scores");
    }
}
