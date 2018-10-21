using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutEnemy : Enemy {

    // Use this for initialization
    private void Awake()
    {
        speed = Constants.SCOUT_ENEMY_SPEED;
        health = Constants.SCOUT_ENEMY_HEALTH;
        weaponCoolDown = Constants.SCOUT_ENEMY_WEAPON_DELAY;
        base.init();
        this.enemyWorth = Constants.SCOUT_ENEMY_WORTH;
    }

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        performEnemyBasicFunctions();
    }
}
