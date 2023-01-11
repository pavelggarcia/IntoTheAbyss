using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorpedoe : MonoBehaviour
{
    private int _speed = 10;
    private void Start()
    {
        Destroy(this.gameObject, 7f);
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -13)
        {
            Destroy(gameObject);
        }
    }
}
