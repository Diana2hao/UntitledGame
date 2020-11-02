using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float minX;
    public float maxX;
    public Transform following;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = following.position.x;

        if (following.position.x > maxX)
        {
            x = maxX;
        }
        if (following.position.x < minX)
        {
            x = minX;
        }

        this.transform.position = new Vector3(x, this.transform.position.y, this.transform.position.z);
        

    }
}
