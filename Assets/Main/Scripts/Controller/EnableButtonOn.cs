using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButtonOn : MonoBehaviour
{
    public int book = 1;
    public int chapter = 1;
    public string redDotCode = "";
    private Button button;
    Action onDisable = () => { };
    // Start is called before the first frame update
    void Awake()
    {
        if (SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_BOOK_CHAPTER + book, CurrentPlaySingleton.GetInstance().initialChapter) >= chapter)
        {
            GetComponent<Button>().interactable = true;

            gameObject.AddComponent<RedDot>().DotActivate(20, 70, redDotCode);

        }
        else
        {
            onDisable();

            button = GetComponent<Button>();
            button.targetGraphic.color = Color.gray;
            Destroy(button);
            StartCoroutine(AddLockButton());

        }
    }
    IEnumerator AddLockButton()
    {
        yield return button != null;
        button = gameObject.AddComponent<Button>();
       
            button.onClick.AddListener(() =>
            {
                EventManager.Instance.GetEventData().SetFloat(-1);
                EventManager.TriggerEvent(EventName.MAIN_TEXT, EventManager.Instance.GetEventData().SetString(
                    ("Complete") + " " +
                    ("Book") + " " + book + " " +
                    ("Chapter") + " " + chapter
            ));
            });
        
    }
    internal void AddOnDisable(Action onRequirementDisable)
    {
        onDisable = onRequirementDisable;
    }
}
  