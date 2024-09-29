using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequiredWorldLevel : MonoBehaviour
{
    public int worldToSay = 1;
    public int levelToSay = 1;
    public bool comingSoon = false;
    Button button = null;
    Action onDisable = () => { };
    // Start is called before the first frame update
    void Start()
    {

        string worldCode = "";
        if (worldToSay > 1)
        {
            worldCode = worldToSay.ToString();
        }
        var code = PlayerPrefs.GetInt("levelunlock" + worldCode, 1);
        //print("levelunlock " + code);
        if (code <= levelToSay || comingSoon)
        {
#if !UNITY_EDITOR
            
            onDisable();

            button = GetComponent<Button>();
            button.targetGraphic.color = Color.gray;
            Destroy(button);
            StartCoroutine(AddLockButton());
#endif
        }
    }
    IEnumerator AddLockButton()
    {
        yield return button != null;
        button = gameObject.AddComponent<Button>();
        if (comingSoon)
        {
            button.onClick.AddListener(() =>
            {
                EventManager.Instance.GetEventData().SetFloat(-1);
                EventManager.TriggerEvent("settext",EventManager.Instance.GetEventData().SetString(
                    LanguageManager.Instance.GetTextValue("Coming Soon"))
            );
            });
        }
        else
        {
            button.onClick.AddListener(() =>
            {
                EventManager.Instance.GetEventData().SetFloat(-1);
                EventManager.TriggerEvent("settext",EventManager.Instance.GetEventData().SetString(
                    LanguageManager.Instance.GetTextValue("Complete") + " " +
                    LanguageManager.Instance.GetTextValue("World") + " " + worldToSay + " " +
                    LanguageManager.Instance.GetTextValue("Level") + " " + levelToSay
            ));
            });
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    internal void AddOnDisable(Action onRequirementDisable)
    {
        onDisable = onRequirementDisable;
    }
}
