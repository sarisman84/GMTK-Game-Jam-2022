using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Jump", menuName = "Abilities/Wall Jump", order = 2)]
public class WallJump : ScriptableAbility {

    public float wallCheckXOffset = 1;
    public Vector2 wallCheckSize = new Vector2(0.1f, 1);

    [Space]

    public float wallSlideVel = 1;

    [Space]

    public float jumpForceX;
    public float XForceDamp = 0.1f;
    private float addVelX;



    public Vector2 WallCheckPos(MovementController player) { return player.transform.position + player.transform.right * (wallCheckXOffset + 0.5f * wallCheckSize.x) * player.facingDir; }
    public bool OnWall(MovementController player) {
        return Physics2D.OverlapBox(WallCheckPos(player), wallCheckSize, player.transform.eulerAngles.z, player.groundCheckMask);//use the facing direction to check the right direction for a wall jump 
    }

    #region Velocity Modifier
    public void AddXJump(ref Vector2 vel, MovementController player) {
        vel.x += addVelX;
        DampValue(ref addVelX, XForceDamp);
    }
    public void WallSlide(ref Vector2 vel, MovementController player) {
        if (!player.grounded && vel.y < 0)
            vel.y = wallSlideVel;//override gravity with slide velocity
    }
    #endregion

    public void Jump(MovementController player) {
        player.Jump(player.jumpForce);
        addVelX = jumpForceX * -player.facingDir;
        player.onVelocityModifier += AddXJump;
    }

    protected override void OnActivation(PollingStation station) {
        Debug.Log("WallJump");
    }

    protected override void OnDeactivation(PollingStation station) {}

    protected override bool OnFixedUpdate(PollingStation station) {
        MovementController player = station.movementController;

        if (OnWall(player)) {
            player.onVelocityModifier += WallSlide;
            if (player.jumpInput > 0 && player.currentKoyoteTime == 0)//if player cant jump of the ground, but gives a jump input
                Jump(player);
        }
        else {
            player.onVelocityModifier -= WallSlide;
        }
        return true;
    }



    public override void OnCustomDrawGizmos(MovementController player) {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(WallCheckPos(player), wallCheckSize);
    }
}
