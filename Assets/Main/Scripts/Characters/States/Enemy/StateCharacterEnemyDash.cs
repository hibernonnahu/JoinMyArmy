using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyDash : StateCharacterEnemy
{
    private ParticleSystem footParticle;
    private ParticleSystem collisionParticle;
    private float initialMass;
    private float initialDrag;
    private const float DAMAGE_MULTIPLIER = 1.5f;
    private const float DASH_SPEED = 20;
    private const float PREPARE_TIME = 0.5f;
    private const float DURATION = 2;
    private float counter;
    private bool prepare;
    private HitEffectController hitEffectController;
    public StateCharacterEnemyDash(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, ParticleSystem footParticle, ParticleSystem collisionParticle) : base(stateMachine, characterEnemy)
    {
        this.footParticle = footParticle;
        this.collisionParticle = collisionParticle;
        hitEffectController = new HitEffectController();
    }
    public override void Awake()
    {
        initialMass = enemy.Rigidbody.mass;
        enemy.Rigidbody.mass = 100;
        initialDrag = enemy.Rigidbody.drag;
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.SetAnimation("prepare", 0);
        var closest = enemy.CharacterManager.GetClosestEnemyInRange(enemy.team, 00000, enemy.transform.position);
        if (closest != null)
        {
            enemy.model.transform.forward = closest.transform.position - enemy.transform.position;
        }
        counter = 0;
        prepare = true;
    }

    public override void Sleep()
    {
        footParticle.Stop();
        enemy.Rigidbody.mass = initialMass;
        enemy.Rigidbody.drag = initialDrag;

    }

    public override void Update()
    {
        counter += Time.deltaTime;
        if (prepare)
        {
            if (counter > PREPARE_TIME)
            {
                prepare = false;
                enemy.Rigidbody.drag = 0;
                enemy.SetAnimation("dash", 0);
                footParticle.Play();

                enemy.Rigidbody.velocity = enemy.model.transform.forward * DASH_SPEED;
            }
        }
        else
        {
            if (counter > DURATION)
            {
                ChangeState(enemy.IdleState);
            }
        }
    }

    internal override void OnCollisionEnter(Collision collision)
    {
        if (enemy.HitsLayer(collision.gameObject.layer))
        {
            var character = collision.gameObject.GetComponentInParent<Character>();
            hitEffectController.CreateEffect(enemy.transform.position, character, 0.97f, enemy.model.transform.forward);
            collisionParticle.Play();
            enemy.Rigidbody.velocity = enemy.model.transform.forward * DASH_SPEED;

            character.GetHit(enemy, DAMAGE_MULTIPLIER);

        }
    }
}
