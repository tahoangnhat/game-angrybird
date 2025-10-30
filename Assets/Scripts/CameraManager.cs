using Unity.Cinemachine;
using UnityEngine;
using System.Collections; 

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _idleCam;
    [SerializeField] private CinemachineCamera _followCam;
    [SerializeField] private float _followDuration = 1.5f;

    private Coroutine _followCoroutine;

    private void Awake()
    {
        SwitchToIdleCam();
    }

    public void SwitchToIdleCam()
    {
        if (_followCoroutine != null)
        {
            StopCoroutine(_followCoroutine);
            _followCoroutine = null;
        }

        _idleCam.enabled = true;
        _followCam.enabled = false;
    }

    public void SwitchToFollowCam(Transform followTransform)
    {
        _followCam.Follow = followTransform;
        _idleCam.enabled = false;
        _followCam.enabled = true;

        if (_followCoroutine != null)
            StopCoroutine(_followCoroutine);

        _followCoroutine = StartCoroutine(ReturnToIdleAfterDelay(_followDuration));
    }

    private IEnumerator ReturnToIdleAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SwitchToIdleCam();
    }
}
