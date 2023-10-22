
using System;
using UnityEngine;
using UnityEngine.UI;

public class HintDragUI : MonoBehaviour
{
    private const float TIME = 1.7F;
    private const int REPEAT = -1;
    private bool started = false;

    protected int id = 3;

    protected GameObject handSprite;
    private GameObject background;
    protected GameObject point;
    private bool canSwap = true;
    protected bool showTrash = false;
    protected virtual void Start()
    {
        handSprite = GameObject.FindWithTag("hand ui");
        background = GameObject.FindWithTag("background ui");
        point = GameObject.FindWithTag("point ui");
        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StartListening(EventName.TUTORIAL_END, OnEnd);
    }

    private void OnEnd(EventData arg0)
    {
        if (arg0.intData == id && started)
        {
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(false));
            //EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(false));

            FindObjectOfType<CharacterMain>().floatingJoystick.OnPointerUp(null);
            FindObjectOfType<RecluitController>().trashEnable = true;


            started = false;
            Time.timeScale = 1;
            point.SetActive(false);
            background.transform.position = Vector3.right * 3000;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(false);
            LeanTween.cancel(point);
            GameObject.FindWithTag("tutorial text").GetComponent<Text>().text = "";
            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 1);
            FindObjectOfType<RecluitController>().canSwap = true;
            Destroy(this);
        }
    }

    protected virtual void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));
            // EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(true));


            started = true;
            var RC = FindObjectOfType<RecluitController>();
            RC.trashEnable = showTrash;
            RC.canSwap = canSwap;
            var parent = background.transform.parent;
            background.transform.SetParent(null, true);
            background.transform.SetParent(parent, true);
            background.transform.position = Vector3.up * Screen.height;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(true);

            handSprite.transform.position = arg0.transformData.position + Vector3.up * 25;

            LeanTween.move(handSprite, Vector3.up * arg0.floatData + Vector3.right * arg0.floatData2, TIME).setDelay(1).setIgnoreTimeScale(true).setRepeat(REPEAT).setOnComplete(() =>
              {
                  handSprite.SetActive(false);
              });
            parent = arg0.transformData.parent;
            Vector3 pos = arg0.transformData.position;
            arg0.transformData.SetParent(null);
            arg0.transformData.SetParent(parent);
            arg0.transformData.position = pos;

            point.transform.SetParent(null);
            point.transform.SetParent(parent);
            point.transform.position = arg0.floatData * Vector3.up + arg0.floatData2 * Vector3.right;
            LeanTween.scale(point, Vector3.zero, 0.9f).setIgnoreTimeScale(true).setRepeat(REPEAT);

            Time.timeScale = 0;
        }
    }

    protected virtual void OnDestroy()
    {
        EventManager.StopListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StopListening(EventName.TUTORIAL_END, OnEnd);

    }

    internal void SetID(int tutorialID)
    {
        id = tutorialID;
    }

    internal void DisableSwap()
    {
        canSwap = false;
    }

    internal void ShowTrash()
    {
        showTrash = true;
    }
}
