using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region Game Manager
    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    #endregion

    public PauseMenu pauseMenu;

    public void Start()
    {
        Resume();
    }
    
    public bool isPaused;
    
    public void ChangeScene(int sceneIndex)
    {   
        SceneManager.LoadScene(sceneIndex);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
    
   public SettingsMenu SettingsMenu;
   
   public bool isSettings;



}