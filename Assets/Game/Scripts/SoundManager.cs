using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip bgMusicClip;
    [SerializeField] private AudioClip reelSpinClip;
    [SerializeField] private AudioClip reelStopClip;
    
    [SerializeField] private AudioClip hp1Clip;
    [SerializeField] private AudioClip hp2Clip;
    [SerializeField] private AudioClip hp3Clip;
    [SerializeField] private AudioClip hp4Clip;
    [SerializeField] private AudioClip lpClip;


    private void Start()
    {
        audioSource.clip = bgMusicClip;
        audioSource.Play();
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonClip);
    }
    
    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winClip);
    }
    
    public void PlayReelSpinSound()
    {
        audioSource.PlayOneShot(reelSpinClip);
    }
    
    public void PlayReelStopSound()
    {
        audioSource.PlayOneShot(reelStopClip);
    }

    public void PlayLPSound()
    {
        audioSource.PlayOneShot(lpClip);
    }

    public void PlayHP1Sound()
    {
        audioSource.PlayOneShot(hp1Clip);
    }

    public void PlayHP2Sound()
    {
        audioSource.PlayOneShot(hp2Clip);
    }

    public void PlayHP3Sound()
    {
        audioSource.PlayOneShot(hp3Clip);
    }

    public void PlayHP4Sound()
    {
        audioSource.PlayOneShot(hp4Clip);
    }
}
