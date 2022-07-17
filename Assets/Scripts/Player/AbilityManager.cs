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

    [Serializable]
    public struct Ability
    {
        public ScriptableAbility ability;
        public Sprite abilityIcon;
        public string abilityLabel;
    }

    //First pause the game
    //Then select an ability
    // -If an ability was selected, excecute it.
    // -If an ability wasnt selected, after x duration, leave the method.

    [Header("Global Settings")]
    public float slowMowScale;
    public float selectionCooldown;
    public float selectionDuration;


    public MeshFilter diceModelRef;
    public Transform dicePivot;

    [Header("UI")]
    public CanvasGroup uiIndicator;
    public Image uiIcon;
    public TMPro.TextMeshProUGUI uiLabel;
    public Image uiCD;

    [Header("Contents")]
    public List<Ability> listOfAbilities;
    //public List<GameObject> usedAbilityIndicators;


    [Header("Input")]
    public InputActionReference pauseInput;
    public InputActionReference selectAbilityInput;

    [Header("Debug")]
    public bool showDebug;

    private int selectedAbility;
    private float currentDuration;
    private float currentCooldown;
    private bool onExitFlag;
    private bool resetFlag;
    private int previousSelectedAbility;



    //Private Components
    private MeshRenderer meshRenderer;
    private MovementController movementController;
    private PlayerSoundManager player;


    private Quaternion modelRotOffset;
    private List<bool> activeAbilities;
    private bool hasRolledTheDice = false;


    //Debug
    private List<Color> faceColors;
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


    private void AssignFaceColor()
    {
        faceColors = new List<Color>();
        for (int i = 0; i < GetNormalCount(); i += 3)
        {
            Color c = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 0, 1, 1, 1);
            faceColors.AddRange(new Color[] { c, c, c });
        }
    }

    private void Awake()
    {


        FetchComponents();
    }
    private void Start()
    {


        ResetAbilityStates();


        currentCooldown = selectionCooldown;
        currentDuration = selectionDuration;




        modelRotOffset = diceModelRef.transform.localRotation;


        uiIndicator.DOFade(0, 0);
        uiCD.DOFade(0, 0);


        UpdateIndicator();




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

        if (!hasRolledTheDice && !movementController.grounded && pauseInput.action.ReadValue<float>() > 0)
        {
            hasRolledTheDice = OnStartDiceRoll();
            onExitFlag = true;
            resetFlag = true;
        }
        else if (hasRolledTheDice && (TryTrackDuration() || pauseInput.action.ReadValue<float>() < 1))
        {
            if (onExitFlag)
            {
                OnExitDiceRoll();
                currentCooldown = selectionCooldown;
                onExitFlag = false;
            }



            currentCooldown -= Time.unscaledDeltaTime;
            currentCooldown = Mathf.Clamp(currentCooldown, 0, selectionCooldown);

            if (currentCooldown <= 0)
            {
                currentDuration = selectionDuration;
                hasRolledTheDice = false;
            }

        }
        else if (hasRolledTheDice && UpdateInput() != 0 && selectAbilityInput.action.triggered)
        {
            if (showDebug)
                Debug.Log($"Selected ability: {selectedAbility}");
            // RotateDiceToRandomDirs();
            UpdateIndicator();
        }


        if (resetFlag && movementController.grounded)
        {
            resetFlag = false;
            ResetAbilityStates();
        }



    }

    private int UpdateInput()
    {

        int input = Mathf.CeilToInt(selectAbilityInput.action.ReadValue<float>());
        if (input != 0 && selectAbilityInput.action.triggered)
        {
            previousSelectedAbility = selectedAbility;
            selectedAbility += input;
            selectedAbility = selectedAbility >= listOfAbilities.Count ? 0 : selectedAbility < 0 ? listOfAbilities.Count - 1 : selectedAbility;

            int attempts = 100;
            while (!activeAbilities[selectedAbility] && attempts > 0)
            {
                attempts--;
                selectedAbility += input;
                selectedAbility = selectedAbility >= listOfAbilities.Count ? 0 : selectedAbility < 0 ? listOfAbilities.Count - 1 : selectedAbility;
            }
        }
        return input;
    }

    private bool OnStartDiceRoll()
    {
        StopAllCoroutines();
        player.Play(SoundType.DieRoll);
        player.EditParamater(SoundType.DieRoll, "Dice Activate", 0);
        onExitFlag = false;


        listOfAbilities[previousSelectedAbility].ability.OnEndEffect(movementController);

        SlowdownTime();

        uiIndicator.DOFade(1, 0.15f);
        return true;
    }


    private void UpdateIndicator()
    {
        uiIcon.sprite = listOfAbilities[selectedAbility].abilityIcon;
        uiLabel.text = listOfAbilities[selectedAbility].abilityLabel;

        if (!activeAbilities[selectedAbility])
        {
            uiCD.DOFade(1, 0.15f);
        }
        else
        {
            uiCD.DOFade(0, 0.15f);
        }
    }

    private bool OnExitDiceRoll()
    {
        player.EditParamater(SoundType.DieRoll, "Dice Activate", 1);
        uiIndicator.DOFade(0, 0.15f);
        ResetTime();

        dicePivot.transform.DORotateQuaternion(Quaternion.identity, 0.15f);

        if (selectedAbility >= listOfAbilities.Count || !activeAbilities[selectedAbility]) return false;
        activeAbilities[selectedAbility] = listOfAbilities[selectedAbility].ability.TriggerEffect(movementController);



        if (showDebug)
            Debug.Log($"Ability Triggered! - {listOfAbilities[selectedAbility].abilityLabel} at {selectedAbility}");
        return false;
    }



    private void OnValidate()
    {
        AssignFaceColor();
    }

    private void OnDrawGizmos()
    {
        if (selectedAbility < listOfAbilities.Count)
            listOfAbilities[selectedAbility].ability.OnGizmosDraw(movementController);



        if (!showDebug) return;
        for (int i = 0; i < GetNormalCount(); i++)
        {
            Vector3 normal = GetNormal(i);
            Gizmos.color = faceColors[i];
            Gizmos.DrawRay(diceModelRef.transform.rotation * diceModelRef.sharedMesh.vertices[i] + diceModelRef.transform.position, diceModelRef.transform.rotation * normal);
        }


        for (int i = 0; i < 4; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(dicePivot.position, dicePivot.rotation * GetDiceSide(i));
        }





    }

    private int GetNormalCount()
    {
        if (!diceModelRef)
            return 0;
        return diceModelRef.sharedMesh.normals.Length;
    }
    private Vector3 GetNormal(int aVertexID)
    {
        var normals = diceModelRef.sharedMesh.normals;
        if (aVertexID >= normals.Length) return Vector3.up;
        return normals[aVertexID];
    }
    public Vector3 GetDiceSide(int aSideID)
    {
        int finalID = aSideID * 3;

        var normals = diceModelRef.sharedMesh.normals;

        if (finalID >= normals.Length) return Vector3.up;

        return normals[finalID];

    }

    private int GetDiceSideAmm()
    {
        return diceModelRef.mesh.normals.Length;
    }








}
