using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Sprite[] _liveSprite;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartLevelText;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField]private TMP_Text _waveText;
    [SerializeField] private TMP_Text _torpedoeText;
    

    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.enabled = false;
        _restartLevelText.enabled = false; 
        _waveText.enabled = false;
    }

    // Update is called once per frame
    
    public void UpdateScore(int PlayerScore)
    {
        _scoreText.text = "Score: " + PlayerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprite[currentLives];
    }
    public void GameOverText()
    {
        StartCoroutine(FlashGameOverRoutine());
        
    }
    public void UpdateAmmoText(int AmmoCount)
    {
        _ammoText.text = AmmoCount + " / 100";
    }

    public void UpdateTorpedoeText ( int TorpedoeCount)
    {
        _torpedoeText.text = TorpedoeCount + " / 10";
    }

    IEnumerator FlashGameOverRoutine()
    {
        while(true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(0.3f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.3f);
            
        }
    }
    public void RestartLevelText()
    {
        _restartLevelText.enabled = true;
        
    }

    public void GetShowWaveText(int wave)
    {
        StartCoroutine(ShowWaveText(wave));
    }
    IEnumerator ShowWaveText(int wave)
    {
        _waveText.enabled = true;
        _waveText.text = "Wave " + wave;
        yield return new WaitForSeconds(2);
        _waveText.enabled = false;
        
    }
}
