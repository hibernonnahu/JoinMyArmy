using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniPopup : MonoBehaviour
{
    public Text text;
    public Image image;
    private Popup popup;
    private Action onCloseAction = () => { };
    private Action onAcceptAction = () => { };
    // Start is called before the first frame update
    void Awake()
    {
        popup = GetComponentInParent<Popup>();
    }

    public void Open()
    {
        popup.Open();
    }
    public void OnCancel()
    {
        onCloseAction();
        popup.JustClose();
    }
    public void OnAccept()
    {
        onAcceptAction();
        popup.JustClose();
    }
    public void SetAcceptAction(Action action)
    {
        onAcceptAction = action;
    }

    public void SetCancelAction(Action action)
    {
        onCloseAction = action;
    }
    public void SetImage(string imageName)
    {
        if (imageName == "")
        {
            image.color = Color.clear;
        }
        else
        {
            image.color = Color.white;
            image.sprite = Resources.Load<Sprite>("Texture/"+imageName);
        }
    }
}
