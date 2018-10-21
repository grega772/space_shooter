using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

     [SerializeField] public WaveConfiguration[] waves;
    private int enemyHealth;
    private int waveIndex;
    bool inWaveDelay = false;
    DateTime waveSpawnDelay;


    private void Start()
    {
        waveIndex = 0;
    }

    private void Update()
    {
        spawnWaves();
        checkDelays();
    }

    private void spawnWaves()
    {
        if (!inWaveDelay)
        {
            waves[waveIndex].Init();
            waveSpawnDelay = DateTime.Now.AddSeconds(waves[waveIndex].getTimeBetweenWaves());
            inWaveDelay = true;
            if (++waveIndex>=waves.Length)
            {
                waveIndex = 0;
            }
        }
        else
        {
            if (DateTime.Now>waveSpawnDelay)
            {
                inWaveDelay = false;
            }
        }
    }

    private void checkDelays()
    {

    }

}
