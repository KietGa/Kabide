using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AMInstance {  get; private set; }
    [SerializeField] private AudioSource source;
    public AudioSource bgm;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] bclips;
    private int currentbclip;

    private void Awake()
    {
        AMInstance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayAudioRandom(0, 0.1f);
        }
    }

    public void PlayAudio(int index, float volume = 1f)
    {
        source.pitch = 1f;
        source.volume = volume;
        source.PlayOneShot(clips[index]);
    }

    public void PlayAudioRandom(int index, float volume = 1f)
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.volume = volume;
        source.PlayOneShot(clips[index]);
    }

    public void PlayBGM()
    {
        int stage = BossManager.BMInstance.stage;

        if (stage >= 25)
        {
            bgm.volume = 0.1f;
        }
        else if (stage >= 20)
        {
            bgm.volume = 0.1f;
        }
        else if (stage >= 15)
        {
            bgm.volume = 0.1f;
        }
        else if (stage >= 10)
        {
            bgm.volume = 0.1f;
        }
        else if (stage >= 5)
        {
            bgm.volume = 0.1f;
        }
        else if (stage >= 1)
        {
            bgm.volume = 0.1f;
        }

        if (stage >= 25)
        {
            bgm.clip = bclips[5];
        }
        else
        {
            bgm.clip = bclips[stage / 5];
        }

        bgm.Play();
    }
}
