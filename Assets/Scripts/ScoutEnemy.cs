using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutEnemy : Enemy {

    // Use this for initialization
    private void Awake()
    {
        
        speed = Constants.SCOUT_ENEMY_SPEED;
        health = Constants.SCOUT_ENEMY_HEALTH;
        base.init();
        this.enemyWorth = Constants.SCOUT_ENEMY_WORTH;
        
    }

    void Start () {
        Destroy(gameObject, 3f);
        GameObject.FindGameObjectWithTag("game_level")
            .GetComponent<AudioSource>().PlayOneShot(spawnWarning);
    }

    // Update is called once per frame
    void Update () {
        performScoutEnemyFunctions();
    }
}
