using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private Action onUpdate = () => { };
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    private string defaultMusic;
    private float defaultMusicVol;
    private string lastClip;
    // Use this for initialization
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        DontDestroyOnLoad(gameObject);
    }
    public void StopMusic()
    {
        onUpdate = () =>
        {
            audioSource.volume -= Time.unscaledDeltaTime;
            if (audioSource.volume <= 0)
            {
                audioSource.Stop();
                onUpdate = () => { };
            }
        };
    }

    internal void SetCurrentAsDefault()
    {
        defaultMusic = lastClip;
    }
    public void PlayDefault()
    {
        PlayMusic(defaultMusic,defaultMusicVol,true);
    }

    public void LoadClip(string musicName)
    {
        AudioClip audio = null;
        clips.TryGetValue(musicName, out audio);
        if (audio == null)
        {
            clips.Add(musicName, Resources.Load<AudioClip>("Music/" + musicName));
        }

    }
    private void Update()
    {
        onUpdate();
    }
    public void PlayMusic(string musicName, float vol,bool loop=true)
    {
        defaultMusicVol = vol;
        lastClip = musicName;
        audioSource.loop = loop;
        onUpdate = () => { };
        AudioClip audio = null;
        clips.TryGetValue(musicName, out audio);
        audioSource.clip = audio;
        audioSource.volume = vol;
        audioSource.Play();
    }

}
