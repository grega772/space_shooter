using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour {

    [SerializeField] protected GameObject projectile;
    [SerializeField] protected AudioClip rotateStart;
    [SerializeField] protected AudioClip rotate;
    [SerializeField] protected AudioClip rotateEnd;
    [SerializeField] protected AudioClip[] turretFireSounds;
    protected float projectileSpeed;
    protected float rotationSpeed;
    [SerializeField] protected GameObject muzzleFlash;
    [SerializeField] protected GameObject[] barrelList;
    private bool inFireDelay = false;
    private DateTime fireDelay = DateTime.Now;
    private int barrelIndex = 0;
    public int health;
    private bool rotating;
    private DateTime rotationSoundChange = DateTime.Now;
    private float rotationSoundChangeDelay  = 100f;

    // Use this for initialization

    protected void heavyTurretFunctions()
    {
        rotateToTrackPlayer();
        checkHealth();
        //checkToSeeIfRotating();
    }

    protected void checkToSeeIfRotating()
    {
        if (DateTime.Now > rotationSoundChange)
        {
            if (!rotating && GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Pause();
            }
            else if (rotating && !GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().UnPause();
            }
            rotationSoundChange = DateTime.Now.AddMilliseconds(rotationSoundChangeDelay);
        }
    }

    protected void attemptToFire(Vector3 playerPos)
    {
        if (!inFireDelay
            &&Vector2.Distance(transform.position,playerPos)<8)
        {
            var laser = Instantiate(projectile,
                barrelList[barrelIndex].transform.position,
                transform.rotation);
            var flash = Instantiate(muzzleFlash,
                barrelList[barrelIndex].transform.position,
                transform.rotation);
            var barrelAudioSource = barrelList[barrelIndex].GetComponent<AudioSource>();
            barrelAudioSource
                .PlayOneShot(turretFireSounds[UnityEngine.Random.Range(0,turretFireSounds.Length)]);
            flash.transform.rotation = transform.rotation;
            Destroy(flash,0.2f);
            laser.transform.rotation = transform.rotation;
            laser.AddComponent<Rigidbody2D>();
            fireDelay = DateTime.Now.AddMilliseconds(100);
            inFireDelay = true;
            barrelIndex = ++barrelIndex >= barrelList.Length ? 0 : barrelIndex;
        }
        else
        {
            if (DateTime.Compare(DateTime.Now, fireDelay) > 0)
            {
                inFireDelay = false;
            }
        }
    }

    protected void rotateToTrackPlayer()
    {
        if (GameObject.FindGameObjectWithTag("player_ship") != null)
        {
            var playerPos = GameObject.FindGameObjectWithTag("player_ship").transform.position;
            var screenPoint = transform.position;
            var offset = new Vector2(playerPos.x - screenPoint.x, playerPos.y - screenPoint.y);
            var targetAngle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg) + 270;
            targetAngle = targetAngle > 360 ? targetAngle - 360 : targetAngle;
            var currentRotation = transform.rotation.eulerAngles.z;

            if (Mathf.Abs(currentRotation - targetAngle) < 10)
            {
                attemptToFire(playerPos);
            }

            if (Mathf.Abs(targetAngle - currentRotation) > this.rotationSpeed)
            {
                rotating = true;
                if (targetAngle < currentRotation)
                {
                    currentRotation -= rotationSpeed;
                }
                else if (targetAngle > currentRotation)
                {
                    currentRotation += rotationSpeed;
                }
            }
            else
            {
                rotating = false;
            }
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
        else
        {
            rotating = false;
        }
        
    }

    protected void checkHealth()
    {
        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("space_wars").GetComponent<SpaceWars>()
                    .createExplosionOne(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
