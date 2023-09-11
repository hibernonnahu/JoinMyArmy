using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CharacterMain characterMain;
    private new Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        characterMain = FindObjectOfType<CharacterMain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        characterMain.coinsUIController.AddCoins(1, transform.position);
        collider.enabled = false;
        transform.localScale = Vector3.zero;
    }
}
