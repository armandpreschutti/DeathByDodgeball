using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnHandler : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnRate = 1f;
    public bool canSpawn;
    public ProjectileHandler[] currentBalls;
    public int maxBalls;

    public void TriggerRespawn()
    {

        StartCoroutine(SpawnProjectiles());
    }

    private IEnumerator SpawnProjectiles()
    {
        // Wait for the specified spawn interval before spawning the next projectile
        yield return new WaitForSeconds(spawnRate);
        // Randomly select a spawn point

        // Instantiate the projectile at the selected spawn point
        GameObject ball = Instantiate(projectilePrefab, transform.position, transform.rotation, this.transform);
    }
}
