using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int MaxNumberOfShots = 3;
    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private SlingShotHandler _slingShotHandler;
    [SerializeField] private Image _nextLevelImage;

    private int _usedNumberOfShots;

    private IconHandler _iconHandler;

    private List<Baddie> _baddies = new List<Baddie>();

    private int _currentScore;

    private ScoreUI _scoreUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _scoreUI = FindObjectOfType<ScoreUI>();

        _iconHandler = FindObjectOfType<IconHandler>();

        Baddie[] baddies = FindObjectsOfType<Baddie>();

        for (int i = 0; i < baddies.Length; i++)
        {
            _baddies.Add(baddies[i]);
        }

        _nextLevelImage.enabled = false;
    }

    private void Start()
    {
        if (_iconHandler != null)
        {
            _iconHandler.InitializeIcons(MaxNumberOfShots);
        }
        else
        {
            Debug.LogWarning("IconHandler not found! Make sure BirdIcons object is active in the scene.");
        }
    }

    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);

        checkForLastShot();
    }

    public bool HasEnoughShots()
    {
        if (_usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void checkForLastShot()
    {
        if (_usedNumberOfShots >= MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);

        if (_baddies.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }
    }

    public void RemoveBaddie(Baddie baddie)
    {
        _baddies.Remove(baddie);
        CheckForAllDeadBaddies();
    }

    public void AddScore(int points)
    {
        _currentScore += points;
        if (_scoreUI != null)
        {
            _scoreUI.UpdateScore(_currentScore);
        }
    }

    public void CheckForAllDeadBaddies()
    {
        if (_baddies.Count == 0)
        {
            WinGame();
        }
    }

    #region Win/Lose

    private void WinGame()
    {
        int idx = SceneManager.GetActiveScene().buildIndex;
        LevelScoreService.SetBestScore(idx, _currentScore);

        int count = SceneManager.sceneCountInBuildSettings;
        int sum = 0;
        for (int i = 0; i < count; i++)
        {
            int best = LevelScoreService.GetBestScore(i);
            Debug.Log($"Best[{i}]={best}");
            sum += best;
        }
        Debug.Log($"Total={sum}, currentLevel={idx}, currentScore={_currentScore}");


        _restartScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;



        //do we have more lv to load?
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevelIndex = SceneManager.sceneCountInBuildSettings;
        if (currentLevelIndex + 1 < maxLevelIndex)
        {
            _nextLevelImage.enabled = true;
        }

    }

    public void RestartGame()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    #endregion

}
