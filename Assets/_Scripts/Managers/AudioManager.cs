using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioSource npcSound;

    [SerializeField] private AudioClip sinisterTrack;

    public void PlaySinisterTrack()
    {
        mainAudioSource.Stop();

        mainAudioSource.clip = sinisterTrack;
        mainAudioSource.Play();
        mainAudioSource.loop = true;
    }

    public void PlayEventAudio(AudioClip clip)
    {
        // Optional: stop the current clip if needed
        soundEffect.Stop();

        // Swap the clip and play
        soundEffect.clip = clip;
        soundEffect.Play();
    }

    public void PlayNPCVoice()
    {
        npcSound.pitch = Random.Range(1f, 3f);

        npcSound.Play();
    }
}
