using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    private GameObject _enemyContainer;
    private int _enemyCounter;
    private int _waveCounter;
    private SpawnManager _spawnManager;
    private bool _canSpawn = true;
    //private int _maxWaves = 10;
    private float _enemyNumber = 10;
    private float _enemyMultiplier = 1.2f;
    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GetComponent<SpawnManager>();
        _waveCounter = 1;
        _enemyContainer = this.gameObject.transform.GetChild(0).gameObject;
        if (_enemyContainer == null)
        {
            Debug.LogError("Enemy Container is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        if(_uiManager == null)
        {
            Debug.LogError("UI manager is NULL");
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
        _canSpawn = true;
        _uiManager.GetShowWaveText(_waveCounter);
        yield return new WaitForSeconds(1);

        for (int i = 0; i < _enemyNumber; i++)
        {
            _spawnManager.GetSpawnEnemyRoutine();
            yield return new WaitForSeconds(2);
        }
        while (_canSpawn == true)
        {
            yield return new WaitForSeconds(4);
            if (_enemyCounter == 0)
            {
                _canSpawn = false;
                _waveCounter += 1;
                WaveSwitcher();

            }

        }


        /*  for (int w = 0; w < _maxWaves; w++)
         {
             Debug.Log("Starting wave " + (w + 1));
             yield return new WaitForSeconds(10);
             if (w > 0)
             {
                 _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
             }
         } */
    }
    private void WaveSwitcher()
    {
        switch (_waveCounter)
        {
            case 2:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 3:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 4:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 5:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 6:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 7:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 8:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 9:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
            case 10:
                _enemyNumber = Mathf.Floor(_enemyNumber * _enemyMultiplier);
                StartCoroutine(StartWaveRoutine());
                break;
        }
    }

}
