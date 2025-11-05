using UnityEngine;
using TMPro;
public class TotalScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalScoreText;

    private void OnEnable()
    {
        if (_totalScoreText != null){
            int total = LevelScoreService.GetTotalAcrossLevels();
            _totalScoreText.text = $"Total Score: {total}";
        }
    }
    public void setTotalScore(int total)
    {
        if(_totalScoreText != null)
        {
            _totalScoreText.text = $"Total Score: {total}";
        }
    }
}
