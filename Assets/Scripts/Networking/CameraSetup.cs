using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{

    private CinemachineVirtualCamera _vcam;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        _vcam.Follow = ClientInstance.GetClientInstance()?.Player.transform;
    }

    public void HandlePlayerSpawned()
    {
        _vcam.Follow = ClientInstance.GetClientInstance()?.Player.transform;
    }

}
