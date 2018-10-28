using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	public int waypointIndex { get; set; }
    public float speed { get; set; }
    public List<Transform> waypoints { get; set; }
    bool changedWaypoint = false;
    public int health { get; set; }
    protected float weaponCoolDown;
    [SerializeField] protected GameObject thruster;
    [SerializeField] protected GameObject primaryWeapon;
    [SerializeField] protected AudioClip hitSoundEffect;
    [SerializeField] protected GameObject hiteffect;
    [SerializeField] protected AudioClip[] primaryWeaponSounds;
    [SerializeField] protected AudioClip thrusterSound;
    protected bool primaryWeaponCoolingDown;
    protected DateTime primaryWeaponCoolDownTime;
    protected int enemyWorth;
    protected GameObject spaceWarsUI;
    protected GameObject instantiatedThruster;
    protected AudioSource thrusterNoise;


    private void Awake()
    {
        waypointIndex = 1;
        primaryWeaponCoolingDown = false;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        Destroy(instantiatedThruster);
    }

    protected void init() 
    {
        instantiateThruster();
        spaceWarsUI = GameObject.FindGameObjectWithTag("space_wars_ui");
    }

    protected void moveToWaypoint(float craftSpeed)
    {

        if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < 0.1)
            {
            if (!(waypointIndex==waypoints.Count-1))
            {
                ++waypointIndex;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        transform.position = Vector2.MoveTowards(gameObject.transform.position,
                waypoints[waypointIndex].position, craftSpeed * Time.deltaTime);
        }

    protected void checkHealth()
    {
        if (health<=0)
        {
            spaceWarsUI.GetComponent<UIElements>().setScoreText(this.enemyWorth);
            GameObject.FindGameObjectWithTag("space_wars").GetComponent<SpaceWars>()
                    .createExplosionOne(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            GameObject.FindGameObjectWithTag("space_wars").GetComponent<SpaceWars>().spawnPickup(transform);
            Destroy(this.gameObject);
        }
    }


    protected void calculateCoolDowns()
    {
        if (primaryWeaponCoolingDown)
        {
            if (DateTime.Compare(DateTime.Now, primaryWeaponCoolDownTime) > 0)
            {
                primaryWeaponCoolingDown = false;
            }
        }
    }

    protected void instantiateThruster()
    {
        this.instantiatedThruster = Instantiate(thruster,new Vector2(gameObject.transform.position.x,
            gameObject.transform.position.y+0.8f),
            gameObject.transform.rotation);
    }

    protected void updateThruster()
    {
        this.instantiatedThruster.transform.position = new Vector2(gameObject.transform.position.x,
            gameObject.transform.position.y + 0.8f);
    }


    protected virtual void firePrimaryWeapon(GameObject PrimaryWeapon)
    {
        if (!primaryWeaponCoolingDown)
        {
            if (transform.position.x < 5 && transform.position.x > -5 && transform.position.y < 5)
            {

                gameObject.GetComponent<AudioSource>()
                    .PlayOneShot(primaryWeaponSounds[UnityEngine.Random.Range(0,primaryWeaponSounds.Length)]);
                var firedWeapon = Instantiate(PrimaryWeapon, transform.position, transform.rotation);
                firedWeapon.AddComponent<Rigidbody2D>().velocity = new Vector3(0, -10);
                firedWeapon.AddComponent<PolygonCollider2D>().isTrigger = true;
                primaryWeaponCoolingDown = true;
                primaryWeaponCoolDownTime = DateTime.Now.AddMilliseconds(weaponCoolDown*1000*UnityEngine.Random.RandomRange(0.5f,1f));
            }
        }
    }

    protected void performEnemyBasicFunctions()
    {
        moveToWaypoint(speed);
        calculateCoolDowns();
        firePrimaryWeapon(primaryWeapon);
        checkHealth();
        updateThruster();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag==Constants.PLAYER_WEAPON_TAG)
        {
            var collisionObject = collision.gameObject;

            gameObject.GetComponent<AudioSource>().PlayOneShot(hitSoundEffect);
            var hitLocation = new Vector2(collisionObject.transform.position.x,collisionObject.transform.position.y);
            Destroy(Instantiate(hiteffect,hitLocation,collisionObject.transform.rotation),0.1f);
            Destroy(collisionObject);
            health -= 25;
        }
    }

}
