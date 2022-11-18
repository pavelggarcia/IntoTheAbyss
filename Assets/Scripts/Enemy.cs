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
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }
        _rigidBody2D = GetComponent<Rigidbody2D>();
        if(_rigidBody2D == null)
        {
            Debug.LogError("RigidBody is NULL");
        }
        _boxCollider2D = GetComponent<BoxCollider2D>();
        if(_boxCollider2D == null)
        {
            Debug.LogError("Box Collider is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _enemySpeed);
        if (transform.position.y <= -6)
        {
            float randomX = Random.Range(-12.0f, 12.0f);
            transform.position = new Vector3(randomX, 8, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            RemoveComponents();
            _anim.SetTrigger("OnEnemyDeath");
            
            Destroy(this.gameObject, 2.5f);
        }


        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddToScore(10);
            }
            RemoveComponents();
            _anim.SetTrigger("OnEnemyDeath");
            
            Destroy(this.gameObject, 2.5f);
        }
    }
    private void RemoveComponents()
    {
        Destroy(_rigidBody2D);
        Destroy(_boxCollider2D);
    }
    
}
