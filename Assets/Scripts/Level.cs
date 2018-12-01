using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    [SerializeField] public WaveConfiguration[] waves;
    [SerializeField] AudioClip scoutAlert;
    [SerializeField] AudioClip warningSound;
    [SerializeField] GameObject finalBoss;
    private int enemyHealth;
    private bool inWaveDelay = false;
    private DateTime waveSpawnDelay;
    private WaveConfiguration currentWave;
    private int previousWave;
    private DateTime gameStartTime;
    private Text warningText;
    private bool textIsClear = true;
    private DateTime warningTextFlashDelay = DateTime.Now;
    private DateTime startTime = DateTime.Now;
    private Boolean playWarningSound = false;
    private bool soundPlayed = false;
    private static bool greatSuccessCalled = false;
    private bool bossSpawned = false;
    

    private void Start()
    {
        this.gameStartTime = DateTime.Now;
        warningText = GameObject.FindGameObjectWithTag("warning_text").GetComponent<Text>();
        warningText.color = UnityEngine.Color.clear;
    }

    private void tryToPlayWarningSound()
    {
        if (!soundPlayed)
        {
            if (playWarningSound)
            {
                destroyAllEnemies();
                soundPlayed = true;
                gameObject.GetComponent<AudioSource>().PlayOneShot(warningSound);
            }
        }
    }

    private void destroyAllEnemies()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy e in enemies)
        {
            e.health -= 600;
        }
    }

    private void Update()
    {
        doCurrentPhase();
        tryToPlayWarningSound();
    }

    private void doCurrentPhase()
    {
        if (DateTime.Now < startTime.AddSeconds(Constants.BOSS_SPAWN_DELAY_SECONDS))
        {
            spawnWaves();
        }
        else if (DateTime.Now > startTime.AddSeconds(Constants.BOSS_SPAWN_DELAY_SECONDS) 
            && DateTime.Now < startTime.AddSeconds(Constants.BOSS_SPAWN_DELAY_SECONDS
            + Constants.WARNING_DURATION_SECONDS))
        {
            displayWarningText();
        }
        else
        {
            handleBossLogic();
        }
    }

    private void displayWarningText()
    {
        playWarningSound = true;
        if (DateTime.Now > warningTextFlashDelay)
        {
            if (warningText.color.Equals(Color.clear))
            {
                warningText.color = Color.red;
            }
            else
            {
                warningText.color = Color.clear;
            }
            warningTextFlashDelay = DateTime.Now.AddMilliseconds(700);
        }
    }

    private void handleBossLogic()
    {
        if (!bossSpawned)
        {
            Vector2 bossStartPos = new Vector2(0, 10);
            Instantiate(finalBoss, bossStartPos, transform.rotation);
        }
        bossSpawned = true;
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

    public static void greatSuccess()
    {
        if (!greatSuccessCalled)
        {
            greatSuccessCalled = true;
            for (int i = 0; i < 1; ++i)
            {
                Debug.Log("GREAT SUCCESS");
            }
        }
    }
}
