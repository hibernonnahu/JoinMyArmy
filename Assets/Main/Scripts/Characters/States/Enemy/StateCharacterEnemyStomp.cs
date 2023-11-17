using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyStomp : StateCharacterEnemy
{
    private const float DAMAGE_SQR_DISTANCE = 250;
    private const float DAMAGE_MULTIPLIER = 1f;
    private ParticleSystem particle;

    private AudioSource audio;
    private HitEffectController hitEffectController;
    private float counter;
    private float animationTime;
    private float vulnerableTime;

    public StateCharacterEnemyStomp(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, ParticleSystem particle, AudioSource audio, float animationTime, float vulnerableTime) : base(stateMachine, characterEnemy)
    {
        this.particle = particle;
        this.animationTime = animationTime;
        this.vulnerableTime = vulnerableTime;
        this.audio = audio;
        hitEffectController = new HitEffectController();
    }
    public override void Awake()
    {
        enemy.SetAnimation("stomp", 0);
        counter = animationTime;
        enemy.Rigidbody.velocity = Vector3.zero;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            particle.Play();
            audio.Play();
            EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(1f));
            enemy.VulnerableTime = vulnerableTime;
            enemy.NextState = enemy.IdleState;
            enemy.transform.position = enemy.transform.position.x * Vector3.right + enemy.transform.position.z * Vector3.forward;
            foreach (var character in enemy.CharacterManager.GetEnemiesInRange(enemy.team, DAMAGE_SQR_DISTANCE, enemy.transform.position))
            {
                character.GetHit(enemy, DAMAGE_MULTIPLIER);
                hitEffectController.CreateEffect(enemy.transform.position, character, 1.5f);
            }
            ChangeState(typeof(StateCharacterEnemyVulnerable));
        }
    }



}
