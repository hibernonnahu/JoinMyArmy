using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventHistoryText : EventText
{
    private struct LateText{
        public Vector4 color;
        public float time;
        public string text;
        public string character;
        public string audio;
        public bool think;
    }
    private List<LateText> queue2 = new List<LateText>();
    public RawImage background;
    public Image avatar;
    public AudioSource audiosource;
    float globalVol = 1;
    private FontStyle normalFontStyle;
    public FontStyle thinkFontStyle;
    
    // Start is called before the first frame update
    public override void Start()
    {
        code = EventName.STORY_TEXT;
        base.Start();
        normalFontStyle = text.fontStyle;
        avatar = GetComponentInChildren<Image>();
        EventManager.StartListening("voicevol", OnVoiceVol);

    }

    private void OnVoiceVol(EventData arg0)
    {
        globalVol = arg0.floatData;
        audiosource.volume = globalVol;
    }

    protected override void PlayText(EventData arg0)
    {
        if (text.text == "")
        {
            if (arg0.stringData2 != "")
            {
                avatar.sprite = Resources.Load<Sprite>("CharacterIcons/" + arg0.stringData2);
                avatar.color = Color.white;
            }
            else
            {
                avatar.color = Color.clear;
            }
            text.color = arg0.vec4;
            TEXT_DELAY = arg0.floatData;
            if (arg0.boolData)
            {
                text.fontStyle = thinkFontStyle;
            }
            else
            {
                text.fontStyle = normalFontStyle;

            }
            background.gameObject.SetActive(true);
            PlayAudio(arg0.stringData3);
            base.PlayText(arg0);
        }
        else if(queue2.Count<5)
        {
            var late = new LateText();
            late.character = arg0.stringData2;
            late.audio = arg0.stringData3;
            late.think = arg0.boolData;
            late.text = arg0.stringData;
            late.time = arg0.floatData;
            late.color = arg0.vec4;
            queue2.Add(late);
        }
    }

    private void PlayAudio(string audio)
    {
        if (audio != "")
        {
            audiosource.clip = Resources.Load<AudioClip>("Voice/" + audio);
            audiosource.Stop();

            audiosource.Play();
        }
    }

    protected override void RemoveFirst()
    {
        if (queue2.Count == 0)
        {
            base.RemoveFirst();
            avatar.color = Color.clear;
            background.gameObject.SetActive(false);
        }
        else
        {
            text.text = "";
            background.gameObject.SetActive(false);
            queue.Clear();
            var q = queue2[0];
            queue2.RemoveAt(0);
            var ed = new EventData();
            ed.SetString(q.text).SetString2(q.character).SetString3(q.audio).SetBool(q.think).SetFloat(q.time).SetVec4(q.color);
            PlayText(ed);
        }

    }
    private void OnDestroy()
    {
        EventManager.StopListening("voicevol", OnVoiceVol);
    }
}
