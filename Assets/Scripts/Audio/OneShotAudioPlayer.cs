using UnityEngine;

public class OneShotAudioPlayer : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter emitter;

    void Awake()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void Play() {
        // FMODUnity.RuntimeManager.PlayOneShot(emitter.EventReference, GetComponent<Transform>().position);
        emitter.Play();
    }

    public void Stop() {
        emitter.Stop();
    }
}
