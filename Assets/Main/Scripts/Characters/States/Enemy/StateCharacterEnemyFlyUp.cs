using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyFlyUp : StateCharacterEnemy
{
    private ParticleSystem particle;
    private GameObject groundShadow;

    private const float SPEED_UP = 100;
    private const float MAX_HEIGHT = 100;

    private AudioSource audio;


    public StateCharacterEnemyFlyUp(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, ParticleSystem particle, GameObject groundShadow, AudioSource audio) : base(stateMachine, characterEnemy)
    {
        this.particle = particle;
        this.groundShadow = groundShadow;
        this.audio = audio;

    }
    public override void Awake()
    {
        enemy.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        groundShadow.SetActive(true);
        groundShadow.transform.position = enemy.transform.position.x * Vector3.right + Vector3.forward * enemy.transform.position.z;
        enemy.Rigidbody.drag = 0;
        enemy.Rigidbody.velocity = Vector3.up * SPEED_UP;
        enemy.SetAnimation("jump", 0);
        particle.Play();
        audio.Play();

    }

    public override void Sleep()
    {
      
    }

    public override void Update()
    {
        if (enemy.transform.position.y > MAX_HEIGHT)
        {
            enemy.Rigidbody.velocity = Vector3.zero;
            ChangeState(typeof(StateCharacterEnemyFlyDown));
        }
       
    }
    public override bool GetHit(float damage,Character attacker)
    {
        return false;
    }
    public override bool CanGetEffect()
    {
        return false;
    }

}
