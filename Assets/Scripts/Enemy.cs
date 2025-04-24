using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameBehaviour
{
    [Header("Basics")]
    public EnemyType myType;
    public EnemyState myState;
    public PatrolType myPatrolType;
    public float moveDistance = 1000f;
    public float stoppingDistance = 0.3f;

    [Header("Stats")]
    private float mySpeed = 5;
    public int myHealth;
    private int myMaxHealth;

    [Header("Score")]
    private int myScore;
    public int MyScore => myScore;

    [Header("Patrols")]
    private Transform moveToPos;    //Needed for All Movement
    private Transform startPos;     //Needed for PingPong Movement
    private Transform endPos;       //Needed for PingPong Movement
    private bool reverse;           //Needed for PingPong Movement
    private int patrolPoint;        //Needed for Linear Movement

    [Header("Health Bar")]
    public HealthBar healthBar;

    [Header("AI")]
    [SerializeField]
    private float detectDistance = 50f;
    [SerializeField] private float detectTime = 5f;
    private float currentDetectTime;
    [SerializeField] private float attackDistance = 1f;

    [Header("Animator")]
    [SerializeField]
    private Animator anim;
    private Weapon enemyWeapon;

    [Header("NavMesh")]
    public NavMeshAgent agent;

    [Header("Audio")]
    private AudioSource audioSource;
    private void ChangeSpeed(float _speed) => agent.speed = _speed;
    
    public void Initialize(Transform _startPos, string _name)
    {
        switch(myType)
        {
            case EnemyType.Onehanded:
                mySpeed = 3;
                myHealth = 100;
                myScore = 100;
                myPatrolType = PatrolType.Linear;
                break;
            case EnemyType.Twohanded:
                mySpeed = 2;
                myHealth = 200; 
                myScore = 200;
                myPatrolType = PatrolType.PingPong;
                break;
            case EnemyType.Archer:
                mySpeed = 4;
                myHealth = 50;
                myScore = 150;
                myPatrolType = PatrolType.Random;
                break;
            default:
                mySpeed = 100;
                myHealth = 100;
                myScore = 100;
                myPatrolType = PatrolType.Random;
                break;
        }
        myMaxHealth = myHealth;

        startPos = _startPos;
        endPos = _EM.GetRandomSpawnPoint;
        moveToPos = endPos;

        healthBar.SetName(_name);
        healthBar.UpdateHealthBar(myHealth, myMaxHealth);
        /*
        if(myType == EnemyType.Onehanded) 
        {
            mySpeed = 10;
            myHealth = 100;
            myDamage = 100;
        }

        if (myType == EnemyType.Onehanded)
        {
            mySpeed = 5;
            myHealth = 200;
            myDamage = 200;
        }
        
        if (myType == EnemyType.Onehanded)
        {
            mySpeed = 20;
            myHealth = 50;
            myDamage = 75;
        }
        */

        if (GetComponentInChildren<Weapon>() != null)
        {
            enemyWeapon = GetComponentInChildren<Weapon>();
        }
        else
        {
            Debug.LogError("Couldn't find weapon");
            return;
        }

        if (GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();

        audioSource = GetComponent<AudioSource>();

            SetupAi();
    }

    private void SetupAi()
    {
        myState = EnemyState.Patrol;
        switch (myPatrolType)
        {
            case PatrolType.Linear:
                moveToPos = _EM.GetSpecificSpawnPoint(patrolPoint);
                patrolPoint = patrolPoint != _EM.spawnPoints.Length - 1 ? patrolPoint + 1 : 0; //Brendan Explain Next Week
                break;

            case PatrolType.PingPong:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;

            case PatrolType.Random:

                moveToPos = _EM.GetRandomSpawnPoint;
                break;
        }
        agent.SetDestination(moveToPos.position);
        currentDetectTime = detectTime;
        ChangeSpeed(mySpeed);
    }

    private void Update()
    {
        if (myState == EnemyState.Die)
            return;

        anim.SetFloat("Speed", agent.speed);

        //Get the distance between us and the player
        float distToPlayer = Vector3.Distance(transform.position, _PLAYER.transform.position);
        if(distToPlayer < detectDistance && myState != EnemyState.Attack)
        {
            if (myState != EnemyState.Chase)
                myState = EnemyState.Detect;
        }

        switch (myState)
        {
            case EnemyState.Patrol:
                //Get the distance between us and the destination
                float distToDestination = Vector3.Distance(transform.position, moveToPos.position);
                //Debug.Log(distToDestination);
                if (distToDestination < 1)
                    SetupAi();
                break;

            case EnemyState.Detect:
                ChangeSpeed(0);
                agent.SetDestination(transform.position);
                currentDetectTime -= Time.deltaTime;

                    if (distToPlayer <= detectDistance)
                    {
                        myState = EnemyState.Chase;
                        currentDetectTime = detectTime;
                    }
                    if (currentDetectTime <= 0)
                    {
                        SetupAi();
                    }
                    break;

            case EnemyState.Chase:
                ChangeSpeed(mySpeed * 1.5f);
                agent.SetDestination(_PLAYER.transform.position);
                if (distToPlayer > detectDistance)
                    myState = EnemyState.Detect;
                if (distToPlayer < attackDistance)
                {
                    ChangeSpeed(0);
                    StartCoroutine(Attack());
                }
                break;


                case EnemyState.Hit:
                    ChangeSpeed(0);
                    break;
        }     
    }

    private IEnumerator Attack()
    {
        myState = EnemyState.Attack;
        PlayAnimation("Attack", 3);
        yield return new WaitForSeconds(2f);
        myState = EnemyState.Chase;
    }

    private IEnumerator Hit()
    {
        myState = EnemyState.Hit;
        ChangeSpeed(0);
        yield return new WaitForSeconds(0.5f);
        myState = EnemyState.Chase;
    }

    public void Hit(int _damage)
    {
        myHealth -= _damage;
        _GM.AddScore(myScore);

        healthBar.UpdateHealthBar(myHealth, myMaxHealth);

        if (myHealth <= 0)
        {
            myHealth = 0;
            Die();
        }
        else
        {
            StartCoroutine(Hit());
            PlayAnimation("Hit", 3);
            _AUDIO.PlayEnemyHit(audioSource);
        }
    }

    public void Die()
    {
        myState = EnemyState.Die;
        ChangeSpeed(0);
        agent.SetDestination(transform.position);
        PlayAnimation("Die", 3);
        _AUDIO.PlayEnemyDie(audioSource);
        GetComponent<Collider>().enabled = false;
        healthBar.gameObject.SetActive(false);
    }

    private void PlayAnimation(string _animationName, int _animationCount)
    {
        int rnd = Random.Range(1, _animationCount + 1);
        anim.SetTrigger(_animationName + rnd);
    }

    public void EnableCollider()
    {
        _AUDIO.PlayEnemyAttack(audioSource);
        enemyWeapon.SetCollider(true);
    }

    public void DisableCollider()
    {
        enemyWeapon.SetCollider(false);
    }
    
    public void Footstep()
    {
        _AUDIO.PlayFootstep(audioSource);
    }
}
