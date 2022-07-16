using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    //Singleton
    static MusicManager ins;
    public static MusicManager Get
    {
        get
        {
            if (!ins)
            {
                var o = GameObject.FindObjectOfType<MusicManager>();
                ins = o ? o : new GameObject("Music Manager (Empty)").AddComponent<MusicManager>();
            }



            return ins;
        }
    }

    [System.Serializable]
    public struct MusicClip
    {
        public string name;
        public OneShotAudioPlayer clip;
        public bool playOnAwake;
    }


    //Repeat of PlayerSoundManager

    public List<MusicClip> musicClips;

    private string currentPlayingClip;

    private void Awake()
    {
        foreach (var item in musicClips)
        {
            if (item.playOnAwake)
            {
                Play(item.name);
                break;
            }
        }
    }

    //Plays the inputed clip. Check Music Manager in Unity for the ID.
    public void Play(string musicClip)
    {

        var music = GetMusic(musicClip);

        if (!music) return;

        currentPlayingClip = musicClip;
        music.Play();
    }

    //Stops the inputed clip from playing. Check Music Manager in Unity for the ID.
    public void Stop(string musicClip)
    {
        var music = GetMusic(musicClip);
        if (!music) return;
        music.Stop();
    }

    //Edits the parameters of the inputed clip. Check Music Manager in Unity for the ID.
    public void EditParameter(string musicClip, string name, float value)
    {
        var music = GetMusic(musicClip);
        music.EditParamateter(name, value);
    }


    public void StopCurrentMusic()
    {
        Stop(currentPlayingClip);
    }

    public void EditCurrentMusicParams(string name, float value)
    {
        EditParameter(currentPlayingClip, name, value);
    }

    private OneShotAudioPlayer GetMusic(string musicClip)
    {
        var result = musicClips.Find(x => x.name.Equals(musicClip));
        if (result.clip)
            return result.clip;
        return null;
    }



}
