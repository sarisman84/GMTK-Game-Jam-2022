using UnityEngine;

[CreateAssetMenu(fileName = "Double Jump Ability", menuName = "Custom/Ability", order = 0)]
public class DoubleJump : ScriptableAbility
{
  [SerializeField] float jumpForceMultiplier = 1.0f;
  [SerializeField] float allowJumpVerticalVelocityThreshold = 0.5f;

  public override void ApplyEffect(GameObject player)
  {
    var movementController = player.GetComponent<MovementController>();

    // Already double jumping
    if (movementController.jumpCount > 2 || movementController.vel.y > allowJumpVerticalVelocityThreshold)
    {
      return;
    }

    // Jump again
    movementController.vel.y = movementController.jumpForce * jumpForceMultiplier;

    // TODO: Play sound once the FMod event is implemented
    // var emitter = player.GetComponent<FMODUnity.StudioEventEmitter>();
    // emitter.Play();
  }
}
