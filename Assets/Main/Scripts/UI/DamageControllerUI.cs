using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageControllerUI : MonoBehaviour
{
    public Transform scarContainer;
    Vector4 colorVec = new Vector4(1, 0, 0, 1);
    public Image[] images;
  
    private void Awake()
    {
        UpdateUI(0);
    }
    public void UpdateUI(float percent,bool heal=false)
    {
        foreach (var item in images)
        {
            item.color = colorVec * percent*1.3f;
        }
       
    }

}
