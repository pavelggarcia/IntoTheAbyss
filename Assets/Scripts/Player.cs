using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private float _speedMultiplier = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _isTripleShotActive = false;
    private int _TripleShotTime = 5;
   // private bool _isSpeedBoostActive = false;
    //[SerializeField] private bool _isSpeedActive = false;
   // [SerializeField] private int _SpeedTime = 3;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }
    void FireLaser()
    {

        _canFire = Time.time + _fireRate;



        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

       
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        


       
        
        transform.Translate(direction * _speed * Time.deltaTime);
        


        // THis code restricts the player movement in the Y axis
        if (transform.position.y >= 6)
        {
            transform.position = new Vector3(transform.position.x, 6, 0);
        }
        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        // This code makes it so that the player wraps around when going off screen in the X axis
        if (transform.position.x >= 13)
        {
            transform.position = new Vector3(-13, transform.position.y, 0);
        }
        else if (transform.position.x <= -13)
        {
            transform.position = new Vector3(13, transform.position.y, 0);
        }
    }
    public void Damage()
    {
        _lives -= 1;
        
        if(_lives < 1)
        {
            _spawnManager.onPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void TripleShotActive()
    {
        
        _isTripleShotActive = true;
        StartCoroutine("TripleShotPowerDownRoutine");
    }

   
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_TripleShotTime);
        _isTripleShotActive = false;
    }


    public void SpeedBoostActive()
    {
        //_isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        //_isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    /* public void SpeedActive()
    {
        _isSpeedActive = true;
        StartCoroutine("SpeedPowerUpRoutine");

    }

    IEnumerator SpeedPowerUpRoutine()
    {
        _speed = 8.5f;
        yield return new WaitForSeconds(_SpeedTime);
        _isSpeedActive = false;
        _speed = 5.0f;
    }  */

   
    

}
