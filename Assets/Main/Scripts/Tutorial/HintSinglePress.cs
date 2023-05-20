
using UnityEngine;

public class HintSinglePress : MonoBehaviour
{
    private const float COLOR = 0F;
    private const float MOVE_DISTANCE = 0.8F;
    private const int REPEAT = -1;
    private bool started = false;
    private int id = 1;

    private GameObject hud;
    private GameObject handSprite;
    private GameObject background;
    private void Start()
    {
        handSprite = GameObject.FindWithTag("hand");
        hud = GameObject.FindWithTag("hud");
        background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        background.transform.localScale = Vector3.one * 5000;
        background.transform.rotation = Quaternion.Euler(Vector3.right * 90);
        var mr = background.GetComponent<MeshRenderer>();
        mr.material.shader = Shader.Find("Unlit/MobileTransparentTint");
        mr.material.color = new Vector4(COLOR,COLOR,COLOR,0.9F);
        background.SetActive(false);
        EventManager.StartListening(EventName.TUTORIAL_START, OnTrigger);
        EventManager.StartListening(EventName.TUTORIAL_END, OnEnd);
    }

    private void OnEnd(EventData arg0)
    {
        if (arg0.intData == id && started)
        {
            started = false;
            background.SetActive(false);
            hud.SetActive(true);
            LeanTween.cancel(handSprite);
            handSprite.SetActive(false);
            SaveData.GetInstance().Save(SaveDataKey.TUTORIAL + id, 1);
            Time.timeScale = 1;
            Destroy(this);
        }
    }

    private void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            started = true;
            LeanTween.cancel(handSprite);
            handSprite.SetActive(true);
            handSprite.transform.position = Vector3.right * arg0.floatData + Vector3.up * (arg0.floatData2 + 1) + Vector3.forward * (arg0.floatData3 + MOVE_DISTANCE);
            hud.SetActive(false);
            background.SetActive(true);

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
}
