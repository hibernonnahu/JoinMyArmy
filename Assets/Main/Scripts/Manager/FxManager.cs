using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    [Header("Battle")]
    public ParticleSystem slash;
    public ParticleSystem swordHit;
    public ParticleSystem enemyRecluit;
    public ParticleSystem startEnemyRecluit;
    public ParticleSystem levelUp;

    public void Start()
    {
        swordHit.gameObject.transform.SetParent(null);
    }
}
