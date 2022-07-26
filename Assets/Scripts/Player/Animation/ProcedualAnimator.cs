using System;
using System.Collections;
using UnityEngine;


public class ProcedualAnimator : MonoBehaviour
{
    public float spinSpeed = 1f;
    public Vector3 targetSpinDirection, currentSpinDirection;


    PollingStation station;

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, $"Object <{gameObject.name}>"))
        {
            return;
        }


    }

    private void OnEnable()
    {
        station.abilityController.onDiceRollBegin += OnSpinDiceBegin;
        station.abilityController.onDiceRollEnd += OnSpinDiceEnd;
    }

    private void OnDisable()
    {
        station.abilityController.onDiceRollBegin -= OnSpinDiceBegin;
        station.abilityController.onDiceRollEnd -= OnSpinDiceEnd;
    }

    private void OnSpinDiceBegin(PollingStation obj)
    {
        // StartCoroutine(SpinDice());
    }

    private void OnSpinDiceEnd(PollingStation obj)
    {
        //StopCoroutine(SpinDice());
    }

    IEnumerator SpinDice() //This is not working (The coroutine is not getting called) - Spyro
    {
        Debug.Log("Starting to spin!");
        currentSpinDirection = UnityEngine.Random.insideUnitSphere;
        yield return new WaitForFixedUpdate();
        while (true)
        {
            Debug.Log("Spinning");
            currentSpinDirection = Vector3.Lerp(currentSpinDirection, targetSpinDirection, Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentSpinDirection, transform.up), Time.fixedDeltaTime * spinSpeed);
            yield return new WaitForFixedUpdate();
            if (currentSpinDirection == targetSpinDirection)
            {
                targetSpinDirection = UnityEngine.Random.insideUnitSphere;
            }
        }

    }


}
