using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability", order = 0)]
public abstract class ScriptableAbility : ScriptableObject
{

    public abstract void ApplyEffect(GameObject player);
}


//[CreateAssetMenu(fileName = "New Jump Ability", menuName = "Custom/Ability", order = 0)]
//public class Jump : ScriptableAbility
//{

//    public override void ApplyEffect(GameObject player)
//    {

//    }
//}