using UnityEngine;

[CreateAssetMenu(fileName = "New Double Jump", menuName = "Player Abilities/Double Jump", order = 0)]
public class DoubleJump : ScriptableAbility
{
    [SerializeField] float jumpForceMultiplier = 1.0f;
    [SerializeField] float allowJumpVerticalVelocityThreshold = 0.5f;

    public override void ApplyEffect(MovementController player)
    {
        Debug.Log("Double Jump Triggered!");

        // Already double jumping
        if (player.jumpCount > 1)
        {
            return;
        }

        // Jump again
        //player.vel.y = player.jumpForce * jumpForceMultiplier;

        player.Jump(player.jumpForce * jumpForceMultiplier);
        // TODO: Play sound once the FMod event is implemented
        // var emitter = player.GetComponent<FMODUnity.StudioEventEmitter>();
        // emitter.Play();
    }

    public override void OnEndEffect(MovementController player)
    {

    }

    public override void UpdateEffect(MovementController player)
    {

    }


    public override void OnGizmosDraw(MovementController player) {
    }
}
