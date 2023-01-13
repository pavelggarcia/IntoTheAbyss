using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitcher : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _bossOneMusic;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlayBackgroundMusic()
    {
        _audioSource.clip = _backgroundMusic;
    }

    public void PlayBossMusic()
    {
        _audioSource.clip = _bossOneMusic;
    }

    
}
