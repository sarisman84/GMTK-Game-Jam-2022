using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public InputActionAsset inputAsset;

    void Start() {
        inputAsset.Enable();
    }

    void Update() {
        Vector2 test = inputAsset.FindAction("Movement").ReadValue<Vector2>();
    }
}
