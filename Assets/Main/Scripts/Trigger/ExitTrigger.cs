using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private new Collider collider;
    public bool pause = false;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!pause)
        {
            collider.enabled = false;
            FindObjectOfType<Game>().OnExitTrigger(transform.position);
        }
    }
}
