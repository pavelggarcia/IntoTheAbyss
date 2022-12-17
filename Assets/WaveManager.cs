using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private GameObject _enemyContainer;
    private int _enemyCounter;
    private int _waveCounter;
    private SpawnManager _spawnManager;
    private bool _canSpawn = true;
    private int _maxWaves = 10;
    private float _enemyNumber = 10;
    private float _enemyMultiplier = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GetComponent<SpawnManager>();
        _waveCounter = 0;
        _enemyContainer = this.gameObject.transform.GetChild(0).gameObject;
        if (_enemyContainer == null)
        {
            Debug.LogError("Enemy Container is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }


    }

    // Update is called once per frame
    void Update()
    {
        _enemyCounter = _enemyContainer.transform.childCount;
        // Debug.Log("Enemies on Screen " + _enemyCounter);
    }

    public void StartTheWaves()
    {
        StartCoroutine(StartWaveRoutine());

    }
    IEnumerator StartWaveRoutine()
    {


        for (int w = 0; w < _maxWaves; w++)
        {
            Debug.Log("Starting wave " + (w + 1));
            yield return new WaitForSeconds(10);
            if(w > 0)
            {
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
            }

            for (int i = 0; i < _enemyNumber; i++)
            {
                _spawnManager.GetSpawnEnemyRoutine();
                yield return new WaitForSeconds(2);
            }


        }
    }

}
