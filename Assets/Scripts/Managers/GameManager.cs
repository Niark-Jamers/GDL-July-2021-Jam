using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player
{
    public int level;
    public string height;
    public Vector3 position;
}

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public GUIManager guiManager;
    public AudioManager audioManager;

    [Space]
    public bool pause;

    public Transform playerPosition;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Time.timeScale = 1;
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        guiManager.Pause();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
