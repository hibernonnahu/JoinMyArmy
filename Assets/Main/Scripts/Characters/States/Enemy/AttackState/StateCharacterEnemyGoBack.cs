using UnityEngine;

public class StateCharacterEnemyGoBack : StateCharacterEnemy
{
    private const float TICK_TIME = 0.5f;
    protected float counter;
    public StateCharacterEnemyGoBack(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        counter = TICK_TIME;
        Vector3 difVector = CustomMath.XZNormalize(enemy.ReturnPosition + Vector3.forward * Random.Range(-3f, 3f) - enemy.transform.position);

        UpdateMovement(difVector.x, difVector.z);
        if (enemy.speed > 0)
            enemy.SetAnimation("walk");
    }
    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            ChangeState(enemy.IdleState);
        }
    }


    internal override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 4 || collision.gameObject.layer == 11)//Bound
        {
            ChangeState(enemy.IdleState);
        }
    }
}
