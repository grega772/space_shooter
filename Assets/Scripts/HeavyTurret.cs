using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTurret : Turret {

	// Use this for initialization
	void Start () {
        this.rotationSpeed = Constants.HEAVY_TURRET_MOVE_SPEED;
        this.health = Constants.HEAVY_TURRET_HEATH;
	}
	
	// Update is called once per frame
	void Update () {
        base.heavyTurretFunctions();
    }
}
