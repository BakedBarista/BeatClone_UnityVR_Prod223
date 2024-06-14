using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    public GameObject[] cubes; // Array to hold the two colored cube prefabs
    public Transform[] spawnPoints; // Array to hold the spawn points
    public SoundManager soundManager; // Reference to the SoundManager component
    private int currentBeatIndex = 0; // Index for the current beat
    private bool isSpawning = true; // Flag to control spawning
    public int totalBeats;
    private Dictionary<float, (int, Vector3)> spawnMap; // Dictionary for spawn times, cube type, and rotations

    void Start()
    {
        // Initialize the spawn map
        InitializeSpawnMap();
        totalBeats = Mathf.FloorToInt((soundManager.musicTrack.length / 60f) * soundManager.songBpm);
        // Start the beat spawning coroutine
        StartCoroutine(SpawnBeats());
    }

    private IEnumerator SpawnBeats()
    {
        // Wait for the initial delay (3 seconds) before starting the beat spawning
        yield return new WaitForSeconds(3f);

        while (isSpawning)
        {
            if (totalBeats > 0)
            {
                // Check if the current song position has reached the next beat time
                if (soundManager.songPositionInBeats >= currentBeatIndex * (60f / soundManager.songBpm)*2)
                {
                    SpawnBeat();

                    // Move to the next beat in the pattern
                    currentBeatIndex++;

                    totalBeats--;
                }
            }
            else
            {
                // Stop spawning if the song is not playing
                isSpawning = false;
            }

            yield return null;
        }
        ScoreManager.Instance.EndGame();
    }

    void SpawnBeat()
    {
        // Get the spawn point index from the current beat index
        int spawnPointIndex = currentBeatIndex % spawnPoints.Length;

        // Ensure the index is within the bounds of the spawnPoints array
        if (spawnPointIndex >= 0 && spawnPointIndex < spawnPoints.Length)
        {
            // Get the rotation from the spawn map
            var (cubeType, rotation) = spawnMap[currentBeatIndex % spawnMap.Count];

            // Spawn the cube prefab at the current spawn point
            Instantiate(cubes[cubeType], spawnPoints[spawnPointIndex].position, Quaternion.Euler(rotation));
        }
    }

    void InitializeSpawnMap()
    {
        // Assuming a song length of 120 seconds and bpm of 120
        // Evenly distribute spawn times every 0.5 beats
        float songLengthInBeats = (soundManager.musicTrack.length / 60f) * soundManager.songBpm * 1.9f;
        spawnMap = new Dictionary<float, (int, Vector3)>();

        for (float i = 0; i < songLengthInBeats; i += 1f)
        {
            int cubeType = (int)i % 2; // Randomly choose between cube types
            Vector3 rotation = new Vector3(0, 0, (int)(i * 90f) % 360); // Generate rotations in 45-degree increments
            spawnMap[i] = (cubeType, rotation);
        }
    }
}
