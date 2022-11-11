using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 4;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
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
            Destroy(this.gameObject);
        }


        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddToScore(10);
            }
            
            Destroy(this.gameObject);
        }
    }
}
