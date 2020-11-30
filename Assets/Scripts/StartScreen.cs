using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        if (!PlayerData.gameStarted)
        {
            PlayerData.isPlayingCutscene = true;
        }
        else
        {
            this.gameObject.SetActive(false);
            PlayerData.isPlayingCutscene = false;
        }
    }

    public void StartGame()
    {
        anim.SetBool("playerJoined", true);
    }
}
