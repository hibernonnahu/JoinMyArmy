using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairController : MonoBehaviour
{
    private Action onUpdate = () => { };


    // Update is called once per frame
    void Update()
    {
        onUpdate();
    }

    public void UnFollow()
    {
        onUpdate = () => { };
        transform.position = Vector3.one * 9999;
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
    }
    public void FollowEnemy(GameObject enemy)
    {
        StartTween();
        onUpdate = () =>
        {
            transform.position = Vector3.right * enemy.transform.position.x + Vector3.forward * enemy.transform.position.z;
        };
    }

    private void StartTween()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.3f).setEaseOutBounce().setLoopPingPong();
    }
   
}
