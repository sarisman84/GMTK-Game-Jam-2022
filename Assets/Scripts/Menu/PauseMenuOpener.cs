using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuOpener : MonoBehaviour
{

    public MovementController movementController;
    public GameObject pause_menu_prefab;
    private GameObject pause_menu;
    public InputActionAsset inputAsset;
    private bool escape_press;
    private bool check; //checks if pause_menu_prefab was instantiated so that it could instantiate only 1.
    private bool wait;
    void Start()
    {
        inputAsset.Enable();
    }
    void Update()
    {
        escape_press = inputAsset.FindAction("Pause").IsPressed();
        if (escape_press && !check && !wait)
        {

            check = true;
            wait = true;
            pause_menu = Instantiate(pause_menu_prefab, this.transform.position, this.transform.rotation) as GameObject; //Instantiate pause menu
            StartCoroutine(WaitUntilDone(0.2f)); //fixes flickering issues 
        }
        if (escape_press && check && !wait)
        {

            movementController.enabled = false;
            Destroy(pause_menu); //destroys pause menu
            check = false;
            wait = true;
            StartCoroutine(WaitUntilDone(0.2f)); //fixes flickering issues 
        }
    }
    public void CloseMenu()
    {
        if (check && !wait)
        {
            Destroy(pause_menu); //destroys pause menu
            check = false;
            StartCoroutine(WaitUntilDone(0.2f));
        }
    }
    IEnumerator WaitUntilDone(float time)
    {
        yield return new WaitForSeconds(time);
        wait = false;
    }
}
