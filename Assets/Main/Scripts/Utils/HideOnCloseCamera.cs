using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnCloseCamera : MonoBehaviour
{
    public float CAMERA_DISTANCE_SQR = 1500;//1764.2f;
    private float CAMERA_Z_OFFSET = -10;
    private Material material;
    private Vector4 currentColor;
    private float variant = 1;
    private static Vector4 alpha = new Vector4(0, 0, 0, 1);

    // Start is called before the first frame update
    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        currentColor = material.GetColor("_Color");
        currentColor = new Vector4(currentColor.x, currentColor.y, currentColor.z);
        if (Camera.main == null)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

        float dist = CustomMath.SqrDistance2((Camera.main.transform.position.x - transform.position.x), (Camera.main.transform.position.z + CAMERA_Z_OFFSET - transform.position.z));
        if (dist < CAMERA_DISTANCE_SQR)
        {
            variant = dist / CAMERA_DISTANCE_SQR;

        }
        else
        {
            variant = 1;
        }

        material.SetColor("_Color", currentColor + alpha * variant);
    }
}
