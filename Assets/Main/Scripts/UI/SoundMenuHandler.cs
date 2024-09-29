using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMenuHandler : MonoBehaviour
{
   
    public Slider musicVol;
    public Slider fxVol;
    public Slider voiceVol;
    // Start is called before the first frame update
    void Start()
    {
       
        int musicVolSave = PlayerPrefs.GetInt("musicvolume", 5);
        musicVol.value = (musicVolSave / 10f);
        int fxVolSave = PlayerPrefs.GetInt("fxvolume", 5);
        fxVol.value = (fxVolSave / 10f);
        int voiceVolSave = PlayerPrefs.GetInt("voicevolume", 5);
        voiceVol.value = (voiceVolSave / 10f);

        UpdateMusicVol(1);
        UpdateFxVol(1);
        UpdateVoiceVol(1);
    }
    
   
   
   
    public void UpdateMusicVol(float vol)
    {
        PlayerPrefs.SetInt("musicvolume", (int)(musicVol.value*10));
        EventManager.TriggerEvent("musicvol", EventManager.Instance.GetEventData().SetFloat(musicVol.value));
      
    }
    public void UpdateVoiceVol(float vol)
    {
        PlayerPrefs.SetInt("voicevolume", (int)(voiceVol.value * 10));
        EventManager.TriggerEvent("voicevol", EventManager.Instance.GetEventData().SetFloat(voiceVol.value));

    }
    public void UpdateFxVol(float vol)
    {
        PlayerPrefs.SetInt("fxvolume", (int)(fxVol.value * 10));
        EventManager.TriggerEvent("fxvol", EventManager.Instance.GetEventData().SetFloat(fxVol.value));

    }
}
