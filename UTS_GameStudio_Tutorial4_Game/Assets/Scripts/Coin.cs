using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start() {
        Destroy(this.gameObject, 10.0f); //Self-destruct coin after 10 seconds
    }
    
    void Update() {
        
    }

    public void OnCoinPickUp() {
        GameManager.instance.IncreaseScore();
        Destroy(this.gameObject);
    }
}
