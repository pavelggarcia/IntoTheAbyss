using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorpedoe : MonoBehaviour
{
    private int _speed = 10;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -13)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
