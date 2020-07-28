using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : InteractableController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract(GameObject player)
    {
        GameObject handheld = player.GetComponent<PlayerController>().CurrentHandheldObject;
        if (handheld.CompareTag("Bucket"))
        {
            handheld.GetComponent<BucketController>().FillWithWater();
        }
    }

}
