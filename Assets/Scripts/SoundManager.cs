using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicTrack;
    public float firstBeatOffset;
    public float songBpm;
    public float songPositionInBeats;
    float secPerBeat;
    float songPosition;
    float dspSongTime;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        StartCoroutine(PlayMusicTrackWithDelay(firstBeatOffset)); // Start the coroutine with a 3-second delay
    }

    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;
    }

    IEnumerator PlayMusicTrackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        audioSource.PlayOneShot(musicTrack); // Play the music track
    }
}
