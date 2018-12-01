using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpaceWars : MonoBehaviour {

    [SerializeField] GameObject background;
    [SerializeField] GameObject explosionOne;
    [SerializeField] GameObject[] debris;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] upgrades;
    [SerializeField] GameObject[] superUpgrades;
    [SerializeField] GameObject[] enemyPilots;
    [SerializeField] GameObject bigExplosion;
    protected GameObject SpaceWarsUI;
    public int playerLives;
    protected bool isRespawn = false;
    protected DateTime respawnTimer;


    bool switchedImage = false;
    bool twoAtOnce = false;

    private GameObject secondBackground;

	// Use this for initialization
	void Start () {
        this.playerLives = 3;
        SpaceWarsUI = GameObject.FindGameObjectWithTag("space_wars_ui");
    }
	
	// Update is called once per frame
	void Update () {
        scrollBackground();
        checkForRespawn();
    }

    private void checkForRespawn()
    {
        if (isRespawn)
        {
            if (DateTime.Now>respawnTimer)
            {
                Instantiate(player,new Vector3(0,-4,-1),transform.rotation);
                isRespawn = false;
                SpaceWarsUI.GetComponent<UIElements>().setHealthText(Constants.PLAYER_HEALTH);
            }
        }
    }

    private void scrollBackground()
    {
        var backgroundPos = background.transform.position;

        background.transform.position = new Vector3(backgroundPos.x,backgroundPos.y-0.05f,backgroundPos.z);
        
        if (!switchedImage) {
            if (backgroundPos.y < -11.4)
            {
                secondBackground = Instantiate(background,(transform.position = new Vector3(
                    backgroundPos.x,21.75f, backgroundPos.z)),transform.rotation);
                switchedImage = true;
                twoAtOnce = true;
            }
        }


        if (twoAtOnce)
        {
            var secondBackgroundPos = secondBackground.transform.position;

            secondBackground.transform.position = new Vector3(secondBackgroundPos.x, secondBackgroundPos.y - 0.05f, secondBackgroundPos.z);
        }

        if (backgroundPos.y < -21.4f)
        {
            Destroy(background);
            background = secondBackground;
            twoAtOnce = false;
            switchedImage = false;
        }
       

    }

    public void createExplosionOne(Vector3 location, Quaternion rotation)
    {
        var expl = Instantiate(explosionOne, location, rotation);
        this.createDebris(location, rotation);
        expl.GetComponent<AudioSource>().PlayOneShot(expl.GetComponent<AudioClip>());
        Destroy(expl, 1f);
        var pilot = Instantiate(enemyPilots[0], location, transform.rotation);
        var pilotXVel = UnityEngine.Random.RandomRange(-5, 5);
        var pilotYVel = UnityEngine.Random.RandomRange(-5, 5);
        if (pilotXVel==0)
        {
            pilotXVel += 1;
        }
        if (pilotYVel==0)
        {
            pilotYVel += 1;
        }

        pilot.AddComponent<Rigidbody2D>().velocity = 
            new Vector2(pilotXVel, pilotYVel);
    }


    public void createDebris(Vector3 location, Quaternion rotation)
    {
        int rand = UnityEngine.Random.Range(2, 5);

        for (int i=0; i<rand;i++)
        {
            foreach (GameObject chunk in this.debris)
            {
                var randXVel = UnityEngine.Random.Range(-7, 7);
                var randYVel = UnityEngine.Random.Range(-7, 7);
                if (randXVel==0)
                {
                    randXVel = 1;
                }
                if (randYVel==0)
                {
                    randYVel = 1;
                }
                var randAngularVelocity = UnityEngine.Random.Range(-50, 50);
                var newChunk = Instantiate(chunk, location, rotation);
                newChunk.GetComponent<Rigidbody2D>().velocity = new Vector2(randXVel, randYVel);
                newChunk.GetComponent<Rigidbody2D>().rotation = randAngularVelocity;
            }
        }
    }

    public void instantiateNewPlayer()
    {
        if (--playerLives >0)
        {
            isRespawn = true;
            respawnTimer = DateTime.Now.AddMilliseconds(1200);
        }
        else
        {

        }
        
    }

    public void spawnPickup(Transform location)
    {
        var randNum = UnityEngine.Random.Range(1,11);

        if (randNum < 5)
        {
            GameObject upgrade = null;
            Vector3 spawnLocation = location.position;
            if (randNum < 3)
            {
                var randChoice = UnityEngine.Random.Range(0,upgrades.Length);
                //var randChoice = 0;
                upgrade = Instantiate(upgrades[randChoice],spawnLocation,transform.rotation);
                upgrade.AddComponent<Rigidbody2D>().velocity = new Vector3(0, -3);
            }
            else if (randNum == 3)
            {
            }
            
        }
    }

    public void createExplosionTwo(Vector3 location, Quaternion rotation)
    {
            var expl = Instantiate(explosionOne, location, rotation);
            this.createDebris(location, rotation);
            expl.GetComponent<AudioSource>().PlayOneShot(expl.GetComponent<AudioClip>());
            Destroy(expl, 1f);
    }

    public void createExplosionThree(Vector3 location, Quaternion rotation)
    {
        var expl = Instantiate(bigExplosion, location, rotation);
        expl.GetComponent<AudioSource>().PlayOneShot(expl.GetComponent<AudioClip>());
        Destroy(expl, 1f);
    }

}
