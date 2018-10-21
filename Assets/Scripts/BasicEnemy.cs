using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy {

    private void Awake()
    {
    }

    void Start () {
        speed = Constants.BASIC_ENEMY_SPEED;
        health = Constants.BASIC_ENEMY_HEALTH;
        weaponCoolDown = Constants.BASIC_ENEMY_WEAPON_DELAY;
        base.init();
        this.enemyWorth = Constants.BASIC_ENEMY_WORTH;
    }
	

	void Update () {
        performEnemyBasicFunctions();
    }
}
