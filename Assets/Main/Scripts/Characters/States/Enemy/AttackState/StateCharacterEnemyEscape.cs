using UnityEngine;

public class StateCharacterEnemyEscape : StateCharacterEnemy
{
    private const float TICK_TIME = 1.2f;
    protected float counter;
    public StateCharacterEnemyEscape(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        counter = TICK_TIME;
        Move();
        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            enemy.SetAnimation("walk");
        }
        enemy.Rigidbody.drag = 0;

    }
    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            ChangeState(enemy.NextState);
        }
    }

    protected void Move()
    {
        Vector3 difVector = enemy.transform.position - enemy.lastEnemyTarget.transform.position + (enemy.ReturnPosition - enemy.transform.position) * Random.Range(0.5f, 3f);

        difVector = Quaternion.AngleAxis(-Random.Range(-80f, 80), Vector3.up) * difVector;
        enemy.model.transform.forward = CustomMath.XZNormalize(difVector);
        enemy.Rigidbody.velocity = enemy.model.transform.forward * enemy.speed;
    }
    internal override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13|| collision.gameObject.layer == 11|| collision.gameObject.layer == 4)//Bound wall water
        {
            ChangeState(enemy.NextState);
        }
    }
}
