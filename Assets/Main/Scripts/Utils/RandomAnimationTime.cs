using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationTime : MonoBehaviour
{
    public float maxSpeed = 1.1f;
    public float minSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().speed = Random.Range(minSpeed, maxSpeed);
    }

}
