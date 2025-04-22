using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Class that handles scene management
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Dictionary<int, Vector3[]> levelData;
    public int totalLevels;
    public int currentLevel;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }

    void Start()
    {
        LoadLevel();
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        Debug.Log(currentLevel);

        if (currentLevel <= totalLevels)
        {
            LoadLevel();
        }
    }

    // Loads a scene
    private void LoadLevel()
    {
        string sceneToLoad = "Level " + currentLevel;
        Debug.Log("Loading scene: " + sceneToLoad);
        if (Time.timeScale != 0) {
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }

    public void restartLoop() {
        currentLevel = 1;
        LoadLevel();
    }
}