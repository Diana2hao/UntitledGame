using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void glow();
    void unglow();
    void OnPlayerInteract(GameObject player);
    void OnDrop();
    bool OnThrow(float throwForce);
    
}
