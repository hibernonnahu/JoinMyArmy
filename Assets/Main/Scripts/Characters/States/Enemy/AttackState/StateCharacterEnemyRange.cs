using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyRange : StateCharacterEnemy
{
    private const float SCALE_TIME = 0.3f;
    private const float Y_BULLET_POSITION = 2f;
    private BulletPool bulletPool;
    private GameObject[] toHide;

    private float bulletSpeed;
    private bool shot;
    private ParticleSystem preCast;
    private AudioSource preCastSound;
    private float castExpulsion;
    private float expulsionSqrRange;
    private HitEffectController hitEffectController;

    public StateCharacterEnemyRange(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, BulletPool bulletPool, GameObject[] toHide, float bulletSpeed, ParticleSystem preCast, float castExpulsion, float expulsionSqrRange) : base(stateMachine, characterEnemy)
    {
        this.bulletPool = bulletPool;
        this.toHide = toHide;
        this.bulletSpeed = bulletSpeed;
        this.castExpulsion = castExpulsion;
        this.expulsionSqrRange = expulsionSqrRange;
        if (castExpulsion > 0)
        {
            hitEffectController = new HitEffectController();
        }
        this.preCast = preCast;
        if (preCast != null)
        {
            preCastSound = preCast.GetComponent<AudioSource>();
        }
    }
    public override void Awake()
    {
        if ((enemy.transform.position - enemy.lastEnemyTarget.transform.position).sqrMagnitude < enemy.attackDistanceSqr)
        {
            LoadAttack();
        }
        else
        {
            enemy.NextState = typeof(StateCharacterEnemyRange);
            ChangeState(typeof(StateCharacterEnemyChase));
        }
    }
    private void LoadAttack()
    {
        enemy.animator.SetFloat("attackspeed", enemy.AttackSpeed);
        if (preCast != null)
        {
            preCast.Stop();
            preCast.Play();
            if (preCastSound != null)
            {
                preCastSound.Play();
            }
        }
        if (castExpulsion > 0)
        {
            foreach (var toExpulse in enemy.CharacterManager.GetEnemiesInRange(enemy.team, expulsionSqrRange, enemy.transform.position))
            {

                hitEffectController.CreateEffect(enemy.transform.position, toExpulse, castExpulsion);
            }
        }
        AttackInit();
    }
    private void AttackInit()
    {
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.SetAnimation("attack", 0.02f, 0);
        enemy.Rigidbody.drag = 50;

        shot = false;
        enemy.model.transform.forward = enemy.lastEnemyTarget.transform.position - enemy.transform.position;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        if (!shot)
        {

            if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("attack end"))
            {
                shot = true;
                var bullet = bulletPool.GetBullet(enemy);
                foreach (var item in toHide)
                {
                    bullet.transform.position = item.transform.position;
                    
                    LeanTween.cancel(item);
                    LeanTween.scale(item, Vector3.zero, SCALE_TIME);
                    LeanTween.scale(item, Vector3.one, SCALE_TIME).setDelay(SCALE_TIME);
                }
                bullet.transform.position = bullet.transform.position.x * Vector3.right + bullet.transform.position.z * Vector3.forward + Vector3.up * Y_BULLET_POSITION + enemy.model.transform.forward * 2;
                bullet.Shot(bulletSpeed);
            }
        }
        else
        {
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                ChangeState(enemy.IdleState);
            }
        }
    }
}
