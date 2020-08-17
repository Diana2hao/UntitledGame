using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : MonoBehaviour
{
    public Vector3 startTree;
    public Vector3 endCamera;
    public GameObject point;

    public float radius;
    public float frequency;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 unitDir = (endCamera - startTree).normalized;

        float i = 0f;
        while(i <= (endCamera - startTree).y)
        {
            Vector3 p = new Vector3(i*(radius*Mathf.Cos(frequency*i)+unitDir.x/unitDir.y), i, i*(radius*Mathf.Sin(frequency*i) + unitDir.z / unitDir.y)) + startTree;

            GameObject aPoint = Instantiate(point, p, Quaternion.Euler(0, 0, 0));
            i+=0.5f;
        }

    }

    // Update is called once per frame
    void Update()
    { 
        
    }
}
