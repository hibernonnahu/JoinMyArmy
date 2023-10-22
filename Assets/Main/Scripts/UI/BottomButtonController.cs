using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomButtonController : MonoBehaviour
{
    public RectTransform[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        SelectButton(2);
    }

    public void SelectButton(int id)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            LeanTween.cancel(buttons[i].gameObject);
            if (id == i)
            {
                LeanTween.scale(buttons[i].gameObject, Vector3.one * 1.15f, 0.5f);
                buttons[i].GetComponentInChildren<Text>().color = Color.black;
            }
            else
            {
                LeanTween.scale(buttons[i].gameObject, Vector3.one * 0.95f, 0.3f);
                buttons[i].GetComponentInChildren<Text>().color = Color.clear;
            }
        }
    }
}
