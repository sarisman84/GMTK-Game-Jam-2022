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
    #region Shadow Realm
    //RenderTrackSelf();



    //float input = pauseInput.action.ReadValue<float>();
    //if (input <= 0)
    //{
    //    //Debug.Log("Selection Resetted!");
    //    currentDuration = selectionDuration;

    //}

    //bool canSelect = input > 0 /*&& currentDuration > 0*/;


    //if (movementController.grounded && resetFlag)
    //{
    //    ResetAbilities();
    //    resetFlag = false;
    //}


    //if (canSelect && !movementController.grounded)
    //{
    //    if (!triggerAudioOnce)
    //    {
    //        //diceRollEmitter.Play();
    //        player.Play(SoundType.DieRoll);
    //        player.EditParamater(SoundType.DieRoll, "Dice Activate", 0.0f);

    //        triggerAudioOnce = true;
    //    }


    //    //if (usedAbilityIndicators[selectedAbility].activeSelf)
    //    //{
    //    //    selectedAbility++;
    //    //    selectedAbility = selectedAbility >= listOfAbilities.Count ? 0 : selectedAbility < 0 ? viewAngles.Count - 1 : selectedAbility;
    //    //}

    //    // ParticleManager.Get.SpawnParticle("Test1", transform.position);

    //    resetFlag = true;
    //    triggerAbility = true;
    //    if (uiIndicator)
    //        uiIndicator.DOFade(1, 0.1f);
    //    SlowdownTime();
    //    ChooseAbility();
    //}
    //else if (triggerAbility)
    //{
    //    if (triggerAudioOnce)
    //    {
    //        player.EditParamater(SoundType.DieRoll, "Dice Activate", selectedAbility + 1);
    //        triggerAudioOnce = false;
    //    }

    //    triggerAbility = false;

    //    if (uiIndicator)
    //        uiIndicator.DOFade(0, 0.1f);
    //    ResetTime();
    //    transform.DORotateQuaternion(defaultRotation, 0.15f);
    //    currentTrackingOffset = defaultTrackingOffset;

    //    if (selectedAbility < listOfAbilities.Count && !usedAbilityIndicators[selectedAbility].activeSelf)
    //    {
    //        listOfAbilities[selectedAbility].ApplyEffect(movementController);
    //        usedAbilityIndicators[selectedAbility].SetActive(true);

    //        Debug.Log("Ability Triggered!");
    //    }
    //    else
    //    {
    //        Debug.Log("Ability Disabled!");
    //    }




    //}



    //renderTargetPos.LookAt(transform);
    #endregion


    //First pause the game
    //Then select an ability
    // -If an ability was selected, excecute it.
    // -If an ability wasnt selected, after x duration, leave the method.

    [Header("Global Settings")]
    public float slowMowScale;
    public float selectionCooldown;
    public float selectionDuration;

    public RawImage uiRenderTargetIndicator;

    public MeshFilter diceModelRef;
    public Transform renderCamRef;
    public Transform mainCamRef;
    [Header("Contents")]
    public List<ScriptableAbility> listOfAbilities;
    //public List<GameObject> usedAbilityIndicators;


    [Header("Input")]
    public InputActionReference pauseInput;
    public InputActionReference selectAbilityInput;

    [Header("Debug")]
    public bool showDebug;

    private int selectedAbility;
    private int amountOfActionsLeft;
    private float currentDuration;
    private float currentCooldown;
    private bool triggerAbility;
    private bool resetFlag;
    private bool triggerAudioOnce;



    //Private Components
    private MeshRenderer meshRenderer;
    private MovementController movementController;
    private PlayerSoundManager player;


    private Quaternion modelRotOffset;
    private List<bool> activeAbilities;
    private bool hasRolledTheDice = false;
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


    private void FetchComponents()
    {
        meshRenderer = diceModelRef ? diceModelRef.GetComponent<MeshRenderer>() : GetComponent<MeshRenderer>();
        movementController = GetComponent<MovementController>();
        player = GetComponent<PlayerSoundManager>();

    }

    private void ResetAbilityStates()
    {
        if (activeAbilities == null)
            activeAbilities = new List<bool>();

        for (int i = 0; i < listOfAbilities.Count; i++)
        {
            if (activeAbilities.Count <= i)
            {
                activeAbilities.Add(true);
            }
            else
                activeAbilities[i] = true;
        }
    }


    private bool TryTrackDuration()
    {
        currentDuration -= Time.unscaledDeltaTime;
        currentDuration = Mathf.Clamp(currentDuration, 0, selectionDuration);

        return currentDuration == 0;
    }


    private void Awake()
    {

        ResetAbilityStates();
        FetchComponents();

        currentCooldown = selectionCooldown;
        currentDuration = selectionDuration;




        modelRotOffset = diceModelRef.transform.localRotation;


        uiRenderTargetIndicator.DOFade(0, 0);

    }



    private void SlowdownTime()
    {

        MusicManager.Get.EditCurrentMusicParams("Freeze Time", 1);
        Time.timeScale = Time.timeScale != slowMowScale ? slowMowScale : Time.timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;



    }

    private void ResetTime()
    {
        Time.timeScale = Time.timeScale != 1.0f ? 1.0f : Time.timeScale;
        Time.fixedDeltaTime = 0.01f;
        MusicManager.Get.EditCurrentMusicParams("Freeze Time", 0);
    }



    private void Update()
    {

        if (!hasRolledTheDice && !movementController.grounded && pauseInput.action.ReadValue<float>() > 0 && currentCooldown <= 0)
        {
            hasRolledTheDice = OnStartDiceRoll();
        }

        if (hasRolledTheDice)
        {
            currentDuration -= Time.unscaledDeltaTime;
            currentDuration = currentDuration <= 0 ? 0 : currentDuration;
        }

        if (hasRolledTheDice && UpdateInput() != 0)
        {

            RotateDiceToRandomDirs();
            MakeRenderCamViewSelectedDiceSide();
        }

        if (hasRolledTheDice && (TryTrackDuration() || pauseInput.action.ReadValue<float>() < 1))
        {
            hasRolledTheDice = OnExitDiceRoll();

            currentCooldown = selectionCooldown;
        }

        if (!hasRolledTheDice)
        {
            currentCooldown -= Time.unscaledDeltaTime;
            currentCooldown = Mathf.Clamp(currentCooldown, 0, selectionCooldown);

            if (currentCooldown <= 0)
            {
                currentDuration = selectionDuration;
            }
        }

        if (!resetFlag && movementController.grounded)
        {
            resetFlag = true;
            ResetAbilityStates();
        }

    }

    private int UpdateInput()
    {

        int input = Mathf.CeilToInt(selectAbilityInput.action.ReadValue<float>());
        if (input != 0 && selectAbilityInput.action.triggered)
        {
            selectedAbility += input;
            selectedAbility = selectedAbility >= listOfAbilities.Count ? 0 : selectedAbility < 0 ? listOfAbilities.Count - 1 : selectedAbility;
        }
        return input;
    }

    private bool OnStartDiceRoll()
    {
        resetFlag = false;
        SlowdownTime();

        uiRenderTargetIndicator.DOFade(1, 0.15f);
        //diceModelRef.transform.rotation = Quaternion.identity;

        return true;
    }

    private bool OnExitDiceRoll()
    {
        uiRenderTargetIndicator.DOFade(0, 0.15f);
        ResetTime();
        RotateSelectedSideTowardsCamera();
        if (selectedAbility >= listOfAbilities.Count || !activeAbilities[selectedAbility]) return false;
        activeAbilities[selectedAbility] = listOfAbilities[selectedAbility].ApplyEffect(movementController);
        return false;
    }

    private void FixedUpdate()
    {
        if (selectedAbility < listOfAbilities.Count && activeAbilities[selectedAbility])
            activeAbilities[selectedAbility] = listOfAbilities[selectedAbility].UpdateEffect(movementController);
    }


    private void OnDrawGizmos()
    {
        if (selectedAbility < listOfAbilities.Count)
            listOfAbilities[selectedAbility].OnGizmosDraw(movementController);
        Gizmos.color = Color.red;
        for (int i = 0; i < diceModelRef.sharedMesh.normals.Length; i++)
        {
            Vector3 normal = GetDiceSide(i);
            Gizmos.DrawRay(diceModelRef.transform.rotation * diceModelRef.sharedMesh.vertices[i] + transform.position, diceModelRef.transform.rotation * normal);
        }





    }


    private Vector3 GetDiceSide(int aSideID)
    {
        List<Vector3> normals = new List<Vector3>();
        //for (int i = 0; i < diceModelRef.mesh.triangles.Length; i += 3)
        //{
        //    var trag1 = diceModelRef.mesh.vertices[diceModelRef.mesh.triangles[i]];
        //    var trag2 = diceModelRef.mesh.vertices[diceModelRef.mesh.triangles[i + 1]];
        //    var trag3 = diceModelRef.mesh.vertices[diceModelRef.mesh.triangles[i + 2]];

        //    Vector3 dir1 = (trag1 - trag2).normalized;
        //    Vector3 dir2 = (trag1 - trag3).normalized;

        //    normals.Add(Vector3.Cross(dir1, dir2).normalized);
        //}

        normals.AddRange(diceModelRef.sharedMesh.normals);


        if (aSideID >= normals.Count) return Vector3.up;

        return normals[aSideID];

    }

    private int GetDiceSideAmm()
    {
        return diceModelRef.mesh.normals.Length;
    }



    private void MakeRenderCamViewSelectedDiceSide()
    {
        Vector3 normal = diceModelRef.transform.localRotation * GetDiceSide(selectedAbility);
        Vector3 renderCamOffset = transform.localPosition + normal * 5f;

        renderCamRef.localPosition = renderCamOffset;
        renderCamRef.DORotateQuaternion(Quaternion.LookRotation(-normal), 0.15f);
    }

    private void RotateDiceToRandomDirs()
    {
        meshRenderer.transform.DORotate(UnityEngine.Random.insideUnitSphere, 0.15f);
    }

    private void RotateSelectedSideTowardsCamera()
    {

        int maxSize = Mathf.Min(GetDiceSideAmm(), listOfAbilities.Count);

        selectedAbility = selectedAbility >= maxSize ? 0 : selectedAbility < 0 ? maxSize - 1 : selectedAbility;

        var dir = mainCamRef.position - transform.position;
        meshRenderer.transform.DORotateQuaternion(Quaternion.FromToRotation(GetDiceSide(selectedAbility), dir.normalized), 0.15f);

    }

}
