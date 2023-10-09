using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnHandler : MonoBehaviour
{
    public GameObject _projectilePrefab;
    public float _spawnRate = 1f;
    public bool _spawnFull;
    
    public void TriggerRespawn()
    {
        StartCoroutine(SpawnBall());
    }

    private IEnumerator SpawnBall()
    {
        _spawnFull = false;
        yield return new WaitForSeconds(_spawnRate);
        GameObject ball = Instantiate(_projectilePrefab, transform.position, transform.rotation, this.transform);
        _spawnFull= true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player")
           && _spawnFull
           && !collider.GetComponent<PlayerStateMachine>().IsEquipped)
        {
            TriggerRespawn();           
        }
    }
}
