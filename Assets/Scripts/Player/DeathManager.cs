using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public static DeathManager instance;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("There is more than one DeathManager");
            return;
        }
        instance = this;

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public MovementController player;
    public Vector2 spawnPoint;

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        player = FindObjectOfType<MovementController>();
    }

    public void SetRespawnPoint(Vector2 pos) {
        spawnPoint = pos;
    }

    public void OnDeath() {
        Debug.Log("YOU DIED");
        player.transform.position = spawnPoint;
        player.vel = Vector2.zero;
    }
}
