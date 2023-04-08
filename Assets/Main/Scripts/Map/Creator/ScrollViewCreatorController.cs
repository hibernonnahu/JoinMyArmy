using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewCreatorController : MonoBehaviour
{
    public ButtonResourcesCreator buttonPrefab;
    public string resourcesFolder = "";

    private void Start()
    {
        var prefabs= Resources.LoadAll<GameObject>(resourcesFolder);
        foreach (var prefab in prefabs)
        {
            var buttonResourcesCreator=Instantiate<ButtonResourcesCreator>(buttonPrefab);
            buttonResourcesCreator.prefab = prefab;
            buttonResourcesCreator.asset = prefab.name;
            buttonResourcesCreator.transform.SetParent(transform);
        }
    }
}
