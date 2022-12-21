using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellites : MonoBehaviour
{

    [SerializeField] private float _satelliteSpeed;    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down *Time.deltaTime *_satelliteSpeed);
        if(transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }
}
