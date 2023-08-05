using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptToScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Utils.AdapteToResolution(GetComponent<RectTransform>(), transform, GetComponentInParent<Canvas>().GetComponent<RectTransform>(),true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
