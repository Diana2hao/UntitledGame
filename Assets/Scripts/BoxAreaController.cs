using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAreaController : MonoBehaviour
{
    int numPlayerWPInside;
    List<GameObject> playerInsideWithPlant;
    bool isPlanted;

    // Start is called before the first frame update
    void Start()
    {
        numPlayerWPInside = 0;
        playerInsideWithPlant = new List<GameObject>();
        isPlanted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (numPlayerWPInside > 0)
        {
            foreach(GameObject player in playerInsideWithPlant)
            {
                //calculate if center position inside
                if (CenterIsInside(player))
                {
                    if (isPlanted)
                    {
                        //light up red

                    }
                    else
                    {
                        //light up blue

                    }
                    

                }
            }
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().HasPlant)
            {
                numPlayerWPInside += 1;
                playerInsideWithPlant.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().HasPlant)
            {
                numPlayerWPInside -= 1;
                playerInsideWithPlant.Remove(other.gameObject);
            }
        }
    }

    private bool CenterIsInside(GameObject player)
    {
        //Todo: calculate
        return true;
    }
}
