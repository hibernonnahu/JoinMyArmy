
using UnityEngine;

public class HintDragUI : MonoBehaviour
{
    private const float TIME = 1.7F;
    private const int REPEAT = 10;

    private int id = 3;

    private GameObject handSprite;
    private void Start()
    {
        handSprite = GameObject.FindWithTag("hand ui");
        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StartListening(EventName.TUTORIAL_END, OnEnd);
    }

    private void OnEnd(EventData arg0)
    {
        if (arg0.intData == id)
        {
            LeanTween.cancel(handSprite);
            handSprite.SetActive(false);
            SaveData.GetInstance().Save("tutorial" + id, 1);
            Destroy(this);
        }
    }

    private void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            LeanTween.cancel(handSprite);
            handSprite.SetActive(true);

            handSprite.transform.position = Vector3.right * (arg0.floatData) + Vector3.up * (arg0.floatData2+25) + Vector3.forward * (arg0.floatData3);

            LeanTween.moveY(handSprite, arg0.floatData2 - 300, TIME).setDelay(1).setRepeat(REPEAT).setOnComplete(() =>
            {
                handSprite.SetActive(false);
            });
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StopListening(EventName.TUTORIAL_END, OnEnd);

    }
}
