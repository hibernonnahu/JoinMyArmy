﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFxManager: MonoBehaviour {
    [System.Serializable]
    public struct DictionaryFill
    {
        public string key;
        public AudioClip audio;
        public float volume;
    }
    private AudioSource[] audioSource;
    int currentIndex = 0;
    public List<DictionaryFill> dictionaryFill;
    public Dictionary<string, AudioClip> dictionary=new Dictionary<string, AudioClip>();
    public Dictionary<string, float> dictionaryVol=new Dictionary<string, float>();
    private float globalVol = 1;
	// Use this for initialization
	void Awake () {
        audioSource = GetComponents<AudioSource>();
        FillDictionary(dictionaryFill);
        EventManager.StartListening(EventName.PLAY_FX, PlayFx);
	}

    public void FillDictionary(List<DictionaryFill> dictionaryFill)
    {
        foreach (var item in dictionaryFill)
        {
            if (!dictionary.ContainsKey(item.key))
            {
                dictionary.Add(item.key, item.audio);
                dictionaryVol.Add(item.key, item.volume);
            }
            else
            {
                dictionary[item.key] = item.audio;
                dictionaryVol[item.key] = item.volume;
            }
        }
    }

    public void Start()
    {
        EventManager.StartListening("fxvol", OnFxVolume);
    }

    private void OnFxVolume(EventData arg0)
    {
        globalVol = arg0.floatData;
    }
    
    public void PlayFx(string n)
    {
        PlayFx(EventManager.Instance.GetEventData().SetString(n));
    }
	private void PlayFx(EventData e)
    {
        var n = e.stringData;
        //Debug.Log("PLAYFX " + n);
        AudioClip audio=null;
        dictionary.TryGetValue(n, out audio);
        float vol = 1;
        dictionaryVol.TryGetValue(n, out vol);
        if (audio == null)
        {
            print("audio " + n + " not found");
        }
        audioSource[currentIndex].clip = audio;
        audioSource[currentIndex].volume = vol*globalVol;
        audioSource[currentIndex].Play();
        currentIndex++;
        if (currentIndex >= audioSource.Length)
        {
            currentIndex = 0;
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.PLAY_FX, PlayFx);
        EventManager.StopListening("fxvol", OnFxVolume);

    }
}
