using UnityEngine;

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class OneShotAudioPlayer : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter emitter;

    void Awake()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void Play()
    {
        // FMODUnity.RuntimeManager.PlayOneShot(emitter.EventReference, GetComponent<Transform>().position);
        emitter.Play();
    }

    public void Stop()
    {
        emitter.Stop();
    }

    public void EditParamateter(string aName, float aValue)
    {
        emitter.SetParameter(aName, aValue);
    }
}
