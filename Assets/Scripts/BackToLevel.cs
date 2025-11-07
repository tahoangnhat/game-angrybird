using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLevel : MonoBehaviour
{
    public void GoBackToLevelSelect()
    {
        SceneManager.LoadScene("Level");
    }
}
