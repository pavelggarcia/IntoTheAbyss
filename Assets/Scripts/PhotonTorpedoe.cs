using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTorpedoe : MonoBehaviour
{
    private GameObject[] _enemies;
    private GameObject _closestEnemy;
    private float _distance = Mathf.Infinity;
    private float _movementSpeed = 10f;
    
    void Update()
    {
        CalculateNearestEnemy();
        if(_closestEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _closestEnemy.transform.position, _movementSpeed * Time.deltaTime);
        } else
        {
            transform.Translate(Vector3.up *_movementSpeed *Time.deltaTime);
        }
        
        if(transform.position.y > 13 || transform.position.y < -13)
        {
            Destroy(gameObject);
        }
    }

    private void CalculateNearestEnemy()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in _enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < _distance)
            {
                _closestEnemy = e;
                _distance = dist;
                if(_closestEnemy.GetComponent<Rigidbody2D>() == null)
                {
                    _distance = Mathf.Infinity;
                    _closestEnemy = null;
                }
            }
        }
    }
}
