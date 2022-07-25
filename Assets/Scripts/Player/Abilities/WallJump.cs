using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Jump", menuName = "Abilities/Wall Jump", order = 2)]
public class WallJump : ScriptableAbility {

    public float wallCheckXOffset = 1;
    public Vector2 wallCheckSize = new Vector2(0.1f, 1);

    [Space]

    public float wallSlideVel = 3;

    [Space]

    public float jumpForceX;
    public float XForceDamp = 0.98f;
    public float minXVel = 0.1f;
    private float addVelX;

    private bool onWall = false;

    public Vector2 WallCheckPos(MovementController player) { return player.transform.position + player.transform.right * (wallCheckXOffset + 0.5f * wallCheckSize.x) * player.facingDir; }
    public bool OnWall(MovementController player) {
        return Physics2D.OverlapBox(WallCheckPos(player), wallCheckSize, player.transform.eulerAngles.z, player.groundCheckMask);//use the facing direction to check the right direction for a wall jump 
    }

    #region Velocity Modifier
    public void AddXJump(ref Vector2 vel, MovementController player) {
        vel.x += addVelX;
        DampValue(ref addVelX, XForceDamp);

        if (Mathf.Abs(addVelX) < minXVel)//if AddXJump is done
            player.onVelocityModifier -= AddXJump;//remove it
    }
    public void WallSlide(ref Vector2 vel, MovementController player) {
        if (!onWall) {
            player.onVelocityModifier -= WallSlide;
            return;
        }


        if (!player.grounded && vel.y < 0)
            vel.y = -wallSlideVel;//override gravity with slide velocity          
    }
    #endregion

    public void Jump(MovementController player) {
        player.facingDir *= -1;

        player.Jump(player.jumpForce);
        addVelX = jumpForceX * player.facingDir;
        player.onVelocityModifier += AddXJump;
    }

    protected override void OnActivation(PollingStation station) {
        Debug.Log("WallJump");
    }

    protected override void OnDeactivation(PollingStation station) {
        onWall = false;
        station.movementController.onVelocityModifier -= WallSlide;
    }

    protected override bool OnFixedUpdate(PollingStation station) {
        MovementController player = station.movementController;

        onWall = OnWall(player);
        if (onWall) {
            player.onVelocityModifier += WallSlide;
            if (station.inputManager.GetButton(InputManager.InputPreset.Jump) && player.currentKoyoteTime <= 0) {//if player cant jump of the ground, but gives a jump input
                Debug.Log("Jump of the wall");
                Jump(player);
                return false;
            }
        }
        return true;
    }



    public override void OnCustomDrawGizmos(MovementController player) {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(WallCheckPos(player), wallCheckSize);
    }
}
