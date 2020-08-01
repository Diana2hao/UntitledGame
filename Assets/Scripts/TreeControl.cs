using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeControl : InteractableController
{
    //3 growth stage models (childs) plus one indicator model
    GameObject Tree0;
    GameObject Tree1;
    GameObject Tree2;
    GameObject[] treeArray;
    public GameObject TransparentFinalModel;

    //enemy interaction related
    public bool isTarget;
    public bool isCutting;

    //health related
    public WaterBar wBar;
    public Canvas canvas;
    public int initialHealth;
    public int waterPerGrowth;
    int maxHealth;
    int healthPerWater;
    int currentHealth;

    //model change related
    public int FinalSize;
    int growCount;
    int curTree;
    int[] trigColliderSizeArray;
    BoxCollider bc;
    float[] barHeights;
    Renderer[] rdArray;

    //player interaction related
    List<GameObject> interactPlayers;
    int playerNum;
    bool isPlanted;
    bool isFullyGrown;
    public BirdsControl birdsControl;


    public bool IsPlanted { get => isPlanted; set => isPlanted = value; }
    public bool IsFullyGrown { get => isFullyGrown; set => isFullyGrown = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int HealthPerWater { get => healthPerWater; set => healthPerWater = value; }


    // Start is called before the first frame update
    void Start()
    {
        //set initial values
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
        Tree0.SetActive(true);
        Tree1.SetActive(false);
        Tree2.SetActive(false);

        //numbers obtained by experimenting in scene
        trigColliderSizeArray = new int[] { 2, 4, 5 };
        barHeights = new float[] { 1.5f, 2.5f, 4.5f };
        
        bc = gameObject.GetComponent<BoxCollider>();
        ResizeCollider();

        Renderer rd0 = Tree0.GetComponent<Renderer>();
        Renderer rd1 = Tree1.GetComponent<Renderer>();
        Renderer rd2 = Tree2.GetComponent<Renderer>();
        rdArray = new Renderer[] { rd0, rd1, rd2 };

        //set health related values
        maxHealth = initialHealth * 3;
        healthPerWater = initialHealth / waterPerGrowth;
        currentHealth = initialHealth;
        wBar.SetMaxValue(maxHealth);
        wBar.SetCurrentValue(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: change tree model according to health
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
        if (other.CompareTag("Farmer"))
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
        if (other.CompareTag("Farmer"))
        {
            
        }
    }

    //set trigger collider size for current model
    private void ResizeCollider()
    {
        int size = trigColliderSizeArray[curTree];
        bc.center = new Vector3(0, size/2, 0);
        bc.size = new Vector3(size, size, size);
    }

    private void waterTree()
    {
        growCount += 1;
        currentHealth += healthPerWater;

        wBar.SetCurrentValue(currentHealth);

        //TODO: redo this part based on health change
        if (growCount >= 3 && curTree < 2)
        {
            //deactivate current tree model
            treeArray[curTree].SetActive(false);

            //activate next growth level tree model
            curTree += 1;
            treeArray[curTree].SetActive(true);

            //move up the health bar
            canvas.transform.position = new Vector3(transform.position.x, barHeights[curTree], transform.position.z);
            wBar.SetCurrentValue(0);

            //glow(); make the new model glow (without increase interacting player count)
            rdArray[curTree].material.SetColor("_EmissionColor", new Color(0.35f, 0.35f, 0.35f, 1.0f));

            //set trigger collider size for current model
            ResizeCollider();

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
