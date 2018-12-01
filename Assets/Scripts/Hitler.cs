using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hitler : Enemy {

    [SerializeField] GameObject scout;
    [SerializeField] AudioClip[] laserEyeSounds;
    private bool hitlerIsDead = false;
    private SpaceWars spaceWars;
    private int hitlerExplosionDelayMilliseconds = 100;
    private DateTime lastColorChange = DateTime.Now;
    private DateTime lastHitlerExplosion = DateTime.Now;
    private int colorChangeDelayMilliseconds = 200;
    private bool colorChangeComplete = false;
    private int bossAttackDelayMilliseconds = 3000;
    private DateTime lastBossAttack = DateTime.Now;
    private bool currentlyAttacking = false;
    private bool isLaserEyeAttack = false;
    private bool isScoutAttack = false;
    private bool isFlashingEyes = false;
    private int flashingEyesDurationMilliseconds = 1000;
    private int flashingEyesInterval = 10;
    private DateTime lastEyeFlash = DateTime.Now;
    private bool eyesGettingBrighter = true;
    private float eyeChangeSpeed = 0.2f;

    private int eyeLaserDelayMilliseconds = 5;
    private float eyeLaserRotation = 50f;
    private DateTime lastTimeLaserFired = DateTime.Now;
    private bool eyeRotationIncreasing = true;
    private float eyeRotationIncrement = 3f;
    private int eyeLaserAttackDurationMilliseconds = 6000;

    private DateTime lastTimeScoutsSummoned = DateTime.Now;
    private int scoutSummonDelay = 1000;
    private int numScoutWaves = 3;
    private int numScountsPerWave = 5;

    public bool movingTowardCenter = true;

    private GameObject hitlerEyeGlow;

    private Vector2 center = new Vector2(0, 0);
    private bool movingLeft = false;


    

    void Start()
    {
        spaceWars = GameObject.FindGameObjectWithTag("space_wars").GetComponent<SpaceWars>();
        speed = Constants.HITLER_SPEED;
        health = Constants.HITLER_HEALTH;
        this.enemyWorth = Constants.HITLER_WORTH;
        base.init();
        //lastBossAttack = DateTime.Now.AddMilliseconds(bossAttackDelayMilliseconds * 2);
    }

    private void createMassiveExplosion()
    {
        spaceWars.createExplosionThree(transform.position, transform.rotation);
    }

    private void OnDestroy()
    {
        createMassiveExplosion();
        Destroy(GameObject.FindGameObjectWithTag("hitler_chin"));
        Destroy(GameObject.FindGameObjectWithTag("hitler_eye_glow"));
        Destroy(instantiatedThruster);
    }


    void Update()
    {
        performBossMovement();
        performBossBasicFunctions();
        onHitlerDeathFunctions();
        performBossMoves();
        manageAttacks();
    }

    private void onHitlerDeathFunctions()
    {
        if (hitlerIsDead)
        {
            tryToCreateExplosion();
            tryToChangeSpriteColor();
        }
    }

    private void performBossMovement()
    {
        if (movingTowardCenter)
        {
            if (Vector2.Distance(transform.position,center)>0.9)
            {
                transform.position = 
                    Vector2.MoveTowards(transform.position,center, (speed * Time.deltaTime));
            }
            else
            {
                movingTowardCenter = false;
            }
        }
        else if(!hitlerIsDead)
        {
            moveSideToSide();
        }
        else
        {
            floatUp();
        }
    }

    private void floatUp()
    {
        transform.position = new Vector3(transform.position.x,
        transform.position.y + (speed * Time.deltaTime * 0.2f), -1f);
    }

    

    private void moveSideToSide()
    {
        if (movingLeft)
        {
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime),
                transform.position.y, -1f);
            if (transform.position.x < -2)
            {
                movingLeft = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime),
                transform.position.y, -1f);
            if (transform.position.x > 2)
            {
                movingLeft = true;
            }
        }
    }

    private void tryToCreateExplosion()
    {
        if (DateTime.Now > lastHitlerExplosion)
        {
            

            Vector3 min = gameObject.GetComponent<SpriteRenderer>().bounds.min;
            Vector3 max = gameObject.GetComponent<SpriteRenderer>().bounds.max;

            var minX = min.x;
            var maxX = max.x;

            var minY = min.y;
            var maxY = max.y;

            Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(minX,maxX), 
                UnityEngine.Random.Range(minY,maxY));

            spaceWars.createExplosionTwo(randomPosition,transform.rotation);

            lastHitlerExplosion = DateTime.Now
                .AddMilliseconds(hitlerExplosionDelayMilliseconds);
        }
    }

    private void tryToChangeSpriteColor()
    {
        if (!colorChangeComplete && DateTime.Now > lastColorChange)
        {
            var currentColor = gameObject.GetComponent<SpriteRenderer>().color;

            float green = currentColor.g - 0.05f;
            float blue = currentColor.b - 0.05f;

            if (green <= 0)
            {
                colorChangeComplete = true;
                green = 0;
                blue = 0;
            }

            gameObject.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, green, blue);
            GameObject.FindGameObjectWithTag("hitler_chin")
                .GetComponent<SpriteRenderer>().color = new Color(currentColor.r, green, blue);

            lastColorChange = DateTime.Now.AddMilliseconds(colorChangeDelayMilliseconds);
        }
    }

    private void performBossMoves()
    {
        if (!movingTowardCenter
            &&!currentlyAttacking
            && DateTime.Now > lastBossAttack)
        {
            currentlyAttacking = true;
            lastBossAttack = DateTime.Now.AddMilliseconds(flashingEyesDurationMilliseconds);
            if(UnityEngine.Random.Range(0,2)==1)
            {
                isLaserEyeAttack = true;
            }
            else
            {
                isScoutAttack = true;
            }

        }
    }

    private void manageAttacks()
    {
        if (!hitlerIsDead)
        {
            if (isLaserEyeAttack)
            {
                handleEyeLaserAttack();
            }
            else if (isScoutAttack)
            {
                handleScoutAttack();
            }
        }
    }

    private void handleEyeLaserAttack()
    {
        if (DateTime.Now < lastBossAttack)
        {
            doEyeFlash();
        }
        else if(DateTime.Now < lastBossAttack.AddMilliseconds(eyeLaserAttackDurationMilliseconds))
        {
            fireLasers();
        }
        else
        {
            currentlyAttacking = false;
            isLaserEyeAttack = false;
            lastBossAttack = DateTime.Now.AddMilliseconds(bossAttackDelayMilliseconds);
        }
    }

    private void handleScoutAttack()
    {
        if (DateTime.Now < lastBossAttack)
        {
            doEyeFlash();
        }
        else if(DateTime.Now < lastBossAttack.AddMilliseconds(eyeLaserAttackDurationMilliseconds))
        {
            summonScouts();
        }
        else
        {
            currentlyAttacking = false;
            isScoutAttack = false;
            lastBossAttack = DateTime.Now.AddMilliseconds(bossAttackDelayMilliseconds);
        }
    }

    private void doEyeFlash()
    {
        if (DateTime.Now > lastEyeFlash)
        { 

            var eyes = GameObject.FindGameObjectWithTag("hitler_eye_glow").GetComponent<SpriteRenderer>();
            var eyeColor = eyes.color;

            var r = eyeColor.r;
            var g = eyeColor.g;
            var b = eyeColor.b;
            var a = eyeColor.a;

            if (eyesGettingBrighter)
            {
                a += eyeChangeSpeed;
                if (a > 0.9f)
                {
                    eyesGettingBrighter = false;
                }
            }
            else
            {
                a -= eyeChangeSpeed;
                if (a < 0.2f)
                {
                    eyesGettingBrighter = true;
                }
            }
            eyes.color = new Color(r,g,b,a);
            lastEyeFlash = DateTime.Now.AddMilliseconds(flashingEyesInterval);
        }
    }

    private void summonScouts()
    {
        if (DateTime.Now > lastTimeScoutsSummoned)
        {

            int startingHeight = 10;

            lastTimeScoutsSummoned = DateTime.Now.AddMilliseconds(1000);
            for (int i=0;i<numScoutWaves;++i)
            {
                float randomX = UnityEngine.Random.Range(-6.5f,3);
                for (int y=0;y<numScountsPerWave;++y)
                {
                    float xInc = y * 0.7f;
                    Vector3 pos = new Vector3(randomX,startingHeight,-5);
                    pos.x += xInc;
                    pos.z = -5;
                    var newScoutEnemy = Instantiate(scout, pos, transform.rotation);
                    newScoutEnemy.GetComponent<Enemy>().waypoints = waypoints;
                    newScoutEnemy.AddComponent<PolygonCollider2D>().isTrigger = true;
                }
                startingHeight += 2;
            }
        }
    }

    private void fireLasers()
    {
        //Debug.Log("firing");
        if (DateTime.Now > lastTimeLaserFired)
        {
            //120 - 220
            if (eyeRotationIncreasing)
            {
                eyeLaserRotation += eyeRotationIncrement;
                if (eyeLaserRotation>290)
                {
                    eyeRotationIncreasing = false;
                }
            }
            else
            {
                eyeLaserRotation -= eyeRotationIncrement;
                if (eyeLaserRotation<50)
                {
                    eyeRotationIncreasing = true;
                }
            }

            //Debug.Log(eyeLaserRotation);

            var hitlerEyeLeft = GameObject.FindGameObjectWithTag("hitler_eye_left").transform;

            var hitlerEyeRight = GameObject.FindGameObjectWithTag("hitler_eye_right").transform;

            var laserRotation = new Quaternion(0f,0f,eyeLaserRotation,0f);

            var leftLaser = Instantiate(primaryWeapon,hitlerEyeLeft.position,transform.rotation);
            var rightLaser = Instantiate(primaryWeapon,hitlerEyeRight.position,transform.rotation);

            leftLaser.transform.Rotate(new Vector3(0,0,eyeLaserRotation));

            rightLaser.transform.Rotate(new Vector3(0, 0, eyeLaserRotation));

            GameObject.FindGameObjectWithTag("hitler_eye_left")
                .AddComponent<AudioSource>()
                .GetComponent<AudioSource>()
                .PlayOneShot(laserEyeSounds[UnityEngine.Random.Range(0,laserEyeSounds.Length-1)]);

            lastTimeLaserFired = DateTime.Now.AddMilliseconds(eyeLaserDelayMilliseconds);
        }
    }



    public void setHitlerIsDead(bool boolean)
    {
        this.hitlerIsDead = boolean;
    }


}
