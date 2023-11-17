using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalitycsFilter : MonoBehaviour
{
    private AnalitycsPrefab[] prefabs = new AnalitycsPrefab[0];
    public InputField inputField;
    public Text totalText;
    public void OnFilterChanged()
    {
        if (inputField.text == "")
        {
            foreach (var item in prefabs)
            {
                item.gameObject.SetActive(true);
            }
        }
        else
        {
            float sum = 0;
            foreach (var item in prefabs)
            {
                item.gameObject.SetActive(item.keyName.text.ToLower().Contains(inputField.text.ToLower()));
                if (item.gameObject.active)
                {
                    sum += item.result;
                }
            }
            totalText.text = sum.ToString();
        }
    }

    internal void LoadPrefabs()
    {
        prefabs = GetComponentsInChildren<AnalitycsPrefab>();
    }
}
