using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _enemySpawnTime = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnRoutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-12.0f, 12.0f), 8,0);
            Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }
    // spawn game objects every 5 seconds
}
