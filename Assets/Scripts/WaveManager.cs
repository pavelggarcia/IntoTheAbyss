using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    private GameObject _enemyContainer;
    private int _enemyCounter;
    [SerializeField] private int _waveCounter;
    private SpawnManager _spawnManager;
    private bool _canSpawn = true;
    private float _enemyNumber = 10;
    private float _enemyMultiplier = 1.2f;
    private UIManager _uiManager;
    [SerializeField] private GameObject _bossPrefab;

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
        if (_uiManager == null)
        {
            Debug.LogError("UI manager is NULL");
        }
    }


    void Update()
    {
        _enemyCounter = _enemyContainer.transform.childCount;
    }

    public void StartTheWaves()
    {
        StartCoroutine(StartWaveRoutine());
    }
    IEnumerator StartWaveRoutine()
    {
        _uiManager.GetShowWaveText(_waveCounter);
        yield return new WaitForSeconds(3);

        for (int i = 0; i < _enemyNumber; i++)
        {
            _spawnManager.SpawnOneEnemy();
            yield return new WaitForSeconds(2);
        }
        _canSpawn = true;
        while (_canSpawn == true)
        {
            yield return new WaitForSeconds(5);
            if (_enemyCounter == 0)
            {
                _canSpawn = false;
                _waveCounter += 1;
                WaveSwitcher();
            }
        }
    }
    private void SpawnBoss()
    {
        Instantiate(_bossPrefab, transform.position, Quaternion.identity);
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
                SpawnBoss();
                _uiManager.ShowBossText();
                break;
            /* case 6:
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
                SpawnBoss();
                _uiManager.ShowBossText();
                break; */
        }
    }

}
