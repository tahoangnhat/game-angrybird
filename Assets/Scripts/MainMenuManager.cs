using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene1"); // 👈 thay bằng tên scene bạn muốn load
    }

    public void Logout()
    {
        SceneManager.LoadScene("SceneLogin"); // 👈 quay lại trang đăng nhập
    }
}
