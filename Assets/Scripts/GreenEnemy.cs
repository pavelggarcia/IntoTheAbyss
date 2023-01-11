using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemy : MonoBehaviour
{
    private Transform _playerPos;
    [SerializeField] private int _speed = 1;
    [SerializeField] private float _roationModifier;
    [SerializeField] private GameObject _laserPrefab;
    private float _angleToPlayer;
    private float _fireRate;
    private float _fireTime = -1;
    private Player _player;


    private void Start()
    {
        _playerPos = GameObject.Find("Player").transform;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (transform != null && _playerPos != null)
        {
            Vector3 _vectorToTarget = _playerPos.position - transform.position;
            _angleToPlayer = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg - _roationModifier;
            Quaternion q = Quaternion.AngleAxis(_angleToPlayer, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _speed);
        }

        if (Time.time > _fireTime)
        {
            FireLaser();
        }
        if (transform.position.y < -13)
        {
            Destroy(gameObject);
        }
    }

    private void FireLaser()
    {
        _fireRate = Random.Range(1.0f, 2f);
        _fireTime = Time.time + _fireRate;

        Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, _angleToPlayer));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject, 0.1f);
        }


        if (other.tag == "Laser"|| other.tag == "Torpedoe")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            Destroy(this.gameObject, 0.1f);
        }
    }
}
