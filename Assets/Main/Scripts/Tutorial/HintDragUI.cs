
using UnityEngine;

public class HintDragUI : MonoBehaviour
{
    private const float TIME = 1.7F;
    private const int REPEAT = -1;
    private bool started = false;

    private int id = 3;

    private GameObject handSprite;
    private GameObject background;
    private GameObject point;
    private void Start()
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
            started = false;
            Time.timeScale = 1;
            point.SetActive(false);
            background.transform.position = Vector3.right * 3000;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(false);
            LeanTween.cancel(point);

            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 1);
            Destroy(this);
        }
    }

    private void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            started = true;
            var parent = background.transform.parent;
            background.transform.SetParent(null);
            background.transform.SetParent(parent);
            background.transform.position = Vector3.up * Screen.height;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(true);

            handSprite.transform.position = arg0.transformData.position + Vector3.up * 25;

            LeanTween.moveY(handSprite, arg0.transformData.position.y - 300, TIME).setDelay(1).setIgnoreTimeScale(true).setRepeat(REPEAT).setOnComplete(() =>
            {
                handSprite.SetActive(false);
            });
            parent = arg0.transformData.parent;
            arg0.transformData.SetParent(null);
            arg0.transformData.SetParent(parent);
            point.transform.SetParent(null);
            point.transform.SetParent(parent);
            point.transform.position = handSprite.transform.position + Vector3.up * -310;
            LeanTween.scale(point, Vector3.zero, 0.9f).setIgnoreTimeScale(true).setRepeat(REPEAT);

            Time.timeScale = 0;
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StopListening(EventName.TUTORIAL_END, OnEnd);

    }
}
