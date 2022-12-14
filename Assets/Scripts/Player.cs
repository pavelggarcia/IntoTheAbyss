using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
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
    private UIManager _uiManager;
    private GameManager _gameManager;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private AudioClip _laserAudio;
    [SerializeField] private AudioClip _explosionAudio;
    private AudioSource _audioSource;
    private SpriteRenderer _shieldSprite;
    private int _laserShots = 100;
    private int _torpedoeShots = 10;
    [SerializeField] private GameObject _secondaryFire;
    [SerializeField] private GameObject _thrusterBarObject;
    private ProgressBar _progressBar;
    private float _xBar;
    [SerializeField] private GameObject _mainCamera;
    private CameraShake _cameraShake;
    private bool _canBoost = true;
    private GameObject[] _powerUp;
    [SerializeField] private GameObject _torpedoePrefab;



    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _progressBar = _thrusterBarObject.GetComponent<ProgressBar>();
        _cameraShake = _mainCamera.GetComponent<CameraShake>();

        if (_cameraShake == null)
        {
            Debug.LogError("CameraShake is NULL");
        }

        if (_progressBar == null)
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


    void Update()
    {
        _xBar = _progressBar.GetXBar();

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_laserShots >= 1)
            {
                _laserShots -= 1;
                FireLaser();
                _uiManager.UpdateAmmoText(_laserShots);
            }

        }
        if (Input.GetKey(KeyCode.C))
        {
            _powerUp = GameObject.FindGameObjectsWithTag("PowerUp");
            if (_powerUp != null)
            {
                foreach (GameObject p in _powerUp)
                {
                    p.GetComponent<PowerUp>().MoveTowardsPlayer();
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_torpedoeShots >= 1)
            {
                _torpedoeShots -= 1;
                Instantiate(_torpedoePrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                _uiManager.UpdateTorpedoeText(_torpedoeShots);
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

        // This code restricts the player movement in the Y axis
        if (transform.position.y >= 10)
        {
            transform.position = new Vector3(transform.position.x, 10, 0);
        }
        else if (transform.position.y <= -10)
        {
            transform.position = new Vector3(transform.position.x, -10, 0);
        }

        // This code makes it so that the player wraps around when going off screen in the X axis
        if (transform.position.x >= 20.5f)
        {
            transform.position = new Vector3(-20.5f, transform.position.y, 0);
        }
        else if (transform.position.x <= -20.5f)
        {
            transform.position = new Vector3(20.5f, transform.position.y, 0);
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

        // This code deals with taking the players lives away
        _lives -= 1;
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager.onPlayerDeath();
            _uiManager.GameOverText();
            _uiManager.RestartLevelText();
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
        _speed = 15f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 10f;
    }
    public void ShieldsActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
        _shieldDamage = 3;
        _shieldSprite = _shield.GetComponent<SpriteRenderer>();
        _shieldSprite.color = new Color(1, 1, 1);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser" || other.tag == "EnemyTorpedoe"|| other.tag == "BossTorpedo")
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
        _laserShots = 100;
        _uiManager.UpdateAmmoText(_laserShots);
    }

    public void AddToTorpedoeAmmo()
    {
        _torpedoeShots = 10;
        _uiManager.UpdateTorpedoeText(_torpedoeShots);
    }
    public void AddToLife()
    {
        if (_lives < 3)
        {
            _lives += 1;
            _uiManager.UpdateLives(_lives);
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

    private void PlayerThruster()
    {
        if (_canBoost == true)
        {
            if (Input.GetKey(KeyCode.LeftShift) && _xBar > 0f)
            {
                _speed = 15f;
                _progressBar.AddThruster();
                if (_xBar <= 0.01f)
                {
                    _speed = 10f;
                    _canBoost = false;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && _xBar < 1.2f)
        {
            _speed = 10f;
            _progressBar.RemoveThruster();
            _canBoost = true;
        }
    }
}
