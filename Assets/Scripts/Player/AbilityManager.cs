using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
public class AbilityManager : MonoBehaviour
{
    //First pause the game
    //Then select an ability
    // -If an ability was selected, excecute it.
    // -If an ability wasnt selected, after x duration, leave the method.

    [Header("Global Settings")]
    public float slowMowScale;
    public float delayUntilNextSelection;
    public float selectionDuration;


    public RawImage uiIndicator;
    public List<ScriptableAbility> listOfAbilities;
    public List<Transform> viewAngles;
    public Transform renderTargetPos;

    [Header("Input")]
    public InputActionReference pauseInput;
    public InputActionReference selectAbilityInput;


    private int selectedAbility;
    private int amountOfActionsLeft;
    private float currentDuration;
    private float currentDelay;

    //Private Components
    private MeshRenderer meshRenderer;
    private MovementController movementController;

    private Quaternion defaultRotation;
    private Transform defaultTrackingOffset;
    private Transform currentTrackingOffset;

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
        movementController = GetComponent<MovementController>();

        currentStage = Stage.Useable;

        defaultRotation = transform.rotation;
        defaultTrackingOffset = renderTargetPos;
        currentTrackingOffset = defaultTrackingOffset;




    }

    private void SlowdownTime()
    {
        Time.timeScale = Time.timeScale != slowMowScale ? slowMowScale : Time.timeScale;
        currentDuration -= Time.unscaledDeltaTime;
        currentDuration = currentDuration <= 0 ? 0 : currentDuration;

        Debug.Log("Selecting Ability!");
    }

    private void ResetTime()
    {
        Time.timeScale = Time.timeScale != 1.0f ? 1.0f : Time.timeScale;
    }

    private void RenderTrackSelf()
    {
        renderTargetPos.localPosition = currentTrackingOffset.localPosition;
        renderTargetPos.localRotation = currentTrackingOffset.localRotation;
    }

    private void Update()
    {

        RenderTrackSelf();



        float input = pauseInput.action.ReadValue<float>();
        if (input <= 0)
        {
            Debug.Log("Selection Resetted!");
            currentDuration = selectionDuration;
        }

        bool canSelect = input > 0 /*&& currentDuration > 0*/;





        if (canSelect && !movementController.grounded)
        {
            if (uiIndicator)
                uiIndicator.DOFade(1, 0.1f);
            SlowdownTime();
            ChooseAbility();
        }
        else
        {
            if (uiIndicator)
                uiIndicator.DOFade(0, 0.1f);
            ResetTime();
            transform.DORotateQuaternion(defaultRotation, 0.15f);
            currentTrackingOffset = defaultTrackingOffset;
        }



        renderTargetPos.LookAt(transform);
    }



    public void ChooseAbility()
    {
        if (listOfAbilities == null)
        {
            Debug.LogWarning("No abilities in manager!", gameObject);
            return;
        }

        int currentSize = listOfAbilities.Count;


        int input = Mathf.CeilToInt(selectAbilityInput.action.ReadValue<float>());

        if (input != 0 && selectAbilityInput.action.triggered)
        {
            selectedAbility += input;
            selectedAbility = selectedAbility >= viewAngles.Count ? 0 : selectedAbility < 0 ? viewAngles.Count - 1 : selectedAbility;
            transform.DORotateQuaternion(Quaternion.LookRotation(UnityEngine.Random.insideUnitSphere), 0.15f);

            currentTrackingOffset = viewAngles[selectedAbility];


        }


        //You selected an ability


        //listOfAbilities[selectedAbility].ApplyEffect(gameObject);

    }

    private void OnDrawGizmos()
    {
        foreach (var item in viewAngles)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(item.position, transform.position);


            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(item.position, 0.25f);

        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(renderTargetPos.position, 0.3f);

        Gizmos.DrawLine(renderTargetPos.position, renderTargetPos.position + renderTargetPos.forward);
    }


}
