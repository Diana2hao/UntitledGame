using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManagerController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject playerPrefab;

    public StartScreen startScreen;
    public Animator spawnAnim;

    public GameObject PlayerTutPrefab;
    public TutorialManager tutorialManager;

    PlayerInputManager pim;
    public static int PlayerNum;
    int maxPlayerPerKeyboard = 2;
    int maxPlayer = 4;

    List<GameObject> players;

    public List<GameObject> Players { get => players; set => players = value; }
    //public int PlayerNum { get => playerNum; set => playerNum = value; }

    // Start is called before the first frame update
    void Start()
    {
        PlayerNum = 0;
        pim = GetComponent<PlayerInputManager>();
        Players = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerJoined(PlayerInput pi)
    {
        PlayerNum++;
        pi.gameObject.transform.position = spawnPoint.transform.position;
        spawnAnim.SetTrigger("playerJoined");

        GameObject playerTut = Instantiate(PlayerTutPrefab);
        playerTut.GetComponent<FollowPlayer>().playerToFollow = pi.gameObject.transform;
        pi.gameObject.GetComponent<PlayerTutorialControl>().tutorialCanvas = playerTut;

        tutorialManager.AddTutCanvas(playerTut);

        //first Player joined
        if (PlayerNum == 1)
        {
            startScreen.StartGame();
            PlayerData.gameStarted = true;
            PlayerData.mainPlayerControlScheme = pi.currentControlScheme;
        }

        if (pi.currentControlScheme == "Gamepad")
        {
            //playerTut.GetComponent<InstructionsControl>().UseGamepad();
            pi.gameObject.GetComponent<PlayerTutorialControl>().InputScheme = (int)MyInputScheme.GAMEPAD;
        }

        //if (PlayerNum == 1)
        //{
        //    GameObject mainPlayer = GameObject.Find("Player");
        //    Players.Add(mainPlayer);
        //}
    }

    void OnPlayerLeft()
    {

    }

    public void JoinKeyboardPlayer(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc.KeyboardShared == false && PlayerNum < maxPlayer)
        {
            pc.KeyboardShared = true;
            pc.pi.SwitchCurrentControlScheme("KeyboardLeft", pc.pi.devices.ToArray());
            var p2 = PlayerInput.Instantiate(playerPrefab, controlScheme: "KeyboardRight", pairWithDevice: Keyboard.current);
            GameObject p2g = p2.gameObject;
            p2g.transform.position = spawnPoint.transform.position;
            p2g.GetComponent<PlayerController>().KeyboardShared = true;
            //Players.Add(p2g);

            //tutorial related
            pc.GetComponent<PlayerTutorialControl>().ChangeInputScheme((int)MyInputScheme.KEYBOARDLEFT);
            p2g.GetComponent<PlayerTutorialControl>().InputScheme = (int)MyInputScheme.KEYBOARDRIGHT;
        }
        
    }
    
}
