using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    public Button fullscreenToggleButton;  // Tombol untuk toggle fullscreen
    public Button windowedToggleButton;
    public Image fullscreenImage;          // Gambar untuk mode fullscreen
    public Image windowedImage;            // Gambar untuk mode windowed

    void Start()
    {
        // Memastikan mode layar sesuai dengan preferensi yang disimpan
        ApplySavedScreenMode();

        // Jika mode fullscreen pertama kali dijalankan, set langsung ke fullscreen
        if (!PlayerPrefs.HasKey("ScreenMode"))
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetString("ScreenMode", "Fullscreen");  // Simpan mode fullscreen
            fullscreenImage.enabled = true;
            windowedImage.enabled = false;
        }
        
        fullscreenToggleButton.onClick.AddListener(ToggleFullscreen);
        windowedToggleButton.onClick.AddListener(ToggleFullscreen);
    }

    private void ToggleFullscreen()
    {
        // Cek apakah mode layar saat ini adalah fullscreen
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            // Ubah ke windowed
            Screen.fullScreenMode = FullScreenMode.Windowed;
            windowedImage.enabled = true;
            fullscreenImage.enabled = false;

            // Simpan pengaturan mode layar ke PlayerPrefs
            PlayerPrefs.SetString("ScreenMode", "Windowed");
        }
        else
        {
            // Ubah ke fullscreen
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            windowedImage.enabled = false;
            fullscreenImage.enabled = true;

            // Simpan pengaturan mode layar ke PlayerPrefs
            PlayerPrefs.SetString("ScreenMode", "Fullscreen");
        }
    }

    private void ApplySavedScreenMode()
    {
        // Mengecek apakah ada pengaturan layar yang disimpan
        if (PlayerPrefs.HasKey("ScreenMode"))
        {
            string savedMode = PlayerPrefs.GetString("ScreenMode");

            if (savedMode == "Fullscreen")
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                fullscreenImage.enabled = true;
                windowedImage.enabled = false;
            }
            else if (savedMode == "Windowed")
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                fullscreenImage.enabled = false;
                windowedImage.enabled = true;
            }
        }
        else
        {
            // Pengaturan default jika belum ada preferensi
            Screen.fullScreenMode = FullScreenMode.Windowed;
            fullscreenImage.enabled = false;
            windowedImage.enabled = true;
        }
    }
}
