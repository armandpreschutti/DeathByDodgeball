using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnHandler : MonoBehaviour
{
    public BallTypesSO spawnableBalls;
    public float _spawnRate = 5f;
    public GameObject lastBall;
    public GameObject normalBall;
        /*public bool _spawnFull;
        public GameObject currentBall;
        public float time;*/

    public Coroutine spawnNewBall;

    private void Start()
    {
        SpawnIntitialBalls();
    }


    public void SpawnBall()
    {
        GameObject ball = spawnableBalls.ballTypes[Random.Range(0, spawnableBalls.ballTypes.Length)];
        if(ball != lastBall || lastBall == normalBall)
        {
            GameObject ballInstance = Instantiate(ball, transform.position, transform.rotation, this.transform);
            lastBall = ball;
            ballInstance.transform.parent = transform;
        }
        else
        {
            SpawnBall();
        }

    }

    public void SpawnNewBall()
    {
        if(spawnNewBall == null)
        {
            StartCoroutine(SpawnDelay());
        }
    }

    public IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(_spawnRate);
        SpawnBall();
    }

    public void SpawnIntitialBalls()
    {
        GameObject ball = normalBall;
        GameObject ballInstance = Instantiate(ball, transform.position, transform.rotation, this.transform);
        //lastBall = ball;
        ballInstance.transform.parent = transform;
    }


}
