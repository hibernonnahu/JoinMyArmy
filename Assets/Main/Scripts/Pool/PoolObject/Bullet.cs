using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public ParticleSystem impactParticle; // Effect spawned when projectile hits a collider
    private AudioSource impactAudio; // Effect spawned when projectile hits a collider
    public ParticleSystem projectileParticle; // Effect attached to the gameobject as child
    private AudioSource projectileAudio; // Effect attached to the gameobject as child
    public ParticleSystem muzzleParticle; // Effect instantly spawned when gameobject is spawned
    public Character character;
    private new Rigidbody rigidbody;
    private new Collider collider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        collider.enabled = false;
        impactAudio = impactParticle.GetComponent<AudioSource>();
        projectileAudio = projectileParticle.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collider.enabled = false;
        impactParticle.transform.position = projectileParticle.transform.position;
        impactParticle.Play();
        muzzleParticle.Stop();
        projectileParticle.Stop();
        impactAudio?.Play();
        LeanTween.delayedCall(1, Disable);
        rigidbody.velocity = Vector3.zero;
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<Character>().GetHit(character);
        }
    }

    private void Disable()
    {
        impactParticle.Stop();
        gameObject.SetActive(false);
    }

    internal void Shot(float speed)
    {
        
        collider.enabled = true;
        muzzleParticle.Play();
        projectileParticle.Play();
        projectileAudio?.Play();
        projectileParticle.transform.forward = character.model.transform.forward;
        rigidbody.velocity = character.model.transform.forward * speed;
    }
}
