﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using System;

public class AudioManager : MonoBehaviour
{




    private PollingStation station;
    private Dictionary<string, EventDescription> currentEvents;
    private Dictionary<string, Bus> currentBuses;


    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, $"Object <{gameObject.name}> "))
        {
            return;
        }

        station.audioManager = this;



        currentEvents = new Dictionary<string, EventDescription>();


        currentBuses = new Dictionary<string, Bus>();


        LoadFMODData();
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

        if (!keepInstanceAlive)
            result = ins.release();



    }











    void LoadFMODData()
    {
        var banks = Settings.Instance.Banks;
        banks.Add("Master");
        foreach (var bank in banks)
        {
            string path = $"bank:/{bank}";
            Debug.Log($"<Log>[AudioManager]: Fetching bank ({bank}) using {path} as a path");
            Bank b;
            if (FMOD.RESULT.OK != RuntimeManager.StudioSystem.getBank(path, out b)) continue;
            Debug.Log($"<Log>[AudioManager]: Managed to fetch ({bank}) using {path} as a path!");
            Debug.Log($"<Log>[AudioManager]: Fetching events from ({bank})");
            EventDescription[] events;
            if (FMOD.RESULT.OK != b.getEventList(out events)) continue;
            Debug.Log($"<Log>[AudioManager]: Managed to fetch events from ({bank})");
            for (int i = 0; i < events.Length; i++)
            {

                if (FMOD.RESULT.OK != events[i].getPath(out path)) continue;
                Debug.Log($"<Log>[AudioManager]: Adding <[Event]: {path}> to the data set.");
                if (!currentEvents.ContainsKey(path))
                    currentEvents.Add(path, events[i]);
            }

            Debug.Log($"<Log>[AudioManager]: Fetching buses from ({bank})");
            Bus[] buses;
            if (FMOD.RESULT.OK != b.getBusList(out buses)) continue;
            Debug.Log($"<Log>[AudioManager]: Managed to fetch buses from ({bank})");
            for (int i = 0; i < buses.Length; i++)
            {
                if (FMOD.RESULT.OK != buses[i].getPath(out path)) continue;
                Debug.Log($"<Log>[AudioManager]: Adding <[Bus]: {path}> to the data set.");
                if (!currentBuses.ContainsKey(path))
                    currentBuses.Add(path, buses[i]);
            }

        }
    }

}



