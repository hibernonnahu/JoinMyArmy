using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPerTime : MonoBehaviour
{
    public float speed = 40;
    private float current = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        current += Time.deltaTime * speed;
        current = current % 360;
        transform.localRotation = Quaternion.Euler(Vector3.up * current);
    }
}
