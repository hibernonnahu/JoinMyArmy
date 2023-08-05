using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public float shake = 0;
    public float multiplier = 1;
    public bool dissy = false;
    public float sqrSplashDistance = 0;
    public float expulsion = 0;
    public ParticleSystem impactParticle; // Effect spawned when projectile hits a collider
    private AudioSource impactAudio; // Effect spawned when projectile hits a collider
    public ParticleSystem projectileParticle; // Effect attached to the gameobject as child
    private AudioSource projectileAudio; // Effect attached to the gameobject as child
    public ParticleSystem muzzleParticle; // Effect instantly spawned when gameobject is spawned
    public Character character;
    private new Rigidbody rigidbody;
    private new Collider collider;
    private HitEffectController hitEffectController;

    private void Awake()
    {
        hitEffectController = new HitEffectController();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        collider.enabled = false;
        impactAudio = impactParticle.GetComponent<AudioSource>();
        projectileAudio = projectileParticle.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("bullet layer col " +LayerMask.LayerToName( collision.gameObject.layer));
        collider.enabled = false;
        impactParticle.transform.position = projectileParticle.transform.position;
        impactParticle.Play();
        muzzleParticle?.Stop();
        projectileParticle.Stop();
        impactAudio?.Play();
        LeanTween.delayedCall(gameObject, 1, Disable);
        rigidbody.velocity = Vector3.zero;
        if (sqrSplashDistance > 0)
        {
            foreach (var enemy in character.CharacterManager.GetEnemiesInRange(character.team, sqrSplashDistance, this.transform.position))
            {
                enemy.GetHit(character, multiplier, dissy);

                if (expulsion > 0)
                    hitEffectController.CreateEffect(transform.position, enemy, expulsion);
            }
        }
        else if (character.HitsLayer(collision.gameObject.layer))
        {
            collision.gameObject.GetComponent<Character>().GetHit(character, multiplier, dissy);
        }
        if (shake > 0)
        {
            EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(shake));
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
        muzzleParticle?.Play();
        projectileParticle.Play();
        projectileAudio?.Play();
        projectileParticle.transform.forward = character.model.transform.forward;
        rigidbody.velocity = character.model.transform.forward * speed;
    }
    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }
}
