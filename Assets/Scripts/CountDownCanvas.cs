using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownCanvas : MonoBehaviour
{
    public GameLevelStarter gStart;

    public void CountdownFinished()
    {
        gStart.StartLevel();
    }
}
