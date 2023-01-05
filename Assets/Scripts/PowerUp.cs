using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _powerUpSpeed = 3;
    private Player _player;
    [SerializeField] private AudioClip _powerupAudio;

    [SerializeField] private int powerupID;
    private GameObject _playerTransform;
    private Vector3 _playerPos;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _playerTransform = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _powerUpSpeed);
        if (transform.position.y <= -13)
        {
            Destroy(this.gameObject);
        }
        if(_playerTransform != null)
        {
            _playerPos = _playerTransform.transform.position;
        }
        //_playerPos = _playerTransform.transform.position;
    }
    public void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerPos, _powerUpSpeed *3*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                AudioSource.PlayClipAtPoint(_powerupAudio, transform.position);
                switch (powerupID)
                {
                    case 0:
                        _player.TripleShotActive();
                        
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        
                        break;
                    case 2:
                        
                        _player.ShieldsActive();
                        
                        break;
                    case 3:
                        _player.AddToAmmo();
                        break;
                    case 4:
                        _player.AddToLife();
                        break;
                    case 5:
                        _player.SecondaryFire();
                        break;
                    case 6:
                        _player.Damage();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
