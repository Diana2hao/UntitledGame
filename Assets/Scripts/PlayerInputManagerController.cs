using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManagerController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject playerPrefab;

    PlayerInputManager pim;
    int playerNum;
    int maxPlayerPerKeyboard = 2;
    int maxPlayer = 4;

    List<GameObject> players;

    public List<GameObject> Players { get => players; set => players = value; }
    public int PlayerNum { get => playerNum; set => playerNum = value; }

    // Start is called before the first frame update
    void Start()
    {
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
        //if (PlayerNum == 1)
        //{
        //    GameObject mainPlayer = GameObject.Find("Player");
        //    Players.Add(mainPlayer);
        //}

    }

    void OnPlayerLeft()
    {

    }

    void JoinKeyboardPlayer(GameObject player)
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
        }
        
    }
}
