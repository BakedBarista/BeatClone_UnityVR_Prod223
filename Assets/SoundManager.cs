using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    AudioClip musicTrack;
    [SerializeField]
    float firstBeatOffset;
    public float songBpm;
    public float songPositoinInBeats;
    float secPerBeat;
    float songPosition;
    float dspSongTime;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        PlayMusicTrack();
    }

    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
    }
    // Update is called once per frame
    void PlayMusicTrack()
    {
        audioSource.PlayOneShot(musicTrack);
    }
}
