using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionController : MonoBehaviour, IEntrance
{
    public Canvas boardCanvas;
    //public int nextSceneIndex;
    public int loadInsIdx;

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

    public void EnterNextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(this.gameObject.name))
        {
            loader.LoadNextLevel(this.gameObject.name, loadInsIdx);
        }

        //if (nextSceneIndex < 0)
        //{
        //    return;
        //}

        //loader.LoadNextLevel(nextSceneIndex, loadInsIdx);
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
