using System.Collections;
using UnityEngine;


public class AudioManagerTest : MonoBehaviour
{
    public PollingStation station;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Playing Jump Sound as a Test!");
        station.audioManager.Play("Jump");
    }


}
