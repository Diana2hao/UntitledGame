using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCanvas : MonoBehaviour
{
    public GameLevelEnd gEnd;

    public void TimeUp()
    {
        gEnd.LoadScoreScene();
    }
}
