using UnityEngine;

public class SoundUILoader : MonoBehaviour
{
    private static GameObject soundUIInstance;
    public GameObject soundUIPrefab;

    void Awake()
    {

        if (soundUIInstance == null)
        {
            soundUIInstance = Instantiate(soundUIPrefab);
            DontDestroyOnLoad(soundUIInstance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
