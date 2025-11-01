using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // Hàm này được gọi khi nhấn nút
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("SceneMainMenu");
    }
}