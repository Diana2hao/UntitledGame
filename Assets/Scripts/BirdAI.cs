using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BirdAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Animator anim;
    public float speed = 1.0f;
    public ParticleSystem poopEffect;
    public GameObject poopPrefab;

    public float spiralRadius;
    public float spiralFrequency;
    public float turnSampleFreq;
    Camera mainCamera;
    Vector3 targetRestPosition;
    Quaternion restRotation;
    float angleOffset;

    GrownTree targetTree;
    int restPosIdx;
    Vector3 unitDir;
    float currentHeight;
    bool isDeploying;
    Vector3 midDest;

    Vector3 currTarget;

    bool isFlyingAway;
    Vector3 finalDest;

    bool isAttractedToTrap;
    TrapController trap;
    Vector3 trapDest;

    bool isReturningToTree;

    PoopSplatter poop = null;
    public bool canPoop = true;

    GridController gridCon;

    public GameObject levelCon;

    
    public Vector3 TargetRestPosition { get => targetRestPosition; set => targetRestPosition = value; }
    public Quaternion RestRotation { get => restRotation; set => restRotation = value; }
    public GrownTree TargetTree { get => targetTree; set => targetTree = value; }
    public int RestPosIdx { get => restPosIdx; set => restPosIdx = value; }
    public bool IsAttractedToTrap { get => isAttractedToTrap; set => isAttractedToTrap = value; }
    public TrapController Trap { get => trap; set => trap = value; }
    public Vector3 TrapDest { get => trapDest; set => trapDest = value; }
    public bool IsFlyingAway { get => isFlyingAway; set => isFlyingAway = value; }
    public bool IsDeploying { get => isDeploying; set => isDeploying = value; }
    public Vector3 CurrTarget { get => currTarget; set => currTarget = value; }
    public bool CanPoop { get => canPoop; set => canPoop = value; }
    public bool IsReturningToTree { get => isReturningToTree; set => isReturningToTree = value; }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        levelCon = GameObject.FindGameObjectWithTag("LevelControl");
        Debug.Log(levelCon);
        Deploy();
        isFlyingAway = false;

        gridCon = GameObject.Find("Grid").GetComponent<GridController>();
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
                //if very close to final destination, change movement and rotation calculation
                //if(Vector3.Distance(transform.position, targetRestPosition) < 0.5f)
                //{
                //    //anim.SetInteger("State", (int)BirdTransition.IDLEONTREE);
                //    float step = speed * Time.deltaTime; // calculate distance to move
                //    transform.position = Vector3.MoveTowards(transform.position, targetRestPosition, step);
                //    //Vector3 dir = targetRestPosition - transform.position;
                //    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), 0.3f);
                //    transform.rotation = Quaternion.Slerp(transform.rotation, RestRotation, 0.1f);
                //}
                //else
                //{
                    //if near final dest, start landing animation
                    if (Vector3.Distance(transform.position, targetRestPosition) < 2f)
                    {
                        anim.SetInteger("State", (int)BirdTransition.IDLEONTREE);
                    }

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
                //}
            }
            //if at final destination, stop moving and inform level control
            else
            {
                StartCoroutine("CorrectRotation");
                isDeploying = false;
                levelCon.GetComponent<LevelControl>().AddBird();
            }
        }
        
        //if (poop != null)
        //{
        //    canPoop = !poop.IsVacuuming;
        //}

        //Flying Away
        //if (isFlyingAway)
        //{
        //    if (Vector3.Distance(transform.position, finalDest) > 0.001f)
        //    {
        //        anim.SetInteger("State", (int)BirdTransition.FLY);
        //        float step = speed * Time.deltaTime; // calculate distance to move
        //        transform.position = Vector3.MoveTowards(transform.position, finalDest, step);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(finalDest - transform.position), 0.05f);
        //    }
        //    else
        //    {
        //        //destroy
        //        Destroy(this.gameObject);
        //    }

        //}
    }

    void Deploy()
    {
        unitDir = (mainCamera.transform.position - targetRestPosition).normalized;
        angleOffset = restRotation.eulerAngles.y;
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
        Vector3 point = new Vector3(height * (spiralRadius * Mathf.Cos(spiralFrequency * height + angleOffset) + unitDir.x / unitDir.y), height, height * (spiralRadius * Mathf.Sin(spiralFrequency * height + angleOffset) + unitDir.z / unitDir.y)) + targetRestPosition;
        return point;
    }

    public void FlyAway()
    {
        isDeploying = false;
        currTarget = (transform.position - targetTree.tree.transform.position) + transform.position;
        isFlyingAway = true;
    }

    public void AddTrap(TrapController aTrap, Vector3 randDest)
    {
        //IEnumerator coroutine = WaitRandomTime(trap, randDest);
        //StartCoroutine(coroutine);
        isAttractedToTrap = true;
        Trap = aTrap;
        trapDest = randDest;
    }

    IEnumerator WaitRandomTime(TrapController trap, Vector3 randDest)
    {
        float waitTime = Random.Range(3f, 6f);
        yield return new WaitForSeconds(waitTime);
        isAttractedToTrap = true;
        Trap = trap;
        trapDest = randDest;
    }

    IEnumerator CorrectRotation()
    {
        Debug.Log("correcting");
        transform.rotation = Quaternion.Slerp(transform.rotation, RestRotation, 0.1f);
        while (Mathf.Abs(Quaternion.Dot(transform.rotation, RestRotation)) < 0.999f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, RestRotation, 0.1f);
            yield return null;
        }
    }

    public void ReturnToTree()
    {
        isReturningToTree = true;
        currTarget = targetRestPosition;
        anim.SetInteger("State", (int)BirdTransition.FLY);
    }

    public void Poop()
    {
        if (poop == null)
        {
            Vector3 poopPosition = gridCon.GetGridCenterOfPosition(this.transform.position);
            poopPosition += poopPrefab.transform.position;
            GameObject go = Instantiate(poopPrefab, poopPosition, poopPrefab.transform.rotation);
            poop = go.GetComponent<PoopSplatter>();
            poop.AddBird(this.gameObject);
        }
        poopEffect.Play();
    }

    public void PoopToBeDestroyed()
    {
        poop.CanBeDestroyed = true;
    }

    public void ReducePlayerPoints()
    {
        levelCon.GetComponent<LevelControl>().MinusBird();
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log(other);
        //if (poop == null)
        //{
        //    Vector3 poopPosition = gridCon.GetGridCenterOfPosition(this.transform.position);
        //    poopPosition += poopPrefab.transform.position;
        //    GameObject go = Instantiate(poopPrefab, poopPosition, poopPrefab.transform.rotation);
        //    poop = go.GetComponent<PoopSplatter>();
        //    poop.BAI = this;
        //    poop.AddPoop();
        //}
    }
    
}
