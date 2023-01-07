using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTorpedoe : MonoBehaviour
{
    private GameObject[] _enemies;
    private GameObject _target;
    private float _distance = Mathf.Infinity;
    private float _movementSpeed = 10f;
    
    void Update()
    {
        CalculateNearestEnemy();
        if(_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _movementSpeed * Time.deltaTime);
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
                _target = e;
                _distance = dist;
                if(_target.GetComponent<Rigidbody2D>() == null)
                {
                    _distance = Mathf.Infinity;
                    _target = null;
                }
            }
        }
    }
}
