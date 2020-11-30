using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RegionLevelControl : MonoBehaviour, IEntrance
{
    public GameObject environmentToChange;
    public Canvas boardCanvas;
    public Color restoredColor;
    public TextMeshProUGUI percentageScore;

    //public int nextSceneIndex;
    public int loadInsIdx;

    LevelLoader loader;

    // Start is called before the first frame update
    void Start()
    {
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        int percent = PlayerData.GetLevelPercentage(this.gameObject.name);
        percentageScore.text = percent + "%";
        if (percent >= 50)
        {
            Restore();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restore()
    {
        environmentToChange.transform.GetChild(0).gameObject.SetActive(true);
        environmentToChange.transform.GetChild(1).gameObject.SetActive(false);

        foreach(Transform changeColor in environmentToChange.transform.GetChild(2))
        {
            changeColor.GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", restoredColor);
        }
    }

    public void EnterNextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(this.gameObject.name))
        {
            loader.LoadNextLevel(this.gameObject.name, loadInsIdx);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        boardCanvas.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        boardCanvas.gameObject.SetActive(false);
    }
}
