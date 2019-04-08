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
    float enemySpawnDelay;

    [SerializeField]
    float coinSpawnDelay;

    bool enemyTimerSet = false;

    bool coinTimerSet;

    [SerializeField]
    int maxEnemies;

    int currentEnemies;

    [SerializeField]
    int maxCoins;

    int currentCoins;

    List<Door> unusableDoors = new List<Door>();

    List<CoinSpawn> unusableCoinSpawns = new List<CoinSpawn>();

    [SerializeField]
    CoinSpawn[] coinSpawns;

    public static RandomSpawner instance;

    private void Awake() {

        instance = this;

        GetAllDoors();
        GetAllCoinSpawns();
    }

    private void Start() {

        currentEnemies = 0;
    }

    void GetAllDoors() {

        GameObject[] doorObjects = GameObject.FindGameObjectsWithTag("Door");

        doors = new Door[doorObjects.Length];

        for (int i = 0; i < doorObjects.Length; i++) {
            doors[i] = doorObjects[i].GetComponent<Door>();
        }
    }

    void GetAllCoinSpawns() {

        GameObject[] coinSpawnObjects = GameObject.FindGameObjectsWithTag("CoinSpawn");

        coinSpawns = new CoinSpawn[coinSpawnObjects.Length];

        for (int i = 0; i < coinSpawnObjects.Length; i++) {
            coinSpawns[i] = coinSpawnObjects[i].GetComponent<CoinSpawn>();
        }
    }

    void Update()
    {
        SpawnEnemiesRandomly();
        SpawnCoinsRandomly();
    }

    void SpawnEnemiesRandomly() {

        if (!enemyTimerSet) {

            enemyTimerSet = true;

            StartCoroutine(SpawnTimerEnemy());

        }
    }

    void SpawnCoinsRandomly() {

        if (!coinTimerSet) {

            coinTimerSet = true;

            StartCoroutine(SpawnTimerCoin());
        }
    }

    IEnumerator SpawnTimerCoin() {

        yield return new WaitForSeconds(coinSpawnDelay);

        SpawnCoinAtRandomPosition();

        coinTimerSet = false;
    }

    IEnumerator SpawnTimerEnemy() {

        yield return new WaitForSeconds(enemySpawnDelay);

        SpawnAtRandomDoor();

        enemyTimerSet = false;
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

    void SpawnCoinAtRandomPosition() {

        if (currentCoins < maxCoins) {

            int maxCoinNumber = coinSpawns.Length;

            int spawnArea = Random.Range(0, maxCoinNumber);

            if (coinSpawns[spawnArea] != null && !unusableCoinSpawns.Contains(coinSpawns[spawnArea])) {

                coinSpawns[spawnArea].SpawnCoin();

                unusableCoinSpawns.Add(coinSpawns[spawnArea]);

                StartCoroutine(coinCoolDown(coinSpawns[spawnArea]));

                currentCoins++;
            }
        }
    }

    IEnumerator coinCoolDown(CoinSpawn spawn) {

        yield return new WaitForSeconds(coinSpawnDelay * 3f);

        unusableCoinSpawns.Remove(spawn);
    }

    IEnumerator DoorCoolDown(Door door) {

        yield return new WaitForSeconds(enemySpawnDelay * 10f);

        unusableDoors.Remove(door);
    }

    public void DecrementCoins() {

        currentCoins--;
    }
}
