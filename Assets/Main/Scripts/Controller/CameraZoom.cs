using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    Camera cam;
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            cam.orthographicSize += Time.deltaTime*speed;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            cam.orthographicSize -= Time.deltaTime * speed;
        }
    }
}
