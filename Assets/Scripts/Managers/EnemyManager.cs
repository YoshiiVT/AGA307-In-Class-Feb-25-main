using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;

public enum EnemyType
{
    Onehanded,
    Twohanded,
    Archer
}

public enum PatrolType
{
    Linear,
    PingPong,
    Random,
}

public enum EnemyState 
{ 
    Patrol,
    Detect,
    Chase,
    Attack,
    Hit,
    Die
}
public class EnemyManager : Singleton<EnemyManager>
{
    public int initialSpawnCount = 6;
    public float initialSpawnDelay = 1;

    public string killCondition = "J";

    private string[] enemyNames = new string[] { "Wovok the Abomination", "Kadan the Corruptor", "Istadum the Doctor", "Stratic Shade", "Riovok Livid", "Pokhar the Animator", "Stigan the Plaguemaster", "Nobrum Rotheart", "Soutic the Hallowed", "Wraexor Doomwhisper", "Yauzius the Corruptor", "Utozad Morbide", "Gitic the Animator", "Zothik the Fleshrender", "Ezaurow Mortice", "Owobrum the Crippled", "Vrozor the Black", "Grethum Plasma", "Kezad Calamity", "Hekras Blight " };
    public Transform[] spawnPoints;
    public GameObject[] enemyTypes;
    public List<Enemy> enemies;

    public int EnemyCount => enemies.Count;
    public bool NoEnemies => enemies.Count == 0;

    public Transform GetRandomSpawnPoint => spawnPoints[Random.Range(0, spawnPoints.Length)];
    public Transform GetSpecificSpawnPoint(int _spawnPoint) => spawnPoints[_spawnPoint];
    void Start()
    {
        //SpawnEnemies();
        StartCoroutine(SpawnWithDelay(initialSpawnCount, initialSpawnDelay));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillRandomEnemy();
        if (Input.GetKeyDown(KeyCode.J))
            KillSpecificEnemy(killCondition);
        if (Input.GetKeyDown(KeyCode.H))
            KillAllEnemies();
        if (Input.GetKeyDown(KeyCode.C))
            KillClosestEnemy();
    }

    /// <summary>
    /// Spawns the set amount of enemies with a delay between each spawn.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWithDelay (int _spawnCount, float _spawnDelay)
    {
       
        for (int i = 0; i < _spawnCount; i++)
        {
            yield return new WaitForSeconds(_spawnDelay);
            if (_CurrentGameState == GameState.Playing)
            SpawnEnemy();
        }
        
    }

    /// <summary>
    /// Spawns enemies up until the inital spawn count is met
    /// </summary>
    private void SpawnEnemies()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnEnemy();
        }

    }

    /// <summary>
    /// Spawns a single enemy into our scene at random
    /// </summary>
    private void SpawnEnemy()
    {
        //Instantiate(enemyTypes[0]);
        //enemyTypes[0] = Instantiate(enemyTypes[0], spawnPoints[0].rotation);

        int rndEnemy = Random.Range(0, enemyTypes.Length);
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        int rndName = Random.Range(0, enemyNames.Length);

        GameObject enemy = Instantiate(enemyTypes[rndEnemy], spawnPoints[rndSpawn].transform.position, spawnPoints[rndSpawn].transform.rotation);
        enemy.name = enemyNames[rndName];
        
        enemy.GetComponent<Enemy>().Initialize(GetRandomSpawnPoint, enemy.name);
        enemies.Add(enemy.GetComponent<Enemy>());

        _UI.UpdateEnemyCount(EnemyCount);



    }
    
    /// <summary>
    /// Kills an Enemy
    /// </summary>
    /// <param name="_enemy">The Enemy we want to kill</param>
    private void KillEnemy(Enemy _enemy)
    {
        GameManager.instance.AddScore(_enemy.MyScore);
        Destroy(_enemy.gameObject);
        enemies.Remove(_enemy);
        _UI.UpdateEnemyCount(EnemyCount);
    }

    /// <summary>
    /// Kills a Random Enemy in our scene
    /// </summary>
    private void KillRandomEnemy()
    {
        if (NoEnemies)
            return;

        int rndEnemy = Random.Range(0, EnemyCount);
        KillEnemy(enemies[rndEnemy]);
    }

    private void KillClosestEnemy()
    {
        Transform closest = GetClosestObject(_PLAYER.transform, enemies).transform;
        print(closest.name);
        KillEnemy(closest.GetComponent<Enemy>());
    }

    /// <summary>
    /// Kills a specific enemy in our scene that meets a specidied condition
    /// </summary>
    /// <param name="_condition">The condition to check agaisnt</param>
    private void KillSpecificEnemy(string _condition)
    {
        if (NoEnemies)
            return;

        for(int i = 0; i< EnemyCount; i++)
        {
            if (enemies[i].name.Contains(_condition))
            {
                KillEnemy(enemies[i]);
            }
        }

    }

    /// <summary>
    /// Kills all enemies in our scene
    /// </summary>
    private void KillAllEnemies()
    {
        if (NoEnemies)
            return;
            
        for(int i = EnemyCount - 1; i>= 0; i--)
            KillEnemy(enemies[i]);
    }
}


// Game Manager () > Enemy Manager() > Sound Manager() > UI Manager()
// Declrations < Editor < Functions
// Loop = (Start > EndCondition > Update)
//for (int i = 0; i <= 100; i++)
//{
//    print(i);
//}
// once a game starts, arrays cant be modified
// Lists are better with List.count (Bring in "using System.Collections.Generic;")