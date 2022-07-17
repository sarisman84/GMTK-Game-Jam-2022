using UnityEngine;

[CreateAssetMenu(fileName = "New Dash", menuName = "Player Abilities/Dash", order = 2)]
public class Dash : ScriptableAbility {
    [SerializeField] float Distance = 20f;
    [SerializeField] float Speed = 30;
    float dash = 0;
    int dashDir;

    public override void ApplyEffect(MovementController player) {
        Debug.Log("Dash");
        dash = Distance / Speed;// = duration
        dashDir = player.facingDir;
        player.takeInput = false;
        player.GravityScale = 0;
    }

    public override void OnEndEffect(MovementController player) {
        dash = 0;
        player.takeInput = true;
        player.GravityScale = 1;
    }

    public override void UpdateEffect(MovementController player) {
        if (dash == 0) return;//no dash effect active

        if (dash > 0) {
            dash -= Time.fixedDeltaTime;
            player.vel.x = dashDir * Speed;
            player.vel.y = 0;
        } else {
            OnEndEffect(player);
        }
    }


    public override void OnGizmosDraw(MovementController player) {
    }
}