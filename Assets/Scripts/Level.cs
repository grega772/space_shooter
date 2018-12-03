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
    [SerializeField] AudioClip finalBattleMusic;
    [SerializeField] AudioClip victoryMusic;
    private int enemyHealth;
    private bool inWaveDelay = false;
    private DateTime waveSpawnDelay;
    private WaveConfiguration currentWave;
    private int previousWave;
    private DateTime gameStartTime;
    private Image warningImage;
    private Image victoryImage;
    private bool textIsClear = true;
    private DateTime warningImageFlashDelay = DateTime.Now;
    private DateTime startTime = DateTime.Now;
    private int victoryMessageDelayMilliseconds = 7000;
    private Boolean playWarningSound = false;
    private bool soundPlayed = false;
    private static bool greatSuccessCalled = false;
    private bool bossSpawned = false;
    private AudioSource musicPlayer;
    private bool musicChanged = false;
    private Button quitButton;
    

    private void Start()
    {
        this.gameStartTime = DateTime.Now;
        quitButton = GameObject.FindGameObjectWithTag("quit_button").GetComponent<Button>();
        warningImage = GameObject.FindGameObjectWithTag("warning_text").GetComponent<Image>();
        victoryImage = GameObject.FindGameObjectWithTag("victory_message").GetComponent<Image>();
        warningImage.enabled = false;
        victoryImage.enabled = false;
        quitButton.enabled = false;
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
        tryToDisplayVictoryMessage();
    }

    private void tryToDisplayVictoryMessage()
    {
        if (Hitler.hitlerIsDead && DateTime.Now > DateTime.Now.AddMilliseconds(victoryMessageDelayMilliseconds))
        {
            victoryImage.enabled = true;
        }
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
        if (DateTime.Now > warningImageFlashDelay)
        {
            if (!warningImage.enabled)
            {
                warningImage.enabled = true;
                quitButton.enabled = true;
            }
            else
            {
                warningImage.enabled = false;
                quitButton.enabled = false;
            }
            warningImageFlashDelay = DateTime.Now.AddMilliseconds(700);
        }
    }

    private void handleBossLogic()
    {
        if (!bossSpawned)
        {
            musicPlayer = gameObject
                .AddComponent<AudioSource>();
            musicPlayer.PlayOneShot(finalBattleMusic);
            Vector3 bossStartPos = new Vector3(0, 10, 5);
            Instantiate(finalBoss, bossStartPos, transform.rotation);
            bossSpawned = true;
        }

        if (Hitler.hitlerIsDead && !musicChanged)
        {
            musicPlayer.Stop();
            musicPlayer = gameObject
               .AddComponent<AudioSource>();
            musicPlayer.clip = victoryMusic;
            musicPlayer.PlayDelayed(6f);
            musicChanged = true;
        }
        
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
