using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void playClip(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
    }

    public void playRandomClip(AudioClip[] clips, AudioSource source)
    {
        int randomIndex = Random.Range(0, clips.Length);


        source.clip = clips[randomIndex];
        source.Play(); 
    }

}
