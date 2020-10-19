using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UFOController : MonoBehaviour
{
    //from input, or movement related
    Vector2 inputMovement;
    GameObject earth;
    public float tiltAmount;
    public PlayerInput pi;

    Quaternion origRotation;
    Quaternion earthOrigRotation;

    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.FindGameObjectWithTag("Earth");
        origRotation = this.transform.rotation;
        earthOrigRotation = earth.transform.rotation;

        if(PlayerData.AllPlayers.Count != 0)
        {
            PlayerSettings ps = PlayerData.AllPlayers[0];
            pi.SwitchCurrentControlScheme(ps.controlScheme, ps.inputDevice);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMovement != Vector2.zero)
        {
            Vector3 direction = new Vector3(inputMovement.x, 0.0F, inputMovement.y);
            earth.transform.Rotate(-inputMovement.y, inputMovement.x, 0, Space.World);
        }
        else
        {
            this.transform.rotation = origRotation;
        }
        
    }

    void OnMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
        this.transform.rotation = origRotation;
        this.transform.Rotate(inputMovement.y*tiltAmount, -inputMovement.x*tiltAmount, 0, Space.World);
    }

    void OnThrow()
    {
        earth.transform.rotation = earthOrigRotation;
    }

    void OnInteract()
    {

    }

    IEnumerator TiltUFO(Vector2 direction)
    {
        yield return null;
    }

}
