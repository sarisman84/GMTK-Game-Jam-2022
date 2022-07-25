using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using System;

public class AudioManager : MonoBehaviour
{

    [System.Serializable]
    public class ClipSettings
    {
        public string clipName;
        public EventReference @event;
        public bool playGlobally = false;

    }

    [System.Serializable]
    public class BankSettings
    {
        [FMODUnity.BankRef]
        public string bankPath;

        public bool Compare(string input)
        {
            return bankPath.ToLower().Contains(input.ToLower());
        }
    }



    public List<BankSettings> bankSettings;
    public List<ClipSettings> audioClips;
    private PollingStation station;
    private Dictionary<FMOD.GUID, EventInstance> activeClipInstances;

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, $"Object <{gameObject.name}> "))
        {
            return;
        }

        station.musicManager = this;
        activeClipInstances = new Dictionary<FMOD.GUID, EventInstance>();
    }


    private ClipSettings SearchForClip(string clipName)
    {
        ClipSettings settings = audioClips.Find(x => x.clipName.Contains(clipName));
        if (settings == null) return null;
        return settings;
    }

    private BankSettings SearchForBank(string bankName)
    {
        BankSettings settings = bankSettings.Find(x => x.Compare(bankName));
        if (settings == null) return null;
        return settings;
    }


    public void SetVolume(string bankName, float aValue)
    {
        var settings = SearchForBank(bankName);

        if (settings == null) return;


    }


    public void Play(string clipName, GameObject aTargetObjectToPlayOff = null, bool keepInstanceAlive = false)
    {
        ClipSettings foundSettings = SearchForClip(clipName);
        if (foundSettings == null) return;
        EventInstance instance = RuntimeManager.CreateInstance(foundSettings.@event);

        if (!foundSettings.playGlobally && aTargetObjectToPlayOff)
        {
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(aTargetObjectToPlayOff));
        }
        if (!activeClipInstances.ContainsKey(foundSettings.@event.Guid))
            activeClipInstances.Add(foundSettings.@event.Guid, instance);
        instance.start();

        if (!keepInstanceAlive)
        {
            instance.release();
        }
    }




    public void Stop(string clipName)
    {
        var settings = SearchForClip(clipName);
        if (settings == null || !activeClipInstances.ContainsKey(settings.@event.Guid)) return;
        var instance = activeClipInstances[settings.@event.Guid];
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();

    }


    public void ModifyAudio(string clipName, string propertyName, float propertyValue)
    {
        var settings = SearchForClip(clipName);
        if (settings == null || !activeClipInstances.ContainsKey(settings.@event.Guid)) return;
        EventDescription eventDesc;
        if (FMOD.RESULT.OK != activeClipInstances[settings.@event.Guid].getDescription(out eventDesc)) return;
        PARAMETER_DESCRIPTION paramDesc;
        if (FMOD.RESULT.OK != eventDesc.getParameterDescriptionByName(propertyName, out paramDesc)) return;

        activeClipInstances[settings.@event.Guid].setParameterByID(paramDesc.id, propertyValue);
    }

}



