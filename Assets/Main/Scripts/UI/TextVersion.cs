using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextVersion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "v"+Application.version.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
