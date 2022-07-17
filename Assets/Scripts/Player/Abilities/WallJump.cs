using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Jump", menuName = "Player Abilities/Wall Jump", order = 1)]
public class WallJump : ScriptableAbility {

    public Vector2 jumpForce = new Vector2(5, 20);
    public float pushForceDamp = 0.1f;
    private Vector2 addVel = Vector2.zero;

    public override void ApplyEffect(MovementController player) {
        if (player.onWall) {
            //Execute Wall Jump
            Debug.Log("Wall Jump Executed");
            player.vel += new Vector2(0, jumpForce.y);
            addVel = Vector2.right * jumpForce.x * -player.facingDir;
        }
    }

    public override void OnEndEffect(MovementController player) {
        player.offsetVel = Vector2.zero;
    }

    public override void UpdateEffect(MovementController player) {
        player.offsetVel = addVel;
        addVel *= Mathf.Pow(pushForceDamp, Time.deltaTime);
    }


    public override void OnGizmosDraw(MovementController player) {

    }
}
