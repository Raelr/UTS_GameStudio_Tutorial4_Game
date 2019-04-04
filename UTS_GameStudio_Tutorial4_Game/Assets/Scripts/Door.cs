using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Enemy enemyScript;

    public void SpawnEnemy(Vector3 offset) {

        Instantiate(enemyScript, transform.position + offset, Quaternion.identity);
    }
}
