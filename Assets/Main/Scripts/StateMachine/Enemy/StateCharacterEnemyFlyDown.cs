using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyFlyDown : StateCharacterEnemy
{

    private const float SPEED_DOWN = 100;
    private const float SEEK_DISTANCE_SQR = 10;
    private const float DAMAGE_SQR_DISTANCE = 50;
    private const float VULNERABLE_TIME = 3;
    private const float SPEED_ON_AIR = 15;
    private const float STAY_TIME = 2f;
    private ParticleSystem particle;
    private GameObject groundShadow;
    private bool seek;
    private bool hold;
    private float holdCount;
    private Vector3 destiny;
    private AudioSource audio;
    private HitEffectController hitEffectController;

    public StateCharacterEnemyFlyDown(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, ParticleSystem particle, GameObject groundShadow, AudioSource audio) : base(stateMachine, characterEnemy)
    {
        this.particle = particle;
        this.groundShadow = groundShadow;
        this.audio = audio;
        hitEffectController = new HitEffectController();
    }
    public override void Awake()
    {
        seek = true;
        hold = false;
        holdCount = STAY_TIME;
        enemy.SetAnimation("jumpdown", 0);
        var character = enemy.CharacterManager.GetRandomEnemy(enemy.team);
        if (character != null)
        {
            destiny = character.transform.position;
        }
        else
        {
            destiny = enemy.transform.position;
        }
        destiny = destiny.x * Vector3.right + destiny.z * Vector3.forward + Vector3.up * enemy.transform.position.y;
        enemy.Rigidbody.velocity = CustomMath.Normalize(destiny - enemy.transform.position) * SPEED_ON_AIR;

    }

    public override void Sleep()
    {
        groundShadow.SetActive(false);
        enemy.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        particle.Play();
        audio.Play();
        EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(1.5f));
    }

    public override void Update()
    {
        if (seek)
        {
            groundShadow.transform.position = enemy.transform.position.x * Vector3.right + Vector3.forward * enemy.transform.position.z;
            if ((destiny - enemy.transform.position).sqrMagnitude < SEEK_DISTANCE_SQR)
            {
                seek = false;
                enemy.Rigidbody.velocity = Vector3.zero;
                hold = true;
            }
        }
        else if (hold)
        {
            holdCount -= Time.deltaTime;
            if (holdCount < 0)
            {
                hold = false;
                enemy.Rigidbody.velocity = Vector3.down * SPEED_DOWN;
            }
        }
        else
        {
            if (enemy.transform.position.y <= 0.5f)
            {
                enemy.transform.position = enemy.transform.position.x * Vector3.right + enemy.transform.position.z * Vector3.forward;
                foreach (var character in enemy.CharacterManager.GetTeamMatesInRange((enemy.team + 1) % 2, DAMAGE_SQR_DISTANCE, enemy.transform.position))
                {
                    character.GetHit(enemy, 3);
                    hitEffectController.CreateEffect(enemy, character, 1.5f);
                }
                enemy.SetAnimation("land", 0);
                enemy.VulnerableTime = VULNERABLE_TIME;
                enemy.NextState = enemy.IdleState;
                
                ChangeState(typeof(StateCharacterEnemyVulnerable));
            }
        }
    }


    public override bool GetHit(float damage)
    {
        return false;
    }
}
