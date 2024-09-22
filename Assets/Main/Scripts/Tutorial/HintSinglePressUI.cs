
using System;
using UnityEngine;
using UnityEngine.UI;

public class HintSinglePressUI : MonoBehaviour
{
    protected const float MOVE_DISTANCE = 20F;
    protected const int REPEAT = -1;
    protected bool started = false;
    protected string backgroundName = "background ui";

    protected int id = 2;

    protected GameObject handSprite;
    protected GameObject point;
    protected GameObject background;
    protected Transform parent;
    protected float camPosX;
    protected float camPosZ;
    public Action onTriggerStart = () => { };
    public Action onTriggerEnd = () => { };

    protected void Start()
    {
        handSprite = GameObject.FindWithTag("hand ui");
        point = GameObject.FindWithTag("cross ui");

        background = GameObject.FindWithTag(backgroundName);
        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StartListening(EventName.TUTORIAL_END, OnEnd);
    }
    protected virtual void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            onTriggerStart();
            started = true;
            background.transform.position = Vector3.zero;
            handSprite.SetActive(true);
            handSprite.transform.position =  arg0.transformData.position;
           
            point.transform.SetParent(handSprite.transform.parent);
            handSprite.transform.SetParent(point.transform.parent);
            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 1);
            point.transform.position = handSprite.transform.position;
            LeanTween.scale(point, Vector3.one * 1.5f,0.9f).setDelay(0.5f).setIgnoreTimeScale(true).setRepeat(REPEAT);
            LeanTween.moveY(handSprite, arg0.transformData.position.y + Screen.height * 0.02f, 0.3f).setRepeat(REPEAT).setIgnoreTimeScale(true).setOnComplete(() =>
             {
                 handSprite.SetActive(false);
             });
           
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));
            EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(true));

            camPosX = arg0.floatData;
            camPosZ = arg0.floatData2;
        }
    }
    protected virtual void OnEnd(EventData arg0)
    {
        if (arg0.intData == id && started)
        {
            onTriggerEnd();

           
            LeanTween.cancel(point);
            point.SetActive(false);
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(false));
            EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(false));

            FindObjectOfType<CharacterMain>().floatingJoystick.OnPointerUp(null);

            started = false;
            background.transform.position = Vector3.right * 3000;
            ExtraActionOnEnd(arg0);
            GameObject.FindWithTag("tutorial text").GetComponent<Text>().text = "";

            LeanTween.cancel(handSprite);
            handSprite.transform.position -= Vector3.right * 99999;
            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 2);
            FindObjectOfType<CameraHandler>().GoToPositionOnNoScaleTime(camPosX, camPosZ);
            Destroy(this);
        }
    }


    internal void SetBackgroundUIName(string v)
    {
        backgroundName = v;
    }

    internal void SetID(int v)
    {
        id = v;
    }


    protected virtual void ExtraActionOnStart(EventData arg0)
    {

    }
    protected virtual void ExtraActionOnEnd(EventData arg0)
    {

    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StopListening(EventName.TUTORIAL_END, OnEnd);

    }
}
