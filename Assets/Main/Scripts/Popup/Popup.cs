
using System;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    private const float TIME = 0.5f;
    public string popupName;
    private RectTransform rectTransform;
    private GraphicRaycaster graphicRaycaster;
    public GameObject[] enableOnUse;
    public bool debugEnable = false;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        ApdateToResolution();

        EventManager.StartListening(EventName.POPUP_OPEN, OnOpen);
        EventManager.StartListening(EventName.POPUP_CLOSE, OnCloseCheck);
        EventManager.StartListening(EventName.POPUP_CLOSE_ALL, OnClose);
        if (debugEnable)
        {
            Open();
        }
    }

    private void ApdateToResolution()
    {
        Transform[] childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i);
        }

        var container = new GameObject().AddComponent<RectTransform>();
        container.transform.SetParent(rectTransform.transform);
        float scaleFactor = (transform.parent.GetComponent<RectTransform>().sizeDelta.x / 1080);
        container.sizeDelta = Vector2.right * 1080 + Vector2.up * 1920;
        container.anchoredPosition = Vector2.zero;
        container.transform.localScale = Vector3.one;
        container.gameObject.name = "container";
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].SetParent(container.transform);
        }
        container.transform.localScale = Vector3.one * scaleFactor;

    }

    private void OnClose(EventData arg0)
    {
        Close();
    }

    private void OnCloseCheck(EventData arg0)
    {
        if (arg0.stringData == popupName)
        {
            OnClose(arg0);
        }
    }

    private void OnOpen(EventData arg0)
    {
        if (arg0.stringData == popupName)
        {
            Open();
        }
    }

    // Start is called before the first frame update
    public void Open()
    {
        foreach (var item in enableOnUse)
        {
            item.SetActive(true);
        }
        Time.timeScale = 0;
        graphicRaycaster.enabled = true;
        LeanTween.moveY(rectTransform, -rectTransform.rect.height, TIME).setEaseInCirc().setIgnoreTimeScale(true);
    }
    public void Close()
    {
        LeanTween.moveY(rectTransform, 0, TIME).setEaseOutCirc().setIgnoreTimeScale(true).setOnComplete(() =>
        {
            Time.timeScale = 1;
            graphicRaycaster.enabled = false;
        });
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.POPUP_OPEN, OnOpen);
        EventManager.StopListening(EventName.POPUP_CLOSE, OnCloseCheck);
        EventManager.StopListening(EventName.POPUP_CLOSE_ALL, OnClose);
    }
}
