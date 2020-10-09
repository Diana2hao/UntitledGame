using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusController : InteractableController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void glow()
    {

    }

    public override void unglow()
    {

    }


    public override void OnPlayerInteract(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();

        if (pc.Water())
        {
            waterCactus(player);
        }
    }

    private void waterCactus(GameObject player)
    {
        if (this.transform.parent.CompareTag("Poacher"))
        {
            Debug.Log("poacher");
            transform.parent.GetComponent<PoacherAI>().GetWatered(player);
        }
        else
        {
            Debug.Log("normal cactus");
        }
    }

}
