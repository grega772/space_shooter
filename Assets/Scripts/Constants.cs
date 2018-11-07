using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour {

	public readonly static float Z_INDEX = -1f;
    public readonly static float BASIC_ENEMY_SPEED = 3f;
    public readonly static float SCOUT_ENEMY_SPEED = 6f;
    public readonly static int BASIC_ENEMY_HEALTH = 100;
    public readonly static int SCOUT_ENEMY_HEALTH = 50;
    public readonly static float BASIC_ENEMY_WEAPON_DELAY = 1.5f;
    public readonly static float SCOUT_ENEMY_WEAPON_DELAY = 0.5f;
    public readonly static string PLAYER_WEAPON_TAG = "player_weapon";
    public readonly static int BASIC_WEAPON_DAMAGE = 10;
    public readonly static int BASIC_ENEMY_WORTH = 10;
    public readonly static int SCOUT_ENEMY_WORTH = 20;
    public readonly static int PLAYER_HEALTH = 100;

    public readonly static int HEAVY_ENEMY_HEALTH = 500;
    public readonly static int HEAVY_ENEMY_WORTH = 100;
    public readonly static float HEAVY_ENEMY_SPEED = 1f;

    public readonly static string EXTRA_SHOT = "EXTRA_SHOT";
    public readonly static string EXTRA_HEALTH = "EXTRA_HEALTH";
    public readonly static string MEGA_HEALTH = "MEGA_HEALTH";
    public readonly static string FOURTH_OF_JULY = "FOURTH_OF_JULY";
    public readonly static string RIGHT_TO_BEAR_ARMS = "RIGHT_TO_BEAR_ARMS";
    public readonly static string MOUNT_RUSHMORE_ASSIST = "MOUNT_RUSHMORE_ASSIST";

    public readonly static int HEALTH_PICKUP_VALUE = 25;
    public readonly static int MEGA_HEALTH_PICKUP_VALUE = 50;

    public readonly static float HEAVY_TURRET_MOVE_SPEED = 0.5f;
    public readonly static float HEAVY_PROJECTILE_SPEED = 10;
    public readonly static int HEAVY_TURRET_DAMAGE = 20;
    public readonly static int HEAVY_TURRET_HEATH = 100;
}

