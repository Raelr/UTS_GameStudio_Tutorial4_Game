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
    private Text _pauseUI;
    private Text _pauseInfoUI;

    [SerializeField]
    AudioClip loseSound;
    
    private void Awake() {
        
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        _scoreUI = GameObject.Find("ScoreUI").GetComponent<Text>();
        _pauseUI = GameObject.Find("PauseUI").GetComponent<Text>();
        _pauseInfoUI = GameObject.Find("PauseInstructionsUI").GetComponent<Text>();
    }

    private void Update() {
        //Updates Score on Screen
        _scoreUI.text = _score.ToString("000000");
        //Pause Screen functionality
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0f) {
            Time.timeScale = 0f;
            //Changes transparency of Pause message to show game paused
            Color c = _pauseUI.color;
            c.a = 255;
            _pauseUI.color = c;
            _pauseInfoUI.color = c;
        } else if (Input.GetKeyDown(KeyCode.Return) && Time.timeScale == 0f) {
            Time.timeScale = 1f;
            Color c = _pauseUI.color;
            c.a = 0;
            _pauseUI.color = c;
            _pauseInfoUI.color = c;
        } else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0f) Application.Quit();
    }

    public void KillPlayer() {

        StartCoroutine(PlayDeathClipAndRestart());
    }

    public void IncreaseScore() {
        _score += 10;
    }

    IEnumerator PlayDeathClipAndRestart() {

        float duration = loseSound.length;

        SoundManager.instance.PlaySingleSound(loseSound);

        yield return new WaitForSeconds(duration);

        LevelManager.RestartLevel();
    }
}
