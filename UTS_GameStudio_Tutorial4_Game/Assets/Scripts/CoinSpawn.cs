using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField]
    Coin coinSpawn = null;

    public void SpawnCoin() {

        Instantiate(coinSpawn, transform.position, Quaternion.identity);
    }
}
