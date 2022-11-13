using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private bool _isShieldActive = false;
    [SerializeField] private GameObject _shield;
    [SerializeField] private int _score;
    private UIManager _UIManager;
    private GameManager _gameManager;




    // Start is called before the first frame update
    void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL.");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }
    void FireLaser()
    {

        _canFire = Time.time + _fireRate;



        if (_isTripleShotActive == true)
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


        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        
        }

        _lives -= 1;
        
        _UIManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.onPlayerDeath();
            _UIManager.GameOverText();
            _UIManager.RestartLevelText();
            _gameManager.GameOver();

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

        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _speed /= _speedMultiplier;
    }
    public void ShieldsActive()
    {
        _isShieldActive = true;

        _shield.SetActive(true);

    }

    //public method to add 10 to the game
    //communicate with UI to update Score
    public void AddToScore(int points)
    {
        _score += points;
        _UIManager.UpdateScore(_score);
    }
    






}
