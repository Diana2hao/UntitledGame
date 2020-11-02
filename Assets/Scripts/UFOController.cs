using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UFOController : MonoBehaviour
{
    //from input, or movement related
    Vector2 inputMovement;
    GameObject earth;
    public float tiltAmount;
    public PlayerInput pi;
    public float earthRotateSpeed;
    public float ufoMoveSpeed;

    public float minX;
    public float maxX;

    public Gradient gradient;

    Quaternion origRotation;
    Quaternion earthOrigRotation;

    GameObject nextSceneEntrance;

    bool isEarthScene = false;

    public PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            isEarthScene = true;
            earth = GameObject.FindGameObjectWithTag("Earth");
            origRotation = this.transform.rotation;
            earthOrigRotation = earth.transform.rotation;
        }

        //if(SceneManager.GetActiveScene().buildIndex != 0)
        //{
        //    pauseMenu = GameObject.Find("Canvas/PauseMenu").GetComponent<PauseMenu>();
        //}

        if(PlayerData.AllPlayers.Count != 0)
        {
            PlayerSettings ps = PlayerData.AllPlayers[0];
            Debug.Log(ps.controlScheme);
            pi.SwitchCurrentControlScheme(ps.controlScheme, ps.inputDevice);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMovement != Vector2.zero && !PauseMenu.GameIsPaused)
        {
            if (isEarthScene)
            {
                Vector3 direction = new Vector3(inputMovement.x, 0.0F, inputMovement.y);
                earth.transform.Rotate(-inputMovement.y * earthRotateSpeed, inputMovement.x * earthRotateSpeed, 0, Space.World);
            }

            else
            {
                Vector3 direction = new Vector3(inputMovement.x, 0.0F, 0f);
                this.transform.Translate(direction * ufoMoveSpeed, Space.World);

                if (this.transform.position.x < minX)
                {
                    this.transform.position = new Vector3(minX, this.transform.position.y, this.transform.position.z);
                }

                if (this.transform.position.x > maxX)
                {
                    this.transform.position = new Vector3(maxX, this.transform.position.y, this.transform.position.z);
                }
            }
            
        }
        else
        {
            this.transform.rotation = origRotation;
        }
        
    }

    void OnMove(InputValue value)
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }

        inputMovement = value.Get<Vector2>();
        this.transform.rotation = origRotation;
        if (isEarthScene)
        {
            this.transform.Rotate(inputMovement.y * tiltAmount, -inputMovement.x * tiltAmount, 0, Space.World);
        }
        else
        {
            this.transform.Rotate(0, 0, -inputMovement.x * tiltAmount, Space.World);
        }
    }

    void OnThrow()
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }

        if (isEarthScene)
        {
            earth.transform.rotation = earthOrigRotation;
        }
    }

    void OnInteract()
    {
        if (PlayerData.isPlayingCutscene)
        {
            return;
        }

        if (!PauseMenu.GameIsPaused && nextSceneEntrance != null)
        {
            nextSceneEntrance.GetComponent<EntranceController>().EnterNextScene();
        }
    }

    void OnMenu()
    {
        pauseMenu.PauseUnpause();
    }

    private void OnTriggerEnter(Collider other)
    {
        nextSceneEntrance = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        nextSceneEntrance = null;
    }

    IEnumerator TiltUFO(Vector2 direction)
    {
        yield return null;
    }

}
