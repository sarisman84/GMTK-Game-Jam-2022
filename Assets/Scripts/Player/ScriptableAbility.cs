using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Custom/Ability", order = 0)]
public abstract class ScriptableAbility : ScriptableObject
{
    public abstract bool ApplyEffect(MovementController player);
    public abstract bool UpdateEffect(MovementController player);
    public abstract void OnEndEffect(MovementController player);


    public abstract void OnGizmosDraw(MovementController player);
}
