using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour, IInteractable
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
        Debug.Log("fill");
        if (!isFilled)
        {
            Debug.Log("fill success");
            isFilled = true;
            rd.materials[1].SetColor("_BaseColor", new Color(0.184f, 0.678f, 0.953f, 0.816f));
        }
    }

    public void EmptyWater()
    {
        Debug.Log("unfill");
        if (isFilled)
        {
            Debug.Log("unfill success");
            isFilled = false;
            splashEffect.Play();
            rd.materials[1].SetColor("_BaseColor", new Color(0.184f, 0.678f, 0.953f, 0.0f));
        }
    }

    public void glow()
    {
        playerNum += 1;
        rd.material.SetColor("_EmissionColor", new Color(0.114f, 0.114f, 0.114f, 1.0f));
    }

    public void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            rd.material.SetColor("_EmissionColor", new Color(0.001f, 0.001f, 0.001f, 1.0f));
        }
    }

    public void OnPlayerInteract(GameObject player)
    {
        if(player.GetComponent<PlayerController>().Hold(this.gameObject, this.transform.GetChild(0).GetComponent<BoxCollider>(), Vector3.zero, Quaternion.identity))
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<BoxCollider>().enabled = false;
            this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void OnDrop()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<BoxCollider>().enabled = true;
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
    }


    public bool OnThrow(float throwForce)
    {
        if (!IsFilled)
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            this.GetComponent<BoxCollider>().enabled = true;
            this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            rb.AddForce((this.transform.parent.forward + this.transform.parent.up).normalized * throwForce);

            return true;
        }

        return false;
    }
}
