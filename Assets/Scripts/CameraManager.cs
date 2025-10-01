using Unity.Cinemachine;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _idleCam;
    [SerializeField] private CinemachineCamera _followCam;

    private void Awake()
    {
        SwitchToIdleCam();
    }


    public void SwitchToIdleCam()
    {
        _idleCam.enabled = true;
        _followCam.enabled = false;
    }
    public void SwitchToFollowCam(Transform followTransform)
    {
        _followCam.Follow = followTransform;

        _idleCam.enabled = false;
        _followCam.enabled = true;
    }
}
