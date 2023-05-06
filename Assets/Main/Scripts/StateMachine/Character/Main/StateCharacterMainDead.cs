using System;
using UnityEngine;


public class StateCharacterMainDead : StateCharacter
{
    private const float DELAY_LOSE_POPUP = 2.5f;
    private CharacterMain characterMain;
    public StateCharacterMainDead(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        this.characterMain = character;
    }

    public override void Awake()
    {
        character.IdleState = typeof(StateCharacterMainDead);
        character.Rigidbody.velocity = Vector3.zero;
        character.CharacterManager.RemoveCharacter(character);
        character.SetAnimation("dead");
        characterMain.IsDead = true;
        characterMain.lastEnemyTarget = null;
        character.Rigidbody.drag = 100;
        LeanTween.delayedCall(DELAY_LOSE_POPUP, DisplayLosePopup);
    }
    public override void ChangeState(Type type)
    {

    }
    public override bool GetHit(float damage, Character attacker)
    {
        return false;
    }
    private void DisplayLosePopup()
    {
        SaveData.GetInstance().SaveRam();
        EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.LOSE));
    }
}
