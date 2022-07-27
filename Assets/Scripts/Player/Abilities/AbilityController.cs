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
    private const float defaultTimeScale = 1f;

    public float slowMotionModifier = 0.05f;
    public int maxAbilityUseCount = 3;
    public List<ScriptableAbility> abilities;



    private PollingStation station;
    private Queue<int> queuedAbilitiesToUse;
    private bool diceRollInput;


    public event Action<PollingStation> onDiceRollBegin, onDiceRollEnd;
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

    private void OnEnable()
    {
        onDiceRollBegin += SlowdownTime;
        onDiceRollEnd += ResetTime;
    }

    private void OnDisable()
    {
        onDiceRollBegin -= SlowdownTime;
        onDiceRollEnd -= ResetTime;
    }

    private void Start()
    {
        queuedAbilitiesToUse = new Queue<int>();


        currentAbilityUseCount = maxAbilityUseCount;



        StartCoroutine(CustomUpdate());
    }

    void SlowdownTime(PollingStation station)
    {
        var abilityController = station.abilityController;
        abilityController.ModifyTime(abilityController.slowMotionModifier);
    }


    void ResetTime(PollingStation station)
    {
        var abilityController = station.abilityController;
        abilityController.ModifyTime(defaultTimeScale);
    }

    void ModifyTime(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = scale / 100f;
    }


    private void Update()
    {
        //Debug.Log($"Ability Controller: {transform.position}");
        if (station.movementController.grounded)
        {
            currentAbilityUseCount = maxAbilityUseCount;
            //Debug.Log("Resetting Ability Count");
        }

        diceRollInput = station.inputManager.GetButton(InputManager.InputPreset.DiceRoll) && currentAbilityUseCount > 0 && !station.movementController.grounded;


    }
    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            if (diceRollInput)
            {
                if (onDiceRollBegin != null)
                    onDiceRollBegin(station);


                station.abilityDisplay.SetHotbarActive(true, 0.15f * Time.unscaledDeltaTime);

                int selectedAbility = 0;
                station.abilityDisplay.UpdateHotbarSelectionIndicator(selectedAbility, 0.15f * Time.unscaledDeltaTime);
                while (diceRollInput)
                {

                    ScrollThroughAbilities(ref selectedAbility);
                    yield return new WaitForEndOfFrame();//sync this loop to the frames to catch every input
                }



                if (onDiceRollEnd != null)
                    onDiceRollEnd(station);


                station.abilityDisplay.SetHotbarActive(false, 0.15f, true);

                if (abilities[selectedAbility].abilityType == ScriptableAbility.AbilityType.Jump)
                    queuedAbilitiesToUse.Enqueue(selectedAbility);
                else
                    StartCoroutine(abilities[selectedAbility].OnAbilityEffect(station));
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


#if UNITY_EDITOR
    MovementController player_cache;
    private void OnDrawGizmos()
    {
        if (!player_cache)
            player_cache = FindObjectOfType<MovementController>();

        foreach (ScriptableAbility ability in abilities)
        {
            ability.OnCustomDrawGizmos(player_cache);
        }
    }
#endif
}