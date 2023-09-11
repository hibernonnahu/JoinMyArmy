using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        Destroy(this.gameObject);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
