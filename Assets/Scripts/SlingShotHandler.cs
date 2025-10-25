using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingShotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer _leftLineRenderer, _rightLineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform _leftStarPosition;
    [SerializeField] private Transform _rightStarPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;
    [SerializeField] private Transform _elasticTransform;

    [Header("SlingShot Stats")]
    [SerializeField] private float _maxDistance = 3.5f;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private float _timeBetweenBirdRespawns = 2f;
    [SerializeField] private float _elasticDivider = 1.2f;
    [SerializeField] private AnimationCurve _elasticCurve;
    [SerializeField] private float _maxAnimationTime = 1f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;
    [SerializeField] private CameraManager _cameraManager;

    [Header("Bird")]
    [SerializeField] private AngryBird _angryBirdPrefab;
    [SerializeField] private float _angryBirdPositionOffset = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip _elasticPulledClip;
    [SerializeField] private AudioClip[] _elasticReleasedClips;


    private Vector2 _slingShotLinesPosition;

    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea;
    private bool _birdOnSlingShot;

    private AngryBird _spawnedAngryBird;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;

        SpawnAngryBird();
    }

    // Update is called once per frame
    private void Update()
    {
        if (InputManager.WasLeftMouseButtonPressed && _slingShotArea.IsWithinSlingShotArea())
        {
            _clickedWithinArea = true;

            if (_birdOnSlingShot)
            {
                SoundManager.Instance.playClip(_elasticPulledClip, _audioSource);
                _cameraManager.SwitchToFollowCam(_spawnedAngryBird.transform);
            }
        }
        if (InputManager.IsLeftMousePressed && _clickedWithinArea && _birdOnSlingShot)
        {
            DrawSlingShot();
            PositionAndRotateAngryBird();
        }
        if (InputManager.WasLeftMouseButtonReleased && _birdOnSlingShot && _clickedWithinArea)
        {
            if (GameManager.instance.HasEnoughShots())
            {
                _clickedWithinArea = false;
                _birdOnSlingShot = false;

                _spawnedAngryBird.LaunchBird(_directionNormalized, _shotForce);

                SoundManager.Instance.playRandomClip(_elasticReleasedClips, _audioSource);

                GameManager.instance.UseShot();
                AnimateSlingShot();

                if (GameManager.instance.HasEnoughShots())
                {
                    StartCoroutine(SpawnAngryBirdAfterTime());
                }
            }
        }
    }

    #region SlingShot Methods
    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        _slingShotLinesPosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);

        SetLines(_slingShotLinesPosition);

        _direction = (Vector2)_centerPosition.position - _slingShotLinesPosition;
        _directionNormalized = _direction.normalized;
    }
    private void SetLines(Vector2 position)
    {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStarPosition.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStarPosition.position);
    }

    #endregion

    #region Angry Bird Methods

    private void SpawnAngryBird()
    {
        _elasticTransform.DOComplete();
        SetLines(_idlePosition.position);

        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position + dir * _angryBirdPositionOffset;

        _spawnedAngryBird = Instantiate(_angryBirdPrefab, spawnPosition, Quaternion.identity);
        _spawnedAngryBird.transform.right = dir;

        _birdOnSlingShot = true;
    }

    private void PositionAndRotateAngryBird()
    {
        _spawnedAngryBird.transform.position = _slingShotLinesPosition + _directionNormalized * _angryBirdPositionOffset;
        _spawnedAngryBird.transform.right = -_directionNormalized;
    }

    private IEnumerator SpawnAngryBirdAfterTime()
    {
        yield return new WaitForSeconds(_timeBetweenBirdRespawns);

        SpawnAngryBird();

        _cameraManager.SwitchToIdleCam();
    }

    #endregion

    #region Animate slingshot
    private void AnimateSlingShot()
    {
        _elasticTransform.position = _leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(_elasticTransform.position, _centerPosition.position);

        float time = dist / _elasticDivider;

        _elasticTransform.DOMove(_centerPosition.position, time).SetEase(_elasticCurve);
        StartCoroutine(AnimateSlingShotLines(_elasticTransform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time && elapsedTime < _maxAnimationTime)
        {
            elapsedTime += Time.deltaTime;

            SetLines(trans.position);

            yield return null;
        }
    }


    #endregion
}
