using UnityEngine;
using UnityEngine.UI;

public class SoundButtonController : MonoBehaviour
{
    [Header("UI References")]
    public Button soundButton;
    public Slider volumeSlider;

    [Header("Icons")]
    public Sprite soundOnIcon;  
    public Sprite soundOffIcon;

    const string PREF_MASTER_VOL = "MASTER_VOLUME";
    private bool isSliderVisible = false;
    private Image soundIcon; 

    void Start()
    {
        soundIcon = soundButton.GetComponent<Image>();

        float savedVolume = PlayerPrefs.GetFloat(PREF_MASTER_VOL, 1f);
        AudioListener.volume = savedVolume;

        if (volumeSlider)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            volumeSlider.gameObject.SetActive(false); 
        }

        if (soundButton)
        {
            soundButton.onClick.AddListener(ToggleSliderVisibility);
        }

        UpdateIcon(savedVolume);
    }

    void ToggleSliderVisibility()
    {
        isSliderVisible = !isSliderVisible;
        volumeSlider.gameObject.SetActive(isSliderVisible);
    }

    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(PREF_MASTER_VOL, value);
        UpdateIcon(value);
    }

    void UpdateIcon(float value)
    {
        if (soundIcon != null)
        {
            if (value <= 0.01f && soundOffIcon != null)
                soundIcon.sprite = soundOffIcon;
            else if (value > 0.01f && soundOnIcon != null)
                soundIcon.sprite = soundOnIcon;
        }
    }
}
