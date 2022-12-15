using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _shakeTime;
    private float _shakeLength;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Time.time < _shakeLength)
        {
            float xAmount = Random.Range(-.2f, .2f);
            float yAmount = Random.Range(-.2f, .2f);
            transform.position += new Vector3(xAmount, yAmount, 0);
        }
        if (Time.time > _shakeLength)
        {
            transform.position = new Vector3(0, 0, -10);
            _shakeTime = 0;
        }
    }
    public void ItsShakinTime()
    {
        _shakeTime = .1f;
        _shakeLength = Time.time + _shakeTime;

    }
}
