using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using System;

public class AudioManager : MonoBehaviour
{
    public bool showLogs = false;



    private PollingStation station;
    private Dictionary<string, EventDescription> currentEvents;
    private Dictionary<string, Bus> currentBuses;
    private Dictionary<string, List<EventInstance>> activeInstances;
    EventInstance currentMusicBackground;

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, $"Object <{gameObject.name}> "))
        {
            return;
        }

        station.audioManager = this;



        currentEvents = new Dictionary<string, EventDescription>();
        currentBuses = new Dictionary<string, Bus>();
        activeInstances = new Dictionary<string, List<EventInstance>>();


        LoadFMODData();
    }

    private void Start()
    {
        station.movementController.onJumpEvent += AudioIntegration.Jump;
        station.abilityController.onDiceRollBegin += AudioIntegration.OnDiceRollBegin;
        station.abilityController.onDiceRollEnd += AudioIntegration.OnDiceRollEnd;
    }



    private void OnDisable()
    {
        station.movementController.onJumpEvent -= AudioIntegration.Jump;
        station.abilityController.onDiceRollBegin -= AudioIntegration.OnDiceRollBegin;
        station.abilityController.onDiceRollEnd -= AudioIntegration.OnDiceRollEnd;
    }


    bool SearchForElement<T>(string name, Dictionary<string, T> dataContainer, out T value)
    {
        value = default;

        foreach (var pair in dataContainer)
        {
            if (pair.Key.ToLower().Contains(name.ToLower()))
            {
                value = pair.Value;
                return true;
            }
        }

        return false;
    }


    public void SetVolume(string groupName, float aValue)
    {
        Bus settings;
        if (!SearchForElement<Bus>(groupName, currentBuses, out settings)) return;

        settings.setVolume(aValue);
    }


    public float GetVolume(string groupName)
    {
        Bus settings;
        if (!SearchForElement(groupName, currentBuses, out settings)) return 0;
        float volume;
        if (FMOD.RESULT.OK != settings.getVolume(out volume)) return 0;
        return volume;
    }


    public void Play(string clipName, GameObject aTargetObjectToPlayOff = null, bool keepInstanceAlive = false)
    {
        EventDescription desc;
        FMOD.RESULT result;
        if (!SearchForElement(clipName, currentEvents, out desc)) return;
        EventInstance ins;
        result = desc.createInstance(out ins);
        if (FMOD.RESULT.OK != result) return;
        if (aTargetObjectToPlayOff)
            ins.set3DAttributes(RuntimeUtils.To3DAttributes(aTargetObjectToPlayOff));
        result = ins.start();


        //ins.setCallback((EVENT_CALLBACK_TYPE type, IntPtr eventVal, IntPtr parameterVal) =>
        //{
        //    ins.release();
        //    return FMOD.RESULT.OK;
        //}, EVENT_CALLBACK_TYPE.SOUND_STOPPED);

        RegisterInstance(clipName, ins);
        string path;
        result = desc.getPath(out path);
        if (result != FMOD.RESULT.OK && !path.ToLower().Contains("Music")) return;
        currentMusicBackground = ins;
    }

    public void Stop(string clipName, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
    {
        List<EventInstance> instances;
        if (!SearchForElement(clipName, activeInstances, out instances)) return;

        foreach (var item in instances)
        {
            item.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            item.release();

        }
        //Cleaning up the data set for the active instances after stopping the clip
        activeInstances.Remove(clipName);
    }

    private void RegisterInstance(string clipName, EventInstance ins)
    {
        if (!activeInstances.ContainsKey(clipName))
        {
            activeInstances.Add(clipName, new List<EventInstance>());
        }

        activeInstances[clipName].Add(ins);
    }

    public void ModifyInstance(string clipName, string parameterName, float value)
    {
        FMOD.RESULT result;

        List<EventInstance> instances;

        if (!SearchForElement(clipName, activeInstances, out instances)) return;

        foreach (var ins in instances)
        {
            result = ins.setParameterByName(parameterName, value);

        }
    }

    public void ModifyBackgroundMusic(string parameterName, float value)
    {
        if (currentMusicBackground.setParameterByName(parameterName, value) != FMOD.RESULT.OK) return;
    }











    void LoadFMODData()
    {
        var banks = Settings.Instance.Banks;
        banks.Add("Master");
        foreach (var bank in banks)
        {
            string path = $"bank:/{bank}";
            if (showLogs)
                Debug.Log($"<Log>[AudioManager]: Fetching bank ({bank}) using {path} as a path");
            Bank b;
            if (FMOD.RESULT.OK != RuntimeManager.StudioSystem.getBank(path, out b)) continue;
            if (showLogs)
            {
                Debug.Log($"<Log>[AudioManager]: Managed to fetch ({bank}) using {path} as a path!");
                Debug.Log($"<Log>[AudioManager]: Fetching events from ({bank})");
            }

            EventDescription[] events;
            if (FMOD.RESULT.OK != b.getEventList(out events)) continue;
            if (showLogs)
            {
                Debug.Log($"<Log>[AudioManager]: Managed to fetch events from ({bank})");
            }

            for (int i = 0; i < events.Length; i++)
            {

                if (FMOD.RESULT.OK != events[i].getPath(out path)) continue;
                if (showLogs)
                {
                    Debug.Log($"<Log>[AudioManager]: Adding <[Event]: {path}> to the data set.");
                }

                if (!currentEvents.ContainsKey(path))
                    currentEvents.Add(path, events[i]);
            }
            if (showLogs)
                Debug.Log($"<Log>[AudioManager]: Fetching buses from ({bank})");
            Bus[] buses;
            if (FMOD.RESULT.OK != b.getBusList(out buses)) continue;
            if (showLogs)
                Debug.Log($"<Log>[AudioManager]: Managed to fetch buses from ({bank})");
            for (int i = 0; i < buses.Length; i++)
            {
                if (FMOD.RESULT.OK != buses[i].getPath(out path)) continue;
                if (showLogs)
                    Debug.Log($"<Log>[AudioManager]: Adding <[Bus]: {path}> to the data set.");
                if (!currentBuses.ContainsKey(path))
                    currentBuses.Add(path, buses[i]);
            }

        }
    }

}



