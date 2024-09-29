using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWebGL : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_WEBGL
        Destroy(this.gameObject);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
