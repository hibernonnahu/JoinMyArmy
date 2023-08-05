using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralParticleManager : MonoBehaviour
{
    public ParticleSystem health;
    public ParticleSystem stun;
    public ParticleSystem wallHit;
    public ParticleSystem webTrap;
    public ParticleSystem slime;
    public AudioSource slimeAudio;
    public MeshRenderer meshRenderer;
    private Action onUpdate = () => { };
    private Material material;
    private float currentTransparency = 1f;
    public float cacconFadeSpeed = 1f;
    protected void Start()
    {
        currentTransparency = 1f;

        material = meshRenderer.material;
    }
    public void CacconOn(bool on, bool sound = false)
    {
        if (on)
        {
            onUpdate = CacconOn;
            slime.Play();
            if (sound)
                slimeAudio.Play();
        }
        else
            onUpdate = CacconOff;

    }
    private void Update()
    {
        onUpdate();
    }
    void CacconOff()
    {
        currentTransparency += Time.deltaTime * cacconFadeSpeed;
        material.SetFloat("_Cutoff", currentTransparency);
        if (currentTransparency > 1f)
        {
            currentTransparency = 1f;
            onUpdate = () => { };
        }
    }
    void CacconOn()
    {
        currentTransparency -= Time.deltaTime * cacconFadeSpeed;
        material.SetFloat("_Cutoff", currentTransparency);
        if (currentTransparency < 0.05f)
        {
            currentTransparency = 0.05f;
            onUpdate = () => { };
        }
    }
}
