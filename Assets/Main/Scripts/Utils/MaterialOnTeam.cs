using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOnTeam : MonoBehaviour
{
    public CharacterEnemy character;
    public Material ally;
    public Material enemy;
    // Start is called before the first frame update
    private void Start()
    {
        UpdateMaterial();
    }
    public  void UpdateMaterial()
    {
        if (character.IsEnemy())
        {
            GetComponent<Renderer>().material = enemy;
        }
        else
        {
            GetComponent<Renderer>().material = ally;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
