using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Ability", menuName = "Abilities/Test/Select", order = 0)]
public class TestSelect : ScriptableAbility
{
    protected override bool OnFixedUpdate(MovementController player, AbilityController abilityController)
    {
        return false;
    }

    protected override void OnActivation(MovementController player, AbilityController abilityController)
    {
        Debug.Log("TestSelect Executed!");
    }
}
