using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletManager : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    private int _numberOfBullets = 8;
    private int _startingAngle = 0;
    private float _fireTime = -1f;
    private float _fireRate = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            for(int i =0; i < _numberOfBullets; i++)
            {
                Instantiate(_bulletPrefab, transform.position, Quaternion.AngleAxis(_startingAngle, Vector3.forward));
                _startingAngle += 45;
            }
            //Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        }
        if(Time.time > _fireTime)
        {
            _fireTime = Time.time+_fireRate;
            for(int i =0; i < _numberOfBullets; i++)
            {
                Instantiate(_bulletPrefab, transform.position, Quaternion.AngleAxis(_startingAngle, Vector3.forward));
                _startingAngle += 45;
            }
        }

    }
}
