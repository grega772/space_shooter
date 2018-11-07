using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Wave Config")]
public class WaveConfiguration : ScriptableObject {

    [SerializeField] GameObject enemy;
    [SerializeField] int enemyCount;
    [SerializeField] int distApart;
    [SerializeField] Transform pathPrefab;
    [SerializeField] int timeBetweenWaves;
    private List<Transform> waypoints;
    private List<GameObject> currentEnemies;

	public void Init () {

        currentEnemies = new List<GameObject>();

        waypoints = new List<Transform>();

        foreach (Transform waypoint in pathPrefab.transform)
        {
            waypoints.Add(waypoint);
        }

        var startPos = waypoints[0].position;

        for (int i = 0; i<enemyCount;i++)
        {
            var newEnemy = Instantiate(enemy,new Vector3(startPos.x+(i*distApart), 
                startPos.y, Constants.Z_INDEX), enemy.transform.rotation);
            newEnemy.GetComponent<Enemy>().waypoints = waypoints;
            newEnemy.AddComponent<PolygonCollider2D>().isTrigger = true;
            newEnemy.GetComponent<Enemy>().setWaveConfiguration(this);
            currentEnemies.Add(newEnemy);
        }
	}

    public int getRemainingEnemies()
    {
        return this.currentEnemies.Count;
    }

    public void removeEnemyFromWave(GameObject enemy)
    {
        currentEnemies.Remove(enemy);
    }

    public int getTimeBetweenWaves()
    {
        return timeBetweenWaves;
    }
}
