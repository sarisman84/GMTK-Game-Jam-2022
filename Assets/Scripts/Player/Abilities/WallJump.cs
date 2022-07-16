using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Jump", menuName = "Player Abilities/Wall Jump", order = 0)]
public class WallJump : ScriptableAbility {

    [Header("Wall Check")]
    public float checkXOffset = 0.5f;
    public Vector2 checkSize = Vector2.one;

    [Space]
    [Header("Jump")]
    public float jumpForce = 10;

    private bool IsOnWall(MovementController player) {
        return Physics2D.OverlapBox(
            (Vector2)player.transform.position + Vector2.right * (checkXOffset + 0.5f * checkSize.x) * player.facingDir,//use the facing direction to check the right direction for a wall jump 
            checkSize, 0, player.groundedLayer);
    }

    public override void ApplyEffect(MovementController player) {
        if (IsOnWall(player)) {
            //Execute Wall Jump
            Debug.Log("Wall Jump Executed");
            player.vel += new Vector2(0, jumpForce);
        }
    }

    public override void OnEndEffect(MovementController player) {
        
    }

    public override void UpdateEffect(MovementController player) {
        
    }


    public override void OnGizmosDraw(MovementController player) {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)player.transform.position + Vector2.right * (checkXOffset + 0.5f * checkSize.x) * player.facingDir, checkSize);
    }
}
