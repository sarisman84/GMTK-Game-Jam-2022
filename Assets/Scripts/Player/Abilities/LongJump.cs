using UnityEngine;

[CreateAssetMenu(fileName = "New Long Jump", menuName = "Player Abilities/Long Jump", order = 3)]
public class LongJump : ScriptableAbility
{
    public float Force = 15;

    private void DoLongJump(MovementController player)
    {
        Debug.Log("Long Jump");
        player.vel.y = Force;
    }

    public override bool ApplyEffect(MovementController player)
    {
        if (player.jumpOverride.Count < 3)//dont exceed 3 override jumps
            player.jumpOverride.Push(DoLongJump);
        return false;
    }

    public override void OnEndEffect(MovementController player)
    {
    }

    public override bool UpdateEffect(MovementController player)
    {
        return false;
    }

    public override void OnGizmosDraw(MovementController player)
    {
    }
}