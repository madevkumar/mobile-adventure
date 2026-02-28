using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int totalLevels = 5;
    [SerializeField] private float levelTime = 300f;
    private float timeRemaining;
    private bool levelActive = true;

    public static LevelManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        timeRemaining = levelTime;
    }

    private void Update() {
        if (levelActive) {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0) {
                LevelFailed();
            }
        }
    }

    public void LevelComplete() {
        levelActive = false;
        Debug.Log("Level " + currentLevel + " Complete!");
        if (currentLevel < totalLevels) {
            currentLevel++;
            LoadLevel(currentLevel);
        } else {
            GameComplete();
        }
    }

    public void LevelFailed() {
        levelActive = false;
        Debug.Log("Level " + currentLevel + " Failed!");
        ReloadLevel();
    }

    public void LoadLevel(int levelNumber) {
        SceneManager.LoadScene("Level_" + levelNumber);
    }

    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameComplete() {
        Debug.Log("Game Complete!");
        SceneManager.LoadScene("MainMenu");
    }

    public float GetTimeRemaining() {
        return timeRemaining;
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public void PauseLevel() {
        levelActive = false;
        Time.timeScale = 0f;
    }

    public void ResumeLevel() {
        levelActive = true;
        Time.timeScale = 1f;
    }
}