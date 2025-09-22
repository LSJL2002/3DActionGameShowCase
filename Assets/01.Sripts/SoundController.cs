using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void PlayAudioSource()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
