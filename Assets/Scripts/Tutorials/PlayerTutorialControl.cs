using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MyInputScheme
{
    KEYBOARDALL,
    KEYBOARDRIGHT,
    KEYBOARDLEFT,
    GAMEPAD
}

public class PlayerTutorialControl : MonoBehaviour
{
    public GameObject tutorialCanvas;
    public bool isEnabled = false;

    TutorialManager tutManager;

    InstructionsControl moveTut;
    InstructionsControl interactTut;
    InstructionsControl dashTut;
    InstructionsControl dropThrowTut;
    InstructionsControl menuTut;

    InstructionsControl[] allTuts;
    
    int inputScheme = (int)MyInputScheme.KEYBOARDALL;

    PlayerController pCon;

    int walkCount = 8;
    bool walkFinished = false;
    int dashCount = 2;
    bool dashFinished = false;
    bool interactFinished = false;
    bool dropFinished = false;
    bool throwFinished = false;

    int currActiveTutIndex = 0;

    public int InputScheme { get => inputScheme; set => inputScheme = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (isEnabled)
        {
            GetAllTut();
            pCon = this.GetComponent<PlayerController>();
            moveTut.DisplayAccordingToScheme(inputScheme);
            tutManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
            if (inputScheme == (int)MyInputScheme.GAMEPAD)
            {
                walkCount *= 2;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue value)
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }

        if (isEnabled)
        {
            if (walkFinished)
            {
                return;
            }

            walkCount -= 1;

            if (walkCount <= 0)
            {
                moveTut.gameObject.SetActive(false);
                interactTut.gameObject.SetActive(true);
                interactTut.DisplayAccordingToScheme(inputScheme);
                walkFinished = true;

                foreach (GameObject ball in GameObject.FindGameObjectsWithTag("TutBalls"))
                {
                    ball.transform.GetChild(1).gameObject.SetActive(true);
                }

                currActiveTutIndex++;
            }
        }
    }

    void OnInteract()
    {
        
    }

    public void InteractSuccess()
    {
        if (walkFinished && !interactFinished)
        {
            interactFinished = true;
            interactTut.gameObject.SetActive(false);
            dropThrowTut.gameObject.SetActive(true);
            dropThrowTut.DisplayAccordingToScheme(inputScheme);

            foreach (GameObject ball in GameObject.FindGameObjectsWithTag("TutBalls"))
            {
                ball.transform.GetChild(1).gameObject.SetActive(false);
            }

            currActiveTutIndex++;
        }
    }

    void OnDash()
    {
        //if (!walkFinished || dashFinished)
        //{
        //    return;
        //}

        //dashCount -= 1;
        //if (dashCount <= 0)
        //{
        //    dashTut.SetActive(false);
        //    interactTut.SetActive(true);
        //    dashFinished = true;
        //}
    }

    void OnDrop()
    {
        
    }

    public void DropSuccess()
    {
        if (!dropFinished)
        {
            dropFinished = true;
            if (throwFinished)
            {
                dropThrowTut.gameObject.SetActive(false);

                currActiveTutIndex++;
                tutManager.CanStart();
            }
        }
    }

    void OnThrow()
    {
        
    }

    public void ThrowSuccess()
    {
        if (!throwFinished)
        {
            throwFinished = true;
            if (dropFinished)
            {
                dropThrowTut.gameObject.SetActive(false);

                currActiveTutIndex++;
                tutManager.CanStart();
            }
        }
    }

    void OnMenu()
    {
        
    }

    public void ChangeInputScheme(int schemeIndex)
    {
        inputScheme = schemeIndex;
        allTuts[currActiveTutIndex].DisplayAccordingToScheme(inputScheme);
    }

    private void GetAllTut()
    {
        moveTut = tutorialCanvas.transform.GetChild(0).gameObject.GetComponent<InstructionsControl>();
        interactTut = tutorialCanvas.transform.GetChild(1).gameObject.GetComponent<InstructionsControl>();
        dashTut = tutorialCanvas.transform.GetChild(2).gameObject.GetComponent<InstructionsControl>();
        dropThrowTut = tutorialCanvas.transform.GetChild(3).gameObject.GetComponent<InstructionsControl>();
        menuTut = tutorialCanvas.transform.GetChild(4).gameObject.GetComponent<InstructionsControl>();

        allTuts = new InstructionsControl[] { moveTut, interactTut, dropThrowTut, dashTut, menuTut };
    }
}
