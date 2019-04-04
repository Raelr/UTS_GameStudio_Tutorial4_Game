using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField]
    Door[] doors;

    [SerializeField]
    Vector2 spawnOffset;

    bool timerSet = false;

    [SerializeField]
    int maxEnemies;

    int currentEnemies;

    private void Start() {

        currentEnemies = 0;
    }

    void Update()
    {
        SpawnEnemiesRandomly();
    }

    void SpawnEnemiesRandomly() {

        if (!timerSet) {

            timerSet = true;

            StartCoroutine(SpawnTimer());

        }
    }

    IEnumerator SpawnTimer() {

        yield return new WaitForSeconds(3f);

        SpawnAtRandomDoor();

        timerSet = false;
    }

    void SpawnAtRandomDoor() {

        if (currentEnemies < maxEnemies) {

            int maxDoorNumber = doors.Length;

            int doorNumber = Random.Range(0, maxDoorNumber);

            if (doors[doorNumber] != null) {

                Debug.Log("Spawning enemy at " + doors[doorNumber]);

                doors[doorNumber].SpawnEnemy(spawnOffset);
                currentEnemies++;
            }
        }
    }
}
