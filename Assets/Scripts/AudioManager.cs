using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> audioClips;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void ShotFire()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void PlayerWalkOne()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }
    public void PlayerWalkTwo()
    {
        audioSource.PlayOneShot(audioClips[2]);
    }

}
