using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;
    public GameObject loadingScreen;
    public Slider loadingBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SavePlayerInfos();
        }

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private void SavePlayerInfos()
    {
        foreach(PlayerInput pi in PlayerInput.all)
        {
            PlayerData.AddPlayer(pi.playerIndex, pi.gameObject.GetComponent<PlayerController>().curModel, pi.currentControlScheme, pi.devices[0]);
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBar.value = progress;
            Debug.Log(progress);

            yield return null;
        }
    }
}
