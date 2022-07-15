using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    //First pause the game
    //Then select an ability
    // -If an ability was selected, excecute it.
    // -If an ability wasnt selected, after x duration, leave the method.

    public float slowMowScale;
    public float delayUntilNextSelection;
    public float selectionDuration;

    public List<ScriptableAbility> listOfAbilities;


    public InputActionReference pauseInput, selectAbilityInput;

    private int selectedAbility;
    private float currentDuration;
    private float currentDelay;

    private MeshRenderer meshRenderer;


    enum Stage
    {
        Selecting, Useable
    }


    private Stage currentStage;


    private void OnEnable()
    {
        pauseInput.action.Enable();
        selectAbilityInput.action.Enable();
    }

    private void OnDisable()
    {
        pauseInput.action.Disable();
        selectAbilityInput.action.Disable();
    }


    private void Awake()
    {
        currentDelay = delayUntilNextSelection;
        currentDuration = selectionDuration;

        meshRenderer = GetComponent<MeshRenderer>();

        currentStage = Stage.Useable;
    }

    private void Update()
    {
        float input = pauseInput.action.ReadValue<float>();
        if (input <= 0 && currentDuration <= 0.1f)
        {
            Debug.Log("Selection Resetted!");
            currentDuration = selectionDuration;
        }

        bool canSelect = input > 0 && currentDuration > 0;





        if (canSelect)
        {
            Time.timeScale = Time.timeScale != slowMowScale ? slowMowScale : Time.timeScale;
            currentDuration -= Time.unscaledDeltaTime;
            currentDuration = currentDuration <= 0 ? 0 : currentDuration;

            Debug.Log("Selecting Ability!");
        }
        else
        {
            Time.timeScale = Time.timeScale != 1.0f ? 1.0f : Time.timeScale;
        }




    }

    private bool RaycastToGround()
    {
        if (!meshRenderer)
        {
            Debug.LogWarning($"Missing MeshRenderer in {gameObject.name}. Ground check wont work.", gameObject);
            return false;
        }

        float skinWidth = 0.05f;
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        ray.origin = transform.position - new Vector3(0, (meshRenderer.bounds.size.y / 2.0f) + skinWidth, 0);
        return Physics.Raycast(ray, skinWidth);
    }

    public void ChooseAbility()
    {
        if (listOfAbilities == null)
        {
            Debug.LogWarning("No abilities in manager!", gameObject);
            return;
        }

        int currentSize = listOfAbilities.Count;





    }


}
