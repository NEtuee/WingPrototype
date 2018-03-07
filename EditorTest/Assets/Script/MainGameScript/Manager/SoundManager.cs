using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

	public SoundManager instance;

    public AudioClip[] audioClips;
    public AudioSource soundPlayer;

    public Slider volume;

    public bool check = true;

    public void Start()
    {
        Play(0);
        VolumeUpdate();
    }

    public void Play(int num)
    {
        soundPlayer.clip = audioClips[num];

        soundPlayer.gameObject.SetActive(true);
        soundPlayer.Play();
    }

    public void Stop()
    {
        soundPlayer.Stop();
    }

    public void VolumeUpdate()
    {
        soundPlayer.volume = volume.value;
    }
}
