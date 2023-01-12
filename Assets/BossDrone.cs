using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDrone : MonoBehaviour
{
    private int _moveSpeed = 12;
    private Vector3 _newPos;
    private float _droneMoveTime = -1;
    private float _droneMoveFrequency = 3;
    private float _entranceTime;
    private Transform _playerPos;
    private float _angleToPlayer;
    [SerializeField] GameObject _enemyPlasmaPrefab;
    private GameObject _bossObject;
    private Boss _boss;
    private int _bossHealth;

    void Start()
    {
        _playerPos = GameObject.Find("Player").transform;
        transform.position = new Vector3((Random.Range(-18, 18)), 15, 0);
        _entranceTime = Time.time + 5f;
        NewPosForDrone();
        _bossObject = GameObject.Find("Boss(Clone)");
        if (_bossObject != null)
        {
            _boss = _bossObject.GetComponent<Boss>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        _bossHealth = _boss.GetHealth();
        if (_playerPos != null)
        {
            // This code is calculating the angle to the player every frame
            Vector3 _vectorToTarget = _playerPos.position - transform.position;
            _angleToPlayer = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg - 90;
        }
        if (Time.time < _entranceTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, _newPos, _moveSpeed * Time.deltaTime);
        }

        if (_bossHealth < 350)
        {
            Destroy(this.gameObject);
        }
    }
    private void NewPosForDrone()
    {
        _newPos = new Vector3(Random.Range(-18, 18), Random.Range(5, 10), 0);
    }
    private void CalculateMovement()
    {
        if (Time.time > _droneMoveTime)
        {
            _droneMoveTime = Time.time + _droneMoveFrequency;
            NewPosForDrone();
            FireWeapon();
        }
        transform.position = Vector3.MoveTowards(transform.position, _newPos, _moveSpeed * Time.deltaTime);
    }
    private void FireWeapon()
    {
        Instantiate(_enemyPlasmaPrefab, transform.position, Quaternion.AngleAxis(_angleToPlayer - 180, Vector3.forward));
    }
}
