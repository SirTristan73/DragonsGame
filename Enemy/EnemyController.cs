using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System.Threading;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public static EnemyController SharedInstance;

    [Header("Common enemies")]
    public List<CreatureStats> _enemyPref;
    private List<CreatureStats> _enemiesToSpawn;

    Vector3 _spawnerPosition;

    private Coroutine _spawnCoroutine;

    private float _xSpawnClamp = 5.7f;
    private float _zSpawnClamp = 1f;



    [Header("Bosses")]
    public List<CreatureStats> _bossesPref;

    // Wave types
    public int _waveType = 1;

    // KillCount
    public int _enemiesKilled = 0;

    public int _wavePoints = 15;

    private int _enemiesLeft = 0;

    private float _defaultDelay = 1f;




    private void Awake()
    {
        SharedInstance = this;
    }


    private void OnEnable()
    {
        //TODO
        _enemiesKilled = 0;
        _waveType = 1;
        _enemiesLeft = 0;
    }


    void Start()
    {
        _spawnerPosition = this.transform.position;
        CalculateNextEnemySpawn();

    }


    public void CalculateNextEnemySpawn()
    {
        int minPointsNext = _wavePoints / (_waveType == 0 ? 1 : _waveType);
        int maxPointsNext = _wavePoints * 2;
        int pointsForNextWave = Random.Range(minPointsNext, maxPointsNext);

        _enemiesToSpawn = WaveIncludes(pointsForNextWave);

        StartSpawnSequence(_enemiesToSpawn);

    }


    public void StartSpawnSequence(List<CreatureStats> spawnOrder)
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }

        _spawnCoroutine = StartCoroutine(EnemySpawn(spawnOrder));
    }


    private IEnumerator EnemySpawn(List<CreatureStats> spawningEnemies)
    {
        _enemiesLeft = 0;

        for (int i = 0; i < spawningEnemies.Count; i++)
        {
            GameObject enemy = spawningEnemies[i]._model;

            float health = spawningEnemies[i].BaseStats._health;
            float speed = spawningEnemies[i].BaseStats._speed;

            float posToSpawnX = Random.Range(-_xSpawnClamp, _xSpawnClamp) + _spawnerPosition.x;
            float posToSpawnZ = Random.Range(-_zSpawnClamp, _zSpawnClamp) + _spawnerPosition.z;

            Vector3 spawnPos = new Vector3(posToSpawnX, _spawnerPosition.y, posToSpawnZ);

            GameObject enemyInstance = Instantiate(enemy, spawnPos, Quaternion.identity);

            var enemyStats = enemyInstance.GetComponent<EnemyBehavior>();

            _enemiesLeft++;

            enemyStats.SetEnemyStats(health, speed);

            yield return new WaitForSeconds(_defaultDelay);
        }

        _spawnCoroutine = null;

    }


    private List<CreatureStats> WaveIncludes(int budget)
    {
        List<CreatureStats> waveIncludes = new List<CreatureStats>();
        int currentBudget = budget;

        while (currentBudget > 0)
        {
            List<CreatureStats> affordableEnemies = _enemyPref
            .Where(enemy => enemy.BaseStats._points <= currentBudget)
            .ToList();
            if (affordableEnemies.Count == 0)
            {
                break;
            }
            CreatureStats chosenEnemy = affordableEnemies[Random.Range(0, affordableEnemies.Count)];
            waveIncludes.Add(chosenEnemy);
            currentBudget -= chosenEnemy.BaseStats._points;
        }
        return waveIncludes;
    }


    public void EnemyKilled()
    {
        _enemiesLeft--;
        _enemiesKilled++;

        if (_enemiesLeft == 0)
        {
            NoEnemiesLeft();
        }
    }


    public void NoEnemiesLeft()
    {
        SpawnBoss();
    }


    public void SpawnBoss()
    {
        int bossToSpawn = Random.Range(0, _bossesPref.Count);

        GameObject bossSpawned = _bossesPref[bossToSpawn]._model;

        float bossHP = _bossesPref[bossToSpawn].BaseStats._health;
        float bossSpeed = _bossesPref[bossToSpawn].BaseStats._speed;

        GameObject bossInstance = Instantiate(bossSpawned, this.transform.position, Quaternion.identity);

        var bossStats = bossInstance.GetComponent<BossBehavior>();


        bossStats.SetBossStats(bossHP, bossSpeed);
        
    }


    public void BossKilled()
    {
        _waveType++;
        CalculateNextEnemySpawn();
    }
    
}
