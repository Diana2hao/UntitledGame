using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionController : EntranceController
{
    public Canvas boardCanvas;
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
