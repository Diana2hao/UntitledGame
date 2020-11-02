using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectPolybrushRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        correctRotation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void correctRotation()
    {
        foreach (Transform child in this.transform)
        {
            child.rotation = Quaternion.identity;
        }
    }
}
