using UnityEngine;

public class PollingStation : MonoBehaviour
{
    //everyone references the pollingstation oj -> access to the manager

    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public MusicManager musicManager;
    [HideInInspector] public ParticleManager particleManager;

    [HideInInspector] public AbilityDisplay abilityDisplay;
    [HideInInspector] public AbilityController abilityController;
    [HideInInspector] public MovementController movementController;

    private static bool isCreated = false;//try to only have ONE PollingStation at a time


    public static bool TryRegisterStationToGameObject(ref PollingStation pollingStation, string debugInfo = "")
    {
        pollingStation = GameObject.FindObjectOfType<PollingStation>();

        if (!pollingStation)
        {
            Debug.LogError($"{debugInfo} - Following Object could not find a Polling Station");
            return false;
        }

        return true;
    }

    private void Awake()
    {
        if (isCreated)
        {//check if a PollingStation was already created
            Debug.LogError($"There is more than one PollingStation active! Removing {gameObject.name}!", gameObject);
            Destroy(gameObject);
            return;
        }

        isCreated = true;
        Debug.Log("PollingStation enabled");
        DontDestroyOnLoad(gameObject);//dont destroy this gameobject



        //fetch the Managers in the children -> easy to access
        inputManager = GetComponentInChildren<InputManager>();
        musicManager = GetComponentInChildren<MusicManager>();
        particleManager = GetComponentInChildren<ParticleManager>();

#if UNITY_EDITOR
        if (!inputManager) Debug.LogError("No InputManager found");
        if (!musicManager) Debug.LogError("No MusicManager found");
        if (!particleManager) Debug.LogError("No ParticleManager found");
#endif
    }
}
