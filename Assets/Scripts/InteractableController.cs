using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public virtual void glow() { }
    public virtual void unglow() { }
    public virtual void OnPlayerInteract(GameObject player) { }
    public virtual void OnDrop() { }
    public virtual bool OnThrow(float throwForce) { return false; }
    
}
