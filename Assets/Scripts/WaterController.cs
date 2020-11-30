using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerInteract(GameObject player)
    {
        Debug.Log("try interact water");
        GameObject handheld = player.GetComponent<PlayerController>().CurrentHandheldObject;
        if (handheld.CompareTag("Bucket"))
        {
            Debug.Log("try fill water");
            handheld.GetComponent<BucketController>().FillWithWater();
        }
    }

    public void glow()
    {
        
    }

    public void unglow()
    {
        
    }

    public void OnDrop()
    {
        
    }

    public bool OnThrow(float throwForce)
    {
        return false;
    }
}
