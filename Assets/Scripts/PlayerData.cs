using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class PlayerData
{
    public static int playerCount;

    public static List<PlayerSettings> AllPlayers = new List<PlayerSettings>();

    public static void AddPlayer(int index, int modelNumber, string controlScheme, InputDevice inputDevice)
    {
        PlayerSettings ps = new PlayerSettings(index, modelNumber, controlScheme, inputDevice);
        AllPlayers.Add(ps);
    }
    
}
