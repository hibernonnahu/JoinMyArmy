
using System;
using UnityEngine;


public class CharacterStory:MonoBehaviour
{
    public CharacterEnemy characterEnemy;
    private void Awake()
    {
       characterEnemy= GetComponent<CharacterEnemy>();
    }

    internal void GoToPosition(float x, float z, float speed)
    {
        characterEnemy.destiny = Vector3.right * x + Vector3.forward * z;
        characterEnemy.speed = speed;
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyGoToPosition(characterEnemy.StateMachine,characterEnemy));
        characterEnemy.StateMachine.ChangeState<StateCharacterEnemyGoToPosition>();
    }
}
