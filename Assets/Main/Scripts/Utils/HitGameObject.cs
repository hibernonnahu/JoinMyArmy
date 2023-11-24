
using UnityEngine;

public class HitGameObject : MonoBehaviour
{
    public float time = 2;
    private Animator animator;
    private Vector3 initScale;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        initScale = transform.localScale;
        transform.SetParent(null,false);
        gameObject.SetActive(false);
    }

    public void Play(Vector3 position)
    {
        LeanTween.cancel(gameObject);
        gameObject.SetActive(true);
        gameObject.transform.position = position;
        gameObject.transform.localScale = Vector3.zero;
        animator.CrossFade("attack", 0);
        LeanTween.scale(gameObject, initScale, 0.2f);
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setDelay(time).setOnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }
}
