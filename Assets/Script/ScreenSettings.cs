using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScreenSettings : MonoBehaviour
{
    public Button resolutionUpButton;
    public Button resolutionDownButton;
    public TextMeshProUGUI resolutionText;

    private Resolution[] availableResolutions;
    private int currentResolutionIndex = 0;
    private bool canChangeResolution = true;
    private float resolutionChangeCooldown = 0.2f; // 0.2 detik

    void Start()
    {
        availableResolutions = Screen.resolutions;

        // Mencari resolusi saat ini
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                break;
            }
        }

        // Memuat resolusi yang disimpan (jika ada)
        LoadResolutionFromPrefs();

        resolutionUpButton.onClick.AddListener(IncreaseResolution);
        resolutionDownButton.onClick.AddListener(DecreaseResolution);

        UpdateResolutionText();
    }

    private void IncreaseResolution()
    {
        if (canChangeResolution && currentResolutionIndex < availableResolutions.Length - 1)
        {
            currentResolutionIndex++;
            SetResolution(currentResolutionIndex);
            StartCoroutine(ResolutionChangeCooldown());
        }
    }

    private void DecreaseResolution()
    {
        if (canChangeResolution && currentResolutionIndex > 0)
        {
            currentResolutionIndex--;
            SetResolution(currentResolutionIndex);
            StartCoroutine(ResolutionChangeCooldown());
        }
    }

    private IEnumerator ResolutionChangeCooldown()
    {
        canChangeResolution = false;
        yield return new WaitForSeconds(resolutionChangeCooldown);
        canChangeResolution = true;
    }

    private void SetResolution(int index)
    {
        Resolution resolution = availableResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        UpdateResolutionText();

        // Menyimpan resolusi yang dipilih ke PlayerPrefs
        PlayerPrefs.SetInt("ResolutionWidth", resolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", resolution.height);
        PlayerPrefs.Save();
    }

    private void UpdateResolutionText()
    {
        Resolution resolution = availableResolutions[currentResolutionIndex];
        resolutionText.text = resolution.width + " x " + resolution.height;
    }

    private void LoadResolutionFromPrefs()
    {
        // Mengecek apakah ada preferensi resolusi yang tersimpan
        if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight"))
        {
            int savedWidth = PlayerPrefs.GetInt("ResolutionWidth");
            int savedHeight = PlayerPrefs.GetInt("ResolutionHeight");

            // Mencari resolusi yang cocok dengan yang disimpan
            for (int i = 0; i < availableResolutions.Length; i++)
            {
                if (availableResolutions[i].width == savedWidth && availableResolutions[i].height == savedHeight)
                {
                    currentResolutionIndex = i;
                    break;
                }
            }

            // Set resolusi yang tersimpan
            Resolution savedResolution = availableResolutions[currentResolutionIndex];
            Screen.SetResolution(savedResolution.width, savedResolution.height, Screen.fullScreen);
            UpdateResolutionText();
        }
    }
}
