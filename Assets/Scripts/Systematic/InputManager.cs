using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset inputAsset;

    public InputAction GetAction(string actionName) { return inputAsset.FindAction(actionName); }
}
