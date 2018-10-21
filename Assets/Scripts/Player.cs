using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

    [SerializeField] GameObject playerLaser;
    [Range(1,10)] [SerializeField] float moveSpeed;
    [SerializeField] protected AudioClip[] primaryWeaponSounds;
    [SerializeField] protected AudioClip hitSoundEffect;
    [SerializeField] protected GameObject hiteffect;

    float minX;
    float maxX;
    float maxY;
    float minY;
    bool laserCoolDown = false;
    AudioSource playerAudio;
    System.DateTime laserCoolDownTime;
    protected int health;
    bool colorChanged = false;
    protected DateTime colorChangeCooldown = DateTime.Now;
    protected GameObject SpaceWarsUI;
    protected bool justSpawned = true;
    protected bool isClear = false;
    protected DateTime stopFlashingTime = DateTime.Now.AddSeconds(2);
    protected List<String> upgrades = new List<String>();
    protected int numPrimaryProjectiles;

	// Use this for initialization
	void Start () {
        setUpMoveBoundaries();
        playerAudio = gameObject.GetComponent<AudioSource>();
        health = 100;
        SpaceWarsUI = GameObject.FindGameObjectWithTag("space_wars_ui");
	}
	
	// Update is called once per frame
	void Update () {
        justSpawnedLogic();
        move();
        calculateCoolDowns();
        fireLaser();
        checkHealth();
        checkColorChanged();
	}

    protected void justSpawnedLogic()
    {
        if (DateTime.Now>stopFlashingTime)
        {
            justSpawned = false;
            gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
        }

        if(justSpawned){
            if (DateTime.Now>colorChangeCooldown)
            {
                if (isClear)
                {
                    isClear = false;
                    gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                }
                else
                {
                    isClear = true;
                    gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.clear;
                }
                colorChangeCooldown = DateTime.Now.AddMilliseconds(200);
            }
        }
    }

    public void checkHealth()
    {
        if (this.health<=0)
        {
            var spaceWars = GameObject.FindGameObjectWithTag("space_wars").GetComponent<SpaceWars>();
                SpaceWarsUI.GetComponent<UIElements>().decrementLives();
                spaceWars.createExplosionOne(
                    new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                spaceWars.instantiateNewPlayer();
                Destroy(gameObject);
        }
    }

    private void calculateCoolDowns()
    {
        if (laserCoolDown)
        {
            if (DateTime.Compare(DateTime.Now,laserCoolDownTime) > 0)
            {
                laserCoolDown = false;
            }
        }
    }

    private void fireLaser()
    {
        if (!laserCoolDown && Input.GetKey(KeyCode.Space))
        {
            var firedLaser = Instantiate(playerLaser,
                new Vector3(transform.position.x,transform.position.y,transform.position.z)
                ,transform.rotation);
            firedLaser.AddComponent<Rigidbody2D>().velocity = new Vector3(0,30);
            laserCoolDown = true;
            laserCoolDownTime = DateTime.Now.AddMilliseconds(200);

            playerAudio.PlayOneShot(primaryWeaponSounds[UnityEngine.Random.Range(0, 2)]);
        }
    }

    private void setUpMoveBoundaries()
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 0.3f;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 0.3f;
        maxY = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y;
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y;
    }

    private void move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = transform.position.x + deltaX;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newYPos = transform.position.y + deltaY;
        if (deltaX < 0)
        {
            if (!(transform.position.x < minX))
            {
                transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
            }
        }
        if(deltaX > 0)
        {
            if (!(transform.position.x > maxX)){
                transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
            }
        }
        if (deltaY < 0)
        {
            if (!(transform.position.y < minY))
            {
                transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
            }
        }
        if(deltaY>0)
        {
            if (!(transform.position.y > maxY))
            {
                transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
            }
        }
    }

    private void changeHealth(int change)
    {
        this.health += change;
        SpaceWarsUI.GetComponent<UIElements>().setHealthText(this.health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!justSpawned)
        {
            if (collision.gameObject.tag == "enemy_weapon")
            {
                var collisionObject = collision.gameObject;
                gameObject.GetComponent<AudioSource>().PlayOneShot(hitSoundEffect);
                var hitLocation = new Vector2(collisionObject.transform.position.x, collisionObject.transform.position.y);
                Destroy(Instantiate(hiteffect, hitLocation, collisionObject.transform.rotation), 0.1f);
                var damage = collision.gameObject.GetComponent<Laser>().damage * -1;
                this.changeHealth(damage);
                Destroy(collisionObject);
                notifyPlayerDamage();
            }
        }
        if (collision.gameObject.tag == "power_up")
        {
            var powerUpType = collision.gameObject.GetComponent<Powerup>().powerupName;

            if (powerUpType.Equals(Constants.EXTRA_HEALTH))
            {
                this.changeHealth(Constants.HEALTH_PICKUP_VALUE);
            }
            else if (powerUpType.Equals(Constants.MEGA_HEALTH))
            {
                this.changeHealth(Constants.MEGA_HEALTH_PICKUP_VALUE);
            }
            else if (powerUpType.Equals(Constants.EXTRA_SHOT))
            {
                this.increaseProjectiles();
            }
            Destroy(collision.gameObject);
        }
    }

    private void increaseProjectiles()
    {
        this.numPrimaryProjectiles += 1;
    }


    private void notifyPlayerDamage()
    {
        colorChanged = true;
        colorChangeCooldown = DateTime.Now.AddMilliseconds(50);
        gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
    }

    private void checkColorChanged()
    {
        if (colorChanged)
        {
            if (DateTime.Now>colorChangeCooldown)
            {
                gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                colorChanged = false;
            }
        }
    }

}
