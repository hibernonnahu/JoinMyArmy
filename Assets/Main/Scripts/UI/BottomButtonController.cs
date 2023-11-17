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
        if (SaveData.GetInstance().GetMetric(SaveDataKey.GAME_TYPE, "Campaign") == "Campaign")
            SelectButton(2, true);
        else
            SelectButton(1, true);
    }
    public void SelectButton(int id)
    {
        SelectButton(id, false);
    }
    private void SelectButton(int id, bool bottomHudButton = false)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            LeanTween.cancel(buttons[i].gameObject);
            if (id == i)
            {
                if (bottomHudButton)
                {
                    buttons[i].GetComponent<Button>().onClick.Invoke();
                }
                LeanTween.scale(buttons[i].gameObject, Vector3.one * 1.15f, 0.5f);
                buttons[i].GetComponentInChildren<Text>().color = Color.black;
            }
            else
            {

                buttons[i].GetComponent<BottomHudButton>().isActive = false;

                LeanTween.scale(buttons[i].gameObject, Vector3.one * 0.95f, 0.3f);
                buttons[i].GetComponentInChildren<Text>().color = Color.clear;
            }
        }
    }
}
