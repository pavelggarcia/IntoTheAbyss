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
    //[SerializeField] private AudioClip _explosionAudio;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _laserPrefab;
    private bool _canFire = false;
    private float _fireTime = -1f;
    private float _fireRate;
    private float _xOffset;
    private bool _isAlive = true;
    [SerializeField] private GameObject _shield;
    private bool _isShieldActive = true;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _xOffset = Random.Range(-.5f, .5f);


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

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SwitchX());

        transform.Translate((Vector3.down + new Vector3(_xOffset, 0, 0)) * Time.deltaTime * _enemySpeed);
        if (transform.position.y <= -6)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 8, 0);
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
            if(_isShieldActive == true)
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


        if (other.tag == "Laser")
        {
            if(_isShieldActive == true)
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
    private void RemoveComponents()
    {
        Destroy(_rigidBody2D);
        Destroy(_boxCollider2D);

    }
    private void FireLaser()

    {
        _fireRate = Random.Range(3.0f, 7.1f);
        _fireTime = Time.time + _fireRate;

        _canFire = true;
        if (_canFire == true && _isAlive == true)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
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
}
