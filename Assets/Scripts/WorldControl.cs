﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControl : MonoBehaviour
{
    public GameObject poacherPrefab;

    List<GameObject> birds;

    // Start is called before the first frame update
    void Start()
    {
        birds = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (birds.Count > 0)
        {
            //send poacher
            foreach(GameObject bird in birds)
            {
                if (bird.GetComponent<BirdAI>().IsFree)
                {
                    Vector3 initP = new Vector3(16f, 0f, bird.transform.position.z);
                    Vector3 destP = new Vector3(bird.transform.position.x + 3f, 0f, bird.transform.position.z);
                    GameObject poacher = Instantiate(poacherPrefab, initP, Quaternion.Euler(0, -90, 0));

                    poacher.GetComponent<PoacherAI>().StartWalking(initP, destP);

                    bird.GetComponent<BirdAI>().IsFree = false;
                }
            }
        }
    }

    public void AddBird(GameObject bird)
    {
        birds.Add(bird);
    }
}
