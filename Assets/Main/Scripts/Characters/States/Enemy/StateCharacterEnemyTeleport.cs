using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyTeleport : StateCharacterEnemy
{
    private const float DISAPEAR_TIME = 2;
    private float minX;
    private float minZ;
    private float maxX;
    private float mmaxZ;
    private int mask;
    private float counter;
    private Vector3 initialLocal;
    public StateCharacterEnemyTeleport(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {
        mask = LayerMask.GetMask(new string[] { "Bound", "Wall", "Water" });
        initialLocal = enemy.GeneralParticleHandler.wallHit.transform.localPosition;

    }
    public override void Awake()
    {
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("smoke"));
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.GeneralParticleHandler.wallHit.transform.localPosition = initialLocal;
        enemy.GeneralParticleHandler.wallHit.Stop();

        enemy.GeneralParticleHandler.wallHit.Play();
        Ray ray = new Ray(enemy.transform.position + Vector3.up, Vector3.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
            maxX = hit.point.x;
        }
        ray = new Ray(enemy.transform.position + Vector3.up, Vector3.left);
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
            minX = hit.point.x;
        }
        ray = new Ray(enemy.transform.position + Vector3.up, Vector3.forward);
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
            mmaxZ = hit.point.x;
        }
        ray = new Ray(enemy.transform.position + Vector3.up, Vector3.back);
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
            minZ = hit.point.x;
        }
        enemy.transform.position -= Vector3.up * 1000;
        counter = DISAPEAR_TIME;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            enemy.GeneralParticleHandler.wallHit.transform.localPosition = initialLocal;

            enemy.transform.position = Vector3.right * UnityEngine.Random.Range(minX, maxX) + Vector3.back * UnityEngine.Random.Range(minZ, mmaxZ);
            enemy.GeneralParticleHandler.wallHit.Stop();
            EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("pop"));

            enemy.GeneralParticleHandler.wallHit.Play();
            enemy.VulnerableTime = 1;
            enemy.SetAnimation("idle");
            enemy.NextState = enemy.IdleState;
            ChangeState(typeof(StateCharacterEnemyVulnerable));
        }
    }
}
