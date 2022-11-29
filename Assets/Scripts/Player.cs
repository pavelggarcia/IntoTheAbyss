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
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private AudioClip _laserAudio;
    private AudioSource _audioSource;
    //[SerializeField] private AudioClip _explosionAudio;
    




    // Start is called before the first frame update
    void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL.");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudio;
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
        
        _audioSource.Play();
        
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
        
        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        
        _UIManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.onPlayerDeath();
            //AudioSource.PlayClipAtPoint(_explosionAudio, transform.position);
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
        //I want to have it so that when triple shot is called it starts a 5 second countdown timer(which will be the wait for seconds), if more triple shot is picked up, then the delay should be extended, once the 
        yield return new WaitForSeconds(_TripleShotTime);
        _isTripleShotActive = false;
        
    }
    // Power up touches player, power up script calls triple shot active method, triple shot method starts triple shot corotuine which starts a 5 second timer
    // need to make it so that once the power up is collected, it adds 5 seconds to the TripleShotTime, Triple shot time should start at 0, and once powerup is collected it will add 5 seconds, if a second one is collected it will add another 5 seconds, once the time runs out, back to single laser. 


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
