using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    public GameObject[] cubes;
    public Transform[] spawnPoints;
    public float beatsPerMinute = 0;
    private float timer;
    private int index = 0;
    private int[] beatSequence = { 1, 2, 3, 4, 5, 6, 7, 8 };

    // Update is called once per frame
    void Update()
    {
        float twoBars = 60f / beatsPerMinute * 8;
        if(timer > twoBars)
        {
            SpawnBeat();
            timer -= twoBars;
        }
        index = (index+1) % beatSequence.Length;
        
        
        timer += Time.deltaTime;
    }

    void SpawnBeat()
    {
        int spawnPointIndex = beatSequence[index] % 3;
        int cubeIndex = beatSequence[index] % 2;
        Instantiate(cubes[cubeIndex], spawnPoints[spawnPointIndex]);
    }
}
