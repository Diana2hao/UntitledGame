using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public int TotalPlayerCount;

    public List<int> modelNumbers;
    public List<string> controlSchemes;
    public List<InputDevice> inputDevices;

    /*public struct PlayerSettings 
    { 
        public int modelNumber; 
        public string controlScheme; 
        public InputDevice inputDevice;

        public PlayerSettings(int modelNumber, string controlScheme, InputDevice inputDevice)
        {
            this.modelNumber = modelNumber;
            this.controlScheme = controlScheme;
            this.inputDevice = inputDevice;
        }
    }*/
    public List<PlayerSettings> playerSettings;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayer( int modelNumber, string controlScheme, InputDevice inputDevice)
    {
        PlayerSettings ps = new PlayerSettings(0, modelNumber, controlScheme, inputDevice);
        playerSettings.Add(ps);
    }

    public void LoadPlayer(PlayerController pc)
    {
        PlayerSettings ps = playerSettings[0];
        pc.curModel = ps.modelNumber;
        pc.pi.SwitchCurrentControlScheme(ps.controlScheme, ps.inputDevice);

        playerSettings.Remove(ps);
    }

}
