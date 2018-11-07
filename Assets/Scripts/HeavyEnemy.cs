using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy {

	// Use this for initialization
	void Start () {
        speed = Constants.HEAVY_ENEMY_SPEED;
        health = Constants.HEAVY_ENEMY_HEALTH;
        base.init();
        this.enemyWorth = Constants.HEAVY_ENEMY_WORTH;
    }
	
	// Update is called once per frame
	void Update () {
        performHeavyEnemyFunctions();
    }
}
