using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsControl : MonoBehaviour
{
    List<GameObject> GrownTrees;
    public GameObject birdPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GrownTrees = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGrownTree(GameObject tree)
    {
        GrownTrees.Add(tree);
        DeployBird(tree);
    }

    void DeployBird(GameObject tree)
    {
        Vector3 birdInitP = new Vector3(-16f, 3.6f, tree.transform.position.z);
        Vector3 birdDestP = new Vector3(tree.transform.position.x, 3.6f, tree.transform.position.z);
        GameObject bird = Instantiate(birdPrefab, birdInitP, Quaternion.Euler(0, 0, 0));

        bird.GetComponent<BirdAI>().StartFlying(birdDestP);
    }
}
