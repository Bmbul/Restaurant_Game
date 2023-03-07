using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] internal AudioSource gameMusic;
    [SerializeField] internal AudioSource soundSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void OnButtonClick()
    {
        PlayClip(clickSound);
    }

    private void PlayClip(AudioClip _clip)
    {
        soundSource.clip = _clip;
        soundSource.Play();
    }

    public void PlayWinSound()
    {
        PlayClip(winSound);
    }
    
    public void PlayLoseSound()
    {
        PlayClip(loseSound);
    }
}
