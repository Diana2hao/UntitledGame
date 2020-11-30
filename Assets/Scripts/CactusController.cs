using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusController : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void glow()
    {

    }

    public void unglow()
    {

    }


    public void OnPlayerInteract(GameObject player)
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
            transform.parent.GetComponent<PoacherAI>().GetWatered(player);
        }
    }

    public void OnDrop()
    {
        
    }

    public bool OnThrow(float throwForce)
    {
        return false;
    }
}
