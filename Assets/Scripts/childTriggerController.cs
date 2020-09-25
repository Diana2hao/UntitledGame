using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childTriggerController : BucketController
{
    GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void glow()
    {
        parent.GetComponent<InteractableController>().glow();
    }

    public override void unglow()
    {
        parent.GetComponent<InteractableController>().unglow();
    }

    public override void OnPlayerInteract(GameObject player)
    {
        parent.GetComponent<InteractableController>().OnPlayerInteract(player);
    }

    public override void OnDrop()
    {
        parent.GetComponent<InteractableController>().OnDrop();
    }
}
