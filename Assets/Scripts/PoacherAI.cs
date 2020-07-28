using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherAI : MonoBehaviour
{
    public float speed = 3f;
    public GameObject trapPrefab;
    public Animator anim;
    public WorldControl WC;

    bool isWalking = false;
    Vector3 initP;
    Vector3 destP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            anim.SetBool("isWalking", true);
            if (Vector3.Distance(transform.position, destP) > 0.001f)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, destP, step);
            }
            else
            {
                StartCoroutine(WaitForTrap());
            }
        }

        
    }

    public void StartWalking(Vector3 init, Vector3 dest)
    {
        isWalking = true;
        initP = init;
        destP = dest;
    }

    IEnumerator WaitForTrap()
    {
        isWalking = false;
        GameObject trap = Instantiate(trapPrefab, transform.position + new Vector3(-1f, 0f, 0f), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(3);
        isWalking = true;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        destP = initP;
    }
}
