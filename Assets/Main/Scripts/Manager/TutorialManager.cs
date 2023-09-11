using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private void Start()
    {
        
        if (SaveData.GetInstance().GetValue("tutorial4") == 0)//store
        {
            var hsp = gameObject.AddComponent<HintSinglePressUI>();
            hsp.SetID(4);
            hsp.SetBackgroundUIName("background ui general");
        }

    }
}
