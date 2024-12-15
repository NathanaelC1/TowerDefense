using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button buttonMenu;
    public Button buttonMainMenu;
    public Button buttonResume;

    void Start()
    {
        buttonMainMenu.onClick.AddListener(() => {GameManager.Instance.ChangeScene(0); GameManager.Instance.Resume();});
        buttonMenu.onClick.AddListener(HandleButtonClick);
        buttonResume.onClick.AddListener(HandleButtonClick);
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            HandleButtonClick();
        }
    }

    private void HandleButtonClick()
    {
        if(GameManager.Instance.isPaused)
        {
            pauseMenu.SetActive(false);
            GameManager.Instance.Resume();
            buttonResume.gameObject.SetActive(false);            
        }
        else
        {
            pauseMenu.SetActive(true);
            GameManager.Instance.Pause();           
            buttonResume.gameObject.SetActive(true);           
        }
    }
}
