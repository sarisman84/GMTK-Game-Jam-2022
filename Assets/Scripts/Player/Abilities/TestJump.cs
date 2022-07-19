﻿using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Ability", menuName = "Abilities/Test/Jump", order = 0)]
public class TestJump : ScriptableAbility
{
    protected override bool OnFixedUpdate(MovementController player, AbilityController abilityController)
    {
        return false;
    }


    IEnumerator ResetAfterDelay(float someDelay, AbilityController abilityController)
    {
        yield return new WaitForSecondsRealtime(someDelay);
        abilityController.ClearLastAbility(AbilityType.Jump);
    }

    protected override void OnActivation(MovementController player, AbilityController abilityController)
    {
        Debug.Log("TestJump Executed!");
        player.Jump(100);
        //jump

        //player.jumpPress = 0;//dequeue jump
        //player.groundedTimer = 0;//set to be in air

        player.StartCoroutine(ResetAfterDelay(0.2f, abilityController));
    }



}

