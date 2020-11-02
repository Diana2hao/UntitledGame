using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionLevelControl : EntranceController
{
    public GameObject environmentToChange;
    public Canvas boardCanvas;
    public Color restoredColor;

    public int nextSceneIndex;

    LevelLoader loader;
    

    // Start is called before the first frame update
    void Start()
    {
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
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

    public override void EnterNextScene()
    {
        loader.LoadNextLevel(nextSceneIndex);
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
