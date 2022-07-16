using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            DeathManager.instance.OnDeath();
        }
    }
}
