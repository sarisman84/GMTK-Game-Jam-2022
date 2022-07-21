using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class DeathController : MonoBehaviour
{
    private MovementController player;
    [HideInInspector] public Vector3 spawnPoint;

    private void Awake() {
        player = GetComponent<MovementController>();
        SetRespawnPoint(player.transform.position);
    }

    public void SetRespawnPoint(Vector3 pos) {
        spawnPoint = pos;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Death")
            OnDeath();
    }

    public void OnDeath() {
        Debug.Log("YOU DIED");
        player.transform.position = spawnPoint;
        player.velocity = Vector2.zero;
    }
}
