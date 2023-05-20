
using UnityEngine;

public class HintSinglePressUI : MonoBehaviour
{
    private const float MOVE_DISTANCE = 20F;
    private const int REPEAT = -1;
    private bool started = false;

    private int id = 2;

    private GameObject handSprite;
    private GameObject background;
    private void Start()
    {
        handSprite = GameObject.FindWithTag("hand ui");
        background = GameObject.FindWithTag("background ui");
        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StartListening(EventName.TUTORIAL_END, OnEnd);
    }

    private void OnEnd(EventData arg0)
    {
        if (arg0.intData == id && started)
        {
            started = false;
            background.transform.position = Vector3.right * 3000;
            arg0.transformData.gameObject.GetComponent<IconUIController>().DisableDrag(false);

            Time.timeScale = 1;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(false);
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
            arg0.transformData.gameObject.GetComponent<IconUIController>().DisableDrag(true);
            // Debug.Log(arg0.floatData + " " + arg0.floatData2);
            handSprite.transform.position = Vector3.right * (arg0.transformData.position.x) + Vector3.up * (arg0.transformData.position.y - MOVE_DISTANCE) + Vector3.forward * (arg0.transformData.position.z);

            parent = arg0.transformData.parent;
            arg0.transformData.SetParent(null);
            arg0.transformData.SetParent(parent);

            LeanTween.moveY(handSprite, arg0.transformData.position.y, 1).setRepeat(REPEAT).setIgnoreTimeScale(true).setOnComplete(() =>
            {
                handSprite.SetActive(false);
            });
            Time.timeScale = 0;
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StopListening(EventName.TUTORIAL_END, OnEnd);

    }
}
