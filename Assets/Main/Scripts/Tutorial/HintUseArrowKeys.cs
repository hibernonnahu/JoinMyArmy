
using UnityEngine;

public class HintUseArrowKeys : MonoBehaviour
{

    private bool started = false;
    private int id = 100;

    private GameObject arrowKeysSprite;
    private void Start()
    {
        arrowKeysSprite = GameObject.FindWithTag("arrows ui");
        

        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
       
    }

    private void OnEnd()
    {
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(false));
            EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(false));

            started = false;

            LeanTween.cancel(arrowKeysSprite);
            arrowKeysSprite.SetActive(false);

            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 1);

            Destroy(this);
        
    }
    private void Update()
    {
        if (started)
        {
            if(Input.GetKey(KeyCode.DownArrow)|| Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.RightArrow))
            {
                OnEnd();
            }
        }
    }
    private void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            started = true;
            LeanTween.cancel(arrowKeysSprite);
            arrowKeysSprite.SetActive(true);
            arrowKeysSprite.AddComponent<Pulse>();
            arrowKeysSprite.transform.position = Vector3.right * Screen.width * 0.5f + Vector3.up * Screen.height * 0.7f;


            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));
            EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(true));

        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.TUTORIAL_START, OnTrigger);
     
    }
}
