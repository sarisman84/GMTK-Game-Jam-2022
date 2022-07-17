using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability", order = 0)]
public abstract class ScriptableAbility : ScriptableObject
{
<<<<<<< HEAD
=======


    public bool TriggerEffect(MovementController player)
    {
        bool result = ApplyEffect(player);
        player.StartCoroutine(BackendUpdate(player));

        return result;

    }


    private IEnumerator BackendUpdate(MovementController player)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!UpdateEffect(player))
            {
                break;
            }
        }
        yield return null;
    }

    public abstract bool ApplyEffect(MovementController player);
    public abstract bool UpdateEffect(MovementController player);
    public abstract void OnEndEffect(MovementController player);
>>>>>>> main

    public abstract void ApplyEffect(GameObject player);
}
