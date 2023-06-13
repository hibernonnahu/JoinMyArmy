
using System;
using UnityEngine;

public class HintSinglePressUI : MonoBehaviour
{
    protected const float MOVE_DISTANCE = 20F;
    protected const int REPEAT = -1;
    protected bool started = false;
    protected string backgroundName = "background ui";

    protected int id = 2;

    protected GameObject handSprite;
    protected GameObject background;
    protected Transform parent;

    protected void Start()
    {
        handSprite = GameObject.FindWithTag("hand ui");
        background = GameObject.FindWithTag(backgroundName);
        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StartListening(EventName.TUTORIAL_END, OnEnd);
    }
    protected virtual void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            started = true;
            background.transform.position = Vector3.zero;
            handSprite.SetActive(true);
            handSprite.transform.position = Vector3.right * Screen.width * (0.5f + 0.2f * arg0.intData2) + Vector3.up * Screen.height * 0.65f;
            LeanTween.moveY(handSprite, arg0.transformData.position.y + Screen.height * 0.63f, 1).setRepeat(REPEAT).setIgnoreTimeScale(true).setOnComplete(() =>
             {
                 handSprite.SetActive(false);
             });
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));

        }
    }
    protected virtual void OnEnd(EventData arg0)
    {
        if (arg0.intData == id && started)
        {
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(false));
            FindObjectOfType<CharacterMain>().floatingJoystick.OnPointerUp(null);

            started = false;
            background.transform.position = Vector3.right * 3000;
            ExtraActionOnEnd(arg0);

            Time.timeScale = 1;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(false);
            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 1);
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
