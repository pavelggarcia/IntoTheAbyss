using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryFire : MonoBehaviour
{

    private float RotateSpeed = 180f;
    [SerializeField] private GameObject _laserPrefab;
    private float _fireTime = -1f;
    private float _fireRate;
    [SerializeField] private GameObject _target;

    void Update()
    {
        transform.RotateAround(_target.transform.position, new Vector3(0,0,1), RotateSpeed* Time.deltaTime);
        if(Time.time > _fireTime)
        {
            FireLaser();
        }
    }
    private void FireLaser()
    {
        _fireRate = 0.2f;
        _fireTime = Time.time + _fireRate;

        Instantiate(_laserPrefab, transform.position, Quaternion.identity);
    }
}
