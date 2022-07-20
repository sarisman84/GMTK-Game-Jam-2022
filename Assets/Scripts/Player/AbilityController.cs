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
    private int currentAbilityUseCount;

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, gameObject.name))
        {
            return;
        }

        station.abilityController = this;
        queuedAbilitiesToUse = new Queue<int>();

        station.abilityDisplay.SetHotbarActive(false, 0.15f, true);
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
        if (station.movementController.grounded)
        {
            currentAbilityUseCount = maxAbilityUseCount;
        }

        diceRollInput = station.inputManager.GetButton(InputManager.InputPreset.DiceRoll) && currentAbilityUseCount > 0 && !station.movementController.grounded;
    }
    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            if (diceRollInput)
            {
                station.abilityDisplay.SetHotbarActive(true, 0.15f);
                ModifyTimeScale(slowMotionModifier);
                int selectedAbility = 0;
                station.abilityDisplay.UpdateHotbarSelectionIndicator(selectedAbility, 0.15f);
                while (diceRollInput)
                {
                    ScrollThroughAbilities(ref selectedAbility);
                    yield return null;
                }
                station.abilityDisplay.SetHotbarActive(false, 0.15f);
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
        if (HasQueuedAbilities())
        {
            var ability = queuedAbilitiesToUse.Peek();
            abilities[ability].OnAbilityEffect(station, true);
        }
    }

    private void ScrollThroughAbilities(ref int selectedAbility)
    {
        int input = Mathf.CeilToInt(station.inputManager.GetSingleAxis(InputManager.InputPreset.SelectAbility));

        if (input != 0 && station.inputManager.GetAction(InputManager.InputPreset.SelectAbility).triggered)
        {
            selectedAbility += input;
            selectedAbility = selectedAbility < 0 ? abilities.Count - 1 : selectedAbility >= abilities.Count ? 0 : selectedAbility;
            station.abilityDisplay.UpdateHotbarSelectionIndicator(selectedAbility, 0.15f);
        }

    }
}