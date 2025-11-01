using UnityEngine;

public class BgmToggleButton : MonoBehaviour
{
    public GameObject buttonOn;
    public GameObject buttonOff;
    private BackgroundMusic bgm;

    void Start()
    {
#if UNITY_2023_1_OR_NEWER
        bgm = Object.FindFirstObjectByType<BackgroundMusic>();
#else
        bgm = FindObjectOfType<BackgroundMusic>();
#endif

        if (bgm == null)
        {
            Debug.LogError("Không tìm thấy BackgroundMusic trong scene!");
            return;
        }

        UpdateButtons();
    }

    public void ToggleMusic()
    {
        if (bgm == null) return;
        bgm.ToggleMusic();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (bgm == null) return;

        buttonOn.SetActive(!bgm.IsMuted);
        buttonOff.SetActive(bgm.IsMuted);
    }
}
