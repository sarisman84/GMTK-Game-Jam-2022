using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability", order = 0)]
public abstract class ScriptableAbility : ScriptableObject
{
    public enum AbilityType
    {
        Jump, Select
    }

    public AbilityType abilityType;
    public Sprite abilityIcon;
    public string abilityLabel;

    //Update Loop
    protected abstract bool OnFixedUpdate(MovementController player, AbilityController abilityController);
    //On Awake - happens when you jump (OnJump)
    //On Awake - happens when you select (ConstantUpdate)
    protected abstract void OnActivation(MovementController player, AbilityController abilityController);


    public IEnumerator OnAbilityEffect(MovementController player, AbilityController abilityController)
    {
        OnActivation(player, abilityController);
        while (OnFixedUpdate(player, abilityController))
            yield return new WaitForFixedUpdate();
    }

}



/*
 Currently Queued Abilities | Add Order | Use Order
          LongJump                1           3
          HighJump                2           2
          LongJump                3           1 <-- Remove this (Dequeue)

 
 void FixedUpdate()
{
    //jump logic
    if(jump)
    AbilityController.ExecuteLastAbility(Jump)


    //sprint logic
    if(sprint)
    AbilityController.ExecuteLastAbility(Sprint)
}
 
void ExecuteLastAbility(type)
{
    StopCoroutine(currentAbility) //Stops the current ability loop entirely
    StartCoroutine(newAbility) 
    Dequeue(newAbility)


}

IEnumerator AbilityEffect(*parameters*)
{
    some logic here

    while(x == true)
    {
        yield return new WaitForFixedUpdate();
        more logic here
        

        if(y == true)
        x = false;
    }

    yield return null;

}
 

//MovementController

void FixedUpdate()
{
    

    if(PlayerHasAbility(OnJump)) //This if statement is for the jump
    {
        ExecuteAbility(OnJump); //Jump override
    }
    else
    {
        //Original logic
    }

}



void OnActivation(player)
{
    float newJumpHeight = 20;
    float newGravity = 30;

    //Get the jump method

    player.Jump(newJumpHeight, newGravity);


    player.jumpCount++;
    player.SetTempDirection(10,0,0) ----->>


}
 
 */