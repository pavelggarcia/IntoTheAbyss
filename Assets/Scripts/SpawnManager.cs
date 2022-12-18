using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _enemySpawnTime = 4.0f;
    private bool _stopSpawning = false;
    [SerializeField] private GameObject[] powerups;
    //private int _enemyCounter;
    private WaveManager _waveManager;
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
        //StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine("SpawnPowerUpRoutine");
        _waveManager.StartTheWaves();
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10.0f, 10.0f), 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }
    public void SpawnOneEnemy()
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-10.0f, 10.0f), 8, 0);
        GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {

            Vector3 posToSpawn = new Vector3(Random.Range(-10.0f, 10.0f), 8, 0);
            int randomPowerUp = Random.Range(0, 6);

            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }

    }


    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}
