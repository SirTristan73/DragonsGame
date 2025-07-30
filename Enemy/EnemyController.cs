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

    public List<CreatureStats> _enemyPref;

    Vector3 _spawnerPosition;

    private Coroutine _spawnCoroutine;

    private float _xSpawnClamp = 6f;
    private float _zSpawnClamp = 1f;


    // Enemies in list
    private int _defaultEnemy = 0;
    private int _2ndlvl = 1;
    private int _3rdlvl = 3;

    // Wave types
    public int _waveType;

    // KillCount
    public int _enemiesKilled = 0;



    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        _spawnerPosition = this.transform.position;
        StartSpawnSequence(15, _defaultEnemy, 1f);

    }

    public void StartSpawnSequence(int count, int type, float spawnDelay)
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }
        _spawnCoroutine = StartCoroutine(EnemySpawn(count, type, spawnDelay));
    }

    private IEnumerator DelayedSpawnSequence(float initialDelay, int count, int type, float spawnDelay)
    {
        yield return new WaitForSeconds(initialDelay);
        StartSpawnSequence(count, type, spawnDelay);
    }



    private IEnumerator EnemySpawn(int count, int type, float delay)
    {
        GameObject enemy = _enemyPref[type]._model;

        int waveType = type;

        float health = _enemyPref[type].BaseStats._health;
        float speed = _enemyPref[type].BaseStats._speed;

        for (int i = 0; i < count; i++)
        {

            float posToSpawnX = Random.Range(-_xSpawnClamp, _xSpawnClamp) + _spawnerPosition.x;
            float posToSpawnZ = Random.Range(-_zSpawnClamp, _zSpawnClamp) + _spawnerPosition.z;

            Vector3 spawnPos = new Vector3(posToSpawnX, _spawnerPosition.y, posToSpawnZ);



            GameObject enemyInstance = Instantiate(enemy, spawnPos, Quaternion.identity);

            var enemyStats = enemyInstance.GetComponent<EnemyBehavior>();

            enemyStats.SetEnemyStats(health, speed);

            yield return new WaitForSeconds(delay);

        }

        _spawnCoroutine = null;

        waveType++;

        if (waveType >= 2)
        {
            waveType = 0;
        }

        StartCoroutine(DelayedSpawnSequence(1f, 3, waveType, 1f));

    }


    public void EnemyKilled()
    {
        _enemiesKilled++;
    }

    
    
 
    
}
