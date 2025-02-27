using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class EnemyManager : MonoBehaviour
{
    public int initialSpawnCount = 6;

    public string killCondition = "J";

    private string[] enemyNames = new string[] { "Wovok the Abomination", "Kadan the Corruptor", "Istadum the Doctor", "Stratic Shade", "Riovok Livid", "Pokhar the Animator", "Stigan the Plaguemaster", "Nobrum Rotheart", "Soutic the Hallowed", "Wraexor Doomwhisper", "Yauzius the Corruptor", "Utozad Morbide", "Gitic the Animator", "Zothik the Fleshrender", "Ezaurow Mortice", "Owobrum the Crippled", "Vrozor the Black", "Grethum Plasma", "Kezad Calamity", "Hekras Blight " };
    public Transform[] spawnPoints;
    public GameObject[] enemyTypes;
    public List<GameObject> enemies;

    public int EnemyCount => enemies.Count;
    public bool NoEnemies => enemies.Count == 0;
    void Start()
    {
        SpawnEnemies();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillRandomEnemy();
        if (Input.GetKeyDown(KeyCode.J))
            KillSpecificEnemy(killCondition);
        if (Input.GetKeyDown(KeyCode.H))
            KillAllEnemies();
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

        enemies.Add(enemy);

        print(enemies.Count);

    }
    
    /// <summary>
    /// Kills an Enemy
    /// </summary>
    /// <param name="_enemy">The Enemy we want to kill</param>
    private void KillEnemy(GameObject _enemy)
    {
        Destroy(_enemy);
        enemies.Remove(_enemy);
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