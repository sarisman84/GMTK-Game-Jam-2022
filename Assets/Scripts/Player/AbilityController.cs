using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using FMODUnity;


public class AbilityController : MonoBehaviour
{



    const float defaultScale = 1f;
    const float defaultFixedDelta = 0.01f;


    [Header("Time Parameters")]
    public float slowMotionMultiplier;
    [Header("Ability Usage Settings")]
    public int ammOfAbilitesUsedInARow = 3;
    public InputActionAsset inputAsset;


    public List<ScriptableAbility> abilities;
    public int currentSelectedAbility { get; private set; }
    public int abilityUseCount { get; set; }

    private int lastAbility = -1;
    private bool isSelectingAbilities;


    private MovementController player;


    enum TimeState
    {
        Slowed, Normal
    }

    private void Awake()
    {
        player = GetComponent<MovementController>();

        StartCoroutine(CustomUpdate());


    }
    private void Update()
    {
        isSelectingAbilities = CanUseAbilities();

        if (player.grounded)
        {
            abilityUseCount = ammOfAbilitesUsedInARow;
        }




    }

    public void ClearLastAbility(ScriptableAbility.AbilityType aType)
    {
        if (IsInListBounds(lastAbility))
            if (abilities[lastAbility].abilityType == aType)
            {
                StopCoroutine(abilities[lastAbility].OnAbilityEffect(player, this));
                lastAbility = -1;
            }

    }

    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            yield return null;
            if (isSelectingAbilities && abilityUseCount > 0)
            {
                if (IsInListBounds(lastAbility))
                {
                    if (abilities[lastAbility].abilityType == ScriptableAbility.AbilityType.Select)
                        StopCoroutine(abilities[lastAbility].OnAbilityEffect(player, this));
                }
                ModifyTime(TimeState.Slowed);
                while (isSelectingAbilities)
                {
                    OnAbilitySelect();
                    yield return null;
                }
                ModifyTime(TimeState.Normal);

                lastAbility = currentSelectedAbility;
                if (abilities[lastAbility].abilityType == ScriptableAbility.AbilityType.Select)
                    StartCoroutine(abilities[currentSelectedAbility].OnAbilityEffect(player, this));
                abilityUseCount--;

            }
            yield return null;
        }

    }
    public bool IsLastAbilityAvailable(ScriptableAbility.AbilityType aType)
    {
        if (IsInListBounds(lastAbility))
            return abilities[lastAbility] && abilities[lastAbility].abilityType == aType;
        return false;
    }

    public void ExecuteAbility(ScriptableAbility.AbilityType aTypeToExecute)
    {
        if (abilities[lastAbility].abilityType == aTypeToExecute)
        {
            StartCoroutine(abilities[lastAbility].OnAbilityEffect(player, this));

        }

    }




    bool CanUseAbilities()
    {
        return !player.grounded && inputAsset.FindAction("RollDice").ReadValue<float>() > 0;
    }


    void ModifyTime(TimeState aNewState)
    {
        switch (aNewState)
        {
            case TimeState.Slowed:
                Time.timeScale = slowMotionMultiplier;
                Time.fixedDeltaTime = Time.fixedDeltaTime * slowMotionMultiplier;
                break;
            case TimeState.Normal:
                Time.timeScale = defaultScale;
                Time.fixedDeltaTime = defaultFixedDelta;
                break;
            default:
                break;
        }
    }


    void OnAbilitySelect()
    {

        var action = inputAsset.FindAction("SelectAbility");

        bool pressedThisFrame = action.triggered;
        int selectionInput = (int)action.ReadValue<float>();
        //Display UI (different class)
        //Fetch the Input for the player to select stuff
        //Pause the Input for the movement
        //Update the currentSelectedAbility with the correct ability

        if (selectionInput != 0 && pressedThisFrame)
        {
            currentSelectedAbility += selectionInput;
            currentSelectedAbility = currentSelectedAbility < 0 ? abilities.Count - 1 : currentSelectedAbility >= abilities.Count ? 0 : currentSelectedAbility;
        }

        Debug.Log($"Selecting {abilities[currentSelectedAbility].name}");

    }



    bool IsInListBounds(int anInput)
    {
        return anInput >= 0 && anInput < abilities.Count;
    }





}
