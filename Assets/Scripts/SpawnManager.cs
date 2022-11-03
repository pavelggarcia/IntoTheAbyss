using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _enemySpawnTime = 4.0f;
    private bool _stopSpawning = false;
    [SerializeField] private GameObject _TripleShotPrefab;
    //[SerializeField] private GameObject _SpeedPowerUpPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine("SpawnPowerUpRoutine");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-12.0f, 12.0f), 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {

        // every 3 - 7 seconds spawn power up
        while (_stopSpawning == false)
        {

            Vector3 posToSpawn = new Vector3(Random.Range(-12.0f, 12.0f), 8, 0);
            //Vector3 posToSpawnSpeed = new Vector3(Random.Range(-12.0f, 12.0f), 8, 0);
            GameObject newTripleShot = Instantiate(_TripleShotPrefab, posToSpawn, Quaternion.identity);
            //GameObject newSpeedUp = Instantiate(_SpeedPowerUpPrefab, posToSpawnSpeed, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }

    }


    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}
