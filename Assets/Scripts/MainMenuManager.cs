using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene1"); 
    }

    public void Logout()
    {
        SceneManager.LoadScene("SceneLogin"); 
    }
}
