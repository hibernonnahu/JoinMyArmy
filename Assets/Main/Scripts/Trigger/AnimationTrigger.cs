using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;
    public string animationName = "";
    public AudioSource audioSource;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            animator.CrossFade(animationName, 0);
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
