using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGameController : InteractableController
{
    public PlayerInputManagerController pm;
    public LevelLoader loader;

    Renderer rd;
    int playerNum;


    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Renderer>();
        playerNum = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void glow()
    {
        playerNum += 1;
        rd.material.SetColor("_EmissionColor", new Color(0.067f, 0.515f, 1.0f, 1.0f));
    }

    public override void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            rd.material.SetColor("_EmissionColor", new Color(0.001f, 0.001f, 0.001f, 1.0f));
        }
    }

    public override void OnPlayerInteract(GameObject player)
    {
        if (playerNum == pm.PlayerNum)
        {
            //load next scene
            //SceneManager.LoadScene("SpM_L1");
            pm.GetComponent<PlayerInputManager>().DisableJoining();
            loader.LoadNextLevel(1);
        }
    }
}
