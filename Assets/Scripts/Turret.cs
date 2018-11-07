using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour {

    [SerializeField] protected GameObject projectile;
    protected float projectileSpeed;
    protected float rotationSpeed;
    [SerializeField] protected GameObject muzzleFlash;
    [SerializeField] protected GameObject[] barrelList;
    private bool inFireDelay = false;
    private DateTime fireDelay = DateTime.Now;
    private int barrelIndex = 0;
    public int health;

    // Use this for initialization

    protected void heavyTurretFunctions()
    {
        rotateToTrackPlayer();
        checkHealth();
    }

    protected void attemptToFire()
    {
        if (!inFireDelay)
        {
            var laser = Instantiate(projectile,
                barrelList[barrelIndex].transform.position,
                transform.rotation);
            var flash = Instantiate(muzzleFlash,
                barrelList[barrelIndex].transform.position,
                transform.rotation);
            flash.transform.rotation = transform.rotation;
            Destroy(flash,0.2f);
            laser.transform.rotation = transform.rotation;
            laser.AddComponent<Rigidbody2D>();
            fireDelay = DateTime.Now.AddMilliseconds(200);
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
                attemptToFire();
            }

            if (Mathf.Abs(targetAngle - currentRotation) > this.rotationSpeed)
            {
                if (targetAngle < currentRotation)
                {
                    currentRotation -= rotationSpeed;
                }
                else if (targetAngle > currentRotation)
                {
                    currentRotation += rotationSpeed;
                }
            }
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
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
