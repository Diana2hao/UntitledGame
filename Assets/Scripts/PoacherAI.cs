using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherAI : MonoBehaviour
{
    public float speed = 3f;
    public GameObject trapPrefab;
    public Animator anim;
    public WorldControl WC;

    bool hasTarget;
    Vector3 currTargetDest;

    public Vector3 CurrTargetDest { get => currTargetDest; set => currTargetDest = value; }
    public bool HasTarget { get => hasTarget; set => hasTarget = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTrap()
    {
        //TODO: get trap position and rotation right(grid based)
        Vector3 trapPos = transform.position + new Vector3(-1f, 0f, 0f);
        Quaternion trapRotation = Quaternion.Euler(0, 0, 0);
        GameObject trap = Instantiate(trapPrefab, trapPos, trapRotation);
    }

    public void FindHidePosition()
    {
        //TODO: find a grid based destination for poacher to hide
        //currTargetDest = ...;
    }

    public void Camouflage()
    {
        //TODO: turn into a cactus with some smoke effect

    }

    public void DeCamouflage()
    {
        //TODO: turn back to human mode with some smoke effect

    }

    public void GetTrapAndBird()
    {
        //TODO: recycle trap and capture bird

    }

    //public void StartWalking(Vector3 init, Vector3 dest)
    //{
    //    isWalking = true;
    //    initP = init;
    //    destP = dest;
    //}

    //IEnumerator WaitForTrap()
    //{
    //    isWalking = false;
    //    GameObject trap = Instantiate(trapPrefab, transform.position + new Vector3(-1f, 0f, 0f), Quaternion.Euler(0, 0, 0));
    //    yield return new WaitForSeconds(3);
    //    isWalking = true;
    //    transform.rotation = Quaternion.Euler(0, 90, 0);
    //    destP = initP;
    //}
}
