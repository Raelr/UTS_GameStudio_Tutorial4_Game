using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField]
    Door[] doors;

    [SerializeField]
    Vector3 spawnOffset;

    [SerializeField]
    float spawnDelay;

    bool timerSet = false;

    [SerializeField]
    int maxEnemies;

    int currentEnemies;

    List<Door> unusableDoors = new List<Door>();

    private void Awake() {

        GameObject[] doorObjects = GameObject.FindGameObjectsWithTag("Door");

        doors = new Door[doorObjects.Length];

        for (int i = 0; i < doorObjects.Length; i++) {
            doors[i] = doorObjects[i].GetComponent<Door>();
        }
    }

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

        yield return new WaitForSeconds(spawnDelay);

        SpawnAtRandomDoor();

        timerSet = false;
    }

    void SpawnAtRandomDoor() {

        if (currentEnemies < maxEnemies) {

            int maxDoorNumber = doors.Length;

            int doorNumber = Random.Range(0, maxDoorNumber);

            if (doors[doorNumber] != null && !unusableDoors.Contains(doors[doorNumber])) {

                doors[doorNumber].SpawnEnemy(spawnOffset);

                unusableDoors.Add(doors[doorNumber]);

                StartCoroutine(DoorCoolDown(doors[doorNumber]));

                currentEnemies++;
            }
        }
    }

    IEnumerator DoorCoolDown(Door door) {

        yield return new WaitForSeconds(spawnDelay + 1f);

        unusableDoors.Remove(door);
    }
}
