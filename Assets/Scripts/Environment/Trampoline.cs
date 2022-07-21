using UnityEngine;

public class Trampoline : MonoBehaviour, IPlayerGround
{

    public float jumpVel = 10;

    public void OnPlayerStand(MovementController player)
    {
        player.ApplyForce(Vector3.up, jumpVel);
    }
}
