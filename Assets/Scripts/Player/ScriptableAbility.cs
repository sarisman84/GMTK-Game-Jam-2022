using UnityEngine;
using System.Collections;

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
    protected abstract bool OnFixedUpdate(PollingStation station);
    //On Awake - happens when you jump (OnJump)
    //On Awake - happens when you select (ConstantUpdate)
    protected abstract void OnActivation(PollingStation station);
    protected abstract void OnDeactivation(PollingStation station);

    public IEnumerator OnAbilityEffect(PollingStation station)
    {
        station.abilityController.IsAnAbilityActive = true;
        OnActivation(station);
        while (OnFixedUpdate(station))
            yield return new WaitForFixedUpdate();
        OnDeactivation(station);
        station.abilityController.IsAnAbilityActive = false;
    }


    public abstract void OnCustomDrawGizmos(MovementController player);


    public static void DampValue(ref float value, float damp) {
        value = Mathf.Lerp(0, value, damp);//usage of a more performant method
        //value *= Mathf.Pow(value, Time.fixedDeltaTime);
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