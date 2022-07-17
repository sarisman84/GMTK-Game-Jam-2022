using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string scene;

    #if UNITY_EDITOR//only check the scene if in the unity editor
    private void Awake() {
        if (SceneUtility.GetBuildIndexByScenePath(scene) == -1)//try to get the scene to check for errors
            Debug.LogError("Scene \"" + scene + "\" cant be found");
    }
    #endif

    public static void LoadLevel(string name) {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            LoadLevel(scene);
        }
    }
}
