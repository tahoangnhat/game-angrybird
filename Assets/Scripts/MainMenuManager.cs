using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level"); 
    }

    public void Logout()
    {
        SceneManager.LoadScene("SceneLogin"); 
    }
}
