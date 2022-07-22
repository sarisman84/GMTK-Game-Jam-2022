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

    public float slowMotionModifier = 0.05f;
    public int maxAbilityUseCount = 3;
    public List<ScriptableAbility> abilities;



    private PollingStation station;
    private Queue<int> queuedAbilitiesToUse;
    private bool diceRollInput;

    public int currentAbilityUseCount { get; private set; }


    public bool IsAnAbilityActive { private get; set; }

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, gameObject.name))
        {
            return;
        }

        station.abilityController = this;


    }

    private void Start()
    {
        queuedAbilitiesToUse = new Queue<int>();


        currentAbilityUseCount = maxAbilityUseCount;



        StartCoroutine(CustomUpdate());
    }

    void ModifyTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = scale / 100f;
    }


    private void Update()
    {
        Debug.Log($"Ability Controller: {transform.position}");
        if (station.movementController.grounded)
        {
            currentAbilityUseCount = maxAbilityUseCount;
            Debug.Log("Resetting Ability Count");
        }

        diceRollInput = station.inputManager.GetButton(InputManager.InputPreset.DiceRoll) && currentAbilityUseCount > 0 && !station.movementController.grounded;


    }
    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            if (diceRollInput)
            {
                station.abilityDisplay.SetHotbarActive(true, 0.15f * Time.unscaledDeltaTime);
                ModifyTimeScale(slowMotionModifier);
                int selectedAbility = 0;
                station.abilityDisplay.UpdateHotbarSelectionIndicator(selectedAbility, 0.15f * Time.unscaledDeltaTime);
                const float refreshRate = 1f / 60f;
                while (diceRollInput)
                {

                    ScrollThroughAbilities(ref selectedAbility);
                    yield return new WaitForSecondsRealtime(refreshRate);

                }
                station.abilityDisplay.SetHotbarActive(false, 0.15f, true);
                ModifyTimeScale(defaultScale);
                if (abilities[selectedAbility].abilityType == ScriptableAbility.AbilityType.Jump)
                    queuedAbilitiesToUse.Enqueue(selectedAbility);
                else
                    abilities[selectedAbility].OnAbilityEffect(station);
                currentAbilityUseCount--;

            }


            yield return null;
        }
    }

    public bool HasQueuedAbilities()
    {
        return queuedAbilitiesToUse.Count > 0;
    }

    public void ClearLatestQueuedAbility()
    {
        queuedAbilitiesToUse.Dequeue();
    }

    public void ExecuteQueuedAbility()
    {
        if (HasQueuedAbilities() && !IsAnAbilityActive)
        {
            var ability = queuedAbilitiesToUse.Dequeue();
            StartCoroutine(abilities[ability].OnAbilityEffect(station));
        }
    }

    private void ScrollThroughAbilities(ref int selectedAbility)
    {
        int input = Mathf.CeilToInt(station.inputManager.GetSingleAxis(InputManager.InputPreset.SelectAbility));

        if (input != 0 && station.inputManager.GetAction(InputManager.InputPreset.SelectAbility).triggered)
        {
            selectedAbility += input;
            selectedAbility = selectedAbility < 0 ? abilities.Count - 1 : selectedAbility >= abilities.Count ? 0 : selectedAbility;
            station.abilityDisplay.UpdateHotbarSelectionIndicator(selectedAbility, 0.15f * Time.unscaledDeltaTime);
            Debug.Log($"Selecting Ability: {selectedAbility}");
        }

    }
}
