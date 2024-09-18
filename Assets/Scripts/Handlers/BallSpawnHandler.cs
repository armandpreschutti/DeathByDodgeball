using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnHandler : MonoBehaviour
{
    public BallTypesSO spawnableBalls;
    public float _spawnRate = 5f;
        /*public bool _spawnFull;
        public GameObject currentBall;
        public float time;*/

    public Coroutine spawnNewBall;

    private void Start()
    {
        SpawnBall();
    }


    public void SpawnBall()
    {
        GameObject ball = spawnableBalls.ballTypes[Random.Range(0, spawnableBalls.ballTypes.Length)];
        GameObject ballInstance = Instantiate(ball, transform.position, transform.rotation, this.transform);
        ballInstance.transform.parent = transform;
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


}
