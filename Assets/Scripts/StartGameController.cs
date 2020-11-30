using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGameController : MonoBehaviour, IInteractable
{
    public LevelLoader loader;
    public ParticleSystem transportEffect;
    public GameObject UFO;
    public TutorialManager tutManager;
    public TextMeshProUGUI totalPlayer;
    public TextMeshProUGUI readyPlayer;
    public PlayerInputManager piManager;

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
        totalPlayer.text = piManager.playerCount.ToString();
    }

    public void glow()
    {
        playerNum += 1;
        readyPlayer.text = playerNum.ToString();
        rd.material.SetColor("_EmissionColor", new Color(0.067f, 0.515f, 1.0f, 1.0f));
    }

    public void unglow()
    {
        playerNum -= 1;
        readyPlayer.text = playerNum.ToString();
        if (playerNum == 0)
        {
            rd.material.SetColor("_EmissionColor", new Color(0.001f, 0.001f, 0.001f, 1.0f));
        }
    }

    public void OnPlayerInteract(GameObject player)
    {
        Debug.Log(piManager.playerCount);
        if (playerNum == piManager.playerCount)
        {
            piManager.DisableJoining();
            tutManager.TurnOnDepartureWarning();
            tutManager.DeleteAllTut();

            //departure animation
            PlayerData.isPlayingCutscene = true;
            StartCoroutine("DepartureAnimation");
        }
    }

    IEnumerator DepartureAnimation()
    {
        transportEffect.Play();

        foreach (PlayerInput pi in PlayerInput.all)
        {
            pi.gameObject.GetComponent<Collider>().enabled = false;
            pi.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        int i = 0;
        while (i < 110)
        {
            foreach (PlayerInput pi in PlayerInput.all)
            {
                pi.gameObject.transform.position -= new Vector3(0, 0.02f, 0);
            }

            i++;

            yield return null;
        }

        transportEffect.Stop();

        //ufo fly anim
        UFO.SetActive(true);
        yield return new WaitForSeconds(4f);

        //load next scene
        PlayerData.isPlayingCutscene = false;
        loader.LoadNextLevel(1,3);
    }

    public void OnDrop()
    {
        
    }

    public bool OnThrow(float throwForce)
    {
        return false;
    }
}
