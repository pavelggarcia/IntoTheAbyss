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
    private float _angle;
    [SerializeField] GameObject _enemyPlasmaPrefab;
    private GameObject _bossObject;
    private Boss _boss;
    private int _bossHealth;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject,10f);
        _playerPos = GameObject.Find("Player").transform;
        transform.position = new Vector3((Random.Range(-18, 18)), 15, 0);
        _entranceTime = Time.time + 5f;
        NewPosForDrone();
        _bossObject = GameObject.Find("Boss");
        if(_bossObject != null)
        {
            _boss = _bossObject.GetComponent<Boss>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_boss != null)
        {
            _bossHealth = _boss.GetHealth();
        }
        
        Debug.Log("From drone " + _bossHealth);
        
        if (_playerPos != null)
        {
            // This code is calculating the angle to the player every frame
            Vector3 _vectorToTarget = _playerPos.position - transform.position;
            _angle = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg - 90;
        }
        if (Time.time < _entranceTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, _newPos, _moveSpeed * Time.deltaTime);
        }
        CalculateMovement();
        if(_bossHealth < 350)
        {
            Destroy(this.gameObject);
        }
        
    }
    private void NewPosForDrone()
    {
        _newPos = new Vector3(Random.Range(-18, 18), Random.Range(5, 9), 0);
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
        Instantiate(_enemyPlasmaPrefab, transform.position, Quaternion.AngleAxis(_angle -180 , Vector3.forward));
    }
}
