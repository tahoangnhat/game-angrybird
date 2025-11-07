using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public string sceneName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
