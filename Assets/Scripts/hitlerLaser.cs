using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitlerLaser : BasicLaser {

    [SerializeField] protected int laserSpeed;
    protected Vector3 forwards;
    private float rotationSpeed = 3f;

    void Start()
    {
        forwards = transform.up;
        this.damage = Constants.HEAVY_TURRET_DAMAGE;
    }
    void Update()
    {
        moveForward();
        rotate();
    }

    public void moveForward()
    {
        transform.position += forwards * Time.deltaTime * laserSpeed;
    }

    public void rotate()
    {
        Vector3 rotateTo = new Vector3(transform.rotation.x,
            transform.rotation.y,
            transform.rotation.z +1);
        transform.rotation = Quaternion.FromToRotation(transform.position,rotateTo);
    }

    public void setLaserSpeed(int speed)
    {
        this.laserSpeed = speed;
    }
}
