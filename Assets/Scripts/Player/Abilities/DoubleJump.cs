using UnityEngine;

[CreateAssetMenu(fileName = "New Double Jump", menuName = "Player Abilities/Double Jump", order = 0)]
public class DoubleJump : ScriptableAbility
{
    [SerializeField] float jumpForceMultiplier = 1.0f;
    // [SerializeField] float allowJumpVerticalVelocityThreshold = 0.5f;

    public override bool ApplyEffect(MovementController player)
    {
        Debug.Log("Double Jump Triggered!");

        // Already double jumping
        if (player.jumpCount > 1)
        {
            return false;
        }

        // Jump again
        player.Jump(player.jumpForce * jumpForceMultiplier);

        // Play sound
        var soundManager = player.GetComponent<PlayerSoundManager>();
        soundManager.Play(SoundType.DoubleJump);
        return false;
    }

    public override void OnEndEffect(MovementController player)
    {

    }

    public override bool UpdateEffect(MovementController player)
    {
        return false;
    }


    public override void OnGizmosDraw(MovementController player) {
    }
}
