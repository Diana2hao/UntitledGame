using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPickUpController : MonoBehaviour, IInteractable
{
    Renderer rd;
    int playerNum;
    bool isFree;

    GameObject currPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //childBucket = transform.GetChild(0).gameObject;
        rd = this.GetComponent<Renderer>();
        playerNum = 0;
        isFree = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void glow()
    {
        playerNum += 1;
        rd.material.EnableKeyword("_EMISSION");
    }

    public void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            rd.material.DisableKeyword("_EMISSION");
        }
    }

    public void OnPlayerInteract(GameObject player)
    {
        if (isFree)
        {
            isFree = false;
            if (player.GetComponent<PlayerController>().Hold(this.gameObject, this.transform.GetChild(0).GetComponent<Collider>(), Vector3.zero, Quaternion.identity))
            {
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Collider>().enabled = false;
                this.transform.GetChild(0).GetComponent<Collider>().enabled = false;

                //tutorial related
                player.GetComponent<PlayerTutorialControl>().InteractSuccess();
                currPlayer = player;
            }
            else
            {
                isFree = true;
            }
        }
        else
        {
            player.GetComponent<PlayerController>().FailToHold(this.transform.GetChild(0).GetComponent<Collider>());
        }
    }

    public void OnDrop()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Collider>().enabled = true;
        this.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        isFree = true;

        //tutorial related
        currPlayer.GetComponent<PlayerTutorialControl>().DropSuccess();
    }


    public bool OnThrow(float throwForce)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        this.GetComponent<Collider>().enabled = true;
        this.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        rb.AddForce((this.transform.parent.forward + this.transform.parent.up).normalized * throwForce);
        isFree = true;

        //tutorial related
        currPlayer.GetComponent<PlayerTutorialControl>().ThrowSuccess();

        return true;
    }
}
