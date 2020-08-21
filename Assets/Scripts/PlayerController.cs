using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public
    public GameObject currentPlant;
    public GroundIndicatorController GroundIndicator;
    public float dashForce;
    public ParticleSystem dashEffect;
    public float pushBackDistance;

    //from input, or movement related
    Vector2 inputMovement;
    bool isDashing = false;
    bool isPushedBack = false;
    Vector3 pushedBackDest;

    //set numeric value
    float movementSpeed = 5.0F;
    float rotationSpeed = 0.15F;

    //components
    Animator anim;
    CharacterController cc;
    TreeListController tl;
    SurfaceGenerator sg;
    Rigidbody rb;
    public PlayerInput pi;

    //models
    List<GameObject> playerModels;
    public int curModel;
    int modelCount;

    //Interactions
    bool hasPlant;
    GameObject curBoxarea;
    bool keyboardShared;
    List<Collider> interactableColliders;
    GameObject curInteractObject;
    GameObject lastInteractObject;
    GameObject currentHandheldObject;

    int curObjectSize;
    Vector3 intendedPosition;
    Vector3 lastIntendedPosition;
    GameObject tPrefab;
    GameObject transT;

    
    public bool HasPlant { get => hasPlant; set => hasPlant = value; }
    public GameObject CurBoxarea { get => curBoxarea; set => curBoxarea = value; }
    public bool KeyboardShared { get => keyboardShared; set => keyboardShared = value; }
    public GameObject CurrentHandheldObject { get => currentHandheldObject; set => currentHandheldObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        //get all children player models
        playerModels = new List<GameObject>();
        GetAllChildren();
        curModel = 0;
        modelCount = playerModels.Count;

        anim = playerModels[curModel].GetComponent<Animator>();

        cc = this.GetComponent<CharacterController>();
        rb = this.GetComponent<Rigidbody>();
        tl = GameObject.FindObjectOfType<TreeListController>();
        sg = GameObject.FindObjectOfType<SurfaceGenerator>();

        interactableColliders = new List<Collider>();
        lastInteractObject = null;
        curInteractObject = null;

        //todo: hasplant condition
        HasPlant = true;

        pi = this.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInteraction();

        if (currentHandheldObject != null && currentHandheldObject.CompareTag("Plant"))
        {
            intendedPosition = GroundIndicator.GetPlantPosition(this.transform.position, curObjectSize);
            if(tPrefab == null)
            {
                tPrefab = currentHandheldObject.GetComponent<TreeControl>().TransparentFinalModel;
            }

            if (transT == null)
            {
                transT = Instantiate(tPrefab, intendedPosition + tPrefab.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                transT.SetActive(true);
                transT.transform.position = intendedPosition + tPrefab.transform.position;
            }

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            NextModel();
        }
    }

    
    void FixedUpdate()
    {
        Vector3 direction = new Vector3(inputMovement.x, 0.0F, inputMovement.y);

        if (isPushedBack)
        {
            if (FlatDistance(transform.position, pushedBackDest) > 0.01f)
            {
                float step = movementSpeed * 2f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, pushedBackDest, step);
            }
            else
            {
                isPushedBack = false;
            }
        }
        else
        {
            if (direction != Vector3.zero)
            {
                anim.SetBool("isWalking", true);
                MoveCharacter(direction);

                if (isDashing)
                {
                    rb.AddForce(direction * dashForce);
                    dashEffect.Play();
                    isDashing = false;
                }
            }
            else
            {
                anim.SetBool("isWalking", false);
                isDashing = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            interactableColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            interactableColliders.Remove(other);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cactus"))
        {
            Vector3 dir = transform.position - collision.gameObject.transform.position;
            Vector3 planeDir = new Vector3(dir.x, 0f, dir.z).normalized;
            pushedBackDest = planeDir * pushBackDistance + transform.position;
            isPushedBack = true;
        }
    }

    void MoveCharacter(Vector3 direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed);
        //transform.Translate(direction * movementSpeed * Time.deltaTime * -1.0F, Space.World);

        //cc.Move(direction * movementSpeed * Time.deltaTime);

        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
        //rb.velocity = direction * movementSpeed;
    }

    public void TakePlant(GameObject plant)
    {
        currentPlant = plant;
    }

    private void GetAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            playerModels.Add(child.gameObject);
        }
        playerModels[0].SetActive(true);
    }

    private void NextModel()
    {
        playerModels[curModel].SetActive(false);
        curModel = (curModel + 1) % modelCount;
        playerModels[curModel].SetActive(true);
        anim = playerModels[curModel].GetComponent<Animator>();
    }

    private GameObject GetCurrentInteractObject()
    {
        if (interactableColliders.Count == 0)
        {
            return null;
        }
        GameObject go = interactableColliders[0].gameObject;
        float minDist = -1.0f;
        foreach(Collider c in interactableColliders)
        {
            float dist = FlatDistance(transform.position, c.transform.position);
            if (minDist < 0 || dist < minDist)
            {
                minDist = dist;
                go = c.gameObject;
            }
        }
        return go;
    }

    private void UpdateInteraction()
    {
        lastInteractObject = curInteractObject;
        curInteractObject = GetCurrentInteractObject();
        if (curInteractObject != null && curInteractObject != lastInteractObject)
        {
            curInteractObject.GetComponent<InteractableController>().glow();
        }
        if (lastInteractObject != null && lastInteractObject != curInteractObject)
        {
            lastInteractObject.GetComponent<InteractableController>().unglow();
        }
    }

    private float FlatDistance(Vector3 p1, Vector3 p2)
    {
        Vector3 p12 = p1 - p2;
        p12.y = 0;
        float dist = p12.magnitude;
        return dist;
    }

    public bool Hold(GameObject go, Collider colliderToBeRemoved)
    {
        if (CurrentHandheldObject != null)
        {
            return false;
        }

        anim.SetBool("isHoldingObject", true);
        CurrentHandheldObject = go;
        GameObject dest = playerModels[curModel].transform.GetChild(2).gameObject;
        go.transform.position = dest.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.transform.parent = dest.transform;

        if (colliderToBeRemoved != null)
        {
            interactableColliders.Remove(colliderToBeRemoved);
        }

        UpdateInteraction();

        if (go.CompareTag("Plant"))
        {
            curObjectSize = go.GetComponent<TreeControl>().FinalSize;
            GroundIndicator.ShowMosaic(true, this.gameObject);
        }

        return true;
    }

    public bool Water()
    {
        if (currentHandheldObject.CompareTag("Bucket"))
        {
            BucketController bc = currentHandheldObject.GetComponent<BucketController>();
            if (bc.IsFilled)
            {
                bc.EmptyWater();
                return true;
            }
        }

        return false;
    }

    void OnMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
    }

    void OnInteract()
    {
        if(curInteractObject != null)
        {
            curInteractObject.GetComponent<InteractableController>().OnPlayerInteract(this.gameObject);
        }
        else if (currentHandheldObject.CompareTag("Plant"))
        {
            //instantiate
            currentHandheldObject.transform.position = transT.transform.position;
            currentHandheldObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            currentHandheldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentHandheldObject.GetComponent<TreeControl>().IsPlanted = true;
            currentHandheldObject.GetComponent<TreeControl>().wBar.gameObject.SetActive(true);

            tl.treeList.Add(currentHandheldObject);

            OnDrop();


            //regenerate surface
            //sg.surface.BuildNavMesh();
        }
    }

    void OnDash()
    {
        isDashing = true;
    }

    void OnDrop()
    {
        if (CurrentHandheldObject != null)
        {
            anim.SetBool("isHoldingObject", false);
            CurrentHandheldObject.transform.parent = null;
            CurrentHandheldObject.GetComponent<InteractableController>().OnDrop();

            if (CurrentHandheldObject.CompareTag("Plant"))
            {
                GroundIndicator.ShowMosaic(false, this.gameObject);
                transT.SetActive(false);
            }

            currentHandheldObject = null;
        }
    }

    void OnDeviceLost()
    {

    }

    void OnDeviceRegained()
    {

    }

    void OnControlsChanged()
    {

    }

    public void SavePlayer()
    {
        GlobalControl.Instance.SavePlayer(curModel, pi.currentControlScheme, pi.devices[0]);
    }
}
