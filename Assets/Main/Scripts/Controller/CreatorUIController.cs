using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatorUIController : MonoBehaviour
{
    public new Camera camera;
    private GameObject selected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (selected != null)
        {
            Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            selected.transform.position = Vector3.right * Mathf.FloorToInt(worldPosition.x) + Vector3.forward * Mathf.FloorToInt(worldPosition.z);
            if (Input.GetMouseButtonDown(0))
            {
                selected = null;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                Destroy(selected);
                selected = null;
            }
        }
        else if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > 400)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                Debug.Log(hit.point);
                if (hit.collider)
                {
                    selected = hit.collider.gameObject;
                    do
                    {
                        selected = selected.transform.parent.gameObject;
                    }
                    while (selected.transform.parent != null && selected.GetComponent<Character>() == null);
#if UNITY_EDITOR
                    Selection.activeTransform = selected.transform;
#endif
                }
            }
        }
    }
}
