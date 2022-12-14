using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject _progressBar;
    private float _xBar = 1f;
    private bool _canDischarge = true;


    public void Update()
    {
        if (_canDischarge == false && _xBar <= 1f)
        {

            if (_xBar > 1f)
            {
                _xBar = 1f;
            }
            _xBar += 0.25f * Time.deltaTime;
            _progressBar.GetComponent<RectTransform>().localScale = new Vector3(_xBar, 1, 1);
        }
    }

    public void AddThruster()
    {
        _canDischarge = true;
        if (_xBar >= 0f && _canDischarge == true)
        {

            _xBar -= .25f * Time.deltaTime;
            _progressBar.GetComponent<RectTransform>().localScale = new Vector3(_xBar, 1, 1);
            if (_xBar <= 0)
            {
                _canDischarge = false;
                return;
            }
        }
    }
    public void RemoveThruster()
    {


        _canDischarge = false;


    }
    public float GetXBar()
    {
        return (_xBar);
    }
}
