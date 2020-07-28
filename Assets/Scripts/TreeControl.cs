using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeControl : InteractableController
{
    GameObject Tree0;
    GameObject Tree1;
    GameObject Tree2;
    public bool isTarget;
    public bool isCutting;
    public GameObject TransparentFinalModel;
    public int FinalSize;
    public WaterBar wBar;
    public Canvas canvas;
    public BirdsControl birdsControl;

    int growCount;
    int curTree;
    GameObject[] treeArray;
    int[] sizeArray;
    float[] barHeights;
    BoxCollider bc;

    List<GameObject> interactPlayers;
    bool farmerTrig;
    int playerNum;
    bool isPlanted;
    bool isFullyGrown;

    Renderer[] rdArray;

    public bool IsPlanted { get => isPlanted; set => isPlanted = value; }
    public bool IsFullyGrown { get => isFullyGrown; set => isFullyGrown = value; }


    // Start is called before the first frame update
    void Start()
    {
        growCount = 0;
        curTree = 0;
        interactPlayers = new List<GameObject>();
        playerNum = 0;
        isPlanted = false;
        isFullyGrown = false;

        Tree0 = transform.GetChild(0).gameObject;
        Tree1 = transform.GetChild(1).gameObject;
        Tree2 = transform.GetChild(2).gameObject;

        treeArray = new GameObject[] { Tree0, Tree1, Tree2 };
        sizeArray = new int[] { 2, 4, 5 };
        barHeights = new float[] { 1.5f, 2.5f, 4.5f };

        Tree0.SetActive(true);
        Tree1.SetActive(false);
        Tree2.SetActive(false);

        bc = gameObject.GetComponent<BoxCollider>();
        buildCollider(sizeArray[0]);

        Renderer rd0 = Tree0.GetComponent<Renderer>();
        Renderer rd1 = Tree1.GetComponent<Renderer>();
        Renderer rd2 = Tree2.GetComponent<Renderer>();

        rdArray = new Renderer[] { rd0, rd1, rd2 };
    }

    // Update is called once per frame
    void Update()
    {
        if (interactPlayers.Count > 0)
        {
            if(Input.GetKeyDown(KeyCode.X) && curTree < 2)
            {
                waterTree();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
        if (other.CompareTag("Farmer"))
        {
            farmerTrig = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
        if (other.CompareTag("Farmer"))
        {
            farmerTrig = false;
        }
    }

    private void buildCollider(int size)
    {
        bc.center = new Vector3(0, size/2, 0);
        bc.size = new Vector3(size, size, size);
    }

    private void waterTree()
    {
        growCount += 1;
        wBar.SetWaterCount(growCount);
        if (growCount >= 3 && curTree < 2)
        {
            treeArray[curTree].SetActive(false);

            curTree += 1;
            treeArray[curTree].SetActive(true);
            canvas.transform.position = new Vector3(transform.position.x, barHeights[curTree], transform.position.z);
            wBar.SetWaterCount(0);

            //glow();
            rdArray[curTree].material.SetColor("_EmissionColor", new Color(0.35f, 0.35f, 0.35f, 1.0f));
            buildCollider(sizeArray[curTree]);
            growCount = 0;

            if (curTree == 2)
            {

                isFullyGrown = true;
                wBar.gameObject.SetActive(false);
                birdsControl.AddGrownTree(this.gameObject);
            }
        }
        
    }

    public override void glow()
    {
        playerNum += 1;
        rdArray[curTree].material.SetColor("_EmissionColor", new Color(0.35f, 0.35f, 0.35f, 1.0f));
    }

    public override void unglow()
    {
        playerNum -= 1;
        if(playerNum == 0)
        {
            rdArray[curTree].material.SetColor("_EmissionColor", new Color(0.001f, 0.001f, 0.001f, 1.0f));
        }
    }

    public override void OnPlayerInteract(GameObject player)
    {
        if (!isPlanted)
        {
            if(player.GetComponent<PlayerController>().Hold(this.gameObject, this.GetComponent<BoxCollider>()))
            {
                PickUp(true);
            }
        }
        else
        {
            if (player.GetComponent<PlayerController>().Water())
            {
                waterTree();
            }
        }

    }

    public override void OnDrop()
    {
        PickUp(false);
    }

    private void PickUp(bool up)
    {
        this.GetComponent<Rigidbody>().isKinematic = up;
        this.GetComponent<BoxCollider>().enabled = !up;
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = !up;
    }
}
