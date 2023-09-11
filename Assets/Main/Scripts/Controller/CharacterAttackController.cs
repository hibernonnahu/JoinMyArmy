
using UnityEngine;

public interface ICharacterAttackController
{
    void Init(CharacterMain characterMain, Animator animator, CharacterManager characterManager);
    void GoIdle();
    void GoAlert();
    void GoAttack();
    void GoUpdate();
}

