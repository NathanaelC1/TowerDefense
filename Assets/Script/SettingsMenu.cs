using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public Button buttonSettings;
    public Button buttonFullscreen;
    public Button buttonClose;

    void Start()
    {
        settingsMenu.SetActive(false);
        buttonClose.onClick.AddListener(HandleButtonClick1);
        buttonSettings.onClick.AddListener(HandleButtonClick2);
    }

    public void HandleButtonClick2()
    {
        if(GameManager.Instance.isSettings)
        {
            settingsMenu.SetActive(false);            
        }
        else
        {
            settingsMenu.SetActive(true);
        }
    }
    
    public void HandleButtonClick1()
    {
        if(GameManager.Instance.isSettings)
        {
            settingsMenu.SetActive(true);            
        }
        else
        {
            settingsMenu.SetActive(false);
        }
    }
}