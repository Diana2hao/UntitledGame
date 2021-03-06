﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public int previousSceneIndex;
    public LevelLoader loader;
    public EventSystem eventSystem;
    public Button firstButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseUnpause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        this.gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        this.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(firstButton.gameObject);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Reload()
    {
        Resume();
        loader.LoadNextLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("quitting...");
            Application.Quit();
        }
        else
        {
            Resume();
            loader.LoadNextLevel(previousSceneIndex);
        }
    }
}
