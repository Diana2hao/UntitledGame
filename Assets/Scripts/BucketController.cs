using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : InteractableController
{
    public ParticleSystem splashEffect;

    Renderer rd;
    int playerNum;
    GameObject childBucket;
    bool isFree;
    bool isFilled;

    public bool IsFilled { get => isFilled; set => isFilled = value; }

    // Start is called before the first frame update
    void Start()
    {
        //childBucket = transform.GetChild(0).gameObject;
        rd = this.GetComponent<Renderer>();
        playerNum = 0;
        isFree = true;
        isFilled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillWithWater()
    {
        if (!isFilled)
        {
            isFilled = true;
            rd.materials[1].SetColor("_BaseColor", new Color(0.184f, 0.678f, 0.953f, 0.816f));
        }
    }

    public void EmptyWater()
    {
        if (isFilled)
        {
            isFilled = false;
            splashEffect.Play();
            rd.materials[1].SetColor("_BaseColor", new Color(0.184f, 0.678f, 0.953f, 0.0f));
        }
    }

    public override void glow()
    {
        playerNum += 1;
        rd.material.SetColor("_EmissionColor", new Color(0.114f, 0.114f, 0.114f, 1.0f));
    }

    public override void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            rd.material.SetColor("_EmissionColor", new Color(0.001f, 0.001f, 0.001f, 1.0f));
        }
    }

    public override void OnPlayerInteract(GameObject player)
    {
        if(player.GetComponent<PlayerController>().Hold(this.gameObject, this.transform.GetChild(0).GetComponent<BoxCollider>(), Vector3.zero, Quaternion.identity))
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<BoxCollider>().enabled = false;
            this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override void OnDrop()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<BoxCollider>().enabled = true;
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
    }


    public override bool OnThrow(float throwForce)
    {
        if (!IsFilled)
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            this.GetComponent<BoxCollider>().enabled = true;
            this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            rb.AddForce(this.transform.parent.forward * throwForce);

            return true;
        }

        return false;
    }
}
