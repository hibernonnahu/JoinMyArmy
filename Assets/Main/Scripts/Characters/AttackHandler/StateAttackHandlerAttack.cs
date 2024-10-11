using UnityEngine;
using System.Collections;
using System;

public class StateAttackHandlerAttack : StateAttackHandler
{
    private float counter;
    private bool hit;
    private HitEffectController hitEffectController;

    public StateAttackHandlerAttack(StateMachine<StateAttackHandler> stateMachine, CharacterMain characterMain, Animator animator) : base(stateMachine, characterMain, animator)
    {
        hitEffectController = new HitEffectController();
    }
    public override void Awake()
    {
        characterMain.animator.SetFloat("attackspeed", characterMain.AttackSpeed);

        animator.SetLayerWeight(1, 1);

        characterMain.SetAnimation("attack", 0, 1);

        if (characterMain.FxHandler.slash != null)
        {
            var main = characterMain.FxHandler.slash.main;
            main.simulationSpeed = characterMain.AttackSpeed;
            LeanTween.delayedCall(characterMain.gameObject, (1 / characterMain.AttackSpeed) * 0.28f, PlaySlashFx);

        }
        counter = (1 / characterMain.AttackSpeed) * 0.35f;
        hit = false;

        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("slash"));
    }
    public override void Sleep()
    {
        animator.SetLayerWeight(1, 0);

    }
    private void PlaySlashFx()
    {
        characterMain.FxHandler.slash.Play();
    }
    public override void Update()
    {
        counter -= Time.deltaTime;
        if (!hit)
        {
            if (counter < 0)
            {
                if (characterMain.lastEnemyTarget != null && (characterMain.transform.position - characterMain.lastEnemyTarget.transform.position).sqrMagnitude < characterMain.attackDistanceSqr)
                {
                    if (characterMain.FxHandler.swordHit != null)
                    {
                        characterMain.FxHandler.swordHit.transform.position = characterMain.lastEnemyTarget.transform.position + Vector3.up * 3 + CustomMath.XZNormalize(characterMain.transform.position - characterMain.lastEnemyTarget.transform.position);
                        characterMain.FxHandler.swordHit.Stop();
                        characterMain.FxHandler.swordHit.Play();
                    }
                    hitEffectController.CreateEffect(characterMain.transform.position, characterMain.lastEnemyTarget, characterMain.lastEnemyTarget.GetHit(characterMain), Vector3.zero, 0.1f);
                }
                hit = true;

            }
        }
        else
        {
            if (!characterMain.animator.GetCurrentAnimatorStateInfo(1).IsName("attack"))
            {
                ChangeState(typeof(StateAttackHandlerAlert));
            }
        }
    }
}
