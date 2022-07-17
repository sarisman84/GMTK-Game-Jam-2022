using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Jump", menuName = "Player Abilities/Wall Jump", order = 1)]
public class WallJump : ScriptableAbility {
    [Space]
    [Header("Jump")]
    public float jumpForce = 10;

    public override bool ApplyEffect(MovementController player) {
        if (player.onWall) {
            //Execute Wall Jump
            Debug.Log("Wall Jump Executed");
            player.vel += new Vector2(0, jumpForce);
        }
        return false;
    }

    public override void OnEndEffect(MovementController player) {
        
    }

    public override bool UpdateEffect(MovementController player) {
        return false;
    }


    public override void OnGizmosDraw(MovementController player) {

    }
}