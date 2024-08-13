using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamManager : MonoBehaviour
{
    public static VCamManager Instance;

    [SerializeField] private CinemachineVirtualCamera sniperVCam;
    [SerializeField] private CinemachineVirtualCamera bulletVCam;
    [SerializeField] private CinemachineVirtualCamera trumpVCam;

    private void Awake()
    {
        Instance = this;
    }

    public void VCamToSniper()
    {
        sniperVCam.Priority = 10;
        bulletVCam.Priority = 0;
        trumpVCam.Priority = 0;
    }

    public void VCamToBullet()
    {
        sniperVCam.Priority = 0;
        bulletVCam.Priority = 10;
        trumpVCam.Priority = 0;
    }

    public void VCamToTrump()
    {
        sniperVCam.Priority = 0;
        bulletVCam.Priority = 0;
        trumpVCam.Priority = 10;
    }
}
