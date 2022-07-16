using UnityEngine;

[CreateAssetMenu(fileName = "New Double Jump", menuName = "Player Abilities/Double Jump", order = 0)]
public class DoubleJump : ScriptableAbility
{
    [SerializeField] float jumpForceMultiplier = 1.0f;
    [SerializeField] float allowJumpVerticalVelocityThreshold = 0.5f;

    public override void ApplyEffect(MovementController player)
    {
        Debug.Log("Double Jump Triggered!");
        var movementController = player.GetComponent<MovementController>();

        // Already double jumping
        if (movementController.jumpCount > 1)
        {
            return;
        }

        // Jump again
        //movementController.vel.y = movementController.jumpForce * jumpForceMultiplier;

        movementController.Jump(movementController.jumpForce * jumpForceMultiplier);
        // TODO: Play sound once the FMod event is implemented
        // var emitter = player.GetComponent<FMODUnity.StudioEventEmitter>();
        // emitter.Play();
    }

    public override void OnEndEffect(MovementController movementController)
    {

    }

    public override void UpdateEffect(MovementController movementController)
    {

    }
}
