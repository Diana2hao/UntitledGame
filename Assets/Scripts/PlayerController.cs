﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //public
    public GameObject currentPlant;
    public GroundIndicatorController GroundIndicator;
    public float dashForce;
    public float dashCooldown;
    public ParticleSystem dashEffect;
    public float pushBackDistance;
    public float pushForce;
    public float pushTime;
    public float throwForce;
    public AudioSource dashSound;
    public AudioSource throwSound;
    public AudioSource waterSound;
    public GameObject flowerPrefab;

    //Tutorial Related

    //from input, or movement related
    Vector2 inputMovement;
    bool isDashing = false;
    bool isPushedBack = false;
    Vector3 pushedBackDest;
    Vector3 pushedBackDir;
    float pushBackTimer = 0f;
    float dashCDTime = 0f;

    //set numeric value
    float movementSpeed = 5.0F;
    float rotationSpeed = 0.15F;

    //components
    Animator anim;
    CharacterController cc;
    TreeListController tl;
    Rigidbody rb;
    public PlayerInput pi;
    GridController gridCon;
    GameObject LC;

    //models
    List<GameObject> playerModels = new List<GameObject>();
    public int curModel = 0;
    int modelCount;

    //Interactions
    bool hasPlant;
    bool isPlantable = false;
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

    //pausemenu
    public PauseMenu pauseMenu;
    

    public bool HasPlant { get => hasPlant; set => hasPlant = value; }
    public GameObject CurBoxarea { get => curBoxarea; set => curBoxarea = value; }
    public bool KeyboardShared { get => keyboardShared; set => keyboardShared = value; }
    public GameObject CurrentHandheldObject { get => currentHandheldObject; }
    public GameObject CurInteractObject { get => curInteractObject; }


    // Start is called before the first frame update
    void Start()
    {
        //if (SceneManager.GetActiveScene().buildIndex != 0)
        //{
        //    pauseMenu = GameObject.Find("Canvas/PauseMenu").GetComponent<PauseMenu>();
        //}

        //get all children player models
        GetAllChildren();
        modelCount = playerModels.Count;
        ChangeModel(curModel);

        anim = playerModels[curModel].GetComponent<Animator>();

        cc = this.GetComponent<CharacterController>();
        rb = this.GetComponent<Rigidbody>();
        tl = GameObject.FindObjectOfType<TreeListController>();
        if(GameObject.Find("Grid") != null)
        {
            gridCon = GameObject.Find("Grid").GetComponent<GridController>();
        }

        interactableColliders = new List<Collider>();
        lastInteractObject = null;
        curInteractObject = null;

        //todo: hasplant condition
        HasPlant = true;

        AddFlower();

        ActivateProperTutorial();

        LC = GameObject.FindGameObjectWithTag("LevelControl");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInteraction();

        if (currentHandheldObject != null && currentHandheldObject.CompareTag("Plant"))
        {
            //intendedPosition = GroundIndicator.GetPlantPosition(this.transform.position, curObjectSize);
            if(gridCon.FindPlantPosition(this.transform.position, this.transform.forward, curObjectSize, out intendedPosition))
            {
                isPlantable = true;

                if (tPrefab == null)
                {
                    tPrefab = currentHandheldObject.GetComponent<TreeControl>().TransparentFinalModel;
                }

                if (transT == null)
                {
                    transT = Instantiate(tPrefab, intendedPosition + tPrefab.transform.position, tPrefab.transform.rotation);
                }
                else
                {
                    transT.SetActive(true);
                    transT.transform.position = intendedPosition + tPrefab.transform.position;
                }
            }
            else
            {
                isPlantable = false;
                if (transT != null) { transT.SetActive(false); }
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

        Vector3 normalizedDirection = new Vector3(inputMovement.normalized.x, 0.0f, inputMovement.normalized.y);

        dashCDTime += Time.deltaTime;

        if (isPushedBack)
        {
            //if (FlatDistance(transform.position, pushedBackDest) > 0.01f)
            //{
            //    float step = movementSpeed * 2f * Time.deltaTime;
            //    transform.position = Vector3.MoveTowards(transform.position, pushedBackDest, step);
            //}
            //else
            //{
            //    isPushedBack = false;
            //}
            if(pushBackTimer == 0f)
            {
                rb.AddForce(pushedBackDir * pushForce);
            }
            
            pushBackTimer += Time.deltaTime;

            if (pushBackTimer > pushTime)
            {
                isPushedBack = false;
                pushBackTimer = 0f;
            }
        }
        else
        {
            if (direction != Vector3.zero)
            {
                anim.SetBool("isWalking", true);
                MoveCharacter(direction);

                if (isDashing && dashCDTime >= dashCooldown)
                {
                    rb.AddForce(normalizedDirection.normalized * dashForce);
                    dashEffect.Play(false);
                    dashSound.Play();
                    dashCDTime = 0f;
                }
                isDashing = false;
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
            Vector3 flatDir = new Vector3(dir.x, 0f, dir.z).normalized;
            pushedBackDir = flatDir;
            pushedBackDest = flatDir * pushBackDistance + transform.position;
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

    public void ChangeModel(int index)
    {
        foreach(GameObject m in playerModels)
        {
            m.SetActive(false);
        }

        curModel = index;
        playerModels[curModel].SetActive(true);
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
            curInteractObject.GetComponent<IInteractable>().glow();
        }
        if (lastInteractObject != null && lastInteractObject != curInteractObject)
        {
            lastInteractObject.GetComponent<IInteractable>().unglow();
        }
    }

    private float FlatDistance(Vector3 p1, Vector3 p2)
    {
        Vector3 p12 = p1 - p2;
        p12.y = 0;
        float dist = p12.magnitude;
        return dist;
    }

    public bool Hold(GameObject go, Collider colliderToBeRemoved, Vector3 positionOffset, Quaternion rotationOffset)
    {
        if (currentHandheldObject != null)
        {
            return false;
        }

        anim.SetBool("isHoldingObject", true);
        currentHandheldObject = go;
        GameObject dest = playerModels[curModel].transform.GetChild(2).gameObject;

        go.transform.parent = dest.transform;
        //go.transform.position = dest.transform.position + positionOffset;
        go.transform.localPosition = positionOffset;
        //go.transform.rotation = this.transform.rotation * rotationOffset;
        go.transform.localRotation = rotationOffset;

        if (colliderToBeRemoved != null)
        {
            interactableColliders.Remove(colliderToBeRemoved);
        }

        UpdateInteraction();

        if (go.CompareTag("Plant"))
        {
            curObjectSize = go.GetComponent<TreeControl>().FinalSize;
            gridCon.ShowMosaic(true, this.gameObject);
        }

        return true;
    }

    public void FailToHold(Collider colliderToBeRemoved)
    {
        if (colliderToBeRemoved != null)
        {
            interactableColliders.Remove(colliderToBeRemoved);
        }
    }

    public bool Water()
    {
        if (currentHandheldObject != null && currentHandheldObject.CompareTag("Bucket"))
        {
            BucketController bc = currentHandheldObject.GetComponent<BucketController>();
            if (bc.IsFilled)
            {
                bc.EmptyWater();
                waterSound.Play();
                return true;
            }
        }

        return false;
    }

    public bool Fertilize()
    {
        if(currentHandheldObject != null && currentHandheldObject.CompareTag("FertilizerBag"))
        {
            FertilizerBagController bag = currentHandheldObject.GetComponent<FertilizerBagController>();
            OnDrop();
            bag.DestroyThisBag();
            return true;
        }

        return false;
    }

    void OnMove(InputValue value)
    {
        if (!PlayerData.isPlayingCutscene)
        {
            inputMovement = value.Get<Vector2>();

        }
        else
        {
            inputMovement = Vector2.zero;
        }
    }

    void OnInteract()
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }

        //if has sth in hand, take actions base on the thing
        if(currentHandheldObject != null)
        {
            if (currentHandheldObject.CompareTag("Plant"))
            {
                if (isPlantable)
                {
                    //plant on ground
                    currentHandheldObject.transform.position = transT.transform.position;
                    currentHandheldObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                    currentHandheldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    TreeControl tc = currentHandheldObject.GetComponent<TreeControl>();

                    tl.treeList.Add(currentHandheldObject);
                    gridCon.AddGameObjectOfScale(transT.transform.position, currentHandheldObject, curObjectSize);

                    OnDrop();

                    tc.Plant();  //disable all children colliders
                                 
                    //regenerate surface
                    //sg.surface.BuildNavMesh();
                }
            }
            else if (currentHandheldObject.CompareTag("Vacuum"))
            {
                if (curInteractObject != null && curInteractObject.CompareTag("Station"))
                {
                    //interaction with station
                    curInteractObject.GetComponent<IInteractable>().OnPlayerInteract(this.gameObject);
                }
                else
                {
                    //turn vacuum on and off
                    currentHandheldObject.GetComponent<VacuumController>().VacuumSwitch();
                }
            }
            else if (currentHandheldObject.CompareTag("Bucket"))
            {
                if(curInteractObject != null)
                {
                    curInteractObject.GetComponent<IInteractable>().OnPlayerInteract(this.gameObject);
                }

                //TODO(maybe): if no interact object, pour water anyways

            }
            else if (currentHandheldObject.CompareTag("FertilizerBag"))
            {
                if (curInteractObject != null)
                {
                    curInteractObject.GetComponent<IInteractable>().OnPlayerInteract(this.gameObject);
                }
            }
        }
        //if nothing in hand, interact with the object in environment
        else if (curInteractObject != null)
        {
            curInteractObject.GetComponent<IInteractable>().OnPlayerInteract(this.gameObject);
        }
    }

    void OnDash()
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }


        isDashing = true;
    }

    public void OnDrop()
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }

        if (currentHandheldObject != null)
        {
            anim.SetBool("isHoldingObject", false);
            currentHandheldObject.transform.parent = null;
            currentHandheldObject.GetComponent<IInteractable>().OnDrop();

            if (currentHandheldObject.CompareTag("Plant"))
            {
                gridCon.ShowMosaic(false, this.gameObject);
                transT.SetActive(false);
            }

            currentHandheldObject = null;
        }
    }

    public void OnThrow()
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }

        if (currentHandheldObject != null)
        {
            if (currentHandheldObject.GetComponent<IInteractable>().OnThrow(throwForce))
            {
                throwSound.Play();
                anim.SetBool("isHoldingObject", false);
                currentHandheldObject.transform.parent = null;

                if (currentHandheldObject.CompareTag("Plant"))
                {
                    gridCon.ShowMosaic(false, this.gameObject);
                    transT.SetActive(false);
                }

                currentHandheldObject = null;
            }
        }
    }

    void OnMenu()
    {
        if(pauseMenu == null)
        {
            pauseMenu = (PauseMenu)Resources.FindObjectsOfTypeAll(typeof(PauseMenu))[0];
        }
        pauseMenu.PauseUnpause();
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

    public void AddFlower()
    {
        
        playerModels[curModel].GetComponent<PlayerModel>().AddFlower(flowerPrefab, pi.playerIndex);
    }

    private void ActivateProperTutorial()
    {
        //lobby scene
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            this.GetComponent<PlayerTutorialControl>().enabled = true;
            this.GetComponent<PlayerTutorialControl>().isEnabled = true;
        }

        //spm level 0
        if (SceneManager.GetActiveScene().name == "0_0")
        {
            this.GetComponent<PlayerGameplayTutR1L0>().enabled = true;
            this.GetComponent<PlayerGameplayTutR1L0>().isEnabled = true;
        }

        //spm level 1
        if (SceneManager.GetActiveScene().name == "0_1")
        {
            this.GetComponent<PlayerGameplayTutR1L1>().enabled = true;
            this.GetComponent<PlayerGameplayTutR1L1>().isEnabled = true;
        }
    }
}
