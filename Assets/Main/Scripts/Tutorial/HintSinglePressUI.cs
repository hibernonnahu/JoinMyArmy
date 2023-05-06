
using UnityEngine;

public class HintSinglePressUI : MonoBehaviour
{
    private const float MOVE_DISTANCE = 20F;
    private const int REPEAT = 15;
   
    private int id = 2;
   
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
            Debug.Log(arg0.floatData + " " + arg0.floatData2);
            handSprite.transform.position = Vector3.right * (arg0.floatData)+Vector3.up*(arg0.floatData2 - MOVE_DISTANCE) + Vector3.forward * (arg0.floatData3 );
           
            LeanTween.moveY(handSprite, arg0.floatData2, 1).setRepeat(REPEAT).setOnComplete(()=> {
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
