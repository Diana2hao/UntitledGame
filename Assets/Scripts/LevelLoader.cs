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

    public bool isUFOLevel;
    public GameObject playerPrefab;
    public List<Vector3> playerInitialPositions;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerData.AllPlayers.Count);
        if(!isUFOLevel && PlayerData.AllPlayers.Count != 0)
        {
            LoadPlayerInfos();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel(int levelIndex)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SavePlayerInfos();
        }

        StartCoroutine(LoadLevel(levelIndex));
    }

    private void SavePlayerInfos()
    {
        foreach(PlayerInput pi in PlayerInput.all)
        {
            PlayerData.AddPlayer(pi.playerIndex, pi.gameObject.GetComponent<PlayerController>().curModel, pi.currentControlScheme, pi.devices[0]);
        }
    }

    private void LoadPlayerInfos()
    {
        int i = 0;
        foreach(PlayerSettings ps in PlayerData.AllPlayers)
        {
            Debug.Log("player");
            PlayerInput pi = PlayerInput.Instantiate(playerPrefab, controlScheme: ps.controlScheme, playerIndex: ps.index, pairWithDevices: ps.inputDevice);
            pi.gameObject.GetComponent<PlayerController>().curModel = ps.modelNumber;
            pi.gameObject.transform.position = playerInitialPositions[i];

            i++;
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
