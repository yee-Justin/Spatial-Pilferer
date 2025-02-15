using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Ensures only one instance exists
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // Keeps GameManager across scenes
        InitializeGame();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ========================== INITIALIZATION & MANAGEMENT ==========================

    private void InitializeGame()
    {
        Debug.Log("GameManager Initialized.");
        // Potential future initialization (player stats, score, etc.)
    }

    private void Start()
    {
    }

    // ========================== SCENE MANAGEMENT ==========================
    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == "GameStart")
        {
            Debug.Log("Starting game...");
            AdvanceToTutorial();
        }
        else
        {
            Debug.LogError("StartGame() was called from the wrong scene!");
        }
    }

    public void AdvanceToTutorial()
    {
        Debug.Log("Loading tutorial...");
        StartCoroutine(LoadSceneAsync("Tutorial"));
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Reloading...");
        StartCoroutine(LoadSceneAsync("GameOver"));  // Assume a GameOver scene exists
    }

    public void Win()
    {
        Debug.Log("You Win! Loading Win Scene...");
        StartCoroutine(LoadSceneAsync("WinScene"));  // Assume a WinScene exists
    }

    // ========================== ASYNC SCENE LOADING ==========================
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // ========================== EVENT HANDLING ==========================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // Example: Find the player in the new scene if needed
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) Debug.LogWarning("Player object not found in the new scene!");
        }
    }
}
