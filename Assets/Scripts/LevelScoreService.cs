using UnityEngine;
using UnityEngine.SceneManagement;
public static class LevelScoreService
{
    private static string _keyFor(int BuildIndex) => $"BestLevelScore_{BuildIndex}";

    public static int GetBestScore(int BuildIndex)
    {
        return PlayerPrefs.GetInt(_keyFor(BuildIndex), 0);
    }

    public static void SetBestScore(int BuildIndex, int Score)
    {
        int currentBest = GetBestScore(BuildIndex);
        if (Score > currentBest)
        {
            PlayerPrefs.SetInt(_keyFor(BuildIndex), Score);
            PlayerPrefs.Save();
        }
    }

    public static int GetTotalAcrossLevels()
    {
        int totalScore = 0;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            totalScore += GetBestScore(i);
        }
        return totalScore;
    }


}
