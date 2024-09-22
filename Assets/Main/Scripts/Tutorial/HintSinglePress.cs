
using System;
using UnityEngine;
using UnityEngine.UI;

public class HintSinglePress : MonoBehaviour
{
    private const float COLOR = 0F;
    private const float MOVE_DISTANCE = 0.8F;
    private const int REPEAT = -1;
    private bool started = false;
    private int id = 1;
    private int enemyId = -1;

    private GameObject hud;
    private GameObject handSprite;
    private GameObject background;
    private void Awake()
    {
        handSprite = GameObject.FindWithTag("hand");
        hud = GameObject.FindWithTag("hud");
        background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        background.transform.localScale = Vector3.one * 5000;
        background.transform.rotation = Quaternion.Euler(Vector3.right * 90);
        var mr = background.GetComponent<MeshRenderer>();
        mr.material.shader = Shader.Find("Unlit/MobileTransparentTint");
        mr.material.color = new Vector4(COLOR, COLOR, COLOR, 0.7F);
        background.SetActive(false);
        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StartListening(EventName.TUTORIAL_END, OnEnd);
    }

    private void OnEnd(EventData arg0)
    {
        if (started)
        {
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(false));
            EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(false));
            EventManager.TriggerEvent(EventName.ENABLE_ICON_CONTROLLER_COLLIDER, EventManager.Instance.GetEventData().SetBool2(true));

            FindObjectOfType<CharacterMain>().floatingJoystick.OnPointerUp(null);

            started = false;
            background.SetActive(false);
            hud.SetActive(true);
            LeanTween.cancel(handSprite);
            handSprite.transform.position -= Vector3.right * 99999;


            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 2);
            GameObject.FindWithTag("tutorial text").GetComponent<Text>().text = "";
          

            Time.timeScale = 1;
            Destroy(this);
        }
    }

    private void OnTrigger(EventData arg0)
    {
        if (!started && arg0.intData == enemyId)
        {
            started = true;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(true);
            handSprite.transform.position = Vector3.right * arg0.floatData + Vector3.up * (arg0.floatData2 + 1) + Vector3.forward * (arg0.floatData3 + MOVE_DISTANCE);
            hud.SetActive(false);
            background.SetActive(true);
            if (arg0.boolData)
            {
                handSprite.transform.localScale = Vector3.left * handSprite.transform.localScale.x + Vector3.up * handSprite.transform.localScale.y + Vector3.forward * handSprite.transform.localScale.z;
            }
            EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));
            EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(true));
            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 1);
            background.transform.position = Vector3.up * (arg0.floatData2 - 0.7f);
            LeanTween.moveLocalZ(handSprite, arg0.floatData3, 1).setIgnoreTimeScale(true).setRepeat(REPEAT).setOnComplete(() =>
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

    internal void SetID(int tutorialID)
    {
        id = tutorialID;
    }

    internal void SetEnemyID(int enemyId)
    {
        this.enemyId = enemyId;
    }
}
