using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomHudButton : MonoBehaviour
{
    public GameObject[] toActivate;
    public GameObject[] toDiActivate;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnClick()
    {
        foreach (var item in toDiActivate)
        {
            item.SetActive(false);
        }
        foreach (var item in toActivate)
        {
            item.SetActive(true);
        }
    }
}
