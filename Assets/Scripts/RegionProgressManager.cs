using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionProgressManager : MonoBehaviour
{
    public GameObject UFO;
    public List<GameObject> Levels;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < Levels.Count; i++)
        {
            if (PlayerData.levelHighestPercentage.ContainsKey(Levels[i].name))
            {
                Levels[i].SetActive(true);
            }
            else
            {
                if (PlayerData.levelHighestPercentage.ContainsKey(Levels[i - 1].name) && PlayerData.levelHighestPercentage[Levels[i - 1].name] >= 50)
                {
                    Levels[i].SetActive(true);
                    break;
                }
            }
        }

        if (PlayerData.backFromScore)
        {
            PlayerData.backFromScore = false;
            UFO.transform.position = new Vector3(GameObject.Find(PlayerData.lastFinishedLevel).transform.position.x, UFO.transform.position.y, UFO.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
