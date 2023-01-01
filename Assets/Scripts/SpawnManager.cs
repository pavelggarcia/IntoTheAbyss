using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemy3Prefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _enemySpawnTime = 4.0f;
    private bool _stopSpawning = false;
    [SerializeField] private GameObject[] powerups;
    private WaveManager _waveManager;
    [SerializeField] private GameObject _enemySatellitesPrefab;
    private int _satelliteSpawnTime = 15;
    [SerializeField] private GameObject _satelliteContainer;
    private int _enemyPicker;

    private void Start()
    {
        _waveManager = GetComponent<WaveManager>();
        if (_waveManager == null)
        {
            Debug.LogError("Wave Manager is NULL");
        }
        

    }

    public void StartSpawning()
    {
        
        StartCoroutine(SpawnCommonPowerUpRoutine());
        _waveManager.StartTheWaves();
        StartCoroutine(SpawnEnemySatellites());
        StartCoroutine(SpawnRarePowerUpRoutine());
    }

    
    public void SpawnOneEnemy()
    {
        _enemyPicker = Random.Range(1,3);
        
        Debug.Log(_enemyPicker);

        Vector3 posToSpawn = new Vector3(Random.Range(-18.0f, 18.0f), 13, 0);
        if(_enemyPicker == 1)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
        if(_enemyPicker == 2)
        {
            GameObject newEnemy = Instantiate(_enemy3Prefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        } 
    }
    IEnumerator SpawnRarePowerUpRoutine()
    {
        yield return new WaitForSeconds(30);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-18.0f, 18.0f), 13, 0);
            int randomPowerUp = Random.Range(0, 7);
            if (randomPowerUp == 2 || randomPowerUp == 4)
            {
                Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(15.0f, 30.0f));
            }
        }
    }

    IEnumerator SpawnCommonPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-18.0f, 18.0f), 13, 0);
            int randomPowerUp = Random.Range(0, 7);
            if (randomPowerUp != 2 && randomPowerUp != 4)
            {
                Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
            }
        }
    }
    IEnumerator SpawnEnemySatellites()
    {
        yield return new WaitForSeconds(_satelliteSpawnTime);
        _satelliteSpawnTime = Random.Range(30, 45);
        while (_stopSpawning == false)
        {
            GameObject newSatellite = Instantiate(_enemySatellitesPrefab, new Vector3(0, 13, 0), Quaternion.identity);
            newSatellite.transform.parent = _satelliteContainer.transform;
            yield return new WaitForSeconds(_satelliteSpawnTime);
        }

    }


    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}
