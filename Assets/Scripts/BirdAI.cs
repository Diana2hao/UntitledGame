using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAI : MonoBehaviour
{
    public float speed = 1.0f;
    GameObject WC;

    bool isFlying = false;
    bool isFree = false;
    Vector3 destP;

    public Vector3 DestP { get => destP; set => destP = value; }
    public bool IsFlying { get => isFlying; set => isFlying = value; }
    public bool IsFree { get => isFree; set => isFree = value; }

    // Start is called before the first frame update
    void Start()
    {
        WC = GameObject.Find("WorldControl");
    }

    // Update is called once per frame
    void Update()
    {
        // 
        if (isFlying)
        {
            if (Vector3.Distance(transform.position, destP) > 0.001f)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, destP, step);
            }
            else
            {
                isFlying = false;
                isFree = true;
                WC.GetComponent<WorldControl>().AddBird(this.gameObject);
            }
            
        }
    }

    public void StartFlying(Vector3 dest)
    {
        isFlying = true;
        destP = dest;
    }
}
