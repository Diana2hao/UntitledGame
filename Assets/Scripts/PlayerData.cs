using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class PlayerData
{
    public static bool gameStarted = false;
    public static bool isPlayingCutscene = false;
    public static string mainPlayerControlScheme;  //"KeyboardAll" OR "Gamepad"

    public static int playerCount;
    public static List<PlayerSettings> AllPlayers = new List<PlayerSettings>();
    
    public static Dictionary<string, int> levelHighestPercentage = new Dictionary<string, int>();
    public static string lastFinishedLevel;  //"0_0" etc.
    public static List<int> lastGameScores = new List<int>();
    public static bool backFromScore = false;

    public static void AddPlayer(int index, int modelNumber, string controlScheme, InputDevice inputDevice)
    {
        PlayerSettings ps = new PlayerSettings(index, modelNumber, controlScheme, inputDevice);
        AllPlayers.Add(ps);
    }

    public static void ResetPlayers()
    {
        AllPlayers = new List<PlayerSettings>();
    }
    
    public static void UpdateLastLevelScore(List<int> scores)
    {
        lastGameScores = scores;
    }

    public static void UpdateLevelPercentage(string levelKey, int percent)
    {
        try
        {
            if (percent > levelHighestPercentage[levelKey])
            {
                levelHighestPercentage[levelKey] = percent;
            }
        }
        catch (KeyNotFoundException)
        {
            levelHighestPercentage[levelKey] = percent;
        }
    }

    public static int GetLevelPercentage(string levelKey)
    {
        try
        {
            return levelHighestPercentage[levelKey];
        }
        catch (KeyNotFoundException)
        {
            return 0;
        }
    }
}
