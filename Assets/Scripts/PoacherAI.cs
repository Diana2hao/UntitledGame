using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherAI : MonoBehaviour
{
    public float speed = 3f;
    public GameObject trapPrefab;
    public Animator anim;
    public ParticleSystem smokeEffect;
    public WorldControl WC;
    GridController gridCon;
    BirdsControl bc;

    bool hasTarget;
    GameObject trap;
    TrapController trapCon;
    Vector3 currTargetDest;
    Vector3 trapPosition;
    Vector3 retrieveTrapPosition;
    Vector3 hidePosition;
    bool canCapture;

    //transformation related
    GameObject pModel;
    GameObject[] cacti;
    int cactusIdx;

    public bool HasTarget { get => hasTarget; set => hasTarget = value; }
    public Vector3 CurrTargetDest { get => currTargetDest; set => currTargetDest = value; }
    public Vector3 TrapPosition { get => trapPosition; set => trapPosition = value; }
    public Vector3 HidePosition { get => hidePosition; set => hidePosition = value; }
    public bool CanCapture { get => canCapture; set => canCapture = value; }
    public Vector3 RetrieveTrapPosition { get => retrieveTrapPosition; set => retrieveTrapPosition = value; }
    public TrapController TrapCon { get => trapCon; set => trapCon = value; }

    // Start is called before the first frame update
    void Start()
    {
        bc = GameObject.Find("BirdsControl").GetComponent<BirdsControl>();
        gridCon = GameObject.Find("Grid").GetComponent<GridController>();

        pModel = transform.GetChild(1).gameObject;
        cacti = new GameObject[] { transform.GetChild(2).gameObject, transform.GetChild(3).gameObject };
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasTarget && bc.hasOccupiedTrees())
        {
            if(bc.FindTargetTreeForPoacher(out trapPosition, out currTargetDest, out hidePosition))
            {
                hasTarget = true;
                retrieveTrapPosition = currTargetDest + (hidePosition - currTargetDest) * 0.2f; //when trap is dropped on ground, need some extra space for poacher to stand
            }
        }
        else
        {
            //TODO: check if the three positions are still empty and change behaviour accordingly

        }
    }

    public void SetTrap()
    {
        //set trap with correct position and rotation (grid based)
        Quaternion trapRotation = Quaternion.Euler(-90, 90 + transform.rotation.eulerAngles.y, 0);
        trap = Instantiate(trapPrefab, trapPosition, trapRotation);
        trapCon = trap.GetComponent<TrapController>();
        trapCon.PAI = this;
        bc.AddTrap(trapCon);
        gridCon.AddGameObjectOfScale(trapPosition, trap, 1);
    }

    public void FindHidePosition()
    {
        //TODO: what should poacher do when hide position is occupied during the action
        currTargetDest = hidePosition;
    }

    public void Camouflage()
    {
        //turn into a cactus with smoke effect
        smokeEffect.Play();
        pModel.SetActive(false);
        cactusIdx = Random.Range(0, 2);
        cacti[cactusIdx].SetActive(true);
        gridCon.AddGameObjectOfScale(this.transform.position, this.gameObject, 1);
    }

    public void DeCamouflage()
    {
        //turn back to human mode with some smoke effect
        smokeEffect.Play();
        pModel.SetActive(true);
        cacti[cactusIdx].SetActive(false);
        gridCon.RemoveGameObjectOfScale(this.gameObject, 1);
    }

    public void GetTrapAndBird()
    {
        //recycle(destroy) trap and capture bird
        trapCon.dustEffect.Play();

        List<GameObject> birdsInTrap = trapCon.BirdsInTrap;
        foreach(GameObject bird in birdsInTrap)
        {
            bc.RemoveABird(bird);
            Destroy(bird);
        }
        gridCon.RemoveGameObjectOfScale(trap, 1);
        Destroy(trap);

        hasTarget = false;
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
