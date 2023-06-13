using UnityEngine;

public class ObstacleIdentifier : MonoBehaviour
{
    public int id;
    public int size = 10;
    public int rotation = 0;
    public int colliders = 1;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        float s = size / 10f;
        transform.localScale = Vector3.right * s + Vector3.up + Vector3.forward * s;
        if (colliders == 0)
        {
            foreach (var item in GetComponentsInChildren<Collider>())
            {
                item.enabled = false;
            }
        }
    }
}

