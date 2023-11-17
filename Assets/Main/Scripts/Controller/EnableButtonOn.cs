using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButtonOn : MonoBehaviour
{
    public int book = 1;
    public int chapter = 1;
    public string redDotCode = "";
    // Start is called before the first frame update
    void Awake()
    {
        if (SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_BOOK_CHAPTER + book, 1) >= chapter)
        {
            GetComponent<Button>().interactable = true;

            gameObject.AddComponent<RedDot>().DotActivate(20, 70, redDotCode);

        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }


}
