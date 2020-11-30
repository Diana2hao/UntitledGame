using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childTriggerController : MonoBehaviour, IInteractable
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

    public void glow()
    {
        parent.GetComponent<IInteractable>().glow();
    }

    public void unglow()
    {
        parent.GetComponent<IInteractable>().unglow();
    }

    public void OnPlayerInteract(GameObject player)
    {
        parent.GetComponent<IInteractable>().OnPlayerInteract(player);
    }

    public void OnDrop()
    {
        parent.GetComponent<IInteractable>().OnDrop();
    }

    public bool OnThrow(float throwForce)
    {
        return false;
    }
}
