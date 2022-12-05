using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    
    private int _speed = 8;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed *Time.deltaTime);

        if(transform.position.y <= -8)
        {
            
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            
            Destroy(gameObject);
        }
    }
}
