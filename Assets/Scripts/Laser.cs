using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private int _speed = 16;
   
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed *Time.deltaTime);

        if(transform.position.y >= 13)
        {
            
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            
            Destroy(gameObject);
        }
    }

    
}
