using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private void Start()
    {

        if (SaveData.GetInstance().GetValue("tutorial4") == 0)//store
        {
            var hsp = gameObject.AddComponent<HintSinglePressUI>();
            hsp.SetID(4);
            hsp.SetBackgroundUIName("background ui general");
            hsp.onTriggerStart = () =>
            {
                foreach (var item in FindObjectsOfType<Popup>())
                {
                    if (item.popupName == "win")
                    {
                        var scroll = item.GetComponentInChildren<ScrollRect>();
                        scroll.enabled = false;
                        break;
                    }
                }
            };
            hsp.onTriggerEnd = () =>
            {
                foreach (var item in FindObjectsOfType<Popup>())
                {
                    if (item.popupName == "win")
                    {
                        var scroll = item.GetComponentInChildren<ScrollRect>();
                        scroll.enabled = true;
                        break;
                    }
                }
            };
        }
        if (SaveData.GetInstance().GetValue("tutorial101") == 0 )//&& SaveData.GetInstance().GetValue("tutorial3") != 0)//trash
        {
            var hsp = gameObject.AddComponent<HintDragUITrash>();
            hsp.SetID(101);
            hsp.DisableSwap();
            hsp.ShowTrash();
        }

    }
}
