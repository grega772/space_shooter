using System;
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

    protected override void firePrimaryWeapon(GameObject PrimaryWeapon)
    {
        if (!primaryWeaponCoolingDown)
        {
            if (transform.position.x < 5 && transform.position.x > -5 && transform.position.y < 5)
            {
                float xVel = 0.0f;

                for (int i=0;i<2;i++) {

                    switch (i)
                    {
                        case 0:
                            xVel = 0.5f;
                            break;
                        case 1:
                            xVel = -0.5f;
                            break;
                    }

                    gameObject.GetComponent<AudioSource>()
                    .PlayOneShot(primaryWeaponSounds[UnityEngine.Random.Range(0, primaryWeaponSounds.Length)]);
                    var firedWeapon = Instantiate(PrimaryWeapon, transform.position, transform.rotation);
                    firedWeapon.AddComponent<Rigidbody2D>().velocity = new Vector3(xVel, -10);
                    firedWeapon.AddComponent<PolygonCollider2D>().isTrigger = true;
                    primaryWeaponCoolingDown = true;
                    primaryWeaponCoolDownTime = DateTime.Now.AddMilliseconds(weaponCoolDown * 1000 * UnityEngine.Random.RandomRange(0.5f, 1f));
                }
            }
        }
    }
}
