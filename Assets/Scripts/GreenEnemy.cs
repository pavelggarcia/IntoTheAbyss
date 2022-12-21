using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemy : MonoBehaviour
{
    private Transform _playerPos;
    [SerializeField] private int _speed = 1;
    [SerializeField] private float _roationModifier;
    [SerializeField] private GameObject _laserPrefab;
    private float _angle;
    private float _fireRate;
    private float _fireTime = -1;
    private Player _player;
    


    // Start is called before the first frame update

    private void Start()
    {
        _playerPos = GameObject.Find("Player").transform;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 _vectorToTarget = _playerPos.position - transform.position;
        _angle = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg - _roationModifier;
        Quaternion q = Quaternion.AngleAxis(_angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _speed);


        if (Time.time > _fireTime)
        {
            FireLaser();
        }
        if(transform.position.y <-6)
        {
            Destroy(gameObject);
        }
        
    }

    private void FireLaser()
    {
        _fireRate = Random.Range(1.0f, 2f);
        _fireTime = Time.time + _fireRate;

        Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, _angle));

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
            //RemoveComponents();
            //_anim.SetTrigger("OnEnemyDeath");
            //_audioSource.Play();
            //_isAlive = false;
            Destroy(this.gameObject, 0.1f);
        }


        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            //RemoveComponents();
            //_anim.SetTrigger("OnEnemyDeath");
            //_audioSource.Play();
            //_isAlive = false;
            Destroy(this.gameObject, 0.1f);
        }
    }
}
