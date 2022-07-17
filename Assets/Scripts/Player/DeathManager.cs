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

        if(player.transform.position != null)//if player exists
            SetRespawnPoint(player.transform.position);//set respawn point to current position
    }

    public MovementController player;
    public Vector3 spawnPoint;

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        player = FindObjectOfType<MovementController>();
        SetRespawnPoint(player.transform.position);
    }

    public void SetRespawnPoint(Vector3 pos) {
        spawnPoint = pos;
    }

    public void OnDeath() {
        Debug.Log("YOU DIED");
        player.transform.position = spawnPoint;
        player.vel = Vector2.zero;
    }
}
