using System;
using System.Collections;
using UnityEngine;


public class ProcedualAnimator : MonoBehaviour
{
    public float spinSpeed = 10f;
    public Vector3 targetSpinDirection;


    PollingStation station;
    private bool isSpinning;

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, $"Object <{gameObject.name}>"))
        {
            return;
        }


    }



    private void Start()
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
        Debug.Log("Starting to spin!");
        targetSpinDirection = UnityEngine.Random.insideUnitSphere;
        targetSpinDirection = new Vector3(Mathf.CeilToInt(targetSpinDirection.x), Mathf.CeilToInt(targetSpinDirection.y), Mathf.CeilToInt(targetSpinDirection.z));
        isSpinning = true;
    }

    private void OnSpinDiceEnd(PollingStation obj)
    {
        isSpinning = false;

    }

    private void FixedUpdate()
    {
        if (!isSpinning)
        {
            transform.rotation = Quaternion.identity;
            return;
        }

        Debug.Log("Spinning");

        transform.rotation *= Quaternion.AngleAxis(spinSpeed, targetSpinDirection);
    }

}



