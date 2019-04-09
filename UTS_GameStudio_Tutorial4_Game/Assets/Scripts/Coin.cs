using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start() {

        StartCoroutine(WaitTenSeconds()); //Self-destruct coin after 10 seconds
    }
    
    void Update() {
        
    }

    public void OnCoinPickUp() {

        RandomSpawner.instance.DecrementCoins();

        GameManager.instance.IncreaseScore();

        Destroy(this.gameObject);
    }

    IEnumerator WaitTenSeconds() {

        yield return new WaitForSeconds(10f);

        RandomSpawner.instance.DecrementCoins();

        Destroy(this.gameObject);
    }
}
