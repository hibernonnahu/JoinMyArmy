using UnityEngine;


public class StateCharacterEnemySpawn : StateCharacterEnemy
{
    private const float SPAWN_OFFSET = 5;
    private CharacterEnemy[] spawnEnemies;
    private float castDuration;
    private float castDurationAfter;
    private float counter;
    private ParticleSystem particles;
    private AudioSource audio;
    public StateCharacterEnemySpawn(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, ParticleSystem particles, AudioSource audio, CharacterEnemy[] spawnEnemies, float castDuration, float castDurationAfter) : base(stateMachine, characterEnemy)
    {
        this.spawnEnemies = spawnEnemies;
        this.castDuration = castDuration;
        this.castDurationAfter = castDurationAfter;
        this.particles = particles;
        this.audio = audio;
    }
    public override void Awake()
    {
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.SetAnimation("cast", 0);
        if (particles != null)
        {
            particles.Stop();
            particles.Play();
        }
        if (audio != null)
        {
            audio.Play();
        }
        counter = 0;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter += Time.deltaTime;
        if (counter > castDuration)
        {

            foreach (var spawnEnemy in spawnEnemies)
            {
                if (spawnEnemy.IsDead)
                {
                    spawnEnemy.team = enemy.team;

                    spawnEnemy.transform.position = enemy.transform.position + UnityEngine.Random.Range(-SPAWN_OFFSET, SPAWN_OFFSET) * Vector3.right + UnityEngine.Random.Range(-SPAWN_OFFSET, SPAWN_OFFSET) * Vector3.forward;
                    spawnEnemy.IsDead = false;
                    spawnEnemy.IsKnocked = false;
                    spawnEnemy.UpdateStatsOnLevel(spawnEnemy.level, false, false);
                    spawnEnemy.RecluitIconHandler.gameObject.SetActive(false);
                    spawnEnemy.Heal(spawnEnemy.Health, false);
                    spawnEnemy.GetComponent<EnemyStateAddDefaultInit>().SetDefaultInit();
                    spawnEnemy.ForceIdle();
                    if (spawnEnemy.GetComponent<EnemyStateAddCanBeRecluit>() != null)
                    {
                        spawnEnemy.CanBeRecluit = true;
                    }

                    spawnEnemy.Rigidbody.isKinematic = false;
                    spawnEnemy.HealthBarController.UpdateBarColor(spawnEnemy);
                    spawnEnemy.UpdateColor(!spawnEnemy.IsEnemy());
                    spawnEnemy.RecluitIconHandler.Restore();
                    spawnEnemy.extraAlertRange = enemy.extraAlertRange;

                    spawnEnemy.gameObject.layer = enemy.gameObject.layer;
                    spawnEnemy.SetColliderLayer(enemy.gameObject.layer);
                    foreach (var item in spawnEnemy.GetComponentsInChildren<MaterialOnTeam>())
                    {
                        item.UpdateMaterial();
                    }
                    spawnEnemy.gameObject.SetActive(true);
                    enemy.CharacterManager.AddCharacterEnemy(spawnEnemy, enemy.CharacterMain);
                    spawnEnemy.enabled = true;

                }
            }
            enemy.GeneralParticleHandler.wallHit.Stop();

            enemy.GeneralParticleHandler.wallHit.Play();
            enemy.NextState = enemy.IdleState;
            enemy.VulnerableTime = castDurationAfter;
            ChangeState(typeof(StateCharacterEnemyVulnerable));
        }
    }
}
