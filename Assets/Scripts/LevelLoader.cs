using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum LoadingInstructions
{
    GAMEPAD,
    PLANTTREE,
    ENEMY,
    KEYBOARD
}

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;
    public GameObject loadingScreen;
    public Slider loadingBar;
    public GameObject skipA;
    public GameObject skipSpace;
    GameObject skip;

    public LevelSpecificSettings levelSpecificSettings;
    public GameObject playerPrefab;

    public GameObject[] loadingIns;

    bool isUFOLevel;
    List<Vector3> playerInitialPositions;
    float loadingTime;

    // Start is called before the first frame update
    void Awake()
    {
        GetLevelSettings();
        if(!isUFOLevel && PlayerData.AllPlayers.Count != 0)
        {
            LoadPlayerInfos();
        }
        PlayerData.isPlayingCutscene = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        GameObject gls = GameObject.Find("GameLevelStarter");
        if (gls != null)
        {
            gls.GetComponent<GameLevelStarter>().StartCountdown();
        }
        else
        {
            PlayerData.isPlayingCutscene = false;
        }
    }

    public void LoadNextLevel(int levelIndex, int loadInsIdx = 0)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SavePlayerInfos();
        }

        StartCoroutine(LoadLevel(levelIndex, loadInsIdx));
    }

    public void LoadNextLevel(string levelName, int loadInsIdx = 0)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SavePlayerInfos();
        }

        StartCoroutine(LoadLevel(levelName, loadInsIdx));
    }

    private void SavePlayerInfos()
    {
        PlayerData.ResetPlayers();
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
            PlayerInput pi = PlayerInput.Instantiate(playerPrefab, controlScheme: ps.controlScheme, playerIndex: ps.index, pairWithDevices: ps.inputDevice);
            pi.gameObject.GetComponent<PlayerController>().curModel = ps.modelNumber;
            pi.gameObject.transform.position = playerInitialPositions[i];

            i++;
        }
    }

    IEnumerator LoadLevel(int levelIndex, int loadInsIdx)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);
        SelectInstructions(loadInsIdx);
        float waitTime = GetWaitTime(loadInsIdx);
        
        if (waitTime > 0)
        {
            skip = PlayerData.mainPlayerControlScheme == "KeyboardAll" ? skipSpace : skipA;
        }

        float timePassed = 0f;

        while (!operation.isDone || timePassed < loadingTime)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            timePassed += Time.deltaTime;
            float timeProgress = timePassed / loadingTime;

            loadingBar.value = Mathf.Min(timeProgress, progress);

            if(loadingBar.value >= 1)
            {
                if (waitTime > 0)
                {
                    skip.SetActive(true);
                    if((PlayerData.mainPlayerControlScheme == "KeyboardAll" && Keyboard.current.spaceKey.wasPressedThisFrame) 
                        || (PlayerData.mainPlayerControlScheme == "Gamepad" && Gamepad.current.buttonSouth.wasPressedThisFrame))
                    {
                        operation.allowSceneActivation = true;
                    }
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    operation.allowSceneActivation = true;
                }
            }
            
            yield return null;
        }
    }

    IEnumerator LoadLevel(string levelName, int loadInsIdx)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);
        SelectInstructions(loadInsIdx);
        float waitTime = GetWaitTime(loadInsIdx);

        if (waitTime > 0)
        {
            skip = PlayerData.mainPlayerControlScheme == "KeyboardAll" ? skipSpace : skipA;
        }

        float timePassed = 0f;

        while (!operation.isDone || timePassed < loadingTime)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            timePassed += Time.deltaTime;
            float timeProgress = timePassed / loadingTime;

            loadingBar.value = Mathf.Min(timeProgress, progress);

            if (loadingBar.value >= 1)
            {
                if (waitTime > 0)
                {
                    skip.SetActive(true);
                    if ((PlayerData.mainPlayerControlScheme == "KeyboardAll" && Keyboard.current.spaceKey.wasPressedThisFrame)
                        || (PlayerData.mainPlayerControlScheme == "Gamepad" && Gamepad.current.buttonSouth.wasPressedThisFrame))
                    {
                        operation.allowSceneActivation = true;
                    }
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    private void GetLevelSettings()
    {
        isUFOLevel = levelSpecificSettings.isUFOLevel;
        playerInitialPositions = levelSpecificSettings.playerInitialPositions;
        loadingTime = levelSpecificSettings.loadingTime;
    }

    private void SelectInstructions(int idx)
    {
        //foreach(GameObject ins in loadingIns)
        //{
        //    ins.SetActive(false);
        //}

        loadingIns[idx].SetActive(true);
    }

    private float GetWaitTime(int loadInsIdx)
    {
        switch (loadInsIdx)
        {
            case 0:
                return 0;

            case 1:
                return 15;

            case 2:
                return 10;

            case 3:
                return 0;

            default:
                return 0;
        }
    }
}
