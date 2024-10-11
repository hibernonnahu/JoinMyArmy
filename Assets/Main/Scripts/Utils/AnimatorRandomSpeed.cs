using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRandomSpeed : MonoBehaviour
{
    public float r1;
    public float r2;
    // Start is called before the first frame update
    void Start()
    {
        float speed= Random.Range(r1, r2);
        
        GetComponent<Animator>().SetFloat("speed", speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
