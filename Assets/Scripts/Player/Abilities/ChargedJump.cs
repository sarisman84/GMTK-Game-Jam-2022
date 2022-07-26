using UnityEngine;

[CreateAssetMenu(fileName = "New Changed Jump", menuName = "Abilities/Changed Jump", order = 1)]
public class ChargedJump : ScriptableAbility
{
    public float jumpHeight;

    public float jumpForceX;
    public float XForceDamp = 0.98f;
    public float minXVel = 0.1f;
    private float addVelX;

    public void AddXJump(ref Vector2 vel, MovementController player) {
        vel.x += addVelX;
        DampValue(ref addVelX, XForceDamp);
    }


    protected override void OnActivation(PollingStation station)
    {
        Debug.Log("ChargedJump");
        MovementController player = station.movementController;

        float jumpForceY = MovementController.HeightToForce(jumpHeight, player.upGravity);
        player.ApplyForce(Vector2.up * jumpForceY);

        addVelX = jumpForceX * player.facingDir;
        player.onVelocityModifier += AddXJump;
    }

    public void OnEndEffect(MovementController player)
    {
        //Dont end effect on external call -> velocity should be equally modified after jump
    }

    protected override bool OnFixedUpdate(PollingStation station)
    {
        return Mathf.Abs(addVelX) > minXVel;//if modifying the velocity is still necessary
    }

    protected override void OnDeactivation(PollingStation station)
    {
        Debug.Log("deactivate");
        station.movementController.onVelocityModifier -= AddXJump;
    }



    public override void OnCustomDrawGizmos(MovementController player) { }
}