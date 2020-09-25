﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerBagController : InteractableController
{
    public MeshRenderer rd;
    public Vector3 holdPositionOffset;
    public Quaternion holdRotationOffset;

    int playerNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopOutOfStation()
    {
        DeactivateRigidbody();
    }

    public override void glow()
    {
        playerNum += 1;
        rd.material.EnableKeyword("_EMISSION");
    }

    public override void unglow()
    {
        playerNum -= 1;
        if (playerNum == 0)
        {
            rd.material.DisableKeyword("_EMISSION");
        }
    }

    public override void OnPlayerInteract(GameObject player)
    {
        if (player.GetComponent<PlayerController>().Hold(this.gameObject, this.transform.GetChild(0).GetComponent<BoxCollider>(), holdPositionOffset, holdRotationOffset))
        {
            DeactivateRigidbody();
        }
    }

    public override void OnDrop()
    {
        DeactivateRigidbody(true);
    }

    private void DeactivateRigidbody(bool isInverse = false)
    {
        this.GetComponent<Rigidbody>().isKinematic = !isInverse;
        this.GetComponent<BoxCollider>().enabled = isInverse;
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = isInverse;
    }
}
