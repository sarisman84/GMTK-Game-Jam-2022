using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Ability", menuName = "Abilities/Test/Select", order = 0)]
public class TestSelect : ScriptableAbility
{
    protected override bool OnFixedUpdate(PollingStation station)
    {
        return false;
    }

    protected override void OnActivation(PollingStation station)
    {
        Debug.Log("TestSelect Executed!");
    }

    protected override void OnDeactivation(PollingStation station)
    {

    }
}
