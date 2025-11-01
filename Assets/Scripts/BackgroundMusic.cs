using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;
    public bool IsMuted { get; private set; }

    private void Awake()
    {
        // Giữ nhạc khi đổi scene
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Gán AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("⚠️ Không tìm thấy AudioSource trong BackgroundMusic!");
            return;
        }

        // Lấy trạng thái lưu trước đó
        IsMuted = PlayerPrefs.GetInt("BGM_MUTE", 0) == 1;
        audioSource.mute = IsMuted;
    }

    public void ToggleMusic()
    {
        if (audioSource == null) return;

        IsMuted = !IsMuted;
        audioSource.mute = IsMuted;

        // Lưu trạng thái
        PlayerPrefs.SetInt("BGM_MUTE", IsMuted ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"🎵 Nhạc nền {(IsMuted ? "TẮT" : "BẬT")}");
    }
}
