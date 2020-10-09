using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDistributorController : InteractableController
{
    public GameObject currentPlant;

    Renderer rd;
    int playerNum;

    bool plantReady;

    // Start is called before the first frame update
    void Start()
    {
        rd = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        playerNum = 0;
        plantReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {

    }

    public override void glow()
    {
        playerNum += 1;
        rd.materials[0].EnableKeyword("_EMISSION");
    }

    public override void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            rd.materials[0].DisableKeyword("_EMISSION");
        }
    }

    public override void OnPlayerInteract(GameObject player)
    {
        if (player.GetComponent<PlayerController>().CurrentHandheldObject == null && plantReady)
        {
            GameObject tree = Instantiate(currentPlant, transform.position, Quaternion.Euler(0, 0, 0));

            tree.GetComponent<Rigidbody>().isKinematic = true;
            tree.GetComponent<BoxCollider>().enabled = false;
            tree.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;

            player.GetComponent<PlayerController>().Hold(tree, this.transform.GetChild(0).GetComponent<BoxCollider>(), Vector3.zero, Quaternion.identity);
        }
    }

}
