using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player Player { get; set; }

    public static GameManager instance;

    private int _score = 0;
    
    private void Awake() {
        
        if (instance == null) {
            instance = this;
        }
    }

    public void KillPlayer() {

        LevelManager.RestartLevel();
    }

    public void IncreaseScore() {
        _score += 10;
    }
}
