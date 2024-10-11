
using System;
using UnityEngine;

public class HintSinglePressUINoDrag : HintSinglePressUI
{
    protected override void ExtraActionOnStart(EventData arg0)
    {
        arg0.transformData.gameObject.GetComponent<IconUIController>().DisableDrag(true);
    }
    protected override void ExtraActionOnEnd(EventData arg0)
    {
        arg0.transformData.gameObject.GetComponent<IconUIController>().DisableDrag(false);
    }

    protected override void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            started = true;
            parent = background.transform.parent;
            background.transform.SetParent(null);
            background.transform.SetParent(parent);

            background.transform.position = Vector3.up * Screen.height;
            followCam = arg0.boolData;
            Debug.Log("follow cam" + followCam);
            LeanTween.cancel(handSprite);
            handSprite.SetActive(true);
            ExtraActionOnStart(arg0);

            handSprite.transform.position = Vector3.right * (arg0.transformData.position.x) + Vector3.up * (arg0.transformData.position.y - MOVE_DISTANCE) + Vector3.forward * (arg0.transformData.position.z);


            parent = arg0.transformData.parent;
            Vector3 pos = arg0.transformData.position;
            arg0.transformData.SetParent(null, true);
            arg0.transformData.SetParent(parent);
            arg0.transformData.position = pos;
            camPosX = arg0.floatData;
            camPosZ = arg0.floatData2;

           
            point.transform.SetParent(handSprite.transform.parent);
            handSprite.transform.SetParent(point.transform.parent);
            point.transform.position = handSprite.transform.position;
            LeanTween.scale(point, Vector3.one *1.5f, 0.9f).setDelay(0.5f).setIgnoreTimeScale(true).setRepeat(REPEAT);
            LeanTween.moveY(handSprite, arg0.transformData.position.y, 1).setRepeat(REPEAT).setIgnoreTimeScale(true).setOnComplete(() =>
            {
                handSprite.SetActive(false);
            });
            Time.timeScale = 0;
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));
            EventManager.TriggerEvent(EventName.HIDE_OPTIONS_UI, EventManager.Instance.GetEventData().SetBool(true));

        }
    }

}
