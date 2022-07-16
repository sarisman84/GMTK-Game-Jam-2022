using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using FMODUnity;

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
    [Header("Contents")]
    public List<ScriptableAbility> listOfAbilities;
    public List<Transform> viewAngles;
    public List<GameObject> usedAbilityIndicators;
    public Transform renderTargetPos;

    [Header("Input")]
    public InputActionReference pauseInput;
    public InputActionReference selectAbilityInput;

    [Header("Debug")]
    public bool showDebug;

    private int selectedAbility;
    private int amountOfActionsLeft;
    private float currentDuration;
    private float currentDelay;
    private bool triggerAbility;
    private bool resetFlag;
    private bool triggerAudioOnce;


    private List<bool> abilityStatus;
    //Private Components
    private MeshRenderer meshRenderer;
    private MovementController movementController;
    private Quaternion defaultRotation;
    private Transform defaultTrackingOffset;
    private Transform currentTrackingOffset;
    private PlayerSoundManager playerSoundManager;

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
        abilityStatus = new List<bool>();
        foreach (var item in listOfAbilities)
        {
            abilityStatus.Add(true);
        }

        currentDelay = delayUntilNextSelection;
        currentDuration = selectionDuration;

        meshRenderer = GetComponent<MeshRenderer>();
        movementController = GetComponent<MovementController>();
        playerSoundManager = GetComponent<PlayerSoundManager>();

        currentStage = Stage.Useable;

        defaultRotation = transform.rotation;
        defaultTrackingOffset = renderTargetPos;
        currentTrackingOffset = defaultTrackingOffset;

        Color color = new Color();
        color.r = uiIndicator.color.r;
        color.g = uiIndicator.color.g;
        color.b = uiIndicator.color.b;
        color.a = 0;
        uiIndicator.color = color;

        ResetAbilities();
    }



    private void SlowdownTime()
    {
        Time.timeScale = Time.timeScale != slowMowScale ? slowMowScale : Time.timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        currentDuration -= Time.unscaledDeltaTime;
        currentDuration = currentDuration <= 0 ? 0 : currentDuration;


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

    private void ResetAbilities()
    {
        foreach (var item in usedAbilityIndicators)
        {
            item.SetActive(false);
        }


        for (int i = 0; i < abilityStatus.Count; i++)
        {
            abilityStatus[i] = true;
        }
    }

    private void Update()
    {

        RenderTrackSelf();



        float input = pauseInput.action.ReadValue<float>();
        if (input <= 0)
        {
            //Debug.Log("Selection Resetted!");
            currentDuration = selectionDuration;

        }

        bool canSelect = input > 0 /*&& currentDuration > 0*/;


        if (movementController.grounded && resetFlag)
        {
            ResetAbilities();
            resetFlag = false;
        }


        if (canSelect && !movementController.grounded)
        {
            if (!triggerAudioOnce)
            {
                playerSoundManager.Play(SoundType.DieRoll);
                triggerAudioOnce = true;
            }

            if (!abilityStatus[selectedAbility])
            {
                selectedAbility++;
                selectedAbility = selectedAbility >= abilityStatus.Count ? 0 : selectedAbility < 0 ? viewAngles.Count - 1 : selectedAbility;
            }

            resetFlag = true;
            triggerAbility = true;
            if (uiIndicator)
                uiIndicator.DOFade(1, 0.1f);
            SlowdownTime();
            ChooseAbility();
        }
        else if (triggerAbility)
        {
            if (triggerAudioOnce)
            {
                playerSoundManager.Stop(SoundType.DieRoll);
                triggerAudioOnce = false;
            }

            triggerAbility = false;

            if (uiIndicator)
                uiIndicator.DOFade(0, 0.1f);
            ResetTime();
            transform.DORotateQuaternion(defaultRotation, 0.15f);
            currentTrackingOffset = defaultTrackingOffset;

            if (selectedAbility < listOfAbilities.Count && abilityStatus[selectedAbility])
            {
                listOfAbilities[selectedAbility].ApplyEffect(movementController);
                usedAbilityIndicators[selectedAbility].SetActive(true);
                abilityStatus[selectedAbility] = false;
                Debug.Log("Ability Triggered!");
            }
            else
            {
                Debug.Log("Ability Disabled!");
            }




        }



        renderTargetPos.LookAt(transform);
    }

    private void FixedUpdate()
    {
        if (selectedAbility < listOfAbilities.Count)
            listOfAbilities[selectedAbility].UpdateEffect(movementController);
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







    }

    private void OnDrawGizmos()
    {
        if (!showDebug) return;
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


        if (!movementController) return;
        foreach (var ability in listOfAbilities)
        {
            ability.OnGizmosDraw(movementController);
        }
    }


}
