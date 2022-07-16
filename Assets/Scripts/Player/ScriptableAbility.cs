using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability", order = 0)]
public abstract class ScriptableAbility : ScriptableObject
{
    public abstract void ApplyEffect(MovementController movementController);
    public abstract void UpdateEffect(MovementController movementController);
    public abstract void OnEndEffect(MovementController movementController);
}
