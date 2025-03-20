using Unity.Burst;
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : GameBehaviour
{
    [Header("Basics")]
    public EnemyType myType;
    public PatrolType myPatrolType;
    public float moveDistance = 1000f;
    public float stoppingDistance = 0.3f;

    [Header("Stats")]
    private float mySpeed = 5;
    public int myHealth;
    private int myMaxHealth;
    private int myDamage;

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
    public void Initialize(Transform _startPos, string _name)
    {
        switch(myType)
        {
            case EnemyType.Onehanded:
                mySpeed = 10;
                myHealth = 100;
                myDamage = 100;
                myScore = 100;
                myPatrolType = PatrolType.Linear;
                break;
            case EnemyType.Twohanded:
                mySpeed = 5;
                myHealth = 200;
                myDamage = 200;
                myScore = 200;
                myPatrolType = PatrolType.PingPong;
                break;
            case EnemyType.Archer:
                mySpeed = 20;
                myHealth = 50;
                myDamage = 75;
                myScore = 150;
                myPatrolType = PatrolType.Random;
                break;
            default:
                mySpeed = 100;
                myHealth = 100;
                myDamage = 100;
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

        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        switch(myPatrolType)
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
        transform.LookAt(moveToPos);

        while (Vector3.Distance(transform.position, moveToPos.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, mySpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(Move());
    }

    public void Hit(int _damage)
    {
        myHealth -= _damage;
        _GM.AddScore(myScore);

        healthBar.UpdateHealthBar(myHealth, myMaxHealth);

        if (myHealth <= 0)
            myHealth = 0;
            Die();
    }

    public void Die()
    {
        StopAllCoroutines();
        
    }
    /*
    private IEnumerator Move()
    {
        for(int i = 0; i < moveDistance; i++)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            yield return null;
        }
        transform.Rotate(Vector3.up * 180);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine(Move());
        }
    */
}
