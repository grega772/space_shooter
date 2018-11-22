using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy {

    public void Awake()
    {
        speed = Constants.HEAVY_ENEMY_SPEED;
        health = Constants.HEAVY_ENEMY_HEALTH;
        base.init();
        this.enemyWorth = Constants.HEAVY_ENEMY_WORTH;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        performHeavyEnemyFunctions();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.PLAYER_WEAPON_TAG)
        {
            var collisionObject = collision.gameObject;

            gameObject.GetComponent<AudioSource>().PlayOneShot(hitSoundEffect);
            var hitLocation = new Vector2(collisionObject.transform.position.x, collisionObject.transform.position.y);
            Destroy(Instantiate(hiteffect, hitLocation, collisionObject.transform.rotation), 0.1f);
            Destroy(collisionObject);
            health -= 25;
            notifyEnemyDamage();
        }
        if (collision.gameObject.tag == "heavy_enemy")
        {
            health -= 100;
            notifyEnemyDamage();
        }
    }

    protected override void checkHealth()
    {
        if (health <= 0)
        {
            spaceWarsUI.GetComponent<UIElements>().setScoreText(this.enemyWorth);
            for (int i=0;i<5;++i)
            {
                int displacement = 0;

                switch (displacement)
                {
                    case 1:
                        displacement = 20;
                        break;
                    case 2:
                        displacement = 10;
                        break;
                    case 3:
                        displacement = -10;
                        break;
                    case 4:
                        displacement = -20;
                        break;
                }

                GameObject.FindGameObjectWithTag("space_wars").GetComponent<SpaceWars>()
                    .createExplosionOne(new Vector3(transform.position.x + displacement, transform.position.y + displacement, transform.position.z), transform.rotation);
                GameObject.FindGameObjectWithTag("space_wars").GetComponent<SpaceWars>().spawnPickup(transform);
            }
            Destroy(this.gameObject);
        }
    }

}
