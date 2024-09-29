using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var mm = FindObjectOfType<MusicManager>();
        if (mm != null)
        {
            Destroy(mm.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
