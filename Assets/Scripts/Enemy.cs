using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 4;
    private Player _player;
    private Animator _anim;
    private Rigidbody2D _rigidBody2D;
    private BoxCollider2D _boxCollider2D;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _laserPrefab;
    private bool _canFire = false;
    private float _fireTime = -1f;
    private float _fireRate;
    private float _xOffset;
    private bool _isAlive = true;
    [SerializeField] private GameObject _shield;
    private bool _isShieldActive = true;
    private GameObject _playerPos;
    private float _distanceToRam = 4f;
    private float _angle;
    private float _roationModifier = -90;
    private bool _canFireBackwards = false;
    private bool _canDodge = true;
    private int _xPosToDodge;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _xOffset = Random.Range(-.5f, .5f);
        _playerPos = GameObject.Find("Player");


        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }

        if (_rigidBody2D == null)
        {
            Debug.LogError("RigidBody is NULL");
        }

        if (_boxCollider2D == null)
        {
            Debug.LogError("Box Collider is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is NULL");
        }

        if (_playerPos == null)
        {
            Debug.LogError("Player Position is NULL");
        }
    }

    void Update()
    {
        transform.Translate((Vector3.down + new Vector3(_xOffset, 0, 0)) * Time.deltaTime * _enemySpeed);
        StartCoroutine(SwitchX());

        if(transform.position.x > 18)
        {
            transform.position = new Vector3(18, transform.position.y, 0);
        }
        if(transform.position.x < -18)
        {
            transform.position = new Vector3(-18, transform.position.y, 0);
        }


        if (transform != null && _playerPos != null)
        {
            Vector3 _vectorToTarget = _playerPos.transform.position - transform.position;
            _angle = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg - _roationModifier;

            if (transform.position.y < _playerPos.transform.position.y && gameObject.name == "Enemy3(Clone)")
            {
                _canFireBackwards = true;
            }
            if (transform.position.y > _playerPos.transform.position.y && gameObject.name == "Enemy3(Clone)")
            {
                _canFireBackwards = false;
            }

            if (Vector2.Distance(transform.position, _playerPos.transform.position) < _distanceToRam && (transform.position.y > _playerPos.transform.position.y))
            {
                transform.position = Vector2.MoveTowards(transform.position, _playerPos.transform.position, _enemySpeed * Time.deltaTime);
            }

        }


        if (transform.position.y <= -13)
        {
            float randomX = Random.Range(-18.0f, 18.0f);
            transform.position = new Vector3(randomX, 13, 0);
        }
        if (Time.time > _fireTime)
        {
            FireLaser();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            if (_isShieldActive == true)
            {
                _shield.SetActive(false);
                _isShieldActive = false;

                return;
            }
            RemoveComponents();
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _isAlive = false;
            Destroy(this.gameObject, 2.5f);
        }


        if (other.tag == "Laser" || other.tag == "Torpedoe")
        {
            if (_isShieldActive == true)
            {
                _shield.SetActive(false);
                _isShieldActive = false;
                Destroy(other.gameObject);
                return;
            }
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            RemoveComponents();

            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _isAlive = false;
            Destroy(this.gameObject, 2.5f);
        }
    }

    // This code is for after the enemy is destroyed, so that the enemy can't fire after being destoryed
    private void RemoveComponents()
    {
        _fireRate = Mathf.Infinity;
        _fireTime = Mathf.Infinity;
        _canFire = false;
        _canFireBackwards = false;
        Destroy(_rigidBody2D);
        Destroy(_boxCollider2D);

    }

    private void FireLaser()
    {
        _fireRate = Random.Range(3f, 5f);
        _fireTime = Time.time + _fireRate;
        _canFire = true;
        // This code checks to see if the enemy is a normal enemy and can only fire downwards
        if (_canFire == true && _isAlive == true && _canFireBackwards == false)
        {
            _fireRate = Random.Range(3f, 5f);
            Instantiate(_laserPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        } else if (gameObject.name == "Enemy3(Clone)" && transform.position.y < _playerPos.transform.position.y && _canFireBackwards == true)
        // This code checks to see if this enemy can fire backwards 
        {
            _fireRate = Random.Range(1f, 2f);
            Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, _angle));
        }
        _canFire = false;
    }
    IEnumerator SwitchX()
    {
        if (_xOffset > 0)
        {
            yield return new WaitForSeconds(1);
            _xOffset = -.5f;
        }
        if (_xOffset < 0)
        {
            yield return new WaitForSeconds(1);
            _xOffset = .5f;
        }
    }

    public void EnemyDodge()
    {
        StartCoroutine(EnemyDodgeRoutine());
    }
    IEnumerator EnemyDodgeRoutine()
    {
        int DodgePicker = Random.Range(1,3);
        
        if(DodgePicker == 1)
        {
            _xPosToDodge = 3;
        }
        else if(DodgePicker ==2)
        {
            _xPosToDodge = -3;
        }
        
        Vector3 NewPos = transform.position + new Vector3(_xPosToDodge, 0.5f, 0);
        if(_canDodge == true)
        {
            _canDodge = false;
            transform.position = Vector2.MoveTowards(transform.position, NewPos, _enemySpeed * 30* Time.deltaTime);
            yield return new WaitForSeconds(1);
            _canDodge = true;
        }
        
    }
    public void FireOnPowerUp()
    {
        Instantiate(_laserPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
    }

}
