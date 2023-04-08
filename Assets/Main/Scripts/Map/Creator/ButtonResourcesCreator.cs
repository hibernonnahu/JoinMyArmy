using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonResourcesCreator : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public GameObject prefab;
    public string asset = "";
    public Image image;
    private new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        name = asset;
        text.text = asset;
        image.sprite = Resources.Load<Sprite>("CharacterIcons/" + name);
        if (image.sprite == null)
        {
            image.gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        Instantiate<GameObject>(prefab, Vector3.right * camera.transform.position.x + Vector3.forward * camera.transform.position.z, Quaternion.identity);
    }
}
