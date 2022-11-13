using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Sprite[] _liveSprite;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private TMP_Text _gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
}
