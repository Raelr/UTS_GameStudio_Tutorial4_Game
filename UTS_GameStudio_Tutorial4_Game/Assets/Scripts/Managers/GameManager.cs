using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player Player { get; set; }

    public static GameManager instance;

    private int _score = 0;
    private Text _scoreUI;
    
    private void Awake() {
        
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        _scoreUI = GameObject.Find("ScoreUI").GetComponent<Text>();
    }

    private void Update() {
        _scoreUI.text = _score.ToString("000000");
    }

    public void KillPlayer() {

        LevelManager.RestartLevel();
    }

    public void IncreaseScore() {
        _score += 10;
    }
}
