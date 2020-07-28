using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpController : InteractableController
{
    public PlayerInputManagerController pm;

    Renderer rd;
    int playerNum;


    // Start is called before the first frame update
    void Start()
    {
        rd = transform.GetChild(0).gameObject.GetComponent<Renderer>();
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
        PlayerInput pi = player.GetComponent<PlayerInput>();
        if (pi.currentControlScheme.Contains("Keyboard")) {
            pm.SendMessage("JoinKeyboardPlayer", player);
        }
    }
}
