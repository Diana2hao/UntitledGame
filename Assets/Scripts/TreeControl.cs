using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LastActiveBar
{
    WATERBAR,
    GROWBAR,
    NONE
}

//handles player interaction, tree growth, and tree cutting (farmer interaction)
public class TreeControl : MonoBehaviour, IInteractable
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
    GridController gridCon;

    //health related
    public AudioSource speedUpSound;
    public AudioSource healSound;
    public ParticleSystem speedUpEffect;
    public Canvas canvas;
    public TreeHealthBar healthBar;
    public TreeWaterBar waterBar;
    public TreeGrowingBar growBar;
    public int maxHealth;
    public int waterPerGrowth;
    int currentHealth;
    bool isGrowing;
    int lastActiveBar;

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
    BirdsControl birdsControl;

    //birds related
    List<Vector3> restSpots;
    List<Quaternion> restRotations;

    //game stats related
    GameObject LC;  //level control
    

    public bool IsPlanted { get => isPlanted; set => isPlanted = value; }
    public bool IsFullyGrown { get => isFullyGrown; set => isFullyGrown = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public List<Vector3> RestSpots { get => restSpots; set => restSpots = value; }
    public List<Quaternion> RestRotations { get => restRotations; set => restRotations = value; }
    public int CurTree { get => curTree; set => curTree = value; }
    public bool IsGrowing { get => isGrowing; set => isGrowing = value; }


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
        trigColliderSizeArray = new int[] { 2, 3, 3 };
        barHeights = new float[] { 1.5f, 2.5f, 4.5f };
        
        bc = gameObject.GetComponent<BoxCollider>();
        ResizeCollider();

        Renderer rd0 = Tree0.GetComponent<Renderer>();
        Renderer rd1 = Tree1.GetComponent<Renderer>();
        Renderer rd2 = Tree2.GetComponent<Renderer>();
        rdArray = new Renderer[] { rd0, rd1, rd2 };

        //set health related values
        currentHealth = maxHealth;

        //get rest spots for birds
        restSpots = new List<Vector3>();
        restRotations = new List<Quaternion>();
        foreach (Transform c in transform.GetChild(4))
        {
            restSpots.Add(c.localPosition);
            restRotations.Add(c.localRotation);
        }

        //get birds control
        birdsControl = GameObject.Find("BirdsControl").GetComponent<BirdsControl>();

        //get grid controller
        gridCon = GameObject.Find("Grid").GetComponent<GridController>();

        LC = GameObject.FindGameObjectWithTag("LevelControl");

    }

    // Update is called once per frame
    void Update()
    {
        //health decrease update is done in farmer's cut tree state
        //if health drop below zero, farmer's cut tree state will destroy tree

        //if cutted down to certain health, downgrade the model
        if (currentHealth <= 0)
        {
            //deactivate current tree model
            treeArray[curTree].SetActive(false);

            //activate the smaller tree model
            curTree -= 1;
            treeArray[curTree].SetActive(true);

            //move the health bar to correct height
            canvas.transform.position = new Vector3(transform.position.x, barHeights[curTree], transform.position.z);

            //glow(); make the new model glow (without increase interacting player count)
            //rdArray[curTree].material.SetFloat("_Emission", 1);
            if (playerNum > 0) { rdArray[curTree].material.EnableKeyword("_EMISSION"); }

            //set trigger collider size for current model
            ResizeCollider();

            //if not the biggest tree anymore, inform birds control
            if (curTree == 1)
            {
                isFullyGrown = false;
                //TODO: add interaction with birds control
                birdsControl.RemoveGrownTree(this);
                LC.GetComponent<LevelControl>().MinusTree();
            }

            //set health back to max, reset other 2 bar
            currentHealth = maxHealth;
            healthBar.SetFillAmount(currentHealth/maxHealth);
            ResetWaterGrowBar();
            lastActiveBar = (int)LastActiveBar.NONE;
        }
        
    }

    public void StartCutting()
    {
        isCutting = true;
        isGrowing = false;
        healthBar.gameObject.SetActive(true);

        if (waterBar.gameObject.activeSelf)
        {
            lastActiveBar = (int)LastActiveBar.WATERBAR;
            waterBar.gameObject.SetActive(false);
        }
        else if (growBar.gameObject.activeSelf)
        {
            lastActiveBar = (int)LastActiveBar.GROWBAR;
            growBar.StopGrowing();
            growBar.gameObject.SetActive(false);
        }
        else
        {
            lastActiveBar = (int)LastActiveBar.NONE;
        }
    }

    public void StopCutting()
    {
        isCutting = false;
        //TODO: show an icon to tell player to fix this tree with a bag of fertilizer

    }

    private void ResetWaterGrowBar()
    {
        //TODO: not implemented
        waterBar.ResetBar();
        growBar.ResetBar();
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
        //add to water bar, if return value is true, tree can grow to next stage
        if (waterBar.WaterOnce())
        {
            isGrowing = true;
            //start change bar coroutine
            StartCoroutine("ChangeFromWaterToGrowBar");
        }
    }

    IEnumerator ChangeFromWaterToGrowBar()
    {
        // fade out water bar

        waterBar.gameObject.SetActive(false);
        waterBar.ResetBar();

        // fade in grow bar

        growBar.gameObject.SetActive(true);
        growBar.StartGrowing();

        yield return null;
    }

    IEnumerator ChangeFromGrowToWaterBar()
    {
        // fade out grow bar

        growBar.gameObject.SetActive(false);
        growBar.ResetBar();

        // fade in water bar
        waterBar.gameObject.SetActive(true);

        yield return null;
    }

    public void GrowUpOneStage()
    {
        isGrowing = false;

        //deactivate current tree model
        treeArray[curTree].SetActive(false);

        //activate next growth level tree model
        curTree += 1;
        treeArray[curTree].SetActive(true);

        //move the health bar
        canvas.transform.position = new Vector3(transform.position.x, barHeights[curTree], transform.position.z);

        //glow(); make the new model glow (without increase interacting player count)
        //rdArray[curTree].material.SetFloat("_Emission", 1);
        if (playerNum > 0) { rdArray[curTree].material.EnableKeyword("_EMISSION"); }

        //set trigger collider size for current model
        ResizeCollider();

        //if at max, inform birds control
        if (curTree == 2)
        {
            isFullyGrown = true;
            //wBar.gameObject.SetActive(false);
            birdsControl.AddGrownTree(this);

            //reset grow bar
            growBar.gameObject.SetActive(false);
            growBar.ResetBar();

            LC.GetComponent<LevelControl>().AddTree();
        }
        else
        {
            StartCoroutine("ChangeFromGrowToWaterBar");
        }
    }

    public void glow()
    {
        playerNum += 1;
        //rdArray[curTree].material.SetFloat("_Emission", 1);
        rdArray[curTree].material.EnableKeyword("_EMISSION");
    }

    public void unglow()
    {
        playerNum -= 1;
        if(playerNum == 0)
        {
            //rdArray[curTree].material.SetFloat("_Emission", 0);
            rdArray[curTree].material.DisableKeyword("_EMISSION");
        }
    }

    public void OnPlayerInteract(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (!isPlanted)
        {
            if(pc.Hold(this.gameObject, this.GetComponent<BoxCollider>(), Vector3.zero, Quaternion.identity))
            {
                PickUp(true);
            }
        }
        else
        {
            if (!isGrowing && !isDamaged() && !isFullyGrown && pc.Water())
            {
                waterTree();
            }

            else if(isGrowing && growBar.GetBagCount() < growBar.totalFertilizerBagsAddable && pc.Fertilize())
            {
                growBar.AddOneBagOfFert();
                speedUpSound.Play();
                speedUpEffect.Play();
            }

            else if(isDamaged() && pc.Fertilize())
            {
                healSound.Play();
                StartCoroutine("FixTree");
            }
        }

    }

    IEnumerator FixTree()
    {
        currentHealth = maxHealth;
        healthBar.SetFillAmount(currentHealth/maxHealth);

        yield return new WaitForSeconds(1f);

        healthBar.gameObject.SetActive(false);

        //if not fully grown, set which bar back to active
        if (curTree < 2)
        {
            if (lastActiveBar == (int)LastActiveBar.WATERBAR || lastActiveBar == (int)LastActiveBar.NONE)
            {
                waterBar.gameObject.SetActive(true);
            }
            else
            {
                growBar.gameObject.SetActive(false);
            }
        }

        yield return null;
    }

    private bool isDamaged()
    {
        if (currentHealth < maxHealth)
        {
            return true;
        }

        return false;
    }

    public void OnDrop()
    {
        PickUp(false);
    }

    public bool OnThrow(float throwForce)
    {
        PickUp(false);
        this.GetComponent<Rigidbody>().AddForce((this.transform.parent.forward + this.transform.parent.up).normalized * throwForce);

        return true;
    }

    public void Plant()
    {
        isPlanted = true;
        waterBar.gameObject.SetActive(true);
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        this.transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
        this.transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;
    }

    private void PickUp(bool up)
    {
        this.GetComponent<Rigidbody>().isKinematic = up;
        this.GetComponent<BoxCollider>().enabled = !up;
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = !up;
    }
}
