using System.Collections;
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
	// Use this for initialization
	void Awake () {
        audioSource = GetComponents<AudioSource>();
        foreach (var item in dictionaryFill)
        {
            dictionary.Add(item.key, item.audio);
            dictionaryVol.Add(item.key, item.volume);
        }
        EventManager.StartListening(EventName.PLAY_FX, PlayFx);
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
        audioSource[currentIndex].volume = vol;
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
    }
}
