
using System;
using UnityEngine;


public class CharacterStory : MonoBehaviour
{
    public bool ignoreColliders = false;
    public CharacterEnemy characterEnemy;
    public ParticleSystem[] particles;
   
    private void Awake()
    {
        characterEnemy = GetComponent<CharacterEnemy>();
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyGoToPosition(characterEnemy.StateMachine, characterEnemy, ignoreColliders));

    }

    internal void GoToPosition(float x, float z, float speed)
    {
        characterEnemy.destiny = Vector3.right * x + Vector3.forward * z;
        characterEnemy.speed = speed;
        characterEnemy.StateMachine.ChangeState<StateCharacterEnemyGoToPosition>();
    }


}
