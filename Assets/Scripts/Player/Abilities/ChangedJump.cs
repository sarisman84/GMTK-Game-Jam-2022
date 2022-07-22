using UnityEngine;

[CreateAssetMenu(fileName = "New Changed Jump", menuName = "Abilities/Changed Jump", order = 3)]
public class ChangedJump : ScriptableAbility
{
    public float jumpHeight;

    public float jumpForceX;
    public float XForceDamp = 0.1f;
    private float addVelX;

    public void ModifyVelocity(ref Vector3 vel) {
        vel.x += addVelX;
        addVelX *= Mathf.Pow(XForceDamp, Time.fixedDeltaTime);//TODO: maybe use a more performant method here
    }


    protected override void OnActivation(PollingStation station)
    {
        MovementController player = station.movementController;

        float jumpForceY = player.HeightToForce(jumpHeight);

        addVelX = jumpForceX * player.facingDir;
        player.onVelocityModifier += ModifyVelocity;
    }

    public void OnEndEffect(MovementController player)
    {
        //Dont end effect on external call -> velocity should be equally modified after jump
    }

    protected override bool OnFixedUpdate(PollingStation station)
    {
        return addVelX*addVelX > 0.001f;//if modifying the velocity is still necessary
    }

    protected override void OnDeactivation(PollingStation station)
    {
        //Debug.Log("deactivate");
        station.movementController.onVelocityModifier -= ModifyVelocity;
    }
}



