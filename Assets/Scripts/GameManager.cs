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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _iconHandler = FindObjectOfType<IconHandler>();

        Baddie[] baddies = FindObjectsOfType<Baddie>();
        for (int i = 0; i < baddies.Length; i++)
        {
            _baddies.Add(baddies[i]);
        }

        _nextLevelImage.enabled = false;
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
        _restartScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;

        //do we have more lv to load?
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevelIndex = SceneManager.sceneCountInBuildSettings;
        if(currentLevelIndex + 1 < maxLevelIndex)
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
