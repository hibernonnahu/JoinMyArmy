using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnGameType : MonoBehaviour
{
    public string[] gametype;
    private bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        string gt= SaveData.GetInstance().GetMetric(SaveDataKey.GAME_TYPE, "Campaign");
        foreach (var item in gametype)
        {
            if (item == gt)
            {
                active = true;
            }
        }
        gameObject.SetActive(active);
    }
    private void OnEnable()
    {
        if (!active)
        {
            gameObject.SetActive(active);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
