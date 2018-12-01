using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserDirection : MonoBehaviour {

    [SerializeField] protected int laserSpeed;

	void Start () {
		
	}
	
	void Update () {
        moveForward();
	}

    public void moveForward()
    {
        transform.position += transform.up * Time.deltaTime * laserSpeed;
    }

    public void setLaserSpeed(int speed)
    {
        this.laserSpeed = speed;
    }
}
