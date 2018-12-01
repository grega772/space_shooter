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
    [SerializeField] protected GameObject thruster;
    [SerializeField] protected GameObject pickupEffect;
    protected Vector3 dodgeDestination;
    protected List<String> superMoves = new List<String>();
    protected GameObject[] forwardThrusters;
    protected GameObject[] backwardThrusters;
    protected GameObject leftThruster;
    protected GameObject rightThruster;
    [SerializeField] protected float playerDodgeSpeed;

    protected float minX;
    protected float maxX;
    protected float maxY;
    protected float minY;
    protected bool laserCoolDown = false;
    protected bool dodgeCoolDown = false;
    protected AudioSource playerAudio;
    protected System.DateTime laserCoolDownTime = DateTime.Now;
    protected System.DateTime dodgeCoolDownTime = DateTime.Now;
    protected int health;
    protected bool colorChanged = false;
    protected DateTime colorChangeCooldown = DateTime.Now;
    protected GameObject SpaceWarsUI;
    protected bool justSpawned = true;
    protected bool isClear = false;
    protected DateTime stopFlashingTime = DateTime.Now.AddSeconds(2);
    protected List<String> upgrades = new List<String>();
    protected int numPrimaryProjectiles;
    protected bool forwardThrustersFiring;
    protected bool backwardThrustersFiring;
    protected bool leftThrustersFiring;
    protected bool rightThrustersFiring;
    protected bool dodging = false;

	// Use this for initialization
	void Start () {
        setUpMoveBoundaries();
        playerAudio = gameObject.GetComponent<AudioSource>();
        health = Constants.PLAYER_HEALTH;
        SpaceWarsUI = GameObject.FindGameObjectWithTag("space_wars_ui");
        instantiateThrusters();
	}
	
	// Update is called once per frame
	void Update () {
        justSpawnedLogic();
        move();
        calculateCoolDowns();
        fireLaser();
        dodge();
        doSuperMoves();
        checkHealth();
        checkColorChanged();
        updateThrusters();	
    }

    protected void dodge()
    {
        if (!dodging && !dodgeCoolDown && Input.GetKey(KeyCode.Z))
        {
                forwardThrustersFiring = false;
                backwardThrustersFiring = false;
                leftThrustersFiring = false;
                rightThrustersFiring = false;

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rightThrustersFiring = true;
                    dodgeDestination = new Vector3(-6,transform.position.y,transform.position.z);
                    dodging = true;
                rightThruster.transform.localScale = new Vector2(rightThruster.transform.localScale.x,
                    rightThruster.transform.localScale.y*3);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    leftThrustersFiring = true;
                    dodgeDestination = new Vector3(6, transform.position.y, transform.position.z);
                    dodging = true;
                leftThruster.transform.localScale = new Vector2(leftThruster.transform.localScale.x,
                    leftThruster.transform.localScale.y * 3);
            }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    forwardThrustersFiring = true;
                    dodgeDestination = new Vector3(transform.position.x,
                        5, transform.position.z);
                    dodging = true;
                for (int i=0;i<forwardThrusters.Length;++i)
                {
                    forwardThrusters[i].transform.localScale = new Vector2(forwardThrusters[i].transform.localScale.x,
                    forwardThrusters[i].transform.localScale.y * 3);
                }
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    backwardThrustersFiring = true;
                    dodgeDestination = new Vector3(transform.position.x,
                        -5, transform.position.z);
                    dodging = true;
                for (int i = 0; i < forwardThrusters.Length; ++i)
                {
                    backwardThrusters[i].transform.localScale = new Vector2(backwardThrusters[i].transform.localScale.x,
                    backwardThrusters[i].transform.localScale.y * 3);
                }
            }

                dodgeCoolDownTime = DateTime.Now.AddMilliseconds(1500);
                dodgeCoolDown = true;
        }
        else if (dodging)
        {
            if (Vector3.Distance(transform.position,dodgeDestination) > 0.5f)
            {
                transform.position = Vector3.
                    MoveTowards(transform.position, dodgeDestination, playerDodgeSpeed * Time.deltaTime);
            }
            else
            {
                dodging = false;
                destroyThrusters();
                instantiateThrusters();
            }
        }
    }

    protected void destroyThrusters()
    {
        Destroy(leftThruster);
        Destroy(rightThruster);
        for (int i=0;i<2;++i)
        {
            Destroy(forwardThrusters[i]);
        }
        for (int i=0;i<2;++i)
        {
            Destroy(backwardThrusters[i]);
        }
    }

    protected void doSuperMoves()
    {

    }

    protected void justSpawnedLogic()
    {
        if(justSpawned){
            if (DateTime.Now > stopFlashingTime)
            {
                justSpawned = false;
                gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
            }
            else if (DateTime.Now>colorChangeCooldown)
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

    public void instantiateThrusters()
    {

        forwardThrusters = new GameObject[2];

        for (int i=0;i<2;++i)
        {
            forwardThrusters[i]=Instantiate(thruster,gameObject.transform);
            forwardThrusters[i].GetComponent<SpriteRenderer>().color = Color.clear;
            forwardThrusters[i].transform.localScale = new Vector3(1,1,1);
        }

        backwardThrusters = new GameObject[2];

        for (int i=0;i<2;++i)
        {
            backwardThrusters[i]=Instantiate(thruster, gameObject.transform);
            backwardThrusters[i].GetComponent<SpriteRenderer>().color = Color.clear;
        }

        leftThruster = Instantiate(thruster,gameObject.transform);
        leftThruster.GetComponent<SpriteRenderer>().color = Color.clear;
        leftThruster.transform.localRotation = new Quaternion(0,0, 0.7071068f, 0.7071068f);

        rightThruster = Instantiate(thruster, gameObject.transform);
        rightThruster.GetComponent<SpriteRenderer>().color = Color.clear;
        rightThruster.transform.localRotation = new Quaternion(0,0, -0.7071068f, 0.7071068f);
    }

    public void updateThrusters()
    {
        if (forwardThrustersFiring)
        {
            for (int i=0;i<forwardThrusters.Length;++i)
            {
                float x = 0;
                switch (i)
                {
                    case 0:
                        x = transform.position.x - 0.5f;
                        break;
                    case 1:
                        x = transform.position.x + 0.5f;
                        break;
                }
                forwardThrusters[i].transform.position = 
                    new Vector2(x, transform.position.y-0.9f);
                forwardThrusters[i].GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        else
        {
            for (int i=0;i<forwardThrusters.Length;++i)
            {
                forwardThrusters[i].GetComponent<SpriteRenderer>().color = Color.clear;
            }   
        }
        if (backwardThrustersFiring)
        {
            for (int i = 0; i < backwardThrusters.Length; ++i)
            {
                float x = 0;
                switch (i)
                {
                    case 0:
                        x = transform.position.x - 0.5f;
                        break;
                    case 1:
                        x = transform.position.x + 0.5f;
                        break;
                }
                backwardThrusters[i].transform.position =
                    new Vector2(x, transform.position.y + 0.6f);
                backwardThrusters[i].GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        else
        {
            for (int i = 0; i < forwardThrusters.Length; ++i)
            {
                backwardThrusters[i].GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
        if (leftThrustersFiring)
        {
            leftThruster.transform.position = new Vector2(transform.position.x-0.9f,transform.position.y-0.1f);
            leftThruster.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            leftThruster.GetComponent<SpriteRenderer>().color = Color.clear;
        }
        if (rightThrustersFiring)
        {
            rightThruster.transform.position = new Vector2(transform.position.x+0.9f,transform.position.y-0.1f);
            rightThruster.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            rightThruster.GetComponent<SpriteRenderer>().color = Color.clear;
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

        if (dodgeCoolDown)
        {
            if (DateTime.Compare(DateTime.Now,dodgeCoolDownTime) > 0)
            {
                dodgeCoolDown = false;
            }
        }
    }

    private void fireLaser()
    {
        if (!laserCoolDown && Input.GetKey(KeyCode.Space))
        {
            float xVel = 0f;

            for(int i=0;i<=numPrimaryProjectiles;i++)
            {
                switch (i) {
                    case 0:
                        break;
                    case 1:
                        xVel = 1.5f;
                        break;
                    case 2:
                        xVel = -1.5f;
                        break;
                    case 3:
                        xVel = 3f;
                        break;
                    case 4:
                        xVel = -3f;
                        break;
                }

                var firedLaser = Instantiate(playerLaser,
                new Vector3(transform.position.x, transform.position.y, transform.position.z)
                , transform.rotation);
                firedLaser.AddComponent<Rigidbody2D>().velocity = new Vector3(xVel, 10);
            }
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
        if (!dodging)
        {
            rightThrustersFiring = false;
            leftThrustersFiring = false;
            forwardThrustersFiring = false;
            backwardThrustersFiring = false;

            var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
            var newXPos = transform.position.x;
            var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
            var newYPos = transform.position.y;
            if (deltaX < 0)
            {
                rightThrustersFiring = true;
                if (!(transform.position.x < minX))
                {
                    newXPos = transform.position.x + deltaX;
                }
            }
            if (deltaX > 0)
            {
                leftThrustersFiring = true;
                if (!(transform.position.x > maxX))
                {
                    newXPos = transform.position.x + deltaX;
                }
            }
            if (deltaY < 0)
            {
                backwardThrustersFiring = true;
                if (!(transform.position.y < minY))
                {
                    newYPos = transform.position.y + deltaY;
                }
            }
            if (deltaY > 0)
            {
                forwardThrustersFiring = true;
                if (!(transform.position.y > maxY))
                {
                    newYPos = transform.position.y + deltaY;
                }
            }
            transform.position = new Vector3(newXPos, newYPos, transform.position.z);
        }
    }

    private void changeHealth(int change)
    {
        this.health += change;
        SpaceWarsUI.GetComponent<UIElements>().setHealthText(this.health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!justSpawned && !dodging)
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
            var effect = Instantiate(pickupEffect,
                new Vector3(transform.position.x, transform.position.y), transform.rotation);
            Destroy(effect, 1f);
        }
        
        if ((collision.gameObject.tag=="basic_enemy"
            ||collision.gameObject.tag=="heavy_enemy"
            || collision.gameObject.tag == "scout_enemy")
            &&dodging)
        {
            collision.gameObject.GetComponent<Enemy>().health -= 100;
            collision.gameObject.GetComponent<Enemy>().notifyEnemyDamage();
        }
        else if ((collision.gameObject.tag == "basic_enemy"
            ||collision.gameObject.tag=="heavy_enemy"
            || collision.gameObject.tag == "scout_enemy")
            && !dodging)
        {
            this.changeHealth(-40);
            collision.gameObject.GetComponent<Enemy>().health -= 100;
            collision.gameObject.GetComponent<Enemy>().notifyEnemyDamage();
            notifyPlayerDamage();
        }

        if (collision.gameObject.tag == "heavy_turret"&&dodging)
        {
            collision.gameObject.GetComponent<Turret>().health -= 100;
        }
        
    }

    private void increaseProjectiles()
    {
        if (!(numPrimaryProjectiles>=5))
        {
            this.numPrimaryProjectiles += 1;
        }
        
    }


    private void notifyPlayerDamage()
    {
        colorChanged = true;
        colorChangeCooldown = DateTime.Now.AddMilliseconds(50);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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
