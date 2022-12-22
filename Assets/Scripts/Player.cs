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
    private int _shieldDamage = 3;

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
    [SerializeField] private AudioClip _explosionAudio;
    private AudioSource _audioSource;
    private SpriteRenderer _shieldSprite;
    private int _laserShots = 15;
    [SerializeField] private GameObject _secondaryFire;
    [SerializeField] private GameObject _progressBar;
    private ProgressBar _thrusterBar;
    private float _xBar;
    [SerializeField] private GameObject _mainCamera;
    private CameraShake _cameraShake;
    private bool _canBoost = true;



    void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _thrusterBar = _progressBar.GetComponent<ProgressBar>();
        _cameraShake = _mainCamera.GetComponent<CameraShake>();

        if (_cameraShake == null)
        {
            Debug.LogError("CameraShake is NULL");
        }

        if (_thrusterBar == null)
        {
            Debug.LogError("Thruster Bar is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL.");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
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

        _xBar = _thrusterBar.GetXBar();

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_laserShots >= 1)
            {
                _laserShots -= 1;
                FireLaser();
                _UIManager.UpdateAmmoText(_laserShots);
            }

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



        PlayerThruster();



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
        if (transform.position.x >= 10.5f)
        {
            transform.position = new Vector3(-10.5f, transform.position.y, 0);
        }
        else if (transform.position.x <= -10.5f)
        {
            transform.position = new Vector3(10.5f, transform.position.y, 0);
        }
    }
    public void Damage()
    {
        _cameraShake.ItsShakinTime();
        // This code manages the Shield Damage and visualization

        if (_isShieldActive == true)
        {

            _shieldSprite = _shield.GetComponent<SpriteRenderer>();

            _shieldDamage -= 1;
            if (_shieldDamage == 2)
            {
                _shieldSprite.color = new Color(1, 0.5f, 0.5f);
                return;
            }
            if (_shieldDamage == 1)
            {
                _shieldSprite.color = new Color(1, 0, 0);
                return;
            }
            if (_shieldDamage == 0)
            {
                _isShieldActive = false;
                _shield.SetActive(false);
                _shieldSprite.color = new Color(1, 1, 1);
                _shieldDamage = 3;
                return;
            }


        }



        _lives -= 1;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

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
        //I want to have it so that when triple shot is called it starts a 5 second countdown timer(which will be the wait for seconds), if more triple shot is picked up, then the delay should be extended, once the 
        yield return new WaitForSeconds(_TripleShotTime);
        _isTripleShotActive = false;

    }
    // Power up touches player, power up script calls triple shot active method, triple shot method starts triple shot corotuine which starts a 5 second timer
    // need to make it so that once the power up is collected, it adds 5 seconds to the TripleShotTime, Triple shot time should start at 0, and once powerup is collected it will add 5 seconds, if a second one is collected it will add another 5 seconds, once the time runs out, back to single laser. 


    public void SpeedBoostActive()
    {
        _speed = 10f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 5f;
    }
    public void ShieldsActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _UIManager.UpdateScore(_score);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            if (_isShieldActive == true)
            {
                Destroy(other.gameObject);
            }
            Damage();
            AudioSource.PlayClipAtPoint(_explosionAudio, transform.position);
            Destroy(other.gameObject);

        }
    }

    public void AddToAmmo()
    {
        _laserShots = 15;
        _UIManager.UpdateAmmoText(_laserShots);
    }
    public void AddToLife()
    {
        if (_lives < 3)
        {
            _lives += 1;
            _UIManager.UpdateLives(_lives);
            if (_lives == 3)
            {
                _rightEngine.SetActive(false);
                _leftEngine.SetActive(false);
            }
            if (_lives == 2)
            {
                _leftEngine.SetActive(false);
            }
        }
    }
    public void SecondaryFire()
    {
        _secondaryFire.SetActive(true);
        StartCoroutine(SecondaryFireRoutine());
    }
    IEnumerator SecondaryFireRoutine()
    {
        yield return new WaitForSeconds(5f);
        _secondaryFire.SetActive(false);
    }
    // Need to fix bug where if thruster is active and taken off it sets players speed to 5, but if speed power up is active as well, it resets the player speed to 2.5 instead of 5
    private void PlayerThruster()
    {


        if (_canBoost == true)
        {
            if (Input.GetKey(KeyCode.LeftShift) && _xBar > 0f)
            {

                _speed = 10f;
                _thrusterBar.AddThruster();
                if (_xBar <= 0.01f)
                {
                    _speed = 5f;
                    _canBoost = false;
                }
            }
        }



        if (Input.GetKeyUp(KeyCode.LeftShift) && _xBar < 1.2f)
        {
            _speed = 5f;
            _thrusterBar.RemoveThruster();
            _canBoost = true;
        }
    }





}
