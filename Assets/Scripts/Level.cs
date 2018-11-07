using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    [SerializeField] public WaveConfiguration[] waves;
    private int enemyHealth;
    private bool inWaveDelay = false;
    private DateTime waveSpawnDelay;
    private WaveConfiguration currentWave;
    private int previousWave;

    private void Update()
    {
        spawnWaves();
        checkDelays();
    }

    private void spawnWaves()
    {
        if (!inWaveDelay)
        {
            int randWave;
            do
            {
                randWave = UnityEngine.Random.Range(0, waves.Length);
            } while (randWave == previousWave);
            waves[randWave].Init();
            waveSpawnDelay = DateTime.Now.AddSeconds(UnityEngine.Random.Range(5,10));
            inWaveDelay = true;
            currentWave = waves[randWave];
            previousWave = randWave;
        }
        else
        {
            if (DateTime.Now>waveSpawnDelay || currentWave.getRemainingEnemies() < 2)
            {
                inWaveDelay = false;
            }
        }
    }

    private void checkDelays()
    {

    }

}
