using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Start()
    {
        UpdateScore(0);
    }

    // Update is called once per frame
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }
}
