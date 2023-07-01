using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyMelee : StateCharacterEnemy
{
    private float counter;
    private bool hit;
    private ParticleSystem hitParticle;
    private AudioSource swing;
    private AudioSource hitSound;
    public StateCharacterEnemyMelee(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, ParticleSystem hitParticle, AudioSource swing, AudioSource hit) : base(stateMachine, characterEnemy)
    {
        this.hitSound = hit;
        this.hitParticle = hitParticle;
        this.swing = swing;
    }
    public override void Awake()
    {
        enemy.animator.SetFloat("attackspeed", enemy.AttackSpeed);
        AttackInit();
    }

    private void AttackInit()
    {
        if (swing != null)
        {
            swing.Play();
        }

        enemy.Rigidbody.velocity = Vector3.zero;

        enemy.SetAnimation("attack", 0.01f, 0);
        counter = (1 / enemy.AttackSpeed) * 0.35f;
        hit = false;
        enemy.model.transform.forward = enemy.lastEnemyTarget.transform.position - enemy.transform.position;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (!hit)
        {
            enemy.Rigidbody.velocity = Vector3.zero;
            if (counter < 0)
            {
                if (enemy.lastEnemyTarget.CurrentHealth > 0 && enemy.team != enemy.lastEnemyTarget.team && (enemy.transform.position - enemy.lastEnemyTarget.transform.position).sqrMagnitude < enemy.attackDistanceSqr)
                {
                    if (hitSound != null)
                    {
                        hitSound.Play();
                    }
                    if (hitParticle != null)
                    {
                        hitParticle.Stop();
                        hitParticle.Play();
                    }
                    EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("damage"));
                    enemy.lastEnemyTarget.GetHit(enemy);
                }
                else
                {
                    enemy.lastEnemyTarget = null;
                }
                hit = true;
                counter = enemy.AttackSpeed * 0.65f;
            }
        }
        else
        {
            if (counter < 0)
            {
                ChangeState(enemy.IdleState);
            }
        }
    }
}
