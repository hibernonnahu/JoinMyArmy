using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private void Start()
    {
        if (SaveData.GetInstance().GetValue("tutorial1") == 0)
        {
            gameObject.AddComponent<HintSinglePress>();
        }
        if (SaveData.GetInstance().GetValue("tutorial2") == 0)
        {
            gameObject.AddComponent<HintSinglePressUI>();
        }
        if (SaveData.GetInstance().GetValue("tutorial3") == 0)
        {
            gameObject.AddComponent<HintDragUI>();
        }
    }
}
