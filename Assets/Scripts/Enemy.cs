using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 4;
    // Start is called before the first frame update
    void Start()
    {

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

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Hit :" + other.transform.name);
    }
}
