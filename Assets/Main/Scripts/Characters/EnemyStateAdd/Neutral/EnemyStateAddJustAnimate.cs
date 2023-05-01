using System;
using UnityEngine;

public class EnemyStateAddJustAnimate : EnemyStateAddAttack
{
    public string animationName = "";
    public string soundName = "";
    public float animationSpeed = 1;
    protected CharacterEnemy characterEnemy;
    protected float lenght = -1;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        
        GetClipLength();
        return this;
    }

    private void GetClipLength()
    {

        AnimationClip[] clips = characterEnemy.animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                lenght = clip.length;
            }
        }
        if (lenght == -1)
        {
            throw new Exception("animation name not found " + animationName);
        }

    }
    public override float Execute()
    {
        characterEnemy.Rigidbody.velocity = Vector3.zero;
        characterEnemy.SetAnimation(animationName, 0.02f);
        characterEnemy.VulnerableTime = lenght / animationSpeed;

        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString(soundName));
        characterEnemy.NextState = characterEnemy.IdleState;
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyVulnerable));
        return base.Execute();
    }

}
