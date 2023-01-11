using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private int _moveSpeed = 12;
    private Vector3 _newPos;
    private float _entranceTime;
    private int _health = 1000;
    private float _bossMoveTime;
    private float _bossMoveFrequency = 5f;
    [SerializeField] GameObject _enemyPlasmaPrefab;
    private Transform _playerPos;
    private float _angleToPlayer;
    [SerializeField] GameObject _dronePrefab;
    private int _numberOfBullets = 8;
    private int _startingAngle = 0;
    private float _fireTime = -1f;
    private float _fireRate = .5f;
    [SerializeField] GameObject _bulletPrefab;
    private int _bossHealth;
    public EnemyHealthBar HealthBar;
    private int _maxHealth = 1000;


    void Start()
    {
        _playerPos = GameObject.Find("Player").transform;
        _entranceTime = Time.time + 5f;
        _bossMoveTime = -1f;
        transform.position = new Vector3((Random.Range(-18, 18)), 15, 0);
        NewPosForBoss();
        HealthBar.SetHealth(_health, _maxHealth);

    }

    void Update()
    {
        if (Time.time < _entranceTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, _newPos, _moveSpeed * Time.deltaTime);
        }
        else
        {
            CalculateMovement();
        }
        if (_playerPos != null)
        {
            // This code is calculating the angle to the player every frame
            Vector3 _vectorToTarget = _playerPos.position - transform.position;
            _angleToPlayer = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg - 90;
        }
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
        _bossHealth = _health;
        HealthBar.SetHealth(_health, _maxHealth);
    }
    private void NewPosForBoss()
    {
        _newPos = new Vector3(Random.Range(-18, 18), 8, 0);
    }
    private void CalculateMovement()
    {
        if (_health <= 1000 && _health >= 700)
        {
            if (Time.time > _bossMoveTime)
            {
                _bossMoveTime = Time.time + _bossMoveFrequency;
                NewPosForBoss();
                FirePhaseOneGuns();
            }
            transform.position = Vector3.MoveTowards(transform.position, _newPos, _moveSpeed * Time.deltaTime);
        }
        else if (_health <= 699 && _health >= 350)
        {
            if (Time.time > _bossMoveTime)
            {
                _bossMoveTime = Time.time + _bossMoveFrequency;
                NewPosForBoss();
                FirePhaseOneGuns();
                Instantiate(_dronePrefab, transform.position, Quaternion.identity);
                Instantiate(_dronePrefab, transform.position, Quaternion.identity);
            }
            transform.position = Vector3.MoveTowards(transform.position, _newPos, _moveSpeed * Time.deltaTime);
        }
        else if (_health <= 349)
        {
            Vector3 FinalPos = new Vector3(0, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, FinalPos, _moveSpeed * Time.deltaTime);
            FirePhaseThree();
        }

    }
    private void FirePhaseOneGuns()
    {
        int _numberOfBullets = 5;
        int _fireAngle = 0;
        for (int b = 0; b < _numberOfBullets; b++)
        {
            Debug.Log("hello!");
            Instantiate(_enemyPlasmaPrefab, transform.position, Quaternion.AngleAxis((_angleToPlayer + _fireAngle) - 180, Vector3.forward));
            Instantiate(_enemyPlasmaPrefab, transform.position, Quaternion.AngleAxis((_angleToPlayer - _fireAngle) - 180, Vector3.forward));
            _fireAngle += 15;

        }
    }
    private void FirePhaseThree()
    {
        if (Time.time > _fireTime)
            {
                _fireTime = Time.time + _fireRate;
                for (int i = 0; i < _numberOfBullets; i++)
                {
                    Instantiate(_bulletPrefab, transform.position, Quaternion.AngleAxis(_startingAngle, Vector3.forward));
                    _startingAngle += 45;
                }
            }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _health -= 10;
            Destroy(other.gameObject);
        }
        if (other.tag == "Torpedoe")
        {
            _health -= 50;
            Destroy(other.gameObject);
        }

        Debug.Log(_health);
    }
    public int GetHealth()
    {
        return _bossHealth;
    }

}
