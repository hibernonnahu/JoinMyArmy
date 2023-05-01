using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraHandler : MonoBehaviour
{
    private const float ARRIVE_DIST_SQR = 2;
    public float OFFSET_Z = 6;
    public float speed = 0.5f;
    public MMWiggle mMWiggle;

    private new Rigidbody rigidbody;
    private Action onUpdate = () => { };
    private Transform toFollow;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
        EventManager.StartListening(EventName.SHAKE_CAM_POS, OnShakeCam);
    }

    private void OnShakeCam(EventData arg0)
    {
        mMWiggle.WigglePosition(arg0.floatData);
    }

    // Update is called once per frame
    void Update()
    {
        onUpdate();
    }
    public void FollowGameObject(GameObject gameObject)
    {
        toFollow = gameObject.transform;
        onUpdate = () =>
        {
            if (CustomMath.SqrDistance2(toFollow.position.x - transform.position.x, toFollow.position.z - transform.position.z + OFFSET_Z) > ARRIVE_DIST_SQR)
            {
                rigidbody.drag = 0;
                var direction = ((toFollow.position.x - transform.position.x) * Vector3.right + (toFollow.position.z - transform.position.z + OFFSET_Z) * Vector3.forward);
                rigidbody.velocity = direction * speed;
            }
            else
            {
                rigidbody.drag = 5;
            }
        };

    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.SHAKE_CAM_POS, OnShakeCam);

    }
}
