using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSettings
{
    public int modelNumber;
    public string controlScheme;
    public InputDevice inputDevice;

    public PlayerSettings(int modelNumber, string controlScheme, InputDevice inputDevice)
    {
        this.modelNumber = modelNumber;
        this.controlScheme = controlScheme;
        this.inputDevice = inputDevice;
    }
}
