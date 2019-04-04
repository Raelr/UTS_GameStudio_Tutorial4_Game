using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start() {
        
    }
    
    void Update() {
        
    }

    public void OnCoinPickUp() {
        GameManager.instance.IncreaseScore();
        Destroy(this.gameObject);
    }
}
