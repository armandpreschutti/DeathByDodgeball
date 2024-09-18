using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnHandler : MonoBehaviour
{
    public BallTypesSO spawnableBalls;
    public float _spawnRate = 1f;
    public bool _spawnFull;
    public GameObject currentBall;
    public float time;

    private void Start()
    {
        SpawnBall();
    }

    private void Update()
    {
        RespawnTimer();
        EmptyDetector();
    }

    public void SpawnBall()
    {
        GameObject ball = spawnableBalls.ballTypes[Random.Range(0, spawnableBalls.ballTypes.Length)];
        GameObject ballInstance = Instantiate(ball, transform.position, transform.rotation, this.transform);
        ballInstance.transform.parent = transform;
    }

    public void RespawnTimer()
    {
        if (!_spawnFull)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
    }
    public void EmptyDetector()
    {
        if (transform.childCount == 0)
        {
            _spawnFull = false;
        }
        else
        {
            _spawnFull = true;
        }
    }
}
