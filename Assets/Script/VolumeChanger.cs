using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;
    public Image[] volumeBars;  // Tempatkan 6 Image bar di sini
    public AudioSource audioSource;  // Sumber audio di game
    public float volumeStep = 0.2f;  // Langkah volume saat tombol ditekan
    public Button increaseButton;
    public Button decreaseButton;
    
    private const string VolumePrefKey = "GameVolume";  // Key untuk menyimpan volume di PlayerPrefs

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Membuat object ini tidak dihancurkan saat pindah scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Membaca volume dari PlayerPrefs (default ke 0.5 jika belum ada data)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1.0f);
        SetVolume(savedVolume);

        increaseButton.onClick.AddListener(IncreaseVolume);
        decreaseButton.onClick.AddListener(DecreaseVolume);
    }

    public void UpdateVolumeBars(float volume)
    {
        int activeBars = Mathf.FloorToInt(volume * volumeBars.Length);  // Menghitung jumlah bar aktif sesuai volume (0-1)
        for (int i = 0; i < volumeBars.Length; i++)
        {
            volumeBars[i].enabled = i <= activeBars;  // Menyalakan/mematikan bar sesuai volume
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;  // Mengatur volume audio
        UpdateVolumeBars(volume);     // Mengupdate tampilan bar

        // Simpan volume ke PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }

    public void IncreaseVolume()
    {
        float newVolume = Mathf.Clamp(audioSource.volume + volumeStep, 0f, 1f);  // Menjaga volume di antara 0-1
        SetVolume(newVolume);
    }

    public void DecreaseVolume()
    {
        float newVolume = Mathf.Clamp(audioSource.volume - volumeStep, 0f, 1f);  // Menjaga volume di antara 0-1
        SetVolume(newVolume);
    }
}
