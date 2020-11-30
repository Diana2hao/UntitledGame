using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<GameObject> ScoreBoards;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        foreach(GameObject s in ScoreBoards)
        {
            if(s.name == PlayerData.lastFinishedLevel)
            {
                s.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
