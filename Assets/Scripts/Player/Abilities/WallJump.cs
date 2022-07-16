using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Jump", menuName = "Player Abilities/Wall Jump", order = 0)]
public class WallJump : ScriptableAbility {

    [Header("Wall Check")]
    public float checkXOffset = 0.5f;
    public Vector2 checkSize = Vector2.one;

    [Space]
    [Header("Jump")]
    public Vector2 jumpForce = Vector2.one;

    private bool IsOnWall(MovementController player) {
        return Physics2D.OverlapBox(
            (Vector2)player.transform.position + Vector2.right * (checkXOffset - 0.5f * checkSize.x) * player.facingDir,//use the facing direction to check the right direction for a wall jump 
            checkSize, 0, player.groundedLayer);
    }

    public override void ApplyEffect(MovementController player) {
        Debug.Log("Wall Jump Requested");

        if (IsOnWall(player)) {
            //Execute Wall Jump
            player.vel += new Vector2(jumpForce.x * -player.facingDir, jumpForce.y);//use the wall side to jump in the opposite direction
        }
    }

    public override void OnEndEffect(MovementController player) {
        
    }

    public override void UpdateEffect(MovementController player) {
        
    }


    public override void OnGizmosDraw(MovementController player) {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)player.transform.position + Vector2.right * (checkXOffset - 0.5f * checkSize.x) * player.facingDir, checkSize);
    }
}
