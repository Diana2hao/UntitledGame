using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BirdAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Animator anim;
    public float speed = 1.0f;
    
    public float spiralRadius;
    public float spiralFrequency;
    public float turnSampleFreq;
    Camera mainCamera;
    Vector3 targetRestPosition;
    GameObject targetTree;
    int restPosIdx;
    Vector3 unitDir;
    float currentHeight;
    bool isDeploying;
    Vector3 midDest;

    bool isFlyingAway;
    Vector3 finalDest;

    GameObject LC;

    
    public Vector3 TargetRestPosition { get => targetRestPosition; set => targetRestPosition = value; }
    public GameObject TargetTree { get => targetTree; set => targetTree = value; }
    public int RestPosIdx { get => restPosIdx; set => restPosIdx = value; }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        LC = GameObject.Find("LevelControl");
        Deploy();
        isFlyingAway = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Deploying
        if (isDeploying)
        {
            //if not at final destination, continue moving
            if(Vector3.Distance(transform.position, targetRestPosition) > 0.001f)
            {
                //if close to final destination, change state and animation
                if(Vector3.Distance(transform.position, targetRestPosition) < 3f)
                {
                    anim.SetInteger("State", (int)BirdTransition.IDLEONTREE);
                    float step = speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, targetRestPosition, step);
                    Vector3 dir = targetRestPosition - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), 0.3f);
                }
                else
                {
                    //move towards intermediate destination
                    if (Vector3.Distance(transform.position, midDest) > 0.001f)
                    {
                        float step = speed * Time.deltaTime; // calculate distance to move
                        transform.position = Vector3.MoveTowards(transform.position, midDest, step);
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(midDest - transform.position), 0.05f);
                    }
                    else
                    {
                        //get new intermediate spiral destination
                        currentHeight -= turnSampleFreq;
                        if (currentHeight < 0f) { currentHeight = 0f; }
                        midDest = GetSpiralPosition(currentHeight);
                    }
                }
            }
            //if at final destination, stop moving and inform level control
            else
            {
                isDeploying = false;
                LC.GetComponent<WorldControl>().AddBird(this.gameObject);
            }
            
        }

        //Flying Away
        if (isFlyingAway)
        {
            if (Vector3.Distance(transform.position, finalDest) > 0.001f)
            {
                anim.SetInteger("State", (int)BirdTransition.FLY);
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, finalDest, step);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(finalDest - transform.position), 0.05f);
            }
            else
            {
                //destroy
                Destroy(this.gameObject);
            }
                
        }
    }

    void Deploy()
    {
        unitDir = (mainCamera.transform.position - targetRestPosition).normalized;
        currentHeight = (mainCamera.transform.position - targetRestPosition).y;
        Vector3 initP = GetSpiralPosition(currentHeight);
        this.transform.position = initP;
        isDeploying = true;
        currentHeight -= turnSampleFreq;
        midDest = GetSpiralPosition(currentHeight);
    }

    Vector3 GetSpiralPosition(float height)
    {
        //calculate the spiral position of current height
        Vector3 point = new Vector3(height * (spiralRadius * Mathf.Cos(spiralFrequency * height) + unitDir.x / unitDir.y), height, height * (spiralRadius * Mathf.Sin(spiralFrequency * height) + unitDir.z / unitDir.y)) + targetRestPosition;
        return point;
    }

    public void FlyAway()
    {
        isDeploying = false;
        finalDest = (transform.position - targetTree.transform.position) + transform.position;
        isFlyingAway = true;
    }
}
