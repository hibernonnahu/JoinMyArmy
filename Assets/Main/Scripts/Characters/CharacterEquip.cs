using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquip : MonoBehaviour
{
    public Transform[] node;

    public void Equip(string weapon, int nodeIndex)
    {
        foreach (Transform child in node[nodeIndex])
        {
            Destroy(child.gameObject);
        }
        if (weapon != "")
        {
            GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>("Weapon/" + weapon));
            go.transform.SetParent((node[nodeIndex]), false);
        }
    }
}
