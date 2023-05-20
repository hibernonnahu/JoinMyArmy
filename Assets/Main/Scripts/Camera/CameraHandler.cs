using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraHandler : MonoBehaviour
{
    private const float ARRIVE_DIST_SQR = 2;
    private const float CINEMATIC_TRANSITION_TIME = 1.5f;
    private const float CINEMATIC_WAIT_TIME = 10;
    public float defaultSize = 25;
    public float offsetSize = 5;
    public float speedSize = 5;
    private float currentOffsetSize = 0;
    public float OFFSET_Z = 6;
    public float speed = 0.5f;
    public MMWiggle mMWiggle;

    private new Rigidbody rigidbody;
    private Action onUpdate = () => { };
    private Transform toFollow;
    private new Camera camera;
    private float timeStill = 0;
    private float initialY;
    private float initialRotation;
    private Vector3 toFollowInitialPos;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
        EventManager.StartListening(EventName.SHAKE_CAM_POS, OnShakeCam);
        camera = GetComponentInChildren<Camera>();
        initialY = transform.position.y;
        initialRotation = transform.localRotation.eulerAngles.x;
        timeStill = CINEMATIC_WAIT_TIME * 0.5f;
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
    public void FollowGameObject(GameObject gameObject, bool warp = true)
    {
        toFollow = gameObject.transform;
        if (warp)
        {
            transform.position = ((toFollow.position.x) * Vector3.right + (toFollow.position.z + OFFSET_Z) * Vector3.forward) + Vector3.up * transform.position.y;
        }
        onUpdate = ToFollowUpdate;

    }

    private void ToFollowUpdate()
    {
        if (CustomMath.SqrDistance2(toFollow.position.x - transform.position.x, toFollow.position.z - transform.position.z + OFFSET_Z) > ARRIVE_DIST_SQR)
        {
            rigidbody.drag = 0;
            var direction = ((toFollow.position.x - transform.position.x) * Vector3.right + (toFollow.position.z - transform.position.z + OFFSET_Z) * Vector3.forward);
            rigidbody.velocity = direction * speed;
            currentOffsetSize += Time.deltaTime * speedSize;
            if (currentOffsetSize > offsetSize)
            {
                currentOffsetSize = offsetSize;
            }
            timeStill = 0;
        }
        else
        {
            rigidbody.drag = 5;
            currentOffsetSize -= Time.deltaTime * speedSize * 0.5f;
            if (currentOffsetSize < 0)
            {
                currentOffsetSize = 0;
            }
            timeStill += Time.deltaTime;
            if (timeStill > CINEMATIC_WAIT_TIME)
            {
                toFollowInitialPos = toFollow.position;
                onUpdate = Cinematic;
                LeanTween.moveLocalY(gameObject, 6, CINEMATIC_TRANSITION_TIME);
                LeanTween.rotateX(gameObject, 30, CINEMATIC_TRANSITION_TIME);
            }
        }
        camera.orthographicSize = defaultSize + currentOffsetSize;
    }

    private void Cinematic()
    {
        float temp = camera.orthographicSize;
        temp -= Time.deltaTime * speedSize;
        if (temp < 10)
        {
            temp = 10;
        }
        camera.orthographicSize = temp;
        if ((toFollow.position - toFollowInitialPos).sqrMagnitude > 10)
        {
            onUpdate = ToFollowUpdate;
            LeanTween.moveLocalY(gameObject, initialY, CINEMATIC_TRANSITION_TIME);
            LeanTween.rotateX(gameObject, initialRotation, CINEMATIC_TRANSITION_TIME);
            timeStill = 0;
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.SHAKE_CAM_POS, OnShakeCam);

    }
}
