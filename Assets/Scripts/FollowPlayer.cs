using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerToFollow;
    public Vector3 positionOffset;

    PlayerController playerCon;

    // Start is called before the first frame update
    void Start()
    {
        playerCon = playerToFollow.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = playerToFollow.position + positionOffset;
    }
}
